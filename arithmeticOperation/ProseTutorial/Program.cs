using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using Microsoft.ProgramSynthesis.Compiler;
using Microsoft.ProgramSynthesis.Diagnostics;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Learning.Strategies;
using Microsoft.ProgramSynthesis.Specifications;
using Microsoft.ProgramSynthesis.VersionSpace;

namespace ProseTutorial
{
    internal class Program
    {
        private static readonly Result<Grammar> grammarResult = DSLCompiler.Compile(new CompilerOptions
        {
            InputGrammarText = File.ReadAllText("synthesis/grammar/arithmetic.grammar"),
            References = CompilerReference.FromAssemblyFiles(typeof(Program).GetTypeInfo().Assembly)
        });
        private static readonly Grammar Grammar = grammarResult.Value;

        private static SynthesisEngine _prose;

        private static readonly Dictionary<State, object> Examples = new Dictionary<State, object>();
        private static ProgramNode _topProgram;

        private static void Main(string[] args)
        {
            if(Grammar == null)
            {
                grammarResult.TraceDiagnostics();
                return;
            }
            
            _prose = ConfigureSynthesis();
            var menu = @"Select one of the options: 
1 - provide new example
2 - run synthesized program on a new input
3 - exit";
            var option = 0;
            while (option != 3)
            {
                Console.Out.WriteLine(menu);
                try
                {
                    option = short.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.Out.WriteLine("Invalid option. Try again.");
                    continue;
                }

                try
                {
                    RunOption(option);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("Something went wrong...");
                    Console.Error.WriteLine("Exception message: {0}", e.Message);
                }
            }
        }

        private static void RunOption(int option)
        {
            switch (option)
            {
                case 1:
                    LearnFromNewExample();
                    break;
                case 2:
                    RunOnNewInput();
                    break;
                default:
                    Console.Out.WriteLine("Invalid option. Try again.");
                    break;
            }
        }

        private static void LearnFromNewExample()
        {
            Console.Out.Write("Provide a new input-output example (e.g., {1,2,3,4}=5 ): ");
            try
            {
                string input = Console.ReadLine();
                if (input != null)
                {
                    int listStart = input.IndexOf("{", StringComparison.Ordinal) + 1;
                    int listEnd = input.IndexOf("}", StringComparison.Ordinal) - 1;

                    if (listStart >= listEnd)
                        throw new Exception(
                            "Invalid example format. Please try again");

                    string listAsString = input.Substring(listStart, listEnd - listStart + 1);
                    List<string> charList = listAsString.Split(',').ToList();
                    List<int> inputExample = charList.ConvertAll(int.Parse);

                    int outputStartIndex = input.IndexOf("=") + 1;
                    string outputString = input.Substring(outputStartIndex);
                    int outputExample = int.Parse(outputString);

                    State inputState = State.CreateForExecution(Grammar.InputSymbol, inputExample);
                    Examples.Add(inputState, outputExample); 
                }
            }
            catch (Exception)
            {
                throw new Exception("Invalid example format. Please try again");
            }

            var spec = new ExampleSpec(Examples);
            Console.Out.WriteLine("Learning a program for examples:");
            foreach (KeyValuePair<State, object> example in Examples)
            {                
                Console.WriteLine("\"{0}\" -> \"{1}\"", example.Key.Bindings.First().Value.ToString(), example.Value);
            }
                
            
            //var scoreFeature = new RankingScore(Grammar);
            ProgramSet topPrograms = _prose.LearnGrammar(spec);
            if (topPrograms.IsEmpty) throw new Exception("No program was found for this specification.");

            _topProgram = topPrograms.RealizedPrograms.First();
            Console.Out.WriteLine(_topProgram.PrintAST(ASTSerializationFormat.HumanReadable));            
        }

        private static void RunOnNewInput()
        {
            if (_topProgram == null)
                throw new Exception("No program was synthesized. Try to provide new examples first.");
            Console.Out.WriteLine("Top program: {0}", _topProgram);

            try
            {
                Console.Out.Write("Insert a new input: ");
                string newInput = Console.ReadLine();
                if (newInput != null)
                {
                    int listStart = newInput.IndexOf("{", StringComparison.Ordinal) + 1;
                    int listEnd = newInput.IndexOf("}", StringComparison.Ordinal) - 1;
                    string listAsString = newInput.Substring(listStart, listEnd - listStart + 1);
                    List<string> charList = listAsString.Split(',').ToList();
                    List<int> newInputExample = charList.ConvertAll(int.Parse);
                    State newInputState = State.CreateForExecution(Grammar.InputSymbol, newInputExample);
                    Console.Out.WriteLine("RESULT: \"{0}\" -> \"{1}\"", string.Join(",", newInputExample), _topProgram.Invoke(newInputState));
                }
            }
            catch (Exception)
            {
                throw new Exception("The execution of the program on this input thrown an exception");
            }
        }

        public static SynthesisEngine ConfigureSynthesis()
        {
            var witnessFunctions = new WitnessFunctions(Grammar);
            var deductiveSynthesis = new DeductiveSynthesis(witnessFunctions);
            var synthesisExtrategies = new ISynthesisStrategy[] {deductiveSynthesis};
            var synthesisConfig = new SynthesisEngine.Config {Strategies = synthesisExtrategies};
            var prose = new SynthesisEngine(Grammar, synthesisConfig);
            
            return prose;
        }
    }
}
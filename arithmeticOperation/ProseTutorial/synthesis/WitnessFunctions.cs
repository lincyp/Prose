using System.Collections.Generic;
using System.Linq;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;

namespace ProseTutorial
{
    public class WitnessFunctions : DomainLearningLogic
    {
        //private static HashSet<int> dividendMap = new HashSet<int>();
        public WitnessFunctions(Grammar grammar) : base(grammar)
        {
        }
                /*
        [WitnessFunction(nameof(Semantics.Divide), 0, Verify = true)]
        public DisjunctiveExamplesSpec WitnessDividePara1(GrammarRule rule, ExampleSpec spec)
        {
            var result = new Dictionary<State, IEnumerable<object>>();
            foreach (KeyValuePair<State, object> example in spec.Examples)
            {
                State inputState = example.Key;                
                var quotient = (int)example.Value;

                var occurences = new List<int>();

                for (int divisor = 1; quotient > 1 && divisor < 10; divisor++)
                {
                   // int dividend = divisor * quotient;
                    
                    if (occurences.IndexOf(divisor) == -1)
                            occurences.Add(divisor);
                    
                }

                if (occurences.Count == 0)
                {
                    return null;
                }

                result[inputState] = occurences.Cast<object>();
            }
            return new DisjunctiveExamplesSpec(result);
        }

        [WitnessFunction(nameof(Semantics.Divide), 1, DependsOnParameters = new[] { 0 })]
        public ExampleSpec WitnessDividePara2(GrammarRule rule, ExampleSpec spec, ExampleSpec para1Spec)
        {
            var result = new Dictionary<State, object>();
            foreach (KeyValuePair<State, object> example in spec.Examples)
            {
                State inputState = example.Key;                
                var output = (int)example.Value;
                
                var a = (int)para1Spec.Examples[inputState];

                if (output == 0 || output == 1)
                {
                    return null;
                }

                result[inputState] =  a*output;
            }
            return new ExampleSpec(result);
        }*/

        [WitnessFunction(nameof(Semantics.Multiply), 0, Verify = true)]
        public DisjunctiveExamplesSpec WitnessMultiplyPara1(GrammarRule rule, DisjunctiveExamplesSpec spec)
        {
            var result = new Dictionary<State, IEnumerable<object>>();
            foreach (KeyValuePair<State, IEnumerable<object>> example in spec.DisjunctiveExamples)
            {
                State inputState = example.Key;
                //var v = inputState[rule.Body[0]] as List<int>;
                var productList = example.Value;

                var occurrences = new List<int>();

                foreach(int product in productList)
                {
                    for (int x = 1; product > 1 && x*x <= product; x++)
                    {
                        if (product % x == 0)// && v.IndexOf(product / x) > -1)
                        {
                            if(occurrences.IndexOf(x) == -1)
                                occurrences.Add(x);
                        }
                    }
                }

                if(occurrences.Count == 0)
                {
                    return null;
                }

                result[inputState] = occurrences.Cast<object>();
            }
            return new DisjunctiveExamplesSpec(result);
        }

        [WitnessFunction(nameof(Semantics.Multiply), 1, DependsOnParameters = new[] { 0 })]
        public ExampleSpec WitnessMultiplyPara2(GrammarRule rule, ExampleSpec spec, ExampleSpec para1Spec)
        {
            var result = new Dictionary<State, object>();
            foreach (KeyValuePair<State, object> example in spec.Examples)
            {
                State inputState = example.Key;
                //var v = inputState[rule.Body[0]] as List<int>;
                var output = (int)example.Value;
                var a = (int)para1Spec.Examples[inputState];
                if(a == 0 || output == 1)
                {
                    return null;
                }
                result[inputState] = output/a;
            }
            return new ExampleSpec(result);
        }

        [WitnessFunction(nameof(Semantics.Add), 0, Verify = true)]
        public DisjunctiveExamplesSpec WitnessAddPara1(GrammarRule rule, ExampleSpec spec)
        {
            var result = new Dictionary<State, IEnumerable<object>>();
            foreach (KeyValuePair<State, object> example in spec.Examples)
            {
                State inputState = example.Key;
                //var v = inputState[rule.Body[0]] as List<int>;
                var sum = (int)example.Value;
                
                var occurences = new List<int>();                
                
                for (int x = sum/2; x > 0; x--)
                {
                    if (sum - x >= 0)// && v.IndexOf(sum - x) > -1)
                    {
                        if (occurences.IndexOf(x) == -1)
                            occurences.Add(x);
                    }
                }

                if(occurences.Count == 0)
                {
                    return null;
                }

                result[inputState] = occurences.Cast<object>();
            }
            return new DisjunctiveExamplesSpec(result);
        }

        [WitnessFunction(nameof(Semantics.Add), 1, DependsOnParameters = new[] { 0 })]
        public ExampleSpec WitnessAddPara2(GrammarRule rule, ExampleSpec spec, ExampleSpec para1Spec)
        {
            var result = new Dictionary<State, object>();
            foreach (KeyValuePair<State, object> example in spec.Examples)
            {
                State inputState = example.Key;
                //var v = inputState[rule.Body[0]] as List<int>;
                var output = (int)example.Value;
                var a = (int)para1Spec.Examples[inputState];
               if(output == 1)
                {
                    return null;
                }
                result[inputState] = output-a;
            }
            return new ExampleSpec(result) ;
        }

        [WitnessFunction(nameof(Semantics.ElementAt), 1, Verify = true)]
        public DisjunctiveExamplesSpec WitnessK(GrammarRule rule, DisjunctiveExamplesSpec spec)
        {
            var result = new Dictionary<State, IEnumerable<object>>();
            foreach (KeyValuePair<State, IEnumerable<object>> example in spec.DisjunctiveExamples)
            {
                State inputState = example.Key;
                var v = inputState[rule.Body[0]] as List<int>;

                var positions = new List<int>();

                foreach(int element in example.Value)
                {
                    int elementIndex = v.IndexOf(element);

                    if (elementIndex > -1)
                    {
                        positions.Add(elementIndex);                       
                    }
                }

                if(positions.Count == 0)
                {
                    return null;
                }

                result[inputState] = positions.Cast<object>();
            }
            return new DisjunctiveExamplesSpec(result);
        }
        
        
    }
}
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using Microsoft.ProgramSynthesis.Features;

namespace ProseTutorial
{
    public class RankingScore : Feature<double>
    {
        public RankingScore(Grammar grammar) : base(grammar, "Score")
        {
        }

        protected override double GetFeatureValueForVariable(VariableNode variable)
        {
            return 0;
        }
        /*
        [FeatureCalculator(nameof(Semantics.Substring))]
        public static double Substring(double v, double start, double end)
        {
            return start * end;
        }*/

        [FeatureCalculator(nameof(Semantics.ElementAt))]
        public static double ElementAt(double v, double k)
        {
            return k;
        }
        
        [FeatureCalculator(nameof(Semantics.Add))]
        public static double Add(double v, double a, double b)
        {
            return a*b;
        }

        [FeatureCalculator(nameof(Semantics.Multiply))]
        public static double Multiply(double v, double a, double b)
        {
            return a*b;
        }

        [FeatureCalculator(nameof(Semantics.Divide))]
        public static double Divide(double v, double a, double b)
        {
            return a*b;
        }

        [FeatureCalculator("k", Method = CalculationMethod.FromLiteral)]
        public static double K(int k)
        {
            return 0;
        }
    }
}
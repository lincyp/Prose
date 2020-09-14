using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace ProseTutorial
{
    public static class Semantics
    {
        public static int? ElementAt(List<int> v, int k)
        {
            if (k >= v.Count || k < 0)
            {
                return null;
            }

            return v[k];
        }
        public static int? Add(int? a, int? b)
        {
            if(a ==null || b == null)
            {
                return null;
            }

            return a + b;
        }       

        public static int? Multiply(int? a, int? b)
        {
            if (a == null || b == null)
            {
                return null;
            }

            return a * b;
        }
        public static int? Divide(int? a, int? b)
        {
            if (a == null || b == null)
            {
                return null;
            }
            else if( b % a != 0)
            {
                return null;
            }

            return b/a;
        }
    }
}
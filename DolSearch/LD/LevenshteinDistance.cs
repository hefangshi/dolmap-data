using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DolSearch.LD
{
    public class LevenshteinDistance:LevenshteinDistanceBase
    {
        public override double ExchangeCost(int a, int b)
        {
            return a == b ? 0 : 1;
        }

        public override double AddCost(int a, int b)
        {
            return 1;
        }

        public override double DelCost(int a, int b)
        {
            return 1;
        }

        public override double InitVal(int a)
        {
            return 1;
        }
    }
}

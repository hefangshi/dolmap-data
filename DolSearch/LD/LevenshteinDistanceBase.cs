using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DolSearch.LD
{
    public abstract class LevenshteinDistanceBase
    {
        public abstract double ExchangeCost(int a, int b);
        public abstract double AddCost(int a, int b);
        public abstract double DelCost(int a, int b);
        public abstract double InitVal(int a);

        public double Compute(string s, string t)
        {
            return Compute(stringToInt(s), stringToInt(t));
        }

        private int[] stringToInt(string s)
        {
            return s.ToCharArray().Select(x =>
            {
                return (int)x;
            }).ToArray();
        }

        /// <summary>
        /// Compute the distance between two strings.
        /// </summary>
        public double Compute(int[] s, int[] t)
        {
            int n = s.Length;
            int m = t.Length;
            double[,] d = new double[n + 1, m + 1];
            // Step 1
            if (n == 0)
            {
                return m;
            }
            if (m == 0)
            {
                return n;
            }
            // Step 2
            for (int i = 0; i <= n; i++)
            {
                if (i > 1)
                    d[i, 0] = AddCost(s[i - 2], s[i - 1]) + d[i - 1, 0];
                else if (i == 1)
                    d[i, 0] = InitVal(s[i - 1]);
                else
                    d[i, 0] = 0;
            }
            for (int j = 0; j <= m;j++)
            {
                if (j > 1)
                    d[0, j] = 1 + d[0, j - 1];
                else if (j == 1)
                    d[0, j] = InitVal(t[j - 1]);
                else
                    d[0, j] = 0;
            }
            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    double exchangeCost = ExchangeCost(t[j - 1], s[i - 1]);
                    double addCost = AddCost(t[j - 1], s[i - 1]);
                    double delCost = DelCost(t[j - 1], s[i - 1]);
                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + delCost, d[i, j - 1] + addCost),
                        d[i - 1, j - 1] + exchangeCost);
                }
            }
            for (int i = 0; i < m+1; i++)
            {
                for (int j = 0; j < n+1; j++)
                {
                    Debug.Write(d[j, i]);
                    Debug.Write(",");
                }
                Debug.WriteLine("");
            }
            // Step 7
            return d[n, m];
        }
    }
}

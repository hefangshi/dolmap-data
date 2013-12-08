using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestTransFix
{
    /// <summary>
    /// 编辑距离算法
    /// </summary>
    public class EditDistance
    {
        /**
    * 求三个数中的最小数Mar 1, 2007
    * 
    * @param a
    * @param b
    * @param c
    * @return
    */
        private static int Minimum(int a, int b, int c)
        {
            int mi;

            mi = a;
            if (b < mi)
            {
                mi = b;
            }
            if (c < mi)
            {
                mi = c;
            }
            return mi;
        }

        /**
         * 计算两个字符串间的编辑距离Mar 1, 2007
         *  @param s
         *  @param t
         *  @return
         */
        public static int getEditDistance(String s, String t)
        {
            int[,] d; // matrix
            int n = 0; // length of s
            int m = 0; // length of t
            int i; // iterates through s
            int j; // iterates through t
            char s_i; // ith character of s
            char t_j; // jth character of t
            int cost; // cost

            // Step 1
            if (s == null && t == null)
                return 0;
            if (s != null && t == null)
                return s.Length;
            if (s == null && t != null)
                return t.Length;
            n = s.Length;
            m = t.Length;
            if (n == 0)
            {
                return m;
            }
            if (m == 0)
            {
                return n;
            }
            d = new int[n + 1, m + 1];
            //d = new int[n+1][+1];

            // Step 2

            for (i = 0; i <= n; i++)
            {
                d[i, 0] = 1;
            }

            for (j = 0; j <= m; j++)
            {
                d[0, j] = j;
            }

            // Step 3

            for (i = 1; i <= n; i++)
            {
                s_i = s[i - 1];
                // Step 4
                for (j = 1; j <= m; j++)
                {
                    t_j = t[j - 1];
                    // Step 5
                    if (s_i == t_j)
                    {
                        cost = 0;
                    }
                    else
                    {
                        cost = 1;
                    }
                    // Step 6
                    d[i, j] = Minimum(d[i - 1, j] + 1, d[i, j - 1] + 1,
                            d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];

        }


    }
}

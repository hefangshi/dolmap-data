using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dol.Base;
using Dol.Base.DataModel;
using System.Diagnostics;

namespace DolSearch.LD
{
    public class DolRoute
    {
        public IDictionary<int, Point> cityLocationDic = new Dictionary<int, Point>();
        public IDictionary<string, int> cityCodeDic = new Dictionary<string, int>();

        public DolRoute()
        {
            LoadData();
        }

        public int GetCityID(string name)
        {
            return cityCodeDic[name];
        }

        private void LoadData()
        {
            CityDM cityDM = new CityDM();
            var cityList = cityDM.Load();
            cityList.All(city =>
            {
                cityCodeDic.Add(city.Name, city.ID);
                cityLocationDic.Add(city.ID, new Point() { X = city.X, Y = city.Y });
                return true;
            });
        }

        public int Compute(string[] s, string[] t)
        {
            var _s = s.Select(city =>
            {
                return cityCodeDic[city];
            }).ToArray();
            var _t = t.Select(city =>
            {
                return cityCodeDic[city];
            }).ToArray();
            return Compute(_s, _t);
        }


        private double distance(int a, int b)
        {
            var _a = cityLocationDic[a];
            var _b = cityLocationDic[b];
            return Math.Sqrt(Math.Pow(_a.X - _b.X, 2) + Math.Pow(_a.Y - _b.Y, 2));
        }

        public struct Point
        {
            public float X;
            public float Y;
        }


        public int Compute(int[] s, int[] t)
        {
            /*
            循环计算target的每一位
            source:abc
            target:def
            假设算到e，对于每个e插入的位置i， 最优值是通过循环每个d插入的位置k计算出来 ，以此类推
            直到算出最后一位f插入所有的位置的最优解
            循环f插入的所有位置，得到最优解
            */
            //Init
            var m = s.Length;
            var n = t.Length;
            var cost = new double[n, m];
            double sum_s = 0;
            for (var i = 0; i < m; ++i)
            {
                if (i > 0)
                    sum_s += distance(s[i - 1], s[i]);
                cost[0, i] = sum_s + distance(s[i], t[0]);
            }

            // Dynamic Programming
            for (var j = 1; j < n; ++j)	// target
            {
                for (var i = 0; i < m; ++i)	// insert to source position
                {
                    cost[j, i] = -1;
                    for (var k = 0; k <= i; ++k)	// last insert position
                    {
                        var costnow = cost[j - 1, k];
                        if (k == i)
                            costnow += distance(t[j - 1], t[j]);
                        else
                        {
                            costnow += distance(s[i], t[j]);
                            for (var l = k + 1; l <= i; ++l)	//  calc distance from last insert to current insert
                            {
                                if (l == k + 1)
                                    costnow += distance(t[j - 1], s[l]);
                                else
                                    costnow += distance(s[l - 1], s[l]);
                            }
                        }
                        if ((cost[j, i] < 0) || (costnow < cost[j, i]))
                            cost[j, i] = costnow;
                    }
                }
            }
            // Check cost[n,1..m] to get best cost
            double bestcost = -1;
            for (var i = 0; i < m; ++i)
            {
                var costnow = cost[n-1, i];
                for (var j = i + 1; j < m; ++j)
                {
                    if (j == i + 1)
                        costnow += distance(t[n - 1], s[j]);
                    else
                        costnow += distance(s[j - 1], s[j]);
                }
                if ((bestcost < 0) || (costnow < bestcost))
                    bestcost = costnow;
            }

            //for (int i = 0; i < m; i++)
            //{
            //    for (int j = 0; j < n ; j++)
            //    {
            //        Debug.Write(cost[j, i]);
            //        Debug.Write(",");
            //    }
            //    Debug.WriteLine("");
            //}
            return (int)(bestcost / cost[0, m - 1] * 2 * 1000);
        }
    }
}

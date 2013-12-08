using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dol.Base;
using Dol.Base.DataModel;

namespace DolSearch.LD
{
    public class DolRouteLD:LevenshteinDistanceBase
    {
        public IDictionary<int, Point> cityLocationDic = new Dictionary<int, Point>();
        public IDictionary<string, int> cityCodeDic = new Dictionary<string, int>();

        public  DolRouteLD()
        {
            LoadData();
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

        public double Compute(string[] s,string[] t)
        {
            var _s= s.Select(city =>
            {
                return cityCodeDic[city];
            }).ToArray();
            var _t = t.Select(city =>
            {
                return cityCodeDic[city];
            }).ToArray();
            return Compute(_s, _t);
        }

        public override double ExchangeCost(int a, int b)
        {
            return a == b ? 0 : 1;
            return distance(cityLocationDic[a], cityLocationDic[b]);
        }

        public override double AddCost(int a, int b)
        {
            return 1;
            return distance(cityLocationDic[a], cityLocationDic[b]);
        }

        public override double DelCost(int a, int b)
        {
            return 1;
        }

        private double distance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        public struct Point
        {
            public float X;
            public float Y;
        }

        public override double InitVal(int a)
        {
            return 1;
        }
    }
}

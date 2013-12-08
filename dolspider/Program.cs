using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dolspider.Spiders.GVO.Quest;
using Newtonsoft.Json;
using System.IO;
using MySql;
using MySql.Data.MySqlClient;
using Dol.Base;
using Dol.Base.DataModel;
using dolspider.Spiders.GVO.Map;

namespace dolspider
{
    class Program
    {
        static void Main(string[] args)
        {
            var mapspider = new GVOMapSpider();
            mapspider.Start();
            var spider = new GVOQuestSpider();
            spider.Start();
            var dm = new QuestDM();
            spider.QuestList.Concat(mapspider.QuestList).Distinct(new QuestComparer()).All(quest=>{
                dm.Insert(quest);
                return true;
            });
            Console.ReadLine();
        }
    }

    class QuestComparer : IEqualityComparer<Quest>
    {

        #region IEqualityComparer<Quest> Members

        public bool Equals(Quest x, Quest y)
        {
            return x.ID == y.ID;
        }

        public int GetHashCode(Quest obj)
        {
            return (obj.ID + obj.Name).GetHashCode();
        }

        #endregion
    }
}

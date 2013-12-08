using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dolspider.Spiders.GVO.Map.Handlers;
using HtmlAgilityPack;

namespace dolspider.Spiders.GVO.Map
{
    public class GVOMapSpider : ISpider
    {
        private const string QUEST_URL = "http://gvo.cbo.com.tw/Adv_Library.aspx?city={0}";
        private static Encoding ENCODING = Encoding.UTF8;
        public IList<Dol.Base.Quest> QuestList
        {
            get;
            private set;
        }

        #region ISpider Members

        public void Start()
        {
            var url=new Uri(String.Format(QUEST_URL,"2"));
            var doc= Util.GetDoc(url, ENCODING);
            var cityList=PageCountHandler.GetPageCount(doc);
            List<Dol.Base.Quest> questList = new List<Dol.Base.Quest>();
            foreach (string city in cityList)
            {
                url = new Uri(String.Format(QUEST_URL, city));
                doc = Util.GetDoc(url, ENCODING);
                Console.Out.WriteLine("解析城市"+city+"的地图。");
                var nowPageQuestList=PageHandler.GetQuestList(doc);
                questList.AddRange(nowPageQuestList);
                Console.Out.WriteLine("添加" + nowPageQuestList.Count + "条地图，总计" + questList.Count + "条。");
            }
            Console.Out.WriteLine("抓取完成。");
            QuestList = questList;
        }

        #endregion
    }
}

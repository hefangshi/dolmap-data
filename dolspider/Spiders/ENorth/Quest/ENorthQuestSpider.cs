using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dolspider.Spiders.ENorth.Quest.Handlers;
using HtmlAgilityPack;

namespace dolspider.Spiders.ENorth.Quest
{
    public class ENorthQuestSpider:ISpider
    {
        private const string QUEST_URL = "http://dols.enorth.com.cn/mission.do?page={0}&act=list";
        private static Encoding ENCODING = Encoding.GetEncoding("GBK");
        public IList<Dol.Base.Quest> QuestList
        {
            get;
            private set;
        }

        #region ISpider Members

        public void Start()
        {
            var url=new Uri(String.Format(QUEST_URL,"1"));
            var doc= Util.GetDoc(url, ENCODING);
            var count=PageCountHandler.GetPageCount(doc);
            List<Dol.Base.Quest> questList = new List<Dol.Base.Quest>();
            for (int i=0;i<count;i++)
            {
                url = new Uri(String.Format(QUEST_URL, i + 1));
                doc = Util.GetDoc(url, ENCODING);
                Console.Out.WriteLine("解析第"+(i+1)+"页任务，共"+count+"页。");
                var nowPageQuestList=PageHandler.GetQuestList(doc);
                questList.AddRange(nowPageQuestList);
                Console.Out.WriteLine("添加" + nowPageQuestList.Count + "条任务，总计" + questList.Count + "条。");
            }
            Console.Out.WriteLine("抓取完成。");
            QuestList = questList;
        }

        #endregion
    }
}

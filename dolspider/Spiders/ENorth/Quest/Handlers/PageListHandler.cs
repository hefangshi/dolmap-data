using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace dolspider.Spiders.ENorth.Quest.Handlers
{
    public abstract class PageListHandler
    {
        public static IList<HtmlNode> GetQuestHTMLList(HtmlDocument doc)
        {
            //获取总页数
            //table id=demo last tr
            var questLinkList = doc.DocumentNode.SelectSingleNode("//table[@id='demo']").SelectNodes("tr");
            return questLinkList.Skip(1).Take(questLinkList.Count - 2).ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace dolspider.Spiders.GVO.Map.Handlers
{
    public abstract class PageListHandler
    {
        public static IList<HtmlNode> GetQuestHTMLList(HtmlDocument doc)
        {
            //获取总页数
            //table id=demo last tr
            var questLinkList = doc.DocumentNode.SelectSingleNode("//table[@id='ctl00_CP1_GV1']").SelectNodes("tr");
            return questLinkList.Skip(1).ToList();
        }
    }
}

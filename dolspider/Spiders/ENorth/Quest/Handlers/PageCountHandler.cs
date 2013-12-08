using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace dolspider.Spiders.ENorth.Quest.Handlers
{
    public abstract class PageCountHandler
    {
        private static Regex regex = new Regex(@"mission.do\?page=(?'page'\d*)&act=list");

        public static int GetPageCount(HtmlDocument doc)
        {
            //获取总页数
            //table id=demo last tr
            var lastPageLink = doc.DocumentNode.SelectSingleNode("//table[@id='demo']")
                .SelectSingleNode("tr[last()]")
                .SelectSingleNode("td[last()]")
                .SelectSingleNode("a[last()]");
            //<a href="mission.do?page=8&act=list&link=_mt0lt0pl0">尾页</a>
            var url = lastPageLink.Attributes["href"].Value;
            var pageCount = Int32.Parse(regex.Match(url).Groups["page"].Value);
            return pageCount;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace dolspider.Spiders.GVO.Quest.Handlers
{
    public abstract class PageCountHandler
    {
        private static Regex regex = new Regex(@"Adv_Mission.aspx\?city=(?'city'\d*)");
        public static IList<string> GetPageCount(HtmlDocument doc)
        {
            //获取总页数
            //table id=demo last tr
            var pageList = doc.DocumentNode.SelectSingleNode("//table[@id='ctl00_CP1_DataList1']")
                .SelectNodes("descendant::a").Select(link =>
                {
                    var url = link.Attributes["href"].Value;
                    var city = regex.Match(url).Groups["city"].Value;
                    return city;
                }).ToList();
            return pageList;
        }

    }
}

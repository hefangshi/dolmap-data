using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using Dol.Base;

namespace dolspider.Spiders.GVO.Map.Handlers
{
    public static class RemarkHandler
    {
        private static Regex typeRegex = new Regex(@"images/smType(?'type'\d*).jpg");
        private static Regex questRegex = new Regex(@"ADV_MissionDetail.aspx\?MID=(?'id'\d*)");

        public static Remark ParseRemark(HtmlNode remarkNode)
        {
            var remark = new Remark();
            //发现物
            var discoveryNode = remarkNode.SelectSingleNode("a[1]");
            if (discoveryNode != null && String.IsNullOrEmpty(discoveryNode.InnerText) == false)
            {
                var typeNode = discoveryNode.PreviousSibling.PreviousSibling;
                remark.DiscoveryType = Enum.Parse(typeof(DisType), typeRegex.Match(typeNode.Attributes["src"].Value).Groups["type"].Value).ToString();
                remark.Discovery = discoveryNode.InnerText;
            }
            //奖励物
            var awardNode = remarkNode.SelectSingleNode("span[@style='color:Gray;']");
            if (awardNode != null)
                remark.AwardItem = awardNode.InnerText;
            //相关任务
            var relativeNodes = remarkNode.SelectNodes("descendant::a[@style='color:#C000C0;' or @style='color:DarkBlue;']");
            if (relativeNodes != null)
            {
                foreach (HtmlNode relativeNode in relativeNodes)
                {
                    IList<int> questList = null;
                    IList<string> foundNameList = null;
                    if (relativeNode.InnerText.StartsWith("前:"))
                    {
                        foundNameList = remark.PreFoundName;
                        questList = remark.PreQuestID;
                    }
                    else
                    {
                        questList = remark.FollowQuestID;
                    }
                    var match = questRegex.Match(relativeNode.Attributes["href"].Value);
                    if (relativeNode.InnerText.StartsWith("前:港口-") == false)
                        questList.Add(Int32.Parse(match.Groups["id"].Value));
                    else
                        foundNameList.Add(relativeNode.InnerText.Replace("前:港口-",""));
                }
            }
            //接受城市
            //last br next a
            var cityNodes = remarkNode.SelectNodes("descendant::a[@class='MisCity']");
            if (cityNodes != null)
            {
                cityNodes.All(node =>
                {
                    if (node.InnerText == "南美开拓港" || node.InnerText == "东南亚开拓港" ||
                        node.InnerText == "掠夺地图" || node.InnerText == "沉船资讯" || node.InnerText == "沈船资讯")
                        return true;
                    remark.FromCityList.Add(node.InnerText);
                    return true;
                });
            }
            return remark;
        }
    }

    enum DisType
    {
        史迹=1,
        宗教建筑 = 2,
        历史遗物= 3,
        宗教遗物=4,
        美术品=5,
        财宝=6,
        化石=7,
        植物=8,
        昆虫=9,
        鸟类=10,
        小型生物=11,
        中型生物=12,
        大型生物=13,
        海洋生物=14,
        港口村落=15,
        地理=16
    }
}

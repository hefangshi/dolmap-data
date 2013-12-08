using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using Dol.Base;

namespace dolspider.Spiders.ENorth.Quest.Handlers
{
    public static class RemarkHandler
    {
        private static Regex discoveryRegex = new Regex(@"discover.do\?act=look&discover_id=(?'id'\d*)");
        private static Regex questRegex = new Regex(@"mission.do\?act=look&mission_id=(?'id'\d*)");

        public static Remark ParseRemark(HtmlNode remarkNode)
        {
            var remark=new Remark();
            //发现物
            var levelNode = remarkNode.SelectSingleNode("font[@color='red']");
            if (levelNode != null)
            {
                //level node
                //discover.do?act=look&amp;discover_id=3642
                var typeNode = levelNode.PreviousSibling;
                remark.DiscoveryType = typeNode.Attributes["alt"].Value;
                var linkNode = levelNode.NextSibling;
                var discoveryNode = linkNode.SelectSingleNode("font[@color='red']");
                remark.DiscoveryLevel = Int32.Parse(levelNode.InnerText.Substring(0, 1));
                remark.DiscoveryID = Int32.Parse(discoveryRegex.Match(linkNode.Attributes["href"].Value).Groups["id"].Value);
                remark.Discovery = discoveryNode.InnerText;
            }
            //奖励物
            var awardNode = remarkNode.SelectSingleNode("font[@color='#804000']");
            if (awardNode!=null)
                remark.AwardItem = awardNode.InnerText;
            //相关任务
            var relativeNodes = remarkNode.SelectNodes("font[@color='#00008b' or @color='#c000c0']");
            if (relativeNodes != null)
            {
                foreach (HtmlNode relativeNode in relativeNodes)
                {
                    IList<int> questList=null;
                    IList<string> foundNameList = null;
                    if (relativeNode.InnerText == "前:")
                    {
                        foundNameList = remark.PreFoundName;
                        questList=remark.PreQuestID;
                    }
                    else
                    {
                        questList=remark.FollowQuestID;
                    }
                    var questNode = relativeNode.NextSibling;
                    while (questNode != null && questNode.Name != "#text")
                    {
                        var match = questRegex.Match(questNode.Attributes["href"].Value);
                        if (match.Success)
                            questList.Add(Int32.Parse(match.Groups["id"].Value));
                        else
                            foundNameList.Add(questNode.InnerText);
                        questNode = questNode.NextSibling;
                    }
                }
            }
            //接受城市
            //last br next a
            var cityNodes = remarkNode.SelectNodes("br[last()]/following-sibling::a");
            if (cityNodes != null)
            {
                cityNodes.All(node =>
                {
                    if (node.InnerText == "南美开拓港" || node.InnerText == "东南亚开拓港" ||
                        node.InnerText == "掠夺地图" || node.InnerText == "沉船资讯")
                        return true;
                    remark.FromCityList.Add(node.InnerText);
                    return true;
                });
            }
            return remark;
        }
    }
}

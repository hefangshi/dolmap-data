using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using Dol.Base;

namespace dolspider.Spiders.GVO.Map.Handlers
{
    public class QuestHandler
    {
        private static Regex missionRegex = new Regex(@"ADV_MissionDetail.aspx\?MID=(?'id'\d*)");
        private static Regex chapterRegex = new Regex(@"images/Ch(?'chapter'\d*).gif");
        private static Regex levelRegex = new Regex(@"http://dols.enorth.com.cn/jpg/num/(?'level'\d*).gif");
        private static Regex numRegex = new Regex(@"[^\d]");

        public static Dol.Base.Quest GetQuest(HtmlNode node)
        {
            //first td,first a
            var quest = new Dol.Base.Quest();
            var href = node.SelectSingleNode("td[3]").SelectSingleNode("descendant::a").Attributes["href"].Value;
            var img = node.SelectSingleNode("td[3]").SelectSingleNode("descendant::img").Attributes["src"].Value;
            quest.Type = 1;//地图
            quest.ID = Int32.Parse(missionRegex.Match(href).Groups["id"].Value);
            quest.Chapter = Int32.Parse(chapterRegex.Match(img).Groups["chapter"].Value);
            quest.Name = node.SelectSingleNode("td[3]").SelectSingleNode("descendant::a[1]").InnerText;
            quest.Name = quest.Name.Replace("“", "");
            quest.Star = 0;
            quest.Skill = SkillHandler.ParseSkill(node.SelectSingleNode("td[4]"));
            var raw = node.SelectSingleNode("td[5]/span[1]").InnerHtml;
            quest.Content = raw;
            var remark = RemarkHandler.ParseRemark(node.SelectSingleNode("td[6]"));
            quest.FromCityList = remark.FromCityList;
            quest.AwardItem = remark.AwardItem;
            quest.Discovery = remark.Discovery;
            quest.DiscoveryID = remark.DiscoveryID;
            quest.DiscoveryLevel = remark.DiscoveryLevel;
            quest.DiscoveryType = remark.DiscoveryType;
            quest.DiscoveryExp = remark.DiscoveryExp;
            quest.FollowQuestID = remark.FollowQuestID;
            quest.PreFoundName = remark.PreFoundName;
            quest.PreQuestID = remark.PreQuestID;

            //Content解析
            POIs.Land.Concat(POIs.Citys).All(name =>
            {
                var index = raw.IndexOf(name);
                while (index != -1)
                {
                    //POIs.Count[name] = POIs.Count[name] + 1;
                    if (!quest.RoutesDic.Keys.Contains(index))
                    {
                        quest.RoutesDic.Add(index, name);
                    }
                    index = raw.IndexOf(name, index + name.Length);
                }
                return true;
            });
            var keys = quest.RoutesDic.Keys.ToArray();
            var lastRoute = String.Empty;
            foreach (var key in keys)
            {
                var route = quest.RoutesDic[key];
                if (lastRoute == route)
                {
                    quest.RoutesDic.Remove(key);
                }
                lastRoute = route;
            }
            return quest;
        }
    }
}

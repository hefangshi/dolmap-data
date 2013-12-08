using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using Dol.Base;

namespace dolspider.Spiders.ENorth.Quest.Handlers
{
    public class QuestHandler
    {
        private static Regex missionRegex = new Regex(@"mission.do\?act=look&mission_id=(?'id'\d*)");
        private static Regex levelRegex = new Regex(@"http://dols.enorth.com.cn/jpg/num/(?'level'\d*).gif");
        private static Regex numRegex = new Regex(@"[^\d]");

       //<tr bgcolor="#B7CFEB" align="center" style="background-color: rgb(143, 186, 236);">
       //         <td align="left"><a href="mission.do?act=look&amp;mission_id=3976" target="_black"><font class="zi14" color="black">高原白花的地图</font></a>&nbsp;<img alt="南十字星第四章" src="http://dols.enorth.com.cn/jpg\edition\CDS4.gif"></td>
       //         <td>0</td>
       //         <td align="left">
       //         <img alt="生态调查" src="http://dols.enorth.com.cn/jpg\skill\stdc.gif"><img alt="" src="http://dols.enorth.com.cn/jpg/num/5.gif"><img alt="生物学" src="http://dols.enorth.com.cn/jpg\skill\swx.gif"><img alt="" src="http://dols.enorth.com.cn/jpg/num/5.gif">
       //         </td>
       //         <td>0/0<br></td>
       //         <td align="left">热那亚门外 白花附近</td>
       //         <td align="left">
       //           <img alt="植物" src="http://dols.enorth.com.cn/jpg\distype\zw.jpg" width="20" height="20">
        //<font color="red">1★</font>
        //<a href="discover.do?act=look&amp;discover_id=3642" target="_black"><font color="red">火绒草</font></a>
       //           <br>
        //          <font color="#804000">                              </font>
       //           <br>
        //          <a href="mission.do?act=list&amp;link=pl8_mt0lt0pl0" target="_black">那不勒斯</a>
       //         </td>
       //       </tr>
        public static Dol.Base.Quest GetQuest(HtmlNode node)
        {
            //first td,first a
            var quest = new Dol.Base.Quest();
            var href = node.SelectSingleNode("td[1]").SelectSingleNode("a[1]").Attributes["href"].Value;
            quest.ID = Int32.Parse(missionRegex.Match(href).Groups["id"].Value);
            quest.Name = node.SelectSingleNode("td[1]").SelectSingleNode("a[1]").InnerText;
            quest.Name = quest.Name.Replace("“", "");
            quest.Name = Consistency.Parse(quest.Name);
            quest.Star = Int32.Parse(node.SelectSingleNode("td[2]").InnerText);
            quest.Skill = SkillHandler.ParseSkill(node.SelectSingleNode("td[3]"));
            var award = node.SelectSingleNode("td[4]").InnerText.Split('/');
            quest.Exp = Int32.Parse(award[0]);
            var fameStr=numRegex.Replace(award[1], "");
            quest.Fame = Int32.Parse(fameStr);
            var escape = new Escape();
            var raw=node.SelectSingleNode("td[5]").InnerHtml;
            raw=Consistency.Parse(raw);
            quest.Content = raw;
            var remark=RemarkHandler.ParseRemark(node.SelectSingleNode("td[6]"));
            quest.FromCityList = remark.FromCityList;
            quest.AwardItem = remark.AwardItem;
            quest.Discovery = remark.Discovery;
            quest.DiscoveryID = remark.DiscoveryID;
            quest.DiscoveryLevel = remark.DiscoveryLevel;
            quest.DiscoveryType = remark.DiscoveryType;
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
            var keys=quest.RoutesDic.Keys.ToArray();
            var lastRoute=String.Empty;
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

        private static IList<SkillRequirement> ParseSkill(HtmlNode skillNode)
        {
            var skillList=new List<SkillRequirement>();
            var skillNodes=skillNode.SelectNodes("img");
            if (skillNodes == null)
                return skillList;
            if (skillNodes.Count%2!=0)
            {
                Console.Out.WriteLine("任务技能解析出错，格式不正确："+skillNode.OuterHtml);
                return skillList;
            }
            for (int i=0;i<skillNodes.Count/2;i+=2)
            {
                //alt is skill name
                var skillName=skillNodes[i].Attributes["alt"].Value;
                var skillLevel = Int32.Parse(levelRegex.Match(skillNodes[i + 1].Attributes["src"].Value).Groups["level"].Value);
                skillList.Add(new SkillRequirement(){
                    Name=skillName,
                    Level=skillLevel
                });
            }
            return skillList;
        }
    }
}

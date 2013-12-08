using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using Dol.Base;

namespace dolspider.Spiders.ENorth.Quest.Handlers
{
    public class SkillHandler
    {
        private static Regex levelRegex = new Regex(@"http://dols.enorth.com.cn/jpg/num/(?'level'\d*).gif");
       //         <img alt="生态调查" src="http://dols.enorth.com.cn/jpg\skill\stdc.gif"><img alt="" src="http://dols.enorth.com.cn/jpg/num/5.gif"><img alt="生物学" src="http://dols.enorth.com.cn/jpg\skill\swx.gif"><img alt="" src="http://dols.enorth.com.cn/jpg/num/5.gif">

        public static IList<SkillRequirement> ParseSkill(HtmlNode skillNode)
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
            for (int i=0;i<skillNodes.Count;i+=2)
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

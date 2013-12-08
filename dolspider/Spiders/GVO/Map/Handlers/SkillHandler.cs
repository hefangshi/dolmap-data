using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using Dol.Base;

namespace dolspider.Spiders.GVO.Map.Handlers
{
    public class SkillHandler
    {

        public static IList<SkillRequirement> ParseSkill(HtmlNode skillNode)
        {
            var skillList = new List<SkillRequirement>();
            var skillNodes = skillNode.SelectNodes("img");
            if (skillNodes == null)
                return skillList;
            for (int i = 0; i < skillNodes.Count; i++)
            {
                //alt is skill name
                var skillName = skillNodes[i].Attributes["alt"].Value;
                int skillLevel = 0;
                Int32.TryParse(skillNodes[i].NextSibling.NextSibling.InnerText, out skillLevel);
                skillList.Add(new SkillRequirement()
                {
                    Name = skillName,
                    Level = skillLevel
                });
            }
            return skillList;
        }
    }
}

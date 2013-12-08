using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dol.Base;
using System.Text.RegularExpressions;

namespace QuestTransFix
{
    public class SimQuestFinder
    {
        private Regex link = new Regex("<a.*</a>");
        private string[] compareSkill=new string[] {"开锁","探索","生态调查","视认","地理学","宗教学","生物学","考古学","财宝鉴定","美术"};
        public List<KeyValuePair<int, Quest>> FindSim(Quest target, IList<Quest> questList)
        {
            var result=new SortedList<int, Quest>();
            questList.All(quest =>
            {
                var value = GetSimValue(target, quest);
                if (value == Int32.MaxValue)
                    return true;
                while (result.ContainsKey(value) == true)
                    value += 1;
                result.Add(value, quest);
                if (value == Int32.MinValue)
                    return false;
                return true;
            });
            return result.OrderBy(x => x.Key).Take(10).ToList();
        }

        private int GetSimValue(Quest target, Quest template)
        {
            int sim = 1;
            //首先比对名称，如果名称一致则相似度为0
            if (target.Name == template.Name)
                return -5000;
            //比对接受任务地点
            if (target.FromCityList.Count != template.FromCityList.Count)
                return Int32.MaxValue;
            foreach (var city in target.FromCityList)
            {
                if (!template.FromCityList.Contains(city))
                    return Int32.MaxValue;
            }
            if (target.Type == 0 && template.Star != target.Star)
                return Int32.MaxValue;
            if ((target.Discovery == null && template.Discovery != null) ||
                (target.Discovery != null && template.Discovery == null))
            {
                return Int32.MaxValue;
            }
            //对比技能是否有区别
            var comparedSkill = 0;
            foreach (string skill in compareSkill)
            {
                var fromSkill = target.Skill.Where(x => x.Name == skill).FirstOrDefault();
                var toSkill = template.Skill.Where(x => x.Name == skill).FirstOrDefault();
                if ((fromSkill == null && toSkill != null)||
                    (fromSkill != null && toSkill == null))
                {
                    return Int32.MaxValue;
                }
                else if (fromSkill != null && toSkill != null)
                {
                    if (fromSkill.Level != toSkill.Level)
                        return Int32.MaxValue;
                    comparedSkill++;
                }
            }
            //对比发现物
            if (target.DiscoveryType == template.DiscoveryType && target.DiscoveryLevel == template.DiscoveryLevel)
                sim -= 4;
            sim -= comparedSkill * 3;
            //查看路径的相似度
            sim += EditDistance.getEditDistance(target.RoutesDic.Aggregate("", (seed, pair) =>
            {
                return seed += "," + pair.Value;
            }), template.RoutesDic.Aggregate("", (seed, pair) =>
            {
                return seed += "," + pair.Value;
            }));
            sim += EditDistance.getEditDistance(formatContent(target.Content), formatContent(template.Content)) / 20;
            sim += EditDistance.getEditDistance(target.Discovery, template.Discovery);
            //查看任务名称的相似度
            sim += EditDistance.getEditDistance(specialName(target.Name), specialName(template.Name));
            //查看任务前置后续相似度
            //if (target.PreQuestID.Count == template.PreQuestID.Count && template.PreQuestID.Count != 0)
            //    sim -= 1;
            //if (target.FollowQuestID.Count == template.FollowQuestID.Count && template.FollowQuestID.Count != 0)
            //    sim -= 1;
            //if (target.PreQuestID.Count == template.PreQuestID.Count && template.PreQuestID.Count != 0 &&
            //    target.FollowQuestID.Count == template.FollowQuestID.Count && template.FollowQuestID.Count != 0)
            //    sim -= 1;
            return sim;
        }

        private string specialName(string name)
        {
            return name.Replace("达文西", "达·芬奇");
        }

        private string formatContent(string content)
        {
            content = link.Replace(content, "");
            return content.Replace("【", "").Replace("】", "").Replace("*1", "").Replace("*", "次");
        }
    }
}

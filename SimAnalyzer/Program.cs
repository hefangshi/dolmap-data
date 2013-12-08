using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DolSearch.LD;
using Dol.Base.DataModel;
using System.Diagnostics;
using Dol.Base;

namespace SimAnalyzer
{
    class Program
    {

        private static string printRoute(string[] value)
        {
            return value.Aggregate((total, now) =>
            {
                return total + "," + now;
            });
        }

        private static IEnumerable<Sim> Compare(Quest fromQuest, string[] fromRoute, IList<Quest> questDB)
        {
            DolRoute EL = new DolRoute();
            var simList = new List<Sim>();
            questDB.All(quest =>
            {
                if (quest.ID == fromQuest.ID)
                    return true;
                if (quest.RouteForSim.Count != 0)
                {
                    var min = quest.RouteForSim.Min(route =>
                    {
                        if (route.Count < 2)
                            return 2001;
                        var value = EL.Compute(fromRoute, route.ToArray());
                        return value;
                    });
                    if (min < 2000)
                    {
                        simList.Add(new Sim()
                        {
                            CompareID = quest.ID,
                            QuestID = fromQuest.ID,
                            Value = min,
                            StartCity = EL.GetCityID(fromRoute[0])
                        });
                    }
                }
                return true;
            });
            return simList.OrderBy(sim => sim.Value).Take(300);
        }

        static void Main(string[] args)
        {
            var now = DateTime.Now;
            QuestDM dm = new QuestDM();
            SimDM simDM = new SimDM();
            var questList = dm.Load();
            var nowCount = 0;
            questList.All(quest =>
            {
                nowCount++;
                if (quest.RouteForSim.Count != 0)
                {
                    var sw = new Stopwatch();
                    sw.Start();
                    var count = 0;
                    quest.RouteForSim.All(route =>
                    {
                        if (route.Count < 2)
                            return true;
                        var simList = Compare(quest, route.ToArray(), questList);
                        simDM.Save(simList);
                        count += simList.Count();
                        return true;
                    });
                    Console.Out.WriteLine("[" + nowCount + "/" + questList.Count + "]   Name:" + quest.Name + "," + "  Time:" + sw.ElapsedMilliseconds + " 匹配任务：" + count);
                    sw.Restart();
                }
                return true;
            });
            Console.Out.WriteLine("处理完成，总耗时"+(DateTime.Now-now).TotalMinutes+"min");
        }
    }
}

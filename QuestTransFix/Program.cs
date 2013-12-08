using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dol.Base.DataModel;
using System.Diagnostics;
using Dol.Base;
using System.IO;
using MySql.Data.MySqlClient;

namespace QuestTransFix
{
    class Program
    {
        static void Main(string[] args)
        {
            var stream = new StreamReader(@"D:\Code\dolspider\QuestTransFix\bin\x86\Debug\haha.csv", Encoding.Default);
            var line = stream.ReadLine();
            using (MySqlConnection conn = new MySqlConnection("server = localhost; user id = root; password = ; database = test"))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "set character set 'utf8'";
                cmd.ExecuteNonQuery();
                QuestDM dm=new QuestDM();
                var list = dm.Load();
                foreach(Quest quest in list)
                {
                    var namePY = pinyin.GetAllFirstPinyin(quest.Name);
                    var id = quest.ID;
                    Trace.WriteLine(namePY + ":" + quest.Name);
                    var query = "update test.quest set for_short=?forshort where id=?id";
                    cmd = conn.CreateCommand();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("?id", id);
                    cmd.Parameters.AddWithValue("?forshort", namePY);
                    cmd.ExecuteNonQuery();
                }
            }
            return;

            //var stream = new StreamReader(@"D:\Code\dolspider\QuestTransFix\bin\x86\Debug\haha.csv",Encoding.Default);
            //var line = stream.ReadLine();
            //using (MySqlConnection conn = new MySqlConnection("server = localhost; user id = root; password = ; database = test"))
            //{
            //    conn.Open();
            //    var cmd = conn.CreateCommand();
            //    cmd.CommandText = "set character set 'utf8'";
            //    cmd.ExecuteNonQuery();
            //    while (line != null)
            //    {
            //        var split = line.Split(',');
            //        var id = split[0];
            //        var toName = split[2];
            //        var oldName = split[1];
            //        var namePY = pinyin.GetAllFirstPinyin(toName);
            //        var query = "update test.quest set name=?name,for_short=?forshort where oldname=?oldName";
            //        cmd = conn.CreateCommand();
            //        cmd.CommandText = query;
            //        cmd.Parameters.AddWithValue("?oldName", oldName);
            //        cmd.Parameters.AddWithValue("?name", toName);
            //        cmd.Parameters.AddWithValue("?forshort", namePY);
            //        cmd.ExecuteNonQuery();
            //        line = stream.ReadLine();
            //    }
            //}

            //QuestDM dm = new QuestDM();
            //QuestEnDM en_dm = new QuestEnDM();
            //var questList = dm.Load();
            //var questEnList = en_dm.Load().Where(x => !x.Name.StartsWith("发送") && x.Name != "野狗的地图" && x.Name != "星花的地图").ToList();
            //SimQuestFinder finder = new SimQuestFinder();
            //int found = 0;
            //int notfound = 0;
            //int noneed = 0;
            //int wrongFound = 0;
            //Dictionary<string, string> translate = new Dictionary<string, string>();
            //questList.All(quest =>
            //{
            //    var foundQuestList = finder.FindSim(quest, questEnList).OrderBy(x => x.Key);
            //    if (foundQuestList.Count() == 0)
            //    {
            //        notfound++;
            //        return true;
            //    }
            //    foreach (KeyValuePair<int, Quest> toQuest in foundQuestList)
            //    {
            //        var sim = toQuest.Key;
            //        var toName = toQuest.Value.Name;
            //        var fromName = quest.Name;
            //        if (sim < -1000)
            //            noneed++;
            //        else
            //        {
            //            if (sim < 2000)
            //            {
            //                if ((translate.ContainsKey(fromName) == false || translate[fromName] != toName) && translate.ContainsValue(toName) == true)
            //                    continue;
            //                if (translate.ContainsKey(fromName) == false)
            //                    translate.Add(fromName, toName);
            //                if (toName.EndsWith("地图") && sim > 6)
            //                    sim += 30;
            //                found++;
            //                Console.Out.WriteLine(String.Format("{3},{0},{1},{2}", fromName, toName, sim, found));
            //            }
            //            else
            //            {
            //                wrongFound++;
            //            }
            //        }
            //        break;
            //    }
            //    return true;
            //});
            //Console.Out.WriteLine(String.Format("{0}置信翻译，{1}不确定翻译，{2}未寻找到翻译,{3}无需翻译", found, wrongFound, notfound, noneed));
            //Console.ReadLine();
        }
    }
}

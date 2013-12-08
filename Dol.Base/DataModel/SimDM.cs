using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Dol.Base.DataModel
{
    public class SimDM
    {
        public IList<Sim> Load()
        {
            using (MySqlConnection conn = new MySqlConnection("server = localhost; user id = root; password = ; database = test"))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "set character set 'utf8'";
                cmd.ExecuteNonQuery();
                var cmdText = @"select * from sim";
                cmd = conn.CreateCommand();
                cmd.CommandText = cmdText;
                var reader = cmd.ExecuteReader();
                var cityList = new List<Sim>();
                while (reader.Read())
                {
                    var city = new Sim()
                    {
                        ID = reader.GetInt32("id"),
                        QuestID = reader.GetInt32("quest_id"),
                        CompareID = reader.GetInt32("compare_id"),
                        Value = reader.GetInt32("value"),
                    };
                    cityList.Add(city);
                }
                reader.Close();
                return cityList;
            }
        }

        public void Save(IEnumerable<Sim> simList)
        {
            if (simList.Count() == 0)
                return;
            using (MySqlConnection conn = new MySqlConnection("server = localhost; user id = root; password = ; database = test"))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "set character set 'utf8'";
                cmd.ExecuteNonQuery();
                var cmdText = @"select * from quest_sim";
                cmd = conn.CreateCommand();
                cmd.CommandText = cmdText;
                var quest_sim = @"INSERT INTO quest_sim SET quest_id=?quest_id,compare_id=?compare_id,value=?value,start=?start";
                //路径
                simList.All(sim =>
                {
                    cmd = conn.CreateCommand();
                    cmd.CommandText = quest_sim;
                    cmd.Parameters.AddWithValue("?quest_id", sim.QuestID);
                    cmd.Parameters.AddWithValue("?compare_id", sim.CompareID);
                    cmd.Parameters.AddWithValue("?value", sim.Value);
                    cmd.Parameters.AddWithValue("?start", sim.StartCity);
                    cmd.ExecuteNonQuery();
                    return true;
                });
            }
        }

    }
}

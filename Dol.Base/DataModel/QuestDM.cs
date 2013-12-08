using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Dol.Base.DataModel
{
    public class QuestDM
    {
        public IList<Quest> Load()
        {
            using (MySqlConnection conn = new MySqlConnection("server = localhost; user id = root; password = ; database = test"))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "set character set 'utf8'";
                cmd.ExecuteNonQuery();
                var cmdText = @"select id from quest";
                cmd = conn.CreateCommand();
                cmd.CommandText = cmdText;
                var reader = cmd.ExecuteReader();
                var idList = new List<int>();
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    idList.Add(id);
                }
                reader.Close();
                return idList.Select(id =>
                {
                    return GetQuest(id, conn);
                }).ToList();
            }
        }

        private Quest GetQuest(int id,MySqlConnection conn)
        {
            var quest=new Quest();
            var query = "SELECT name,content,star,type,Discovery,DiscoveryType,DiscoveryLevel FROM quest WHERE id =" + id;
            var cmd = conn.CreateCommand();
            cmd.CommandText = query;
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                quest.Name = reader.GetString(0);
                quest.ID = id;
                quest.Content = reader.GetString(1);
                quest.Star = reader.GetInt32(2);
                quest.Type = reader.GetInt32(3);
                quest.Discovery = reader.IsDBNull(4) ? null : reader.GetString(4);
                quest.DiscoveryType = reader.IsDBNull(5) ? null : reader.GetString(5);
                quest.DiscoveryLevel = reader.GetInt32(6);
            }
            reader.Close();
            query = "SELECT route_type,route,route_index FROM quest_routes  WHERE quest_id =" + id + " ORDER BY route_index";
            conn.CreateCommand();
            cmd.CommandText = query;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var routeType= reader.GetInt32(0);
                var route = reader.GetString(1);
                var route_index = reader.GetInt32(2);
                if (routeType != 0)
                    quest.RoutesDic.Add(route_index, route);
                else
                    quest.FromCityList.Add(route);
            }
            reader.Close();
            query = "SELECT skill,level FROM quest_skills  WHERE quest_id =" + id;
            conn.CreateCommand();
            cmd.CommandText = query;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var skill = reader.GetString(0);
                var level = reader.GetInt32(1);
                quest.Skill.Add(new SkillRequirement()
                {
                    Level = level,
                    Name = skill
                });
            }
            reader.Close();
            return quest;
        }

        public void Insert(Quest quest)
        {
            MySqlConnection conn = new MySqlConnection("server = localhost; user id = root; password = ; database = test");
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "set character set 'utf8'";
            cmd.ExecuteNonQuery();
            var cmdText = @"INSERT INTO quest SET ID=?id,Name=?name,
                                    Star=?star,Exp=?exp,Fame=?fame,Content=?content,
                                    Discovery=?discovery,DiscoveryID=?discoveryid,DiscoveryLevel=?discoverylevel,DiscoveryType=?discoveryType,
                                    AwardItem=?awarditem,PreFound=?preFound,for_short=?forshort,DiscoveryExp=?discoveryexp,Chapter=?Chapter,Type=?Type";
            cmd = conn.CreateCommand();
            cmd.CommandText = cmdText;
            cmd.Parameters.AddWithValue("?id", quest.ID);
            cmd.Parameters.AddWithValue("?name", quest.Name);
            cmd.Parameters.AddWithValue("?star", quest.Star);
            cmd.Parameters.AddWithValue("?exp", quest.Exp);
            cmd.Parameters.AddWithValue("?fame", quest.Fame);
            cmd.Parameters.AddWithValue("?content", quest.Content);
            cmd.Parameters.AddWithValue("?discovery", quest.Discovery);
            cmd.Parameters.AddWithValue("?discoveryid", quest.DiscoveryID);
            cmd.Parameters.AddWithValue("?discoveryType", quest.DiscoveryType);
            cmd.Parameters.AddWithValue("?discoverylevel", quest.DiscoveryLevel);
            cmd.Parameters.AddWithValue("?discoveryexp", quest.DiscoveryExp);
            cmd.Parameters.AddWithValue("?Chapter", quest.Chapter);
            cmd.Parameters.AddWithValue("?awarditem", quest.AwardItem);
            cmd.Parameters.AddWithValue("?forshort", quest.ForShort);
            cmd.Parameters.AddWithValue("?Type", quest.Type);
            cmd.Parameters.AddWithValue("?preFound", quest.PreFoundName.Count > 0 ? quest.PreFoundName.Aggregate((result, now) =>
            {
                return result + "," + now;
            }) : null);
            cmd.ExecuteNonQuery();
            var quest_routes = @"INSERT INTO quest_routes SET quest_id=?quest_id,route_type=?route_type,route_index=?route_index,route=?route";
            //路径
            if (quest.RoutesDic.Count != 0)
            {
                quest.RoutesDic.All(pair =>
                {
                    cmd = conn.CreateCommand();
                    cmd.CommandText = quest_routes;
                    cmd.Parameters.AddWithValue("?quest_id", quest.ID);
                    cmd.Parameters.AddWithValue("?route_type", 1);
                    cmd.Parameters.AddWithValue("?route", pair.Value);
                    cmd.Parameters.AddWithValue("?route_index", pair.Key);
                    cmd.ExecuteNonQuery();
                    return true;
                });
            }
            //起点城市
            if (quest.FromCityList.Count != 0)
            {
                quest.FromCityList.All(name =>
                {
                    cmd = conn.CreateCommand();
                    cmd.CommandText = quest_routes;
                    cmd.Parameters.AddWithValue("?quest_id", quest.ID);
                    cmd.Parameters.AddWithValue("?route_type", 0);
                    cmd.Parameters.AddWithValue("?route", name);
                    cmd.Parameters.AddWithValue("?route_index", -1);
                    cmd.ExecuteNonQuery();
                    return true;
                });
            }
            var quest_relation = @"INSERT INTO quest_relation SET quest_id=?quest_id,relation_id=?relation_id,relation_type=?relation_type";
            //Relation
            if (quest.PreQuestID.Count != 0)
            {
                quest.PreQuestID.All(id =>
                {
                    cmd = conn.CreateCommand();
                    cmd.CommandText = quest_relation;
                    cmd.Parameters.AddWithValue("?quest_id", quest.ID);
                    cmd.Parameters.AddWithValue("?relation_id", id);
                    cmd.Parameters.AddWithValue("?relation_type", 0);
                    cmd.ExecuteNonQuery();
                    return true;
                });
            }
            if (quest.FollowQuestID.Count != 0)
            {
                quest.FollowQuestID.All(id =>
                {
                    cmd = conn.CreateCommand();
                    cmd.CommandText = quest_relation;
                    cmd.Parameters.AddWithValue("?quest_id", quest.ID);
                    cmd.Parameters.AddWithValue("?relation_id", id);
                    cmd.Parameters.AddWithValue("?relation_type", 1);
                    cmd.ExecuteNonQuery();
                    return true;
                });
            }
            //skills
            var quest_skills = "INSERT INTO quest_skills set quest_id=?quest_id,skill=?skill,level=?level";
            if (quest.Skill.Count != 0)
            {
                quest.Skill.All(skill =>
                {
                    cmd = conn.CreateCommand();
                    cmd.CommandText = quest_skills;
                    cmd.Parameters.AddWithValue("?quest_id", quest.ID);
                    cmd.Parameters.AddWithValue("?skill", skill.Name);
                    cmd.Parameters.AddWithValue("?level", skill.Level);
                    cmd.ExecuteNonQuery();
                    return true;
                });
            }
        }
    }
}

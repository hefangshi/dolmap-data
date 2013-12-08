using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Dol.Base.DataModel
{
    public class CityDM
    {
        public IList<City> Load()
        {
            using (MySqlConnection conn = new MySqlConnection("server = localhost; user id = root; password = ; database = test"))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "set character set 'utf8'";
                cmd.ExecuteNonQuery();
                var cmdText = @"select * from dol_city";
                cmd = conn.CreateCommand();
                cmd.CommandText = cmdText;
                var reader = cmd.ExecuteReader();
                var cityList = new List<City>();
                while (reader.Read())
                {
                    var city = new City()
                    {
                        ID = reader.GetInt32("id"),
                        Name = reader.GetString("city_name"),
                        X = reader.GetFloat("x"),
                        Y = reader.GetFloat("y")
                    };
                    cityList.Add(city);
                }
                reader.Close();
                return cityList;
            }
        }
    }
}

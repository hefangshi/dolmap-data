using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Dol.Base
{
    public class Quest
    {
        public Quest()
        {
            RoutesDic = new SortedDictionary<int, string>();
            FromCityList = new List<string>();
            Skill = new List<SkillRequirement>();
            PreQuestID=new List<int>();
            FollowQuestID=new List<int>();
        }

        public int Type
        {
            get;
            set;
        }

        public int DiscoveryExp
        {
            get;
            set;
        }

        public int Chapter
        {
            get;
            set;
        }

        public int ID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int Star
        {
            get;
            set;
        }

        public IList<SkillRequirement> Skill
        {
            get;
            set;
        }

        public int Exp
        {
            get;
            set;
        }

        public int Fame
        {
            get;
            set;
        }

        public string Content
        {
            get;
            set;
        }

        public IList<int> PreQuestID
        {
            get;
            set;
        }

        public IList<string> PreFoundName
        {
            get;
            set;
        }

        public IList<int> FollowQuestID
        {
            get;
            set;
        }

        public string Discovery
        {
            get;
            set;
        }

        public int DiscoveryID
        {
            get;
            set;
        }

        public int DiscoveryLevel
        {
            get;
            set;
        }

        public string DiscoveryType
        {
            get;
            set;
        }

        public string AwardItem
        {
            get;
            set;
        }

        public IList<string> FromCityList
        {
            get;
            set;
        }

        public string ForShort
        {
            get
            {
                return pinyin.GetAllFirstPinyin(Name);
            }
        }

        [JsonIgnore]
        public IList<List<string>> RouteForSim
        {
            get
            {
                return FromCityList.Select(city =>
                {
                    var list = new List<string>();
                    list.Add(city);
                    var last = city;
                    RoutesDic.Values.All(route =>
                    {
                        if (route != last)
                        {
                            list.Add(route);
                            last = route;
                        }
                        return true;
                    });
                    return list;
                }).ToList();
            }
        }

        public IList<string> Routes
        {
            get
            {
                return RoutesDic.Values.ToList();
            }
        }

        [JsonIgnore]
        public SortedDictionary<int, string> RoutesDic
        {
            get;
            set;
        }
    }

    public class SkillRequirement
    {
        public string Name
        {
            get;
            set;
        }

        public int Level
        {
            get;
            set;
        }
    }

}

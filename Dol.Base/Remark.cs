using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dol.Base
{
    public class Remark
    {
        public Remark()
        {
            PreQuestID = new List<int>();
            PreFoundName = new List<string>();
            FollowQuestID = new List<int>();
            FromCityList = new List<string>();
        }

        public int DiscoveryExp
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
        public string DiscoveryType
        {
            get;
            set;
        }

        public int DiscoveryLevel
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
    }
}

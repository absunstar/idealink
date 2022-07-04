using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Entity.Mongo
{
    public class UserProfile : MongoEntityNameBase
    {
        public UserProfile()
        {
            MyCompanies = new List<SubItem>();
        }
        public string Email { get; set; }
        public int Type { get; set; }
        public bool IsEmployerLimitedCompanies { get; set; }
        public List<SubItem> MyCompanies { get; set; }
    }

}

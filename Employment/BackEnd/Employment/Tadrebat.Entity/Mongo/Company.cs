using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Entity.Mongo
{
    public class Company: MongoEntityNameBase
    {
        public Company()
        {
            UserCanAccess = new List<string>();
            Country = new SubItem();
            City = new SubItem();
            Industry = new SubItem();
        }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public DateTime Establish { get; set; }
        public SubItem Industry { get; set; }
        public string About { get; set; }
        public string SocialFacebook { get; set; }
        public string SocialTwitter { get; set; }
        public string SocialLinkedin { get; set; }
        public string SocialGooglePlus { get; set; }
        public SubItem Country { get; set; }
        public SubItem City { get; set; }
        public string Address { get; set; }
        public string CompanyLogo { get; set; }
        public List<string> UserCanAccess { get; set; }
        public bool IsApproved { get; set; }
        public Dictionary<string, string> data { get; set; }
    }
    
}


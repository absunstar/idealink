using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model.Model
{
    public class ModelCompany
    {
        public string _Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string IndustryId { get; set; }
        public DateTime Establish { get; set; }
        public string About { get; set; }
        public string SocialFacebook { get; set; }
        public string SocialTwitter { get; set; }
        public string SocialLinkedin { get; set; }
        public string SocialGooglePlus { get; set; }
        public string CountryId { get; set; }
        public string CityId { get; set; }
        public string Address { get; set; }
        public string CompanyLogo { get; set; }
        public Dictionary<string, string> data { get; set; }
    }
}

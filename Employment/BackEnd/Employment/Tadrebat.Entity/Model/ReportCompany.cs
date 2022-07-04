using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Entity.Model
{
    public class ReportCompany : BaseEntity
    {
        public ReportCompany()
        {
            CreatedAt = DateTime.Now;
        }
        public long Id { get; set; }
        public string CompanyId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name{ get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public DateTime Establish { get; set; }
        public string IndustryId { get; set; }
        public string IndustryName { get; set; }
        public string About { get; set; }
        public string SocialFacebook { get; set; }
        public string SocialTwitter { get; set; }
        public string SocialLinkedin { get; set; }
        public string SocialGooglePlus { get; set; }
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string Address { get; set; }
    }
    
}

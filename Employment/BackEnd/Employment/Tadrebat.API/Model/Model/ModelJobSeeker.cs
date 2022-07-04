using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model.Model
{
    public class ModelJobSeeker
    {
        public string _id { get; set; }
        public string Name { get; set; }
        public int Gender { get; set; }
        public string JobTitle { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string ExperienceId { get; set; }
        public string QualificationId { get; set; }
        public string About { get; set; }
        public string SocialFacebook { get; set; }
        public string SocialTwitter { get; set; }
        public string SocialLinkedin { get; set; }
        public string SocialGooglePlus { get; set; }
        public List<string> Languages { get; set; }
        public string CountryId { get; set; }
        public string CityId { get; set; }
    }
    public class ModelResumeItem
    {
        public string _id {get;set;}
        public string Name { get; set; }
        public string SubTitle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
    }
    public class ModelResumeCertification
    {
        public string _id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public string Description { get; set; }
        public string CertificatePath { get; set; }
    }
}

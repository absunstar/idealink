using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Entity.Mongo
{
    public class JobSeeker : MongoEntityNameBase
    {
        public JobSeeker()
        {
            Experience = new SubItem();
            Qualification = new SubItem();
            Country = new SubItem();
            City = new SubItem();
            Languages = new List<SubItem>();
            Education = new List<ResumeItem>();
            WorkHistory = new List<ResumeItem>();
            ExtraCurricular = new List<ResumeItem>();
            Certification = new List<ResumeCertification>();
            ContactPermission = new List<CanAccessContactInformation>();
        }

        public bool IsProfileComplete { get; set; }
        public string UserId { get; set; }
        public string JobTitle { get; set; }
        public DateTime DOB { get; set; }
        public int Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public SubItem Experience { get; set; }
        public SubItem Qualification { get; set; }
        public string About { get; set; }
        public string SocialFacebook { get; set; }
        public string SocialTwitter { get; set; }
        public string SocialLinkedin { get; set; }
        public string SocialGooglePlus { get; set; }
        public List<SubItem> Languages { get; set; }
        public SubItem Country { get; set; }
        public SubItem City { get; set; }
        public List<ResumeItem> Education { get; set; }
        public List<ResumeItem> WorkHistory { get; set; }
        public List<ResumeItem> ExtraCurricular { get; set; }
        public List<ResumeCertification> Certification { get; set; }
        public string CoverLetterFile { get; set; }
        public string ResumeFile { get; set; }
        public string ProfilePicture { get; set; }
        public List<CanAccessContactInformation> ContactPermission { get; set; }
    }
    public class ResumeItem : MongoEntityNameBase
    {
        public string SubTitle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
    }
    public class ResumeCertification : MongoEntityNameBase
    {
        public DateTime StartDate { get; set; }
        public string Description { get; set; }
        public string CertificatePath { get; set; }
        public string CertificateTadrebatId { get; set; }
    }
    public class CanAccessContactInformation
    {
        public CanAccessContactInformation()
        {
            CreatedAt = DateTime.Now;
            IsApproved = -1;
        }
        public string EmployerId { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

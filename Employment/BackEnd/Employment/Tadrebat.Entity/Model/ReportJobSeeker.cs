using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Entity.Model
{
    public class ReportJobSeeker : BaseEntity
    {
        public ReportJobSeeker()
        {
            CreatedAt = DateTime.Now;
        }
        public long Id { get; set; }
        public string JobSeekerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public DateTime? DOB { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string QualificationId { get; set; }
        public string QualificationName { get; set; }
        public string ExperienceId { get; set; }
        public string ExperienceName { get; set; }
        public string About { get; set; }
        public string SocialFacebook { get; set; }
        public string SocialTwitter { get; set; }
        public string SocialLinkedin { get; set; }
        public string SocialGooglePlus { get; set; }
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
    }
    public class ReportJobSeekerLanguage : BaseEntity
    {
        public ReportJobSeekerLanguage()
        {
            CreatedAt = DateTime.Now;
        }
        public long Id { get; set; }
        public string JobSeekerId { get; set; }
        public long FK_JobSeekerId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string LanguageId { get; set; }
        public string LanguageName { get; set; }
    }
    public class ReportJobSeekerResumeItem : BaseEntity
    {
        public ReportJobSeekerResumeItem()
        {
            CreatedAt = DateTime.Now;
        }
        public long Id { get; set; }
        public string JobSeekerId { get; set; }
        public long FK_JobSeekerId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int FK_ResumeItemTypeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Entity.Model
{
    public class ReportJob: BaseEntity
    {
        public ReportJob()
        {
            CreatedAt = DateTime.Now;
        }
        public long Id { get; set; }
        public string JobId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string Skills { get; set; }
        public string Benefits { get; set; }
        public int Gender { get; set; }
        public DateTime Deadline { get; set; }
        public string JobFieldId { get; set; }
        public string JobFieldName { get; set; }
        public string JobSubFieldId { get; set; }
        public string JobSubFieldName { get; set; }
        public string typeId { get; set; }
        public string typeName { get; set; }
        public string ExperienceId { get; set; }
        public string ExperienceName { get; set; }
        public string IndustryId { get; set; }
        public string IndustryName { get; set; }
        public string QualificationId { get; set; }
        public string QualificationName { get; set; }
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string Address { get; set; }
    }
}

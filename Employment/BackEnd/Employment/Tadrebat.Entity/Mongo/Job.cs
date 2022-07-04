using Employment.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Entity.Mongo
{
    public class Job :MongoEntityNameBase
    {
        public Job()
        {
            Company = new SubItemURL();
            JobField = new SubItem();
            JobSubField = new SubItem();
            Experience = new SubItem();
            Industry = new SubItem();
            Qualification = new SubItem();
            Country = new SubItem();
            City = new SubItem();

            Status = EnumJobStatus.Draft;
        }
        public SubItemURL Company { get; set; }
        public string  Description { get; set; }
        public string Skills { get; set; }
        public string Benefits { get; set; }
        public string Remuneration { get; set; }
        public int Gender { get; set; }
        public EnumJobStatus Status { get; set; }
        public DateTime Deadline { get; set; }
        public SubItem JobField { get; set; }
        public SubItem JobSubField { get; set; }
        public JobType type { get; set; }
        public SubItem Experience { get; set; }
        public SubItem Industry { get; set; }
        public SubItem Qualification { get; set; }
        public SubItem Country { get; set; }
        public SubItem City { get; set; }
        public string Address { get; set; }

        public int ApplicantCount { get; set; }
    }
    public class ReportJobCount
    {
        public SubItemURL Company { get; set; }
        public long Count { get; set; }
    }
}

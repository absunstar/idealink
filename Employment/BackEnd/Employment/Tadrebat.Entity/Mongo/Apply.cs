using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Entity.Mongo
{
    public class Apply : MongoEntityNameBase
    {
        public Apply()
        {
            Job = new ApplySubItem();
            JobSeeker = new ApplySubItem();
        }
        public string Message { get; set; }
        public ApplySubItem Job { get; set; }
        public ApplySubItem JobSeeker { get; set; }
        public bool IsHired { get; set; }
    }
    public class ApplySubItem : MongoEntityNameBase
    {
        public string SubName { get; set; }
        public string URL { get; set; }
        public string EntityId { get; set; }
    }
    public class ReportApply
    {
        public string JobName { get; set; }
        public string JobId { get; set; }
        public long Count { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Entity.Model
{
    public class ReportJobApply : BaseEntity
    {
        public ReportJobApply(string jobId, string jobSeekerId)
        {
            CreatedAt = DateTime.Now;
            JobSeekerId = jobSeekerId;
            JobId = jobId;
        }
        public long Id { get; set; }
        public string JobId { get; set; }
        public string JobSeekerId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Entity.Mongo
{
    public class JobFair : MongoEntityNameBase
    {
        public JobFair()
        {
            Registered = new List<JobFairRegisteration>();
            data = new Dictionary<string, string>();
        }
        public DateTime EventDate { get; set; }
        public string Location { get; set; }
        public Boolean IsOnline { get; set; }
        public string Field { get; set; }
        public string ShortDescription { get; set; }
        public List<JobFairRegisteration> Registered { get; set; }
        public Dictionary<string, string> data { get; set; }
    }
    public class JobFairRegisteration : MongoEntityNameBase
    {
        public string UserId { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public string DOB { get; set; }
        public bool IsAttendance { get; set; }
        public long Code { get; set; }
        public Dictionary<string, string> data { get; set; }
    }
}

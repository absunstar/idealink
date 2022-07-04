using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model.Model
{
    public class ModelJobFair
    {
        public string Name { get; set; }
        public string _id { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; }
        public Boolean IsOnline { get; set; }
        public string Field { get; set; }
        public string ShortDescription { get; set; }

        public Dictionary<string, string> data { get; set; }
    }
    public class ModelJobFairRegisteration
    {
        public string JobFairId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public string DOB { get; set; }
        public bool IsAttendance { get; set; }
        public Dictionary<string, object> data { get; set; }
    }
    public class ModelJobFairAttendance
    {
        [Required]
        public string JobFairId { get; set; }
        [Required]
        public long Code { get; set; }
    }
}

using Employment.Entity.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model.Response
{
    public class ResponseJob : ResponseBase
    {
        public ResponseJob()
        {
            Company = new ResponseSubItemURL();
            JobField = new ResponseSubItem();
            JobSubField = new ResponseSubItem();
            Experience = new ResponseSubItem();
            Industry = new ResponseSubItem();
            Qualification = new ResponseSubItem();
            Country = new ResponseSubItem();
            City = new ResponseSubItem();
        }
        public ResponseSubItemURL Company { get; set; }
        public string Description { get; set; }
        public string Skills { get; set; }
        public string Benefits { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public int Gender { get; set; }
        public string Remuneration { get; set; }
        public DateTime Deadline { get; set; }
        public ResponseSubItem JobField { get; set; }
        public ResponseSubItem JobSubField { get; set; }
        public ResponseSubItem Experience { get; set; }
        public ResponseSubItem Industry { get; set; }
        public ResponseSubItem Qualification { get; set; }
        public ResponseSubItem Country { get; set; }
        public ResponseSubItem City { get; set; }
        public string Address { get; set; }
        public int ApplicantCount { get; set; }
        public override void Map(object model)
        {
            var obj = (Job)model;

            Name = obj.Name;
            _id = obj._id;
            IsActive = obj.IsActive.GetValueOrDefault();
            Company = new ResponseSubItemURL( obj.Company._id, obj.Company.Name, obj.Company.URL);
            Description = obj.Description;
            Skills = obj.Skills;
            Benefits = obj.Benefits;
            Status =(int) obj.Status;
            Type = (int)obj.type;
            Deadline = obj.Deadline;
            JobField =  new ResponseSubItem(obj.JobField._id, obj.JobField.Name);
            JobSubField =  new ResponseSubItem(obj.JobSubField._id, obj.JobSubField.Name);
            Experience =  new ResponseSubItem(obj.Experience._id, obj.Experience.Name);
            Industry = new ResponseSubItem( obj.Industry._id, obj.Industry.Name);
            Qualification =  new ResponseSubItem(obj.Qualification._id, obj.Qualification.Name);
            Country = new ResponseSubItem( obj.Country._id, obj.Country.Name);
            City = new ResponseSubItem( obj.City._id, obj.City.Name);
            Address = obj.Address;
            CreatedAt = obj.CreatedAt;
            Gender = obj.Gender;
            ApplicantCount = obj.ApplicantCount;
            Remuneration = obj.Remuneration;
        }
    }
}

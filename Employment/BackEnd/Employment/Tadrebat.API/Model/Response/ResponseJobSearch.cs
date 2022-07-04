using Employment.Entity.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model.Response
{
    public class ResponseJobSearch : ResponseBase
    {

        public string Company { get; set; }



        public string Status { get; set; }



        public int Gender { get; set; }
        public string JobField { get; set; }
        public string JobSubField { get; set; }
        public string Experience { get; set; }
        public string Industry { get; set; }
        public string Qualification { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public override void Map(object model)
        {
            var obj = (JobSearch)model;



            Name = obj.Name;
            _id = obj.id;
            IsActive = obj.IsActive;
            Company = obj.Company;
            Status = obj.Status;



            JobField = obj.JobField;
            JobSubField = obj.JobSubField;
            Experience = obj.Experience;
            Industry = obj.Industry;
            Qualification = obj.Qualification;
            Country = obj.Country;
            City = obj.City;
            Gender = obj.Gender;
        }
    }
}

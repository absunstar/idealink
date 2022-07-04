using Employment.Entity.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model.Response
{
    public class ResponseJobValidation : ResponseBase
    {



        public string Company { get; set; }



        public string JobField { get; set; }




        public override void Map(object model)
        {
            var obj = (JobSearch)model;



            Name = obj.Name;
            _id = obj.id;
            IsActive = obj.IsActive;
            Company = obj.Company;
            JobField = obj.JobField;

        }
    }
}

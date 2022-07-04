using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Entity.Mongo
{
   public class JobSearch
    {
        
            public string  CompanyId { get; set; }
            public int search_job { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
            public string Name { get; set; }
            public string Company { get; set; }
            public string JobField { get; set; }
            public string JobSubField { get; set; }
            public string Experience { get; set; }
            public string Industry { get; set; }
            public string Qualification { get; set; }
            public string Country { get; set; }
            public string City { get; set; }
            public bool IsActive { get; set; } = true;
            public string Status { get; set; } = "Approved";
            public int Gender { get; set; }



        
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Tadrebat.Entity.Mongo
{
    public class MongoEntityBase
    {
        public MongoEntityBase()
        {
            _id = ObjectId.GenerateNewId().ToString();
            IsActive = true;
            CreatedAt = DateTime.Now;
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [Display(Name="Id")]
        public string _id { get; set; }

        public bool? IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public void GenerateId()
        {
            _id = ObjectId.GenerateNewId().ToString();
        }
    }
    public class MongoEntityNameBase : MongoEntityBase
    {
        public MongoEntityNameBase()
        {

        }
        public string Name { get; set; }
    }
}

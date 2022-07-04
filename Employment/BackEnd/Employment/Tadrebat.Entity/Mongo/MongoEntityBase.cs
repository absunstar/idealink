using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization;

namespace Employment.Entity.Mongo
{
    public class MongoEntityBase
    {
        public MongoEntityBase()
        {
            _id = ObjectId.GenerateNewId().ToString();
            IsActive = true;
            CreatedAt = DateTime.Now.Date;
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [Display(Name="Id")]
        public string _id { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? IsActive { get; set; }
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
        public string Name2 { get; set; }
    }
    public class SubItem
    {
        public SubItem()
        {

        }
        public SubItem(string id, string name)
        {
            _id = id;
            Name = name;
        }
        public string _id { get; set; }
        public string Name { get; set; }
    }
    public class SubItemURL : SubItem
    {
        public string URL { get; set; }
    }
    public class SubItemActive : SubItem
    {
        public bool IsApproved { get; set; }
    }
}

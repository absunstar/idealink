using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tadrebat.Entity.Mongo
{
    public class TrainingCategory : MongoEntityBase
    {
        public TrainingCategory()
        {
            Course = new List<Course>();
        }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public string Name3 { get; set; }
        public string TrainingTypeId { get; set; }

        public List<Course> Course { get; set; }

    }
}

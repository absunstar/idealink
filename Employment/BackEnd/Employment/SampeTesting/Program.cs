using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using Employment.MongoDB.Interface;

namespace SampeTesting
{
    class Program
    {
        static void Main(string[] args)
        {

            var client = new MongoClient("mongodb://localhost/TestingDB");
            var db = client.GetDatabase("TestingDB");
            var collection = db.GetCollection<BsonDocument>("grades");

            var doc = new Department();
            doc.Name = "Science";

            collection.InsertOne(doc.ToBsonDocument());

            var lst = collection.Find(new BsonDocument());
            var lst1 = collection.Find(new BsonDocument()).ToList();
            var obj = collection.Find(new BsonDocument()).FirstOrDefault();
            
            var d = BsonSerializer.Deserialize<Department>(obj);
            Console.WriteLine("Hello World!");
        }
    }
    public class Department
    {
        public Department()
        {
            _id = new BsonObjectId(ObjectId.GenerateNewId());
        }
        public BsonObjectId _id { get; set; }
        
        public string Name { get; set; }
    }
}

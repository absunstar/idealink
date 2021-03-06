using System;
using MongoDB.Driver;
using Tadrebat.MongoDB.Interface;

namespace Tadrebat.Mongo.DataLayer
{
    public class MongoDBContext : IMongoDBContext
    {
        private IMongoClient mongoClient;
        private string DBName;
        public IMongoDatabase mongoDB
        {
            get
            {
                return mongoClient.GetDatabase(DBName);
            }    
        }
        public MongoDBContext(string ConnectionString, string _DBName)
        {
            mongoClient = new MongoClient(ConnectionString);
            DBName = _DBName;
        }

    }
}

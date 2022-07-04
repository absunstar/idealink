using Employment.Entity.Mongo;
using Employment.MongoDB.Interface;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Mongo.DataLayer
{
    public class DBJobFair : DBRepositoryMongo<JobFair>, IDBJobFair
    {
        private static string _pDBCollectionName = "JobFair";
        public DBJobFair(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<JobFair>(_DBCollectionName);
        }
        public async Task<bool> RegisterUser(string JobFairId, JobFairRegisteration obj)
        {
            if (obj == null)
                return false;

            var filter = Builders<JobFair>.Filter.Where(x => x._id == JobFairId);
            var update = Builders<JobFair>.Update.Push("Registered", obj);

            await _mongoCollection.UpdateOneAsync(filter, update);

            return true;
        }
    }
}
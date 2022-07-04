using Employment.Entity.Mongo;
using Employment.MongoDB.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Mongo.DataLayer
{
    public class DBYearsOfExperience : DBRepositoryMongo<YearsOfExperience>, IDBYearsOfExperience
    {
        private static string _pDBCollectionName = "YearsOfExperience";
        public DBYearsOfExperience(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<YearsOfExperience>(_DBCollectionName);
        }
    }
}

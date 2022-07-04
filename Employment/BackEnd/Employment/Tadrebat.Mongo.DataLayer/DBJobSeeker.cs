using Employment.Entity.Mongo;
using Employment.MongoDB.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Mongo.DataLayer
{
    public class DBJobSeeker : DBRepositoryMongo<JobSeeker>, IDBJobSeeker
    {
        private static string _pDBCollectionName = "JobSeeker";
        public DBJobSeeker(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<JobSeeker>(_DBCollectionName);
        }
    }
}

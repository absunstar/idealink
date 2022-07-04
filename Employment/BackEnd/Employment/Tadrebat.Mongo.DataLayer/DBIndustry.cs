using Employment.Entity.Mongo;
using Employment.MongoDB.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Mongo.DataLayer
{
    public class DBIndustry : DBRepositoryMongo<Industry>, IDBIndustry
    {
        private static string _pDBCollectionName = "Industry";
        public DBIndustry(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<Industry>(_DBCollectionName);
        }
    }
}

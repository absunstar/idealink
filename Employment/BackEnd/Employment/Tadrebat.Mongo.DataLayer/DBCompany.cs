using Employment.Entity.Mongo;
using Employment.MongoDB.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Mongo.DataLayer
{
    public class DBCompany : DBRepositoryMongo<Company>, IDBCompany
    {
        private static string _pDBCollectionName = "Company";
        public DBCompany(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<Company>(_DBCollectionName);
        }
    }
}

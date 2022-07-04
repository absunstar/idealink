using Employment.Entity.Mongo;
using Employment.MongoDB.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Mongo.DataLayer
{
    
    public class DBQualification : DBRepositoryMongo<Qualification>, IDBQualification
    {
        private static string _pDBCollectionName = "Qualification";
        public DBQualification(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<Qualification>(_DBCollectionName);
        }
    }
}

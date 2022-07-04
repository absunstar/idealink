using System;
using System.Collections.Generic;
using System.Text;
using Tadrebat.Entity.Mongo;
using Tadrebat.MongoDB.Interface;

namespace Tadrebat.Mongo.DataLayer
{
    public class DBCertificate : DBRepositoryMongo<Certificate>, IDBCertificate
    {
        private static string _pDBCollectionName = "Certificate";
        public DBCertificate(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<Certificate>(_DBCollectionName);
        }
    }
}
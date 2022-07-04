using Employment.Entity.Mongo;
using Employment.MongoDB.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Mongo.DataLayer
{
    public class DBLanguages : DBRepositoryMongo<Languages>, IDBLanguages
    {
        private static string _pDBCollectionName = "Languages";
        public DBLanguages(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<Languages>(_DBCollectionName);
        }
    }
}

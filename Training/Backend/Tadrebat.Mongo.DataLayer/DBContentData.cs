using System;
using System.Collections.Generic;
using System.Text;
using Tadrebat.Entity.Mongo;
using Tadrebat.MongoDB.Interface;

namespace Tadrebat.Mongo.DataLayer
{
    public class DBContentData : DBRepositoryMongo<ContentData>, IDBContentData
    {
        private static string _pDBCollectionName = "ContentData";
        public DBContentData(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<ContentData>(_DBCollectionName);
        }
    }
}

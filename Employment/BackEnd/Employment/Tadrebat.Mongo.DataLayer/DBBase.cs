using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using Employment.MongoDB.Interface;
using MongoDB.Bson;

namespace Employment.Mongo.DataLayer
{
    public class DBBase
    {
        protected IMongoDatabase _mongoDB;
        protected string _DBCollectionName;
        
        public DBBase(IMongoDBContext mongoDBContext, string DBCollectionName)
        {
            if (string.IsNullOrEmpty(DBCollectionName))
                throw new Exception("DBCollectionName is not set");

            _DBCollectionName = DBCollectionName;
            _mongoDB = mongoDBContext.mongoDB;
        }
    }
}

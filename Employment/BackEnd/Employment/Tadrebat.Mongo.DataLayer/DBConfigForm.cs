using Employment.Entity.Mongo;
using Employment.MongoDB.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Mongo.DataLayer
{
    public class DBConfigForm : DBRepositoryMongo<ConfigForm>, IDBConfigForm
    {
        private static string _pDBCollectionName = "ConfigForm";
    public DBConfigForm(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
    {
        _mongoCollection = _mongoDB.GetCollection<ConfigForm>(_DBCollectionName);
    }

    }
}
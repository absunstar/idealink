using System;
using System.Collections.Generic;
using System.Text;
using Tadrebat.Entity.Mongo;
using Tadrebat.MongoDB.Interface;

namespace Tadrebat.Mongo.DataLayer
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
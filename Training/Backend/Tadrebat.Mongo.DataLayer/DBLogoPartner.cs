using System;
using System.Collections.Generic;
using System.Text;
using Tadrebat.Entity.Mongo;
using Tadrebat.MongoDB.Interface;

namespace Tadrebat.Mongo.DataLayer
{
    public class DBLogoPartner : DBRepositoryMongo<LogoPartner>, IDBLogoPartner
    {
        private static string _pDBCollectionName = "LogoPartner";
        public DBLogoPartner(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<LogoPartner>(_DBCollectionName);
        }
    }
}


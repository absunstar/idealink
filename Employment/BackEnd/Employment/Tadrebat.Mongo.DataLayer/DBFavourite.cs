using Employment.Entity.Mongo;
using Employment.MongoDB.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Mongo.DataLayer
{
    public class DBFavourite : DBRepositoryMongo<Favourite>, IDBFavourite
    {
        private static string _pDBCollectionName = "Favourite";
        public DBFavourite(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<Favourite>(_DBCollectionName);
        }
    }
}

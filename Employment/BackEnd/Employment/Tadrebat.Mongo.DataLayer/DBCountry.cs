using Employment.Entity.Mongo;
using Employment.MongoDB.Interface;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Mongo.DataLayer
{
     public class DBCountry : DBRepositoryMongo<Country>, IDBCountry
    {
        private static string _pDBCollectionName = "Country";
        public DBCountry(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<Country>(_DBCollectionName);
        }
        public async Task<bool> SubCreate(string MainId, string Name)
        {
            var obj = await GetById(MainId);
            if (obj == null)
                return false;

            var details = new City();
            details.Name = Name;

            var filter = Builders<Country>.Filter.Where(x => x._id == MainId);
            var update = Builders<Country>.Update.Push("subItems", details);

            await _mongoCollection.UpdateOneAsync(filter, update);

            return true;
        }
        public async Task<bool> SubUpdate(string MainId, string SubId, string Name)
        {
            var TrainingCategory = await GetById(MainId);
            if (TrainingCategory == null)
                return false;

            var filter = Builders<Country>.Filter.ElemMatch(y => y.subItems, x => x._id == SubId);
            var update = Builders<Country>.Update.Set(x => x.subItems[-1].Name, Name);

            await _mongoCollection.UpdateOneAsync(filter, update);

            return true;
        }
        public async Task<bool> SubActivate(string MainId, string Id)
        {
            return await SubStatusUpdate(MainId, Id, true);
        }
        public async Task<bool> SubDeActivate(string MainId, string Id)
        {
            return await SubStatusUpdate(MainId, Id, false);
        }
        private async Task<bool> SubStatusUpdate(string MainId, string Id, bool status)
        {
            var TrainingCategory = await GetById(MainId);
            if (TrainingCategory == null)
                return false;

            var filter = Builders<Country>.Filter.ElemMatch(y => y.subItems, x => x._id == Id);
            var update = Builders<Country>.Update.Set(x => x.subItems[-1].IsActive, status);

            await _mongoCollection.UpdateOneAsync(filter, update);

            return true;
        }
    }
}

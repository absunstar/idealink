using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;
using Tadrebat.MongoDB.Interface;

namespace Tadrebat.Mongo.DataLayer
{
    public class DBCity : DBRepositoryMongo<City>, IDBCity
    { 
        private static string _pDBCollectionName = "City";
        public DBCity(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<City>(_DBCollectionName);
        }
        public async Task UpdateName(string Id, string Name)
        {
            var update = Builders<City>.Update.Set(s => s.Name, Name);
            await UpdateAsync(Id, update);
        }
        public async Task<MongoResultPaged<City>> GetPaged(int CurrentPage = 1, int PageSize = 15)
        {
            var filter = Builders<City>.Filter.Where(x => x.IsActive == true);
            var sort = Builders<City>.Sort.Descending(x => x._id);
            return await GetPaged(filter, sort, CurrentPage, PageSize);
        }
        public async Task<MongoResultPaged<City>> CityListAllSearch(string filterText, int CurrentPage = 1, int PageSize = 15)
        {
            var filter = Builders<City>.Filter.Where(x => x.Name.ToLower().Contains(filterText.ToLower()));
            var sort = Builders<City>.Sort.Descending(x => x._id);
            return await GetPaged(filter, sort, CurrentPage, PageSize);
        }
        public async Task<bool> AreaCreate(string CityId, string Name)
        {
            var TrainingCategory = await GetById(CityId);
            if (TrainingCategory == null)
                return false;

            var details = new Area();
            details.Name = Name;

            var filter = Builders<City>.Filter.Where(x => x._id == CityId);
            var update = Builders<City>.Update.Push("areas", details);

            await _mongoCollection.UpdateOneAsync(filter, update);

            return true;
        }
        public async Task<bool> AreaUpdate(string CityId, string AreaId, string Name)
        {
            var TrainingCategory = await GetById(CityId);
            if (TrainingCategory == null)
                return false;

            var filter = Builders<City>.Filter.ElemMatch(y => y.areas, x => x._id == AreaId);
            var update = Builders<City>.Update.Set(x => x.areas[-1].Name, Name);

            await _mongoCollection.UpdateOneAsync(filter, update);

            return true;
        }
        public async Task<bool> AreaActivate(string CityId, string Id)
        {
            return await AreaStatusUpdate(CityId, Id, true);
        }
        public async Task<bool> AreaDeActivate(string CityId, string Id)
        {
            return await AreaStatusUpdate(CityId, Id, false);
        }
        private async Task<bool> AreaStatusUpdate(string CityId, string Id, bool status)
        {
            var TrainingCategory = await GetById(CityId);
            if (TrainingCategory == null)
                return false;

            var filter = Builders<City>.Filter.ElemMatch(y => y.areas, x => x._id == Id);
            var update = Builders<City>.Update.Set(x => x.areas[-1].IsActive, status);

            await _mongoCollection.UpdateOneAsync(filter, update);

            return true;
        }
    }
}

using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;
using Tadrebat.MongoDB.Interface;

namespace Tadrebat.Mongo.DataLayer
{
    public class DBNGOType : DBRepositoryMongo<NGOType>, IDBNGOType
    {
        private static string _pDBCollectionName = "NGOType";
        public DBNGOType(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<NGOType>(_DBCollectionName);
        }
        public async Task UpdateName(string Id, string Name)
        {
            var update = Builders<NGOType>.Update.Set(s => s.Name, Name);
            await UpdateAsync(Id, update);
        }
        public async Task<MongoResultPaged<NGOType>> GetPaged(int CurrentPage = 1, int PageSize = 15)
        {
            var filter = Builders<NGOType>.Filter.Where(x => x.IsActive == true);
            var sort = Builders<NGOType>.Sort.Descending(x => x._id);
            return await GetPaged(filter, sort, CurrentPage, PageSize);
        }
        public async Task<MongoResultPaged<NGOType>> NGOTypeListAllSearch(string filterText, int CurrentPage = 1, int PageSize = 15)
        {
            var filter = Builders<NGOType>.Filter.Where(x => x.Name.ToLower().Contains(filterText.ToLower()));
            var sort = Builders<NGOType>.Sort.Descending(x => x._id);
            return await GetPaged(filter, sort, CurrentPage, PageSize);
        }
    }
}

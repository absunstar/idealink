using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;
using Tadrebat.MongoDB.Interface;

namespace Tadrebat.Mongo.DataLayer
{
    public class DBEntityTrainingCenter : DBRepositoryMongo<EntityTrainingCenter>, IDBEntityTrainingCenter
    {
        private static string _pDBCollectionName = "EntityTrainingCenter";
        public DBEntityTrainingCenter(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<EntityTrainingCenter>(_DBCollectionName);
        }
        public async Task<MongoResultPaged<EntityTrainingCenter>> GetPaged(int CurrentPage = 1, int PageSize = 15)
        {
            var filter = Builders<EntityTrainingCenter>.Filter.Where(x => x.IsActive == true);
            var sort = Builders<EntityTrainingCenter>.Sort.Descending(x => x._id);
            return await GetPaged(filter, sort, CurrentPage, PageSize);
        }
        public async Task<MongoResultPaged<EntityTrainingCenter>> ListAllSearch(string filterText, int CurrentPage = 1, int PageSize = 15)
        {
            var filter = Builders<EntityTrainingCenter>.Filter.Where(x => x.Name.ToLower().Contains(filterText.ToLower()));
            var sort = Builders<EntityTrainingCenter>.Sort.Descending(x => x._id);
            return await GetPaged(filter, sort, CurrentPage, PageSize);
        }
    }
}

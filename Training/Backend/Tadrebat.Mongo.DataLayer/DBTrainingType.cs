using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;
using Tadrebat.MongoDB.Interface;

namespace Tadrebat.Mongo.DataLayer
{
    public class DBTrainingType : DBRepositoryMongo<TrainingType>, IDBTrainingType
    {
        private static string _pDBCollectionName = "TrainingType";
        public DBTrainingType(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<TrainingType>(_DBCollectionName);
        }
        public async Task UpdateName(string Id, string Name)
        {
            var update = Builders<TrainingType>.Update.Set(s => s.Name, Name);
            await UpdateAsync(Id, update);
        }
        public async Task<MongoResultPaged<TrainingType>> GetPaged(int CurrentPage = 1, int PageSize = 15)
        {
            var filter = Builders<TrainingType>.Filter.Where(x => x.IsActive == true);
            var sort = Builders<TrainingType>.Sort.Descending(x => x._id);
            return await GetPaged(filter, sort, CurrentPage, PageSize);
        }
        public async Task<MongoResultPaged<TrainingType>> TrainingTypeListAllSearch(string filterText, int CurrentPage = 1, int PageSize = 15)
        {
            var filter = Builders<TrainingType>.Filter.Where(x => x.Name.ToLower().Contains(filterText.ToLower()));
            var sort = Builders<TrainingType>.Sort.Descending(x => x._id);
            return await GetPaged(filter, sort, CurrentPage, PageSize);
        }
    }
}

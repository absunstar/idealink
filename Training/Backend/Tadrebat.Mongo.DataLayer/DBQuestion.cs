using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;
using Tadrebat.MongoDB.Interface;

namespace Tadrebat.Mongo.DataLayer
{
    public class DBQuestion : DBRepositoryMongo<Question>, IDBQuestion
    {
        private static string _pDBCollectionName = "Question";
        public DBQuestion(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<Question>(_DBCollectionName);
        }
        public async Task UpdateName(string Id, string Name)
        {
            var update = Builders<Question>.Update.Set(s => s.Name, Name);
            await UpdateAsync(Id, update);
        }
        public async Task<MongoResultPaged<Question>> GetPaged(int CurrentPage = 1, int PageSize = 15)
        {
            var filter = Builders<Question>.Filter.Where(x => x.IsActive == true);
            var sort = Builders<Question>.Sort.Descending(x => x._id);
            return await GetPaged(filter, sort, CurrentPage, PageSize);
        }
        public async Task<MongoResultPaged<Question>> QuestionListAllSearch(string filterText, int CurrentPage = 1, int PageSize = 15)
        {
            var filter = Builders<Question>.Filter.Where(x => x.Name.ToLower().Contains(filterText.ToLower()));
            var sort = Builders<Question>.Sort.Descending(x => x._id);
            return await GetPaged(filter, sort, CurrentPage, PageSize);
        }
    }
}

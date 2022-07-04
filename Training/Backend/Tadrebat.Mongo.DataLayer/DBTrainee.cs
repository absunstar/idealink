using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;
using Tadrebat.MongoDB.Interface;

namespace Tadrebat.Mongo.DataLayer
{
    public class DBTrainee : DBRepositoryMongo<Trainee>, IDBTrainee
    {
        private static string _pDBCollectionName = "Trainee";
        public DBTrainee(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<Trainee>(_DBCollectionName);
        }
        public async Task UpdateName(string Id, string Name)
        {
            var update = Builders<Trainee>.Update.Set(s => s.Name, Name);
            await UpdateAsync(Id, update);
        }
        public async Task<MongoResultPaged<Trainee>> GetPaged(int CurrentPage = 1, int PageSize = 15)
        {
            var filter = Builders<Trainee>.Filter.Where(x => x.IsActive == true);
            var sort = Builders<Trainee>.Sort.Descending(x => x._id);
            return await GetPaged(filter, sort, CurrentPage, PageSize);
        }
        public async Task<MongoResultPaged<Trainee>> ListAllSearch(string filterText, int CurrentPage = 1, int PageSize = 15)
        {
            var filter = Builders<Trainee>.Filter.Where(x => x.Name.ToLower().Contains(filterText.ToLower())
                                            || x.Email.ToLower().Contains(filterText.ToLower())
                                            || x.Mobile.ToLower().Contains(filterText.ToLower())
                                            || x.NationalId.ToLower().Contains(filterText.ToLower()));
            var sort = Builders<Trainee>.Sort.Descending(x => x._id);
            return await GetPaged(filter, sort, CurrentPage, PageSize);
        }
    }
}

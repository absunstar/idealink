using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Employment.Entity.Mongo;
using Employment.Enum;
using Employment.MongoDB.Interface;

namespace Employment.Mongo.DataLayer
{
    public class DBUserProfile : DBRepositoryMongo<UserProfile>, IDBUserProfile
    {
        private static string _pDBCollectionName = "UserProfile";
        public DBUserProfile(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<UserProfile>(_DBCollectionName);
        }
        public async Task<MongoResultPaged<UserProfile>> GetPaged(int CurrentPage = 1, int PageSize = 15)
        {
            var filter = Builders<UserProfile>.Filter.Where(x => x.IsActive == true);
            var sort = Builders<UserProfile>.Sort.Descending(x => x.Name);
            return await GetPaged(filter, sort, CurrentPage, PageSize);
        }
        public async Task<MongoResultPaged<UserProfile>> ListAllSearch(string filterText, int filterType, int CurrentPage = 1, int PageSize = 15)
        {
            var filter = Builders<UserProfile>.Filter.Where(x => x.Name.ToLower().Contains(filterText.ToLower())
                                                            || x.Email.ToLower().Contains(filterText.ToLower()));
            if (filterType != 0)
                filter = filter & Builders<UserProfile>.Filter.Where(x => x.Type == filterType) ;

            var sort = Builders<UserProfile>.Sort.Descending(x => x.CreatedAt);
            return await GetPaged(filter, sort, CurrentPage, PageSize);
        }
    }
}

using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;
using Tadrebat.MongoDB.Interface;

namespace Tadrebat.Mongo.DataLayer
{
    public class DBEntitySubPartner : DBRepositoryMongo<EntitySubPartner>, IDBEntitySubPartner
    {
        private static string _pDBCollectionName = "EntitySubPartner";
        public DBEntitySubPartner(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<EntitySubPartner>(_DBCollectionName);
        }
        public async Task<MongoResultPaged<EntitySubPartner>> GetPaged(int CurrentPage = 1, int PageSize = 15)
        {
            var filter = Builders<EntitySubPartner>.Filter.Where(x => x.IsActive == true);
            var sort = Builders<EntitySubPartner>.Sort.Descending(x => x._id);
            return await GetPaged(filter, sort, CurrentPage, PageSize);
        }
        public async Task<MongoResultPaged<EntitySubPartner>> ListAllSearch(string filterText, string UserId, int CurrentPage = 1, int PageSize = 15)
        {
            var filter = Builders<EntitySubPartner>.Filter.Where(x => x.Name.ToLower().Contains(filterText.ToLower()));
            if(!string.IsNullOrEmpty(UserId))
                filter = filter & Builders<EntitySubPartner>.Filter.Where(x => x.MemberCanAccessIds.Contains(UserId));
            var sort = Builders<EntitySubPartner>.Sort.Descending(x => x._id);
            return await GetPaged(filter, sort, CurrentPage, PageSize);
        }
    }
}

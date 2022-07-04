using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;
using Tadrebat.MongoDB.Interface;

namespace Tadrebat.Mongo.DataLayer
{
    public class DBEntityPartner : DBRepositoryMongo<EntityPartner>, IDBEntityPartner
    {
        private static string _pDBCollectionName = "EntityPartner";
        public DBEntityPartner(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<EntityPartner>(_DBCollectionName);
        }
        //public async Task<bool> AddMember(string ParterId, string UserId)
        //{
        //    var filter = Builders<EntityPartner>.Filter.Where(x => x._id == ParterId);
        //    var update = Builders<EntityPartner>.Update.AddToSet(x => x.MemberIds,UserId);
        //    await _mongoCollection.UpdateOneAsync(filter, update);
        //    return true;
        //}
        //public async Task<bool> RemoveMember(string ParterId, string UserId)
        //{
        //    var filter = Builders<EntityPartner>.Filter.Where(x => x._id == ParterId);
        //    var update = Builders<EntityPartner>.Update.Pull(x => x.MemberIds, UserId);
        //    await _mongoCollection.UpdateOneAsync(filter, update);
        //    return true;
        //}
        public async Task<MongoResultPaged<EntityPartner>> GetPaged(int CurrentPage = 1, int PageSize = 15)
        {
            var filter = Builders<EntityPartner>.Filter.Where(x => x.IsActive == true);
            var sort = Builders<EntityPartner>.Sort.Descending(x => x._id);
            return await GetPaged(filter, sort, CurrentPage, PageSize);
        }
        public async Task<MongoResultPaged<EntityPartner>> ListAllSearch(string filterText, string UserId= "",int CurrentPage = 1, int PageSize = 15)
        {
            var filter = Builders<EntityPartner>.Filter.Where(x => x.Name.ToLower().Contains(filterText.ToLower()));
            var sort = Builders<EntityPartner>.Sort.Descending(x => x._id);
            
            //if userid is empty than it is admin, so list all, other with filter by who can have access
            if(!string.IsNullOrEmpty(UserId))
                filter = filter & Builders<EntityPartner>.Filter.Where(x => x.MemberCanAccessIds.Contains(UserId));

            return await GetPaged(filter, sort, CurrentPage, PageSize);
        }
    }
}

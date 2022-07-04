using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;
using Tadrebat.Enum;
using Tadrebat.MongoDB.Interface;

namespace Tadrebat.Mongo.DataLayer
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
            var sort = Builders<UserProfile>.Sort.Descending(x => x._id);
            return await GetPaged(filter, sort, CurrentPage, PageSize);
        }
        public async Task<MongoResultPaged<UserProfile>> ListAllSearch(string filterText, int filterType, string UserId, EnumUserTypes role, List<int> type, int CurrentPage = 1, int PageSize = 15)
        {
            var filter = Builders<UserProfile>.Filter.Where(x => x.Name.ToLower().Contains(filterText.ToLower())
                                                            || x.Email.ToLower().Contains(filterText.ToLower()));
            if (filterType != 0)
                filter = filter & Builders<UserProfile>.Filter.Where(x => x.Type == filterType);

            if (role != EnumUserTypes.Admin) //not admin, add other search
                filter = filter & Builders<UserProfile>.Filter.Where(x => type.Contains(x.Type));

            if (role != EnumUserTypes.Admin) //not admin, add other search
            {
                var user = await GetById(UserId);
                FilterDefinition<UserProfile> filterpartner = Builders<UserProfile>.Filter.Where(x => x._id != "");
                FilterDefinition<UserProfile> filterSubpartner = Builders<UserProfile>.Filter.Where(x => x._id != "");
                if (user.MyPartnerListIds.Count > 0)
                    filterpartner = Builders<UserProfile>.Filter.AnyIn(x => x.MyPartnerListIds, user.MyPartnerListIds);
                if (user.MySubPartnerListIds.Count > 0)
                    filterSubpartner = Builders<UserProfile>.Filter.AnyIn(x => x.MySubPartnerListIds, user.MySubPartnerListIds);

                filter = filter & (filterpartner | filterSubpartner);
            }
            var sort = Builders<UserProfile>.Sort.Descending(x => x._id);
            return await GetPaged(filter, sort, CurrentPage, PageSize);
        }
    }
}

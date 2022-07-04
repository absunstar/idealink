using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;
using Tadrebat.Enum;

namespace Tadrebat.MongoDB.Interface
{
    public interface IDBUserProfile : IRepositoryMongo<UserProfile>
    {
        Task<MongoResultPaged<UserProfile>> GetPaged(int CurrentPage, int PageSize);
        Task<MongoResultPaged<UserProfile>> ListAllSearch(string filterText, int filterType, string UserId, EnumUserTypes role, List<int> type, int CurrentPage = 1, int PageSize = 15);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Employment.Entity.Mongo;
using Employment.Enum;

namespace Employment.MongoDB.Interface
{
    public interface IDBUserProfile : IRepositoryMongo<UserProfile>
    {
        Task<MongoResultPaged<UserProfile>> GetPaged(int CurrentPage, int PageSize);
        Task<MongoResultPaged<UserProfile>> ListAllSearch(string filterText, int filterType, int CurrentPage = 1, int PageSize = 15);
    }
}

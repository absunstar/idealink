using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;

namespace Tadrebat.MongoDB.Interface
{
    public interface IDBEntitySubPartner : IRepositoryMongo<EntitySubPartner>
    {
        Task<MongoResultPaged<EntitySubPartner>> GetPaged(int CurrentPage, int PageSize);
        Task<MongoResultPaged<EntitySubPartner>> ListAllSearch(string filterText, string UserId, int CurrentPage = 1, int PageSize = 15);
    }
}

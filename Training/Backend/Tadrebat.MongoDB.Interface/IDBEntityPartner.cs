using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;

namespace Tadrebat.MongoDB.Interface
{
    public interface IDBEntityPartner : IRepositoryMongo<EntityPartner>
    {
        //Task<bool> AddMember(string ParterId, string UserId);
        //Task<bool> RemoveMember(string ParterId, string UserId);
        Task<MongoResultPaged<EntityPartner>> GetPaged(int CurrentPage, int PageSize);
        Task<MongoResultPaged<EntityPartner>> ListAllSearch(string filterText, string UserId = "", int CurrentPage = 1, int PageSize = 15);
    }
}

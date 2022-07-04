using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;

namespace Tadrebat.Interface
{
    public interface IServiceRepository<T>
    {
       
        Task<T> GetById(string Id);
        Task<bool> Create(string Name);
        Task<bool> Create(T Obj);
        Task<bool> Update(T obj);
        Task<bool> DeActivate(string Id);
        Task<bool> Activate(string Id);
        Task<List<T>> ListActive();
        Task<MongoResultPaged<T>> ListAll(string filterText, int pageNumber = 1, int PageSize = 15);
        Task<MongoResultPaged<T>> ListAllByUserId(string filterText, string UserId, int pageNumber = 1, int PageSize = 15);
        Task<bool> IsExist(string Id);
    }
}

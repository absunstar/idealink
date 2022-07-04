using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;

namespace Tadrebat.MongoDB.Interface
{
    public interface IDBNGOType: IRepositoryMongo<NGOType>
    {
        Task UpdateName(string Id, string Name);
        Task<MongoResultPaged<NGOType>> GetPaged(int CurrentPage, int PageSize);
        Task<MongoResultPaged<NGOType>> NGOTypeListAllSearch(string filterText, int CurrentPage = 1, int PageSize = 15);
    }
}

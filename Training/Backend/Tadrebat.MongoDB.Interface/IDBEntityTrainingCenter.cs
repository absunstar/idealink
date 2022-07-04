using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;

namespace Tadrebat.MongoDB.Interface
{
    public interface IDBEntityTrainingCenter : IRepositoryMongo<EntityTrainingCenter>
    {
        Task<MongoResultPaged<EntityTrainingCenter>> GetPaged(int CurrentPage, int PageSize);
        Task<MongoResultPaged<EntityTrainingCenter>> ListAllSearch(string filterText, int CurrentPage = 1, int PageSize = 15);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;

namespace Tadrebat.MongoDB.Interface
{
    public interface IDBTrainingType : IRepositoryMongo<TrainingType>
    {
        Task UpdateName(string Id, string Name);
        Task<MongoResultPaged<TrainingType>> GetPaged(int CurrentPage, int PageSize);
        Task<MongoResultPaged<TrainingType>> TrainingTypeListAllSearch(string filterText, int CurrentPage = 1, int PageSize = 15);
    }
}

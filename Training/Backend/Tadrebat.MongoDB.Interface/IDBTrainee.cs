using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;

namespace Tadrebat.MongoDB.Interface
{
    public interface IDBTrainee : IRepositoryMongo<Trainee>
    {
        Task UpdateName(string Id, string Name);
        Task<MongoResultPaged<Trainee>> GetPaged(int CurrentPage, int PageSize);
        Task<MongoResultPaged<Trainee>> ListAllSearch(string filterText, int CurrentPage = 1, int PageSize = 15);
    }
}


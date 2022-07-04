using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;

namespace Tadrebat.MongoDB.Interface
{
    public interface IDBQuestion : IRepositoryMongo<Question>
    {
        Task UpdateName(string Id, string Name);
        Task<MongoResultPaged<Question>> GetPaged(int CurrentPage, int PageSize);
        Task<MongoResultPaged<Question>> QuestionListAllSearch(string filterText, int CurrentPage = 1, int PageSize = 15);
    }
}
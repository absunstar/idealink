using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;

namespace Tadrebat.MongoDB.Interface
{
    public interface IDBTraining : IRepositoryMongo<Training>
    {
        Task<MongoResultPaged<Training>> GetPaged(int CurrentPage, int PageSize);
        //Task<bool> UpdateAttendance(string TrainingId, Attendance attendances);
        //Task<MongoResultPaged<Training>> ListAllSearch(string filterText, int CurrentPage = 1, int PageSize = 15);
    }
}

using Employment.Entity.Mongo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Employment.MongoDB.Interface
{
    public interface IDBJobFair : IRepositoryMongo<JobFair>
    {
        Task<bool> RegisterUser(string JobFairId, JobFairRegisteration obj);
    }
}

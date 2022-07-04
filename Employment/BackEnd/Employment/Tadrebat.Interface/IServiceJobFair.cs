using Employment.Entity.Mongo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Interface
{
    public interface IServiceJobFair : IServiceRepository<JobFair>
    {
        Task<bool> UpdateFair(JobFair obj);
        Task<MongoResultPaged<JobFair>> Search(string filterText, int pageNumber = 1, int PageSize = 15);
        Task<bool> Register(string JobFairId, JobFairRegisteration obj, Enum.EnumUserTypes role);
        Task<bool> CheckRegister(string JobFairId, string UserId);
        Task<bool> SetAttendance(string JobFairId, long Code);
    }
}

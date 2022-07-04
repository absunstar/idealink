using Employment.Entity.Mongo;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Employment.MongoDB.Interface
{
    public interface IDBJob : IRepositoryMongo<Job>
    {
        Task<bool> UpdateExpiredJobs();
        Task<List<ReportJobCount>> ReportJobPerCompany(string ComapnyId, DateTime StartDate, DateTime EndDate, string JobFieldId);
        Task GetPaged(IEnumerable<JobSearch> filter, SortDefinition<JobSearch> sort, int pageNumber, int pageSize);
    }
}

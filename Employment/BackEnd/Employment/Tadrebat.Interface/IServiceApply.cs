using Employment.Entity.Mongo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Interface
{
    public interface IServiceApply : IServiceRepository<Apply>
    {
        Task<long> CountApplicants(List<string> jobIds);
        Task<bool> CheckIfApplied(string UserId, string JobId);
        Task<MongoResultPaged<Apply>> GetByJobSeekerId(string JobSeekerId, string filterText, int pageNumber = 1, int PageSize = 15);
        Task<MongoResultPaged<Apply>> GetByJoId(string JobId, string filterText, int pageNumber = 1, int PageSize = 15);
        Task<bool> CheckMyApply(string UserId, string JobId);
        Task<bool> Hire(string JobSeekerId, string JobId, bool Status);
        Task<List<ReportApply>> ReportJobSeekerAppliedPerJobCount(DateTime StartDate, DateTime EndDate);
        Task<long> ReportJobSeekerHiredCount(DateTime StartDate, DateTime EndDate);
    }
}

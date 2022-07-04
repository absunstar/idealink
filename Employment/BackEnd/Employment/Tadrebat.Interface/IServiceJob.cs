using Employment.Entity.Mongo;
using Employment.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Interface
{
    public class JobStats
    {
        public long PostedJobCount { get; set; }
        public long JobCount { get; set; }
        public long ApplicantCount { get; set; }
        public long ActiveJobCount { get; set; }
    }
    public interface IServiceJob : IServiceRepository<Job>
    {
        Task<bool> IncrementApplicantCounter(string JobId);
        Task<bool> UpdateExpiredJobs();
        Task<bool> UpdateStatus(string Id, EnumJobStatus status);
        Task<JobStats> GetMyJobStats(string userId);
        Task<List<Job>> GetJobsByCompanyId(string CompanyId);
        Task<MongoResultPaged<Job>> GetJobWaitingApproval(string filterText, int pageNumber = 1, int PageSize = 15); 
        Task<MongoResultPaged<Job>> ListAllByEmployerId(string UserId, string filterText, int pageNumber = 1, int PageSize = 15);
        Task<MongoResultPaged<JobSearch>> Search(string[] CompanyId, string filterText, List<string> ExperienceId, List<int> GenderId, List<string> Qualificationid, List<string> IndustryId, List<string> JobFieldId, List<string> CountryId, List<string> CityId, int pageNumber = 1, int PageSize = 15);
        Task<MongoResultPaged<JobSearch>> SearchforFilterTextAndCity(string filterText, List<string> CityId, int pageNumber = 1, int PageSize = 15);

        Task<MongoResultPaged<JobSearch>> SearchCompany( int pageNumber = 1, int PageSize = 15);
        Task<MongoResultPaged<Job>> AdminJobSearch(string CompanyId, int StatusId, string filterText, int pageNumber = 1, int PageSize = 15);
        Task<bool> UpdateCompanyInfo(Company obj);
        Task<long> GetJobWaitingApprovalCount();
        Task<List<ReportJobCount>> ReportJobCount(string CompanyId, DateTime StartDate, DateTime EndDate, string JobFieldId);
        Task<MongoResultPaged<JobSearch>> ForSearchValidation(string filterTextValidation, int pageNumber = 1, int PageSize = 15);

        //Task<MongoResultPaged<JobSearch>> ForSearchFilter(List<string> filterTextSearch, int pageNumber = 1, int PageSize = 15);


    }
}

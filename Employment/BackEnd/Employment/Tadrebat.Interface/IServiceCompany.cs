using Employment.Entity.Mongo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Interface
{
    public interface IServiceCompany : IServiceRepository<Company>
    {
        Task<bool> Create(Company obj);
        Task<bool> Update(Company obj);
        Task<bool> RemoveUser(string CompanyId, string UserId);
        Task<bool> AssignUser(string UserId, string CompanyId);
        Task<List<UserProfile>> ListCompanyEmployers(string CompanyId);
        Task<List<Company>> ListCompany();
        Task<List<Company>> ListAnyCompany();
        Task<MongoResultPaged<Company>> ListAnyCompanyPaged(string filterText, int pageNumber = 1, int PageSize = 15);
        Task<List<SubItemActive>> GetUserCompanies(List<SubItemActive> lst);
        Task<MongoResultPaged<Company>> GetCompanyWaitingApproval(string filterText, int pageNumber = 1, int PageSize = 15);
        Task<bool> UpdateStatus(string Id, bool status);
        Task<long> GetCompanyWaitingApprovalCount();
        Task<long> GetCompanyByUserIdCount(string UserId);
        Task<long> ReportCompanyCount(DateTime StartDate, DateTime EndDate);
    }
}

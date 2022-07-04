using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Employment.Entity.Mongo;
using Employment.Enum;

namespace Employment.Interface
{
    public interface IUserProfile
    {
        Task<UserProfile> UserProfileGetByEmail(string Email);
        Task<UserProfile> UserProfileGetById(string Id);
        Task<bool> UserProfileCreate(UserProfile obj);
        Task<bool> UserProfileUpdate(UserProfile obj);
        Task<bool> UserProfileDeActivate(string Id);
        Task<bool> UserProfileActivate(string Id);
        Task<List<UserProfile>> UserProfileListActive();
        Task<MongoResultPaged<UserProfile>> UserProfileListAll(string filterText, int filterType, int pageNumber = 1, int PageSize = 15);
        Task<List<UserProfile>> UserProfileListActiveEmployer(string filterText, int pageNumber = 1, int PageSize = 15);
        Task<List<UserProfile>> UserProfileListActiveJobSeeker(string filterText, int pageNumber = 1, int PageSize = 15); 
        Task<bool> IsUserProfileExist(string Id);
        Task<List<int>> GetTypePermissionByRole(EnumUserTypes type);
        Task<bool> CheckTypePermissionByRole(EnumUserTypes myType, EnumUserTypes checkedType);

        Task<bool> AssignCompany(string UserId, SubItem company);
        Task<bool> RemoveCompany(string UserId, string CompanyId);
        Task<List<UserProfile>> ListByIds(List<string> Ids);

        Task<bool> UpdateUserRole(string UserId, EnumUserTypes type);
        Task<bool> Limit(string Id);
    }
}

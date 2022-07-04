using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;
using Tadrebat.Enum;

namespace Tadrebat.Interface
{
    public interface IUserProfile
    {
        Task<bool> UpdateUserEmail(string EmailOld, string emailNew);
        Task<UserProfile> UserProfileGetByEmail(string Email);
        Task<UserProfile> UserProfileGetById(string Id);
        Task<bool> UserProfileCreate(UserProfile obj);
        Task<bool> UserProfileUpdate(UserProfile obj);
        Task<bool> UserProfileDeActivate(string Id);
        Task<bool> UserProfileActivate(string Id);
        Task<List<UserProfile>> UserProfileListActive();
        Task<MongoResultPaged<UserProfile>> UserProfileListAll(string filterText, int filterType, string UserId, EnumUserTypes type, int pageNumber = 1, int PageSize = 15);
        Task<MongoResultPaged<UserProfile>> GetMyTrainers(string UserId, EnumUserTypes type);
        Task<MongoResultPaged<UserProfile>> GetMyTrainersBySubPartnerId(string UserId, EnumUserTypes type, string SubPartnerId);
        Task<bool> IsUserProfileExist(string Id);
        Task<List<int>> GetTypePermissionByRole(EnumUserTypes type);
        Task<bool> CheckTypePermissionByRole(EnumUserTypes myType, EnumUserTypes checkedType);

        Task<MongoResultPaged<UserProfile>> GetPartnerSearch(string filterText, int pageNumber = 1, int PageSize = 15);
        Task<MongoResultPaged<UserProfile>> GetSubPartnerSearch(string filterText, int pageNumber = 1, int PageSize = 15);
        Task<bool> AddMyPartnerListIds(string UserId, string EnityId);
        Task<bool> AddMyPartnerListIds(string UserId, List<string> EnityId);
        Task<bool> RemoveMyPartnerListIds(string UserId, string EnityId);
        Task<bool> RemoveMyPartnerListIds(string UserId, List<string> EnityId);
        Task<bool> AddMySubPartnerListIds(string UserId, string EnityId);
        Task<bool> AddMySubPartnerListIds(string UserId, List<string> EnityId);
        Task<bool> RemoveMySubPartnerListIds(string UserId, string EnityId);
        Task<bool> RemoveMySubPartnerListIds(string UserId, List<string> EnityId);
        //Task<bool> AddMyAssignedToAccount(string UserId, string AccountId, int type);
        //Task<bool> RemoveMyAssignedToAccount(string UserId, string AccountId, int type);
        Task<long> GetTrainerCount();
        Task<bool> AddTrainerExamPass(string TrainerId, string PartinerId, string TrainingCategoryId);
        Task<MongoResultPaged<UserProfile>> GetTrainerCertificate(string filterText, int pageNumber = 1, int PageSize = 15);
        Task<bool> ApproveTrainerCertificate(string TrainerId, string PartinerId, string TrainingCategoryId, string Path);
        
    }
}

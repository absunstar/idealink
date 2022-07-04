using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employment.Entity.Mongo;
using Employment.Enum;
using Employment.Interface;
using Employment.MongoDB.Interface;
using System.IO;
using System.Reflection;

namespace Employment.Services
{
    public class ServiceUserProfile : IUserProfile
    {
        private readonly IDBUserProfile _dBUserProfile;
        private readonly IServiceJobSeeker _BLServiceJobSeeker;
        public ServiceUserProfile(IDBUserProfile dBUserProfile,
                            IServiceJobSeeker BLServiceJobSeeker)
        {
            _dBUserProfile = dBUserProfile;
            _BLServiceJobSeeker = BLServiceJobSeeker;
        }
        #region UserProfile
        public async Task<UserProfile> UserProfileGetByEmail(string Email)
        {

            try
            {
                var filter = Builders<UserProfile>.Filter.Where(x => x.Email == Email);
                var obj = await _dBUserProfile.GetOne(filter);
                return obj;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public async Task<UserProfile> UserProfileGetById(string Id)
        {
            return await _dBUserProfile.GetById(Id);
        }
        public async Task<bool> UserProfileCreate(UserProfile obj)
        {
            if (string.IsNullOrEmpty(obj.Name))
                return false;

            await _dBUserProfile.AddAsync(obj);

            if (obj.Type == (int)EnumUserTypes.JobSeeker)
            {
                await _BLServiceJobSeeker.Create(obj._id, obj.Email, obj.Name);
            }
            return true;
        }
        public async Task<bool> UserProfileUpdate(UserProfile obj)
        {
            if (string.IsNullOrEmpty(obj.Name) || string.IsNullOrEmpty(obj._id))
                return false;
            var user = await UserProfileGetById(obj._id);
            //obj.Email = user.Email;
            //obj.Type = user.Type;
            user.Name = obj.Name;
            await _dBUserProfile.UpdateObj(obj._id, user);

            return true;
        }
        public async Task<bool> UserProfileDeActivate(string Id)
        {
            var obj = await UserProfileGetById(Id);
            if (obj == null)
                return false;

            await _dBUserProfile.DeactivateAsync(Id);

            return true;
        }
        public async Task<bool> UserProfileActivate(string Id)
        {
            var obj = await UserProfileGetById(Id);
            if (obj == null)
                return false;

            await _dBUserProfile.ActivateAsync(Id);

            return true;
        }
        public async Task<List<UserProfile>> UserProfileListActive()
        {
            var sort = Builders<UserProfile>.Sort.Descending(x => x.Name);
            var lst = await _dBUserProfile.ListActive(sort);
            return lst;
        }
        public async Task<List<UserProfile>> ListByIds(List<string> Ids)
        {
            var filter = Builders<UserProfile>.Filter.Where(x => Ids.Contains(x._id));
            var sort = Builders<UserProfile>.Sort.Descending(x => x.CreatedAt);
            var lst = await _dBUserProfile.ListActive(filter, sort);
            return lst;
        }
        public async Task<MongoResultPaged<UserProfile>> UserProfileListAll(string filterText, int filterType, int pageNumber = 1, int PageSize = 15)
        {
            var lst = await _dBUserProfile.ListAllSearch(filterText, filterType, pageNumber, PageSize);
            return lst;
        }


        public async Task<List<UserProfile>> UserProfileListActiveJobSeeker(string filterText, int pageNumber = 1, int PageSize = 15)
        {
            var filter = Builders<UserProfile>.Filter.Where(x => (x.Name.ToLower().Contains(filterText.ToLower())
                                                                      || x.Email.Contains(filterText))
                                                                && x.IsActive == true
                                                                && x.Type == (int)EnumUserTypes.JobSeeker);

            var sort = Builders<UserProfile>.Sort.Ascending(x => x.Name);

            var lst = await _dBUserProfile.GetPaged(filter, sort, pageNumber, PageSize);
            return lst.lstResult;
        }
        public async Task<List<UserProfile>> UserProfileListActiveEmployer(string filterText, int pageNumber = 1, int PageSize = 15)
        {
            var filter = Builders<UserProfile>.Filter.Where(x => (x.Name.ToLower().Contains(filterText.ToLower())
                                                                      || x.Email.Contains(filterText))
                                                                && x.IsActive == true
                                                                && x.Type == (int)EnumUserTypes.Employer);

            var sort = Builders<UserProfile>.Sort.Ascending(x => x.Name);

            var lst = await _dBUserProfile.GetPaged(filter, sort, pageNumber, PageSize);
            return lst.lstResult;
        }
        public async Task<bool> IsUserProfileExist(string Id)
        {
            var obj = await _dBUserProfile.GetById(Id);
            return obj != null;
        }
        public async Task<bool> UpdateUserRole(string UserId, EnumUserTypes type)
        {
            var user = await UserProfileGetById(UserId);
            if (user == null)
                return false;

            if (user.Type != (int)EnumUserTypes.Undefined)
                return false;

            user.Type = (int)type;
            await _dBUserProfile.UpdateObj(UserId, user);

            if (type == EnumUserTypes.JobSeeker)
            {
                await _BLServiceJobSeeker.Create(user._id, user.Email, user.Name);
            }


            return true;
        }
        #endregion
        #region Helper
        public async Task<List<int>> GetTypePermissionByRole(EnumUserTypes type)
        {
            var lst = System.Enum.GetValues(typeof(EnumUserTypes))
                                .Cast<int>()
                                .Select(x => x) //select all except trainee
                                .OrderBy(x => x)
                                .ToList();
            return lst;
        }
        public async Task<List<int>> GetAllowedTypeAccounts(EnumUserTypes type)
        {
            var lst = System.Enum.GetValues(typeof(EnumUserTypes))
                                .Cast<int>()
                                .Select(x => x) //select all except trainee
                                .OrderBy(x => x)
                                .ToList();
            return lst;
        }
        public async Task<bool> CheckTypePermissionByRole(EnumUserTypes myType, EnumUserTypes checkedType)
        {
            var lst = await GetTypePermissionByRole(myType);
            return lst.Any(x => x == (int)checkedType);
        }
        #endregion

        public async Task<bool> AssignCompany(string UserId, SubItem company)
        {
            try
            {
                FieldDefinition<UserProfile> field = "MyCompanies";
                await _dBUserProfile.AddField(UserId, field, company);
            }
            catch (Exception ex)
            {

            }
            return true;
        }
        public async Task<bool> RemoveCompany(string UserId, string CompanyId)
        {
            //FieldDefinition<UserProfile> field = "MyCompanies";
            //await _dBUserProfile.RemoveField(UserId, field, CompanyId);
            var user = await UserProfileGetById(UserId);
            if (user == null)
                return false;

            user.MyCompanies.RemoveAll(x => x._id == CompanyId);
            await _dBUserProfile.UpdateObj(CompanyId, user);

            return true;
        }
        public async Task<bool> Limit(string Id)
        {
            var obj = await UserProfileGetById(Id);
            if (obj == null)
                return false;

            obj.IsEmployerLimitedCompanies = !obj.IsEmployerLimitedCompanies;

            await _dBUserProfile.UpdateObj(Id, obj);
            return true;
        }
    }
}

using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;
using Tadrebat.Enum;
using Tadrebat.Interface;
using Tadrebat.MongoDB.Interface;

namespace Tadrebat.Services
{
    public class ServiceUserProfile : IUserProfile
    {
        private readonly IDBUserProfile _dBUserProfile;
        private readonly IDBTrainee _dBTrainee;
        private readonly ICacheConfig _BLCacheConfig;
        public ServiceUserProfile(IDBUserProfile dBUserProfile, IDBTrainee dBTrainee, ICacheConfig BLCacheConfig)
        {
            _dBUserProfile = dBUserProfile;
            _dBTrainee = dBTrainee;
            _BLCacheConfig = BLCacheConfig;
        }
        #region UserProfile
        public async Task<bool> UpdateUserEmail(string EmailOld, string emailNew)
        {
            if (string.IsNullOrEmpty(emailNew) || string.IsNullOrEmpty(EmailOld))
                return false;

            var normailzedEmail = emailNew.ToUpper();
            try
            {
                using (SqlConnection con = new SqlConnection(_BLCacheConfig.STSConnection))
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE AspNetUsers set Email = @emailNew, NormalizedEmail = @NormalizedEmail, username = @emailNew, Normalizedusername = @NormalizedEmail where Email = @EmailOld", con))
                    {

                        cmd.Parameters.Add(new SqlParameter("@emailNew", emailNew));
                        cmd.Parameters.Add(new SqlParameter("@NormalizedEmail", normailzedEmail));
                        cmd.Parameters.Add(new SqlParameter("@EmailOld", EmailOld));

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            var user = await UserProfileGetByEmail(EmailOld);
            if (user != null)
            {
                user.Email = emailNew;
                await _dBUserProfile.UpdateObj(user._id, user);
            }
            else
            {
                var trainee = await TraineeGetByEmail(EmailOld);
                if (trainee != null)
                {
                    trainee.Email = emailNew;
                    await _dBTrainee.UpdateObj(trainee._id, trainee);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        public async Task<Trainee> TraineeGetByEmail(string Email)
        {
            var filter = Builders<Trainee>.Filter.Where(x => x.Email == Email);
            var obj = await _dBTrainee.GeOne(filter);
            return obj;
        }
        public async Task<UserProfile> UserProfileGetByEmail(string Email)
        {
            try
            {
                var filter = Builders<UserProfile>.Filter.Where(x => x.Email == Email);

                var obj = await _dBUserProfile.GeOne(filter);
                return obj;
            }
            catch (Exception ex)
            {
                //----------

                string logMessage = "Exception" + ex;
                TextWriter txtWriter;
                string m_exePath = string.Empty;
                m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                using (StreamWriter w = new StreamWriter(m_exePath + "\\" + "log.txt", true))
                {
                    txtWriter = w;
                    txtWriter.Write("\r\nLog Entry : ");
                    txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                        DateTime.Now.ToLongDateString());
                    txtWriter.WriteLine("  :");
                    txtWriter.WriteLine("  :{0}", logMessage);
                    txtWriter.WriteLine("-------------------------------");
                }

                //----------
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

            var user = await UserProfileGetByEmail(obj.Email);
            var trainee = await TraineeGetByEmail(obj.Email);
            if (user != null || trainee != null)
                return false;

            await _dBUserProfile.AddAsync(obj);

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

            if (user.Type == (int)EnumUserTypes.Trainer)
            {
                user.TrainerEndDate = obj.TrainerEndDate;
                user.TrainerStartDate = obj.TrainerStartDate;
                user.TrainerTrainingDetails = obj.TrainerTrainingDetails;
                user.CityId = obj.CityId;
                user.AreaId = obj.AreaId;

                user.MyPartnerListIds = new List<string>();
                user.MySubPartnerListIds = new List<string>();

                user.MyPartnerListIds.AddRange(obj.MyPartnerListIds);
                user.MySubPartnerListIds.AddRange(obj.MySubPartnerListIds);

                user.MyPartnerListIds = user.MyPartnerListIds.Select(x => x).Distinct().ToList();
                user.MySubPartnerListIds = user.MySubPartnerListIds.Select(x => x).Distinct().ToList();
            }

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
        public async Task<MongoResultPaged<UserProfile>> UserProfileListAll(string filterText, int filterType, string UserId, EnumUserTypes role, int pageNumber = 1, int PageSize = 15)
        {
            var allowedTypes = await GetAllowedTypeAccounts(role);
            if (allowedTypes == null)
                return new MongoResultPaged<UserProfile>(1, new List<UserProfile>(), PageSize);

            var lst = await _dBUserProfile.ListAllSearch(filterText, filterType, UserId, role, allowedTypes, pageNumber, PageSize);
            return lst;
        }
        public async Task<MongoResultPaged<UserProfile>> GetMyTrainers(string UserId, EnumUserTypes type)
        {
            var filter = Builders<UserProfile>.Filter.Where(x => (x.Type == (int)EnumUserTypes.Trainer));
            switch (type)
            {
                case EnumUserTypes.Partner:
                    filter = filter & Builders<UserProfile>.Filter.Where(x => (x.MyPartnerListIds.Contains(UserId)));
                    break;
                case EnumUserTypes.SubPartner:
                    filter = filter & Builders<UserProfile>.Filter.Where(x => (x.MySubPartnerListIds.Contains(UserId)));
                    break;
                case EnumUserTypes.Trainer:
                    filter = filter & Builders<UserProfile>.Filter.Where(x => (x._id == UserId));
                    break;
            }
            var sort = Builders<UserProfile>.Sort.Ascending(x => x.Name);
            var lst = await _dBUserProfile.GetPaged(filter, sort, 1, int.MaxValue);
            return lst;
        }
        public async Task<MongoResultPaged<UserProfile>> GetMyTrainersBySubPartnerId(string UserId, EnumUserTypes type, string SubPartnerId)
        {
            var filter = Builders<UserProfile>.Filter.Where(x => (x.Type == (int)EnumUserTypes.Trainer));
            switch (type)
            {
                //case EnumUserTypes.Partner:
                //    filter = filter & Builders<UserProfile>.Filter.Where(x => (x.MyPartnerListIds.Contains(UserId)));
                //    break;
                //case EnumUserTypes.SubPartner:
                //    filter = filter & Builders<UserProfile>.Filter.Where(x => (x.MySubPartnerListIds.Contains(UserId)));
                //    break;
                case EnumUserTypes.Partner:
                case EnumUserTypes.SubPartner:
                case EnumUserTypes.Admin:
                    filter = filter & Builders<UserProfile>.Filter.Where(x => (x.MySubPartnerListIds.Contains(SubPartnerId)));
                    break;
                case EnumUserTypes.Trainer:
                    filter = filter & Builders<UserProfile>.Filter.Where(x => (x._id == UserId));
                    break;
            }
            var sort = Builders<UserProfile>.Sort.Ascending(x => x.Name);
            var lst = await _dBUserProfile.GetPaged(filter, sort, 1, int.MaxValue);
            return lst;
        }
        public async Task<bool> IsUserProfileExist(string Id)
        {
            var obj = await _dBUserProfile.GetById(Id);
            return obj != null;
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
            switch (type)
            {
                case EnumUserTypes.Admin:
                    return lst.Where(x => x < (int)EnumUserTypes.Trainee).ToList();
                    break;
                case EnumUserTypes.Partner:
                    return lst.Where(x => x < (int)EnumUserTypes.Trainee && x > (int)EnumUserTypes.Admin).ToList();
                    break;
                case EnumUserTypes.SubPartner:
                    return lst.Where(x => x < (int)EnumUserTypes.Trainee && x > (int)EnumUserTypes.Partner).ToList();
                    break;
                default:
                    return new List<int>();
                    break;
            }
        }
        public async Task<List<int>> GetAllowedTypeAccounts(EnumUserTypes type)
        {
            var lst = System.Enum.GetValues(typeof(EnumUserTypes))
                                .Cast<int>()
                                .Select(x => x) //select all except trainee
                                .OrderBy(x => x)
                                .ToList();
            switch (type)
            {
                case EnumUserTypes.Admin:
                    return lst.ToList();
                    break;
                case EnumUserTypes.Partner:
                    return lst.Where(x => x == (int)EnumUserTypes.SubPartner || x == (int)EnumUserTypes.Trainer).ToList();
                    break;
                case EnumUserTypes.SubPartner:
                    //return lst.Where(x => x == (int)EnumUserTypes.Partner || x == (int)EnumUserTypes.Trainer).ToList();
                    return lst.Where(x => x == (int)EnumUserTypes.Trainer).ToList();
                    break;
                case EnumUserTypes.Trainer:
                    return lst.Where(x => x == (int)EnumUserTypes.Partner || x == (int)EnumUserTypes.SubPartner).ToList();
                    break;
                default:
                    return null;
                    break;
            }
        }
        public async Task<bool> CheckTypePermissionByRole(EnumUserTypes myType, EnumUserTypes checkedType)
        {
            var lst = await GetTypePermissionByRole(myType);
            return lst.Any(x => x == (int)checkedType);
        }
        #endregion
        public async Task<MongoResultPaged<UserProfile>> GetPartnerSearch(string filterText, int pageNumber = 1, int PageSize = 15)
        {
            var filter = Builders<UserProfile>.Filter.Where(x => (x.Name.ToLower().Contains(filterText))
                                                                && x.Type == (int)EnumUserTypes.Partner
                                                                && x.IsActive == true);
            var sort = Builders<UserProfile>.Sort.Ascending(x => x.Name);
            var lst = await _dBUserProfile.GetPaged(filter, sort, pageNumber, PageSize);
            return lst;
        }
        public async Task<MongoResultPaged<UserProfile>> GetSubPartnerSearch(string filterText, int pageNumber = 1, int PageSize = 15)
        {
            var filter = Builders<UserProfile>.Filter.Where(x => (x.Name.ToLower().Contains(filterText))
                                                                && x.Type == (int)EnumUserTypes.SubPartner
                                                                && x.IsActive == true);
            var sort = Builders<UserProfile>.Sort.Ascending(x => x.Name);
            var lst = await _dBUserProfile.GetPaged(filter, sort, pageNumber, PageSize);
            return lst;
        }
        public async Task<bool> AddMyPartnerListIds(string UserId, string EnityId)
        {
            FieldDefinition<UserProfile> field = "MyPartnerListIds";
            await _dBUserProfile.AddField(UserId, field, EnityId);

            return true;
        }
        public async Task<bool> AddMyPartnerListIds(string UserId, List<string> EnityId)
        {
            FieldDefinition<UserProfile> field = "MyPartnerListIds";
            await _dBUserProfile.AddFieldList(UserId, field, EnityId.ToArray());

            return true;
        }
        public async Task<bool> RemoveMyPartnerListIds(string UserId, string EnityId)
        {
            FieldDefinition<UserProfile> field = "MyPartnerListIds";
            await _dBUserProfile.RemoveField(UserId, field, EnityId);

            return true;
        }
        public async Task<bool> RemoveMyPartnerListIds(string UserId, List<string> EnityId)
        {
            FieldDefinition<UserProfile> field = "MyPartnerListIds";
            await _dBUserProfile.RemoveFieldList(UserId, field, EnityId.ToArray());

            return true;
        }
        public async Task<bool> AddMySubPartnerListIds(string UserId, string EnityId)
        {
            FieldDefinition<UserProfile> field = "MySubPartnerListIds";
            await _dBUserProfile.AddField(UserId, field, EnityId);

            return true;
        }
        public async Task<bool> AddMySubPartnerListIds(string UserId, List<string> EnityId)
        {
            FieldDefinition<UserProfile> field = "MySubPartnerListIds";
            await _dBUserProfile.AddFieldList(UserId, field, EnityId.ToArray());

            return true;
        }
        public async Task<bool> RemoveMySubPartnerListIds(string UserId, string EnityId)
        {
            FieldDefinition<UserProfile> field = "MySubPartnerListIds";
            await _dBUserProfile.RemoveField(UserId, field, EnityId);

            return true;
        }
        public async Task<bool> RemoveMySubPartnerListIds(string UserId, List<string> EnityId)
        {
            FieldDefinition<UserProfile> field = "MySubPartnerListIds";
            await _dBUserProfile.RemoveFieldList(UserId, field, EnityId.ToArray());

            return true;
        }
        //public async Task<bool> AddMyAssignedToAccount(string UserId, string AccountId, int type)
        //{
        //    FieldDefinition<UserProfile> field = "MyAssignedToAccount";
        //    var obj = new AssignedToAccount(AccountId, type);
        //    await _dBUserProfile.AddField(UserId, field, obj);

        //    return true;
        //}
        //public async Task<bool> RemoveMyAssignedToAccount(string UserId, string AccountId, int type)
        //{
        //    FieldDefinition<UserProfile> field = "MyAssignedToAccount";
        //    var obj = new AssignedToAccount(AccountId, type);
        //    await _dBUserProfile.RemoveField(UserId, field, obj);

        //    return true;
        //}
        public async Task<long> GetTrainerCount()
        {
            var filter = Builders<UserProfile>.Filter.Where(x => x.IsActive == true && x.Type == (int)Enum.EnumUserTypes.Trainer);
            var count = await _dBUserProfile.GetCount(filter);
            return count;
        }
        public async Task<bool> AddTrainerExamPass(string TrainerId, string PartnerId, string TrainingCategoryId)
        {
            var user = await UserProfileGetById(TrainerId);
            if (user == null)
                return false;

            var trainingCategory = user.MyTrainerCertificates.Where(x => x.TrainingCategoryId == TrainingCategoryId && x.PartnerId == PartnerId).FirstOrDefault();
            if (trainingCategory == null)
            {
                //add new one
                var tc = new TrainerTraining();
                tc.TrainingCategoryId = TrainingCategoryId;
                tc.PartnerId = PartnerId;
                tc.ExamCount = 1;

                FieldDefinition<UserProfile> field = "MyTrainerCertificates";
                await _dBUserProfile.AddField(TrainerId, field, tc);
            }
            else
            {
                trainingCategory.ExamCount++;
                await _dBUserProfile.UpdateObj(TrainerId, user);
            }

            return true;
        }
        public async Task<MongoResultPaged<UserProfile>> GetTrainerCertificate(string filterText, int pageNumber = 1, int PageSize = 15)
        {
            var filter = Builders<UserProfile>.Filter.Where(x => //(x.Name.ToLower().Contains(filterText))
                                                                 //&& 
                                                               x.Type == (int)EnumUserTypes.Trainer
                                                               && x.IsActive == true
                                                               && x.MyTrainerCertificates.Any(y => y.ExamCount >= _BLCacheConfig.TrainerExamCountCertificate
                                                                                                   && y.HasCertificate == false
                                                                                                   && y.IsApproved == false));
            var sort = Builders<UserProfile>.Sort.Ascending(x => x.Name);
            var lst = await _dBUserProfile.GetPaged(filter, sort, pageNumber, PageSize);
            return lst;
        }
        public async Task<bool> ApproveTrainerCertificate(string TrainerId, string PartnerId, string TrainingCategoryId, string Path)
        {
            var trainer = await UserProfileGetById(TrainerId);
            if (trainer == null)
                return false;

            var trainingCategory = trainer.MyTrainerCertificates.Where(x => x.TrainingCategoryId == TrainingCategoryId && x.PartnerId == PartnerId).FirstOrDefault();
            if (trainingCategory == null)
            {
                return false;
            }
            else if (trainingCategory.ExamCount < _BLCacheConfig.TrainerExamCountCertificate)
            {
                return false;
            }
            else
            {
                trainingCategory.IsApproved = true;
                trainingCategory.HasCertificate = true;
                trainingCategory.CertificatePath = Path;

                await _dBUserProfile.UpdateObj(TrainerId, trainer);
            }
            return true;
        }
    }
}

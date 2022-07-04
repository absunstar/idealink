using AutoMapper;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;
using Tadrebat.Interface;
using Tadrebat.MongoDB.Interface;

namespace Tadrebat.Services
{
    public class ServiceTrainee : ITrainee
    {
        private readonly IDBUserProfile _dBUserProfile;
        private readonly IDBTrainee _dBTrainee;
        private readonly ITraining _BLServiceTraining;
        private readonly IMapper _Mapper;
        public ServiceTrainee(IDBUserProfile dBUserProfile, IDBTrainee dBTrainee, ITraining BLServiceTraining, IMapper mapper)
        {
            _dBUserProfile = dBUserProfile;
            _dBTrainee = dBTrainee;
            _BLServiceTraining = BLServiceTraining;
            _Mapper = mapper;
        }
        public async Task<UserProfile> UserProfileGetByEmail(string Email)
        {
            var filter = Builders<UserProfile>.Filter.Where(x => x.Email == Email);
            var obj = await _dBUserProfile.GeOne(filter);
            return obj;
        }
        public async Task<Trainee> GetByEmail(string Email)
        {
            try
            {
                var filter = Builders<Trainee>.Filter.Where(x => x.Email == Email);

                var obj = await _dBTrainee.GeOne(filter);
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
        public async Task<Trainee> GetById(string Id)
        {
            return await _dBTrainee.GetById(Id);
        }
        public async Task<bool> Create(Trainee obj, string TrainingId)
        {
            if (string.IsNullOrEmpty(obj.Name))
                return false;

            var user = await UserProfileGetByEmail(obj.Email);
            var trainee = await GetByEmail(obj.Email);
            if (user != null || trainee != null)
                return false;

            await _dBTrainee.AddAsync(obj);

            if (!string.IsNullOrEmpty(TrainingId))
            {
                //add training to trainee profile
                await AddTraining(obj._id, TrainingId);
                //var traineeInfo = _Mapper.Map<Trainee, TraineeInfo>(obj);
            }

            return true;
        }
        public async Task<bool> Update(Trainee obj)
        {
            if (string.IsNullOrEmpty(obj.Name) || string.IsNullOrEmpty(obj._id))
                return false;

            var user = await GetById(obj._id);
            if (user == null)
                return false;

            user.Name = obj.Name;
            user.Mobile = obj.Mobile;
            user.Gender = obj.Gender;
            user.data = obj.data;
            user.IdType = obj.IdType;
            user.NationalId = obj.NationalId;

            await _dBTrainee.UpdateObj(obj._id, user);

            return true;
        }
        public async Task<bool> UpdateCertificate(string TraineeId, string TrainingId, string Path, string CertificateNumber)
        {
            var trainee = await GetById(TraineeId);
            if (trainee == null)
                return false;

            var traineeTraining = trainee.myTrainings.Where(x => x.TrainingId == TrainingId).FirstOrDefault();

            traineeTraining.CertificatePath = Path;
            traineeTraining.CertificateNumber = CertificateNumber;
            traineeTraining.HasCertificate = true;

            await _dBTrainee.UpdateObj(TraineeId, trainee);

            return true;
        }
        public async Task<bool> UpdateObj(Trainee obj)
        {
            await _dBTrainee.UpdateObj(obj._id, obj);

            return true;
        }
        public async Task<bool> UpdateNationalId(string UserId, string NationalId)
        {
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(NationalId))
                return false;

            var user = await GetById(UserId);
            if (user == null)
                return false;

            user.NationalId = NationalId;
            await _dBTrainee.UpdateObj(UserId, user);

            return true;
        }
        public async Task<bool> DeActivate(string Id)
        {
            var obj = await GetById(Id);
            if (obj == null)
                return false;

            await _dBTrainee.DeactivateAsync(Id);

            return true;
        }
        public async Task<bool> Activate(string Id)
        {
            var obj = await GetById(Id);
            if (obj == null)
                return false;

            await _dBTrainee.ActivateAsync(Id);

            return true;
        }
        public async Task<List<Trainee>> ListActive()
        {
            var sort = Builders<Trainee>.Sort.Descending(x => x.Name);
            var lst = await _dBTrainee.ListActive(sort);
            return lst;
        }
        public async Task<List<Trainee>> SearchActive(string filterText, int pageNumber = 1, int PageSize = 15)
        {
            var filter = Builders<Trainee>.Filter.Where(x => x.Name.ToLower().Contains(filterText.ToLower())
                                                || x.Email.ToLower().Contains(filterText.ToLower())
                                                || x.Mobile.ToLower().Contains(filterText.ToLower())
                                                || x.NationalId.ToLower().Contains(filterText.ToLower()));
            var sort = Builders<Trainee>.Sort.Ascending(x => x.Name);
            var lst = await _dBTrainee.GetPaged(filter, sort, pageNumber, PageSize);
            return lst.lstResult;
        }
        public async Task<MongoResultPaged<Trainee>> ListAll(string filterText, int pageNumber = 1, int PageSize = 15)
        {
            var lst = await _dBTrainee.ListAllSearch(filterText, pageNumber, PageSize);
            return lst;
        }

        public async Task<bool> AddTraining(string TraineeId, string TrainingId, bool IsApproved = true)
        {

            var trainee = await GetById(TraineeId);
            if (trainee.myTrainings.Where(x => x.TrainingId == TrainingId).Count() > 0)
                return true;

            FieldDefinition<Trainee> field = "myTrainings";
            await _dBTrainee.AddField(TraineeId, field, new TraineeTraining(TrainingId));

            try
            {
                var traineeInfo = _Mapper.Map<Trainee, TraineeInfo>(trainee);
                traineeInfo.IsApproved = IsApproved;

                //add trainee to training object
                await _BLServiceTraining.AddTrainee(TrainingId, traineeInfo);
            }
            catch (Exception ex)
            {

                throw;
            }

            return true;
        }
        public async Task<bool> RemoveTraining(string TraineeId, string TrainingId)
        {
            FieldDefinition<Trainee> field = "myTrainings";
            await _dBTrainee.RemoveField(TraineeId, field, new TraineeTraining(TrainingId));

            //remove trainee to training object
            await _BLServiceTraining.RemoveTrainee(TrainingId, TraineeId);
            return true;
        }
        public async Task<bool> ApproveTraineeRegister(string TraineeId, string TrainingId)
        {
            await RemoveTraining(TraineeId, TrainingId);

            await AddTraining(TraineeId, TrainingId);
            return true;
        }
        public async Task<long> GetTraineeCount()
        {
            var filter = Builders<Trainee>.Filter.Where(x => x.IsActive == true);
            var count = await _dBTrainee.GetCount(filter);
            return count;
        }

    }
}

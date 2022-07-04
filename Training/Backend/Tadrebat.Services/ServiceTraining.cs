using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;
using Tadrebat.Enum;
using Tadrebat.Interface;
using Tadrebat.MongoDB.Interface;
using System.Linq;

namespace Tadrebat.Services
{
    public class ServiceTraining : ITraining
    {
        private readonly IDBEntitySubPartner _dBEntitySubPartner;
        private readonly IDBTraining _dBTraining;
        private readonly IUserProfile _BLServiceUserProfile;
        private readonly IDBTrainee _dBTrainee;
        public ServiceTraining(IDBTraining dBTraining, IUserProfile BLServiceUserProfile, IDBEntitySubPartner dBEntitySubPartner, IDBTrainee dBTrainee)
        {
            _dBTraining = dBTraining;
            _BLServiceUserProfile = BLServiceUserProfile;
            _dBEntitySubPartner = dBEntitySubPartner;
            _dBTrainee = dBTrainee;
        }
        public async Task<Training> GetById(string Id)
        {
            return await _dBTraining.GetById(Id);
        }
        public async Task<bool> Create(Training obj)
        {
            obj = CreateSessions(obj);
            obj = UpdateTrainingConfig(obj);

            var count = await GetTrainerTrainingCount(obj.TrainerId);
            obj.TrainerCount = count + 1;

            await _dBTraining.AddAsync(obj);

            return true;
        }
        private Training CreateSessions(Training training)
        {
            int sessionNumber = 1;
            for (var day = training.StartDate.Date; day.Date <= training.EndDate.Date; day = day.AddDays(1))
            {
                var d = (int)day.DayOfWeek + 1;
                if (training.days.Contains(d.ToString()))
                {
                    var trainingSession = new Sessions()
                    {
                        Day = day,
                        Name = string.Format("#{0} - {1}", sessionNumber.ToString(), day.ToShortDateString())
                    };
                    training.Sessions.Add(trainingSession);
                    sessionNumber++;
                }
            }
            return training;
        }
        private Training UpdateTrainingConfig(Training obj)
        {
            obj.IsAdminApproved = true;
            obj.IsConfirm1 = true;
            obj.IsConfirm2 = false;

            return obj;
        }
        public async Task<List<Training>> ListActive()
        {
            var lst = await _dBTraining.ListActive();

            return lst;
        }
        public async Task<bool> Update(Training obj)
        {
            var training = await GetById(obj._id);
            if (training.EndDate.Date < DateTime.Now.Date)
                return false;

            obj = CreateSessions(obj);

            training.Sessions = obj.Sessions;
            training.PartnerId = obj.PartnerId;
            training.SubPartnerId = obj.SubPartnerId;
            training.StartDate = obj.StartDate;
            training.EndDate = obj.EndDate;
            training.CityId = obj.CityId;
            training.AreaId = obj.AreaId;
            training.days = obj.days;
            training.TrainingCategoryId = obj.TrainingCategoryId;
            training.TrainingTypeId = obj.TrainingTypeId;
            training.TrainingCenterId = obj.TrainingCenterId;
            training.Type = obj.Type;
            training.IsOnline = obj.IsOnline;
            training.TrainerId = obj.TrainerId;

            await _dBTraining.UpdateObj(obj._id, training);

            return true;
        }
        public async Task<bool> DeActivate(string Id)
        {
            var obj = await GetById(Id);
            if (obj == null)
                return false;

            await _dBTraining.DeactivateAsync(Id);

            return true;
        }
        public async Task<MongoResultPaged<Training>> ListAll(string userId, EnumUserTypes role, string PartnerId, string SubPartnerId, string TrainerId, string TrainingTypeId, string TrainingCategoryId, int pageNumber = 1, int PageSize = 15)
        {
            var filter = Builders<Training>.Filter.Where(x => x.IsActive == true);
            var user = await _BLServiceUserProfile.UserProfileGetById(userId);
            switch (role)
            {
                case EnumUserTypes.Partner:
                    filter = filter & Builders<Training>.Filter.Where(x => user.MyPartnerListIds.Contains(x.PartnerId._id));
                    break;
                case EnumUserTypes.SubPartner:
                    filter = filter & Builders<Training>.Filter.Where(x => user.MySubPartnerListIds.Contains(x.SubPartnerId._id));

                    var subFilter = Builders<EntitySubPartner>.Filter.Where(x => x.IsActive == true && user.MySubPartnerListIds.Contains(x._id));
                    var lstSubPartners = await _dBEntitySubPartner.GetPaged(subFilter, null, 1, int.MaxValue);

                    var lstTrainingCentersIds = new List<string>();
                    lstSubPartners.lstResult.ForEach(sub =>
                    {
                        lstTrainingCentersIds.AddRange(sub.TrainingCenterIds);
                    });

                    filter = filter & Builders<Training>.Filter.Where(x => lstTrainingCentersIds.Contains(x.TrainingCenterId._id));
                    break;
                case EnumUserTypes.Trainer:
                    filter = filter & Builders<Training>.Filter.Where(x => x.TrainerId == userId);
                    break;
                case EnumUserTypes.Trainee:
                    // filter = filter & Builders<Training>.Filter.Where(x => x.StartDate > DateTime.Now && x.Type == EnumTrainingType.Public && x.IsAdminApproved == true && x.IsActive == true);
                    // filter = filter & Builders<Training>.Filter.Where(x => x.StartDate > DateTime.Now && x.Type == EnumTrainingType.Public && x.IsAdminApproved == true && x.IsActive == true);
                    // filter = filter & Builders<Training>.Filter.Where(x => x.StartDate > DateTime.Now && x.Type == EnumTrainingType.Public && x.IsAdminApproved == true && x.IsActive == true);
                    filter = filter & Builders<Training>.Filter.Where(x => x.StartDate > DateTime.Now && x.Type == EnumTrainingType.Public && x.IsAdminApproved == true && x.IsActive == true);
                    break;
            }

            if (!string.IsNullOrEmpty(PartnerId))
                filter = filter & Builders<Training>.Filter.Where(x => x.PartnerId._id == PartnerId);

            if (!string.IsNullOrEmpty(SubPartnerId))
                filter = filter & Builders<Training>.Filter.Where(x => x.SubPartnerId._id == SubPartnerId);

            if (!string.IsNullOrEmpty(TrainerId))
                filter = filter & Builders<Training>.Filter.Where(x => x.TrainerId == TrainerId);

            if (!string.IsNullOrEmpty(TrainingTypeId))
                filter = filter & Builders<Training>.Filter.Where(x => x.TrainingTypeId == TrainingTypeId);

            if (!string.IsNullOrEmpty(TrainingCategoryId))
                filter = filter & Builders<Training>.Filter.Where(x => x.TrainingCategoryId == TrainingCategoryId);

            var sort = Builders<Training>.Sort.Descending(x => x.CreatedAt);
            var lst = await _dBTraining.GetPaged(filter, sort, pageNumber, PageSize);

            return lst;
        }

        public async Task<bool> SetAdminApproved(string trainingId)
        {
            var update = Builders<Training>.Update.Set(s => s.IsAdminApproved, true);
            await _dBTraining.UpdateAsync(trainingId, update);

            return true;
        }
        public async Task<bool> SetConfirmed1(string trainingId)
        {
            var update = Builders<Training>.Update.Set(s => s.IsConfirm1, true);
            await _dBTraining.UpdateAsync(trainingId, update);
            return true;
        }
        public async Task<bool> SetConfirmed2(string trainingId)
        {
            var update = Builders<Training>.Update.Set(s => s.IsConfirm1, true)
                                                .Set(s => s.IsConfirm2, true);
            await _dBTraining.UpdateAsync(trainingId, update);

            return true;
        }

        public async Task<bool> AddTrainee(string TrainingId, TraineeInfo trainee)
        {
            FieldDefinition<Training> field = "Trainees";
            await _dBTraining.AddField(TrainingId, field, trainee);
            return true;
        }
        public async Task<bool> RemoveTrainee(string TrainingId, string traineeId)
        {
            FieldDefinition<Training> field = "Trainees";
            var update = Builders<Training>.Update.PullFilter("Trainees", Builders<TraineeInfo>.Filter.Eq("_Id", traineeId));
            await _dBTraining.RemoveField(TrainingId, update);

            return true;
        }
        public async Task<bool> SaveAttendnace(string TrainingId, Attendance attendance)
        {
            FieldDefinition<Training> field = "Attendances";
            var update = Builders<Training>.Update.PullFilter("Attendances", Builders<Attendance>.Filter.Eq("SessionId", attendance.SessionId));
            await _dBTraining.RemoveField(TrainingId, update);

            await _dBTraining.AddField(TrainingId, field, attendance);
            return true;
        }
        public async Task<List<Training>> GetMyTraining(List<string> TrainingIds)
        {
            var filter = Builders<Training>.Filter.Where(x => x.IsActive == true && TrainingIds.Contains(x._id));
            var sort = Builders<Training>.Sort.Descending(x => x.EndDate);

            var lst = await _dBTraining.GetPaged(filter, sort, 1, Int32.MaxValue);
            return lst.lstResult;
        }
        public async Task<long> GetTrainingCount()
        {
            var filter = Builders<Training>.Filter.Where(x => x.IsActive == true);
            var count = await _dBTraining.GetCount(filter);
            return count;
        }
        public async Task<bool> SaveExamTemplate(string TrainingId, string ExamTemplateId)
        {
            var obj = await GetById(TrainingId);
            if (obj == null)
                return false;

            var update = Builders<Training>.Update.Set(s => s.ExamTemplateId, ExamTemplateId);
            await _dBTraining.UpdateAsync(TrainingId, update);
            return true;
        }
        protected async Task<long> GetTrainerTrainingCount(string TrainerId)
        {
            var filter = Builders<Training>.Filter.Where(x => x.IsActive == true && x.TrainerId == TrainerId);

            var count = await _dBTraining.GetCount(filter);

            return count;
        }
        public async Task<List<Trainee>> GetCourseCertificates(string trainingId)
        {
            var training = await GetById(trainingId);

            var lstUserId = training.Trainees.Where(x => x.IsApproved == true).Select(x => x._Id);

            var filter = Builders<Trainee>.Filter.Where(x => lstUserId.Contains(x._id));
            var sort = Builders<Trainee>.Sort.Ascending(x => x.Name);
            var lstUsers = await _dBTrainee.GetPaged(filter, sort, 1, int.MaxValue);
            return lstUsers.lstResult;

            //var lstCertificate = new List<TraineeTraining>();
            //lstUsers.lstResult.ForEach(user => {
            //    var t = user.myTrainings.Where(x => x.TrainingId == trainingId && x.HasCertificate == true).FirstOrDefault();
            //    if(t != null)
            //    {
            //        lstCertificate.Add(t);
            //    }
            //});

            //return lstCertificate;
        }


    }
}

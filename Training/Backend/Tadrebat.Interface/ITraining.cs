using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;
using Tadrebat.Enum;

namespace Tadrebat.Interface
{
    public interface ITraining
    {
        Task<Training> GetById(string Id);
        Task<bool> Create(Training obj);
        Task<bool> Update(Training obj);
        Task<bool> DeActivate(string Id);
        //Task<bool> Activate(string Id);
        Task<List<Training>> ListActive();
        //Task<List<Training>> SearchActive(string filterText);
        Task<MongoResultPaged<Training>> ListAll(string userId, EnumUserTypes role, string PartnerId, string SubPartnerId, string TrainerId, string TrainingTypeId, string TrainingCategoryId, int pageNumber = 1, int PageSize = 15);

        Task<bool> SetAdminApproved(string trainingId);
        Task<bool> SetConfirmed1(string trainingId);
        Task<bool> SetConfirmed2(string trainingId);
        Task<bool> AddTrainee(string TrainingId, TraineeInfo trainee);
        Task<bool> RemoveTrainee(string TrainingId, string traineeId);
        Task<bool> SaveAttendnace(string TrainingId, Attendance attendance);
        Task<List<Training>> GetMyTraining(List<string> TrainingIds);
        Task<long> GetTrainingCount();
        Task<bool> SaveExamTemplate(string TrainingId, string ExamTemplateId);
        Task<List<Trainee>> GetCourseCertificates(string trainingId);
    }
}

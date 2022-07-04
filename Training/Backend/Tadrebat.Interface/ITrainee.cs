using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;

namespace Tadrebat.Interface
{
    public interface ITrainee
    {
        Task<Trainee> GetByEmail(string Email);
        Task<Trainee> GetById(string Id);
        Task<bool> Create(Trainee obj, string TrainingId);
        Task<bool> Update(Trainee obj);
        Task<bool> UpdateObj(Trainee obj);
        Task<bool> UpdateNationalId(string UserID,string NationalId);
        Task<bool> UpdateCertificate(string TraineeId, string TrainingId, string Path, string CertificateNumber);
        Task<bool> DeActivate(string Id);
        Task<bool> Activate(string Id);
        Task<List<Trainee>> ListActive();
        Task<List<Trainee>> SearchActive(string filterText, int pageNumber = 1, int PageSize = 15);
        Task<MongoResultPaged<Trainee>> ListAll(string filterText, int pageNumber = 1, int PageSize = 15);
        Task<bool> AddTraining(string TraineeId, string TrainingId, bool IsApproved = true);
        Task<bool> RemoveTraining(string TraineeId, string TrainingId);
        Task<bool> ApproveTraineeRegister(string TraineeId, string TrainingId);
        Task<long> GetTraineeCount();
    }
}

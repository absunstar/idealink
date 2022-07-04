using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;

namespace Tadrebat.MongoDB.Interface
{
    public interface IDBExam : IRepositoryMongo<Exam>
    {
        Task<List<Exam>> ListByTrainingTraineeId(string Training, string Trainee);
        Task<List<Exam>> ListByTrainingId(string Training);
        Task<List<Exam>> ListByTraineeId(string Trainee);
    }
}
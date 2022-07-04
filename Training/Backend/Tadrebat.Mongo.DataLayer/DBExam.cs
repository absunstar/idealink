using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;
using Tadrebat.MongoDB.Interface;

namespace Tadrebat.Mongo.DataLayer
{
    public class DBExam : DBRepositoryMongo<Exam>, IDBExam
    {
        private static string _pDBCollectionName = "Exam";
        public DBExam(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<Exam>(_DBCollectionName);
        }
        public async Task<List<Exam>> ListByTrainingTraineeId(string TrainingId, string TraineeId)
        {
            var filter = Builders<Exam>.Filter.Where(x => x.TrainingId == TrainingId && x.TraineeId == TraineeId);
            var sort = Builders<Exam>.Sort.Descending(x => x.CreatedAt);
            var result = await GetPaged(filter, sort, 1, int.MaxValue);
            return result.lstResult;
        }
        public async Task<List<Exam>> ListByTrainingId(string TrainingId)
        {
            var filter = Builders<Exam>.Filter.Where(x => x.TrainingId == TrainingId);
            var sort = Builders<Exam>.Sort.Descending(x => x.CreatedAt);
            var result = await GetPaged(filter, sort, 1, int.MaxValue);
            return result.lstResult;
        }
        public async Task<List<Exam>> ListByTraineeId(string TraineeId)
        {
            var filter = Builders<Exam>.Filter.Where(x => x.TraineeId == TraineeId);
            var sort = Builders<Exam>.Sort.Descending(x => x.CreatedAt);
            var result = await GetPaged(filter, sort, 1, int.MaxValue);
            return result.lstResult;
        }
    }
}

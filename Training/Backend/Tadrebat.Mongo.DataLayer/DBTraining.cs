using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;
using Tadrebat.MongoDB.Interface;

namespace Tadrebat.Mongo.DataLayer
{
    public class DBTraining : DBRepositoryMongo<Training>, IDBTraining
    {
        private static string _pDBCollectionName = "Training";
        public DBTraining(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<Training>(_DBCollectionName);
        }
        public async Task<MongoResultPaged<Training>> GetPaged(int CurrentPage = 1, int PageSize = 15)
        {
            var filter = Builders<Training>.Filter.Where(x => x.IsActive == true);
            var sort = Builders<Training>.Sort.Descending(x => x.StartDate);
            return await GetPaged(filter, sort, CurrentPage, PageSize);
        }
        //public async Task<bool> UpdateAttendance(string TrainingId, Attendance attendances)
        //{
        //    var filter = Builders<Training>.Filter.Where(x => x.Attendances.Find(y=>y.SessionId == attendances.SessionId));

        //    var update = Builders<Training>.Update.Set(x => x.Attendances, attendance.Attendances);

        //    //await _dBTraining.UpdateAttendance(TrainingId, attendance);
        //    await _mongoCollection.UpdateManyAsync(filter, update);

        //    return true;
        //}
        //public async Task<MongoResultPaged<Training>> ListAllSearch(string filterText, int CurrentPage = 1, int PageSize = 15)
        //{
        //    var filter = Builders<Training>.Filter.Where(x => x.Name.ToLower().Contains(filterText.ToLower())
        //                                    || x.Email.ToLower().Contains(filterText.ToLower())
        //                                    || x.Mobile.ToLower().Contains(filterText.ToLower())
        //                                    || x.NationalId.ToLower().Contains(filterText.ToLower()));
        //    var sort = Builders<Training>.Sort.Descending(x => x.StartDate);
        //    return await GetPaged(filter, sort, CurrentPage, PageSize);
        //}
    }
}

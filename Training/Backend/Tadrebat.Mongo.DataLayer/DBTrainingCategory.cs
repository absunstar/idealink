using System;
using System.Collections.Generic;
using System.Text;
using Tadrebat.MongoDB.Interface;
using MongoDB.Driver;
using MongoDB.Bson;
using Tadrebat.Entity.Mongo;
using MongoDB.Bson.Serialization;
using System.Threading.Tasks;
using MongoDB.Driver.Builders;

namespace Tadrebat.Mongo.DataLayer
{
    public class DBTrainingCategory : DBRepositoryMongo<TrainingCategory>, IDBTrainingCategory
    {
        private static string _pDBCollectionName = "TrainingCategory";
        public DBTrainingCategory(IMongoDBContext mongoDBContext) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<TrainingCategory>(_DBCollectionName);
        }
        public async Task UpdateName(string Id, string Name,string TrainingTypeId)
        {
            var update = Builders<TrainingCategory>.Update.Set(s => s.Name, Name).Set(s => s.TrainingTypeId, TrainingTypeId);

            await UpdateAsync(Id, update);
        }
        public async Task<MongoResultPaged<TrainingCategory>> GetPaged(int CurrentPage = 1, int PageSize = 15)
        {
            var filter = Builders<TrainingCategory>.Filter.Where(x => x.IsActive == true);
            var sort = Builders<TrainingCategory>.Sort.Descending(x => x._id);
            return await GetPaged(filter, sort, CurrentPage, PageSize);
        }
        public async Task<MongoResultPaged<TrainingCategory>> TrainingCategoryListAllSearch(string filterText, int CurrentPage = 1, int PageSize = 15)
        {
           var filter = Builders<TrainingCategory>.Filter.Where(x => x.Name.ToLower().Contains(filterText.ToLower()));
           var sort = Builders<TrainingCategory>.Sort.Descending(x => x._id);
            return await GetPaged(filter, sort, CurrentPage, PageSize);
        }
        public async Task<bool> CourseCreate(string TrainingCategoryId, string Name)
        {
            var TrainingCategory = await GetById(TrainingCategoryId);
            if (TrainingCategory == null)
                return false;

            var details = new Course();
            details.Name = Name;

            var filter = Builders<TrainingCategory>.Filter.Where(x => x._id == TrainingCategoryId);
            var update = Builders<TrainingCategory>.Update.Push("Course", details);
            
            await _mongoCollection.UpdateOneAsync(filter, update);
            
            return true;
        }
        public async Task<bool> CourseUpdate(string TrainingCategoryId, string CourseId, string Name)
        {
            var TrainingCategory = await GetById(TrainingCategoryId);
            if (TrainingCategory == null)
                return false;

            var filter = Builders<TrainingCategory>.Filter.ElemMatch(y => y.Course, x => x._id == CourseId);
            var update = Builders<TrainingCategory>.Update.Set(x => x.Course[-1].Name, Name);
            
            await _mongoCollection.UpdateOneAsync(filter, update);
            
            return true;
        }
        public async Task<bool> CourseActivate(string TrainingCategoryId, string Id)
        {
            return await CourseStatusUpdate(TrainingCategoryId, Id, true);
        }
        public async Task<bool> CourseDeActivate(string TrainingCategoryId, string Id)
        {
            return await CourseStatusUpdate(TrainingCategoryId, Id, false);
        }
        private async Task<bool> CourseStatusUpdate(string TrainingCategoryId, string Id, bool status)
        {
            var TrainingCategory = await GetById(TrainingCategoryId);
            if (TrainingCategory == null)
                return false;

            var filter = Builders<TrainingCategory>.Filter.ElemMatch(y=>y.Course, x => x._id == Id);
            var update = Builders<TrainingCategory>.Update.Set(x => x.Course[-1].IsActive, status);

            await _mongoCollection.UpdateOneAsync(filter, update);

            return true;
        }
    }
}

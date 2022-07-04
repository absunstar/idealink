using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;
using Tadrebat.Interface;
using Tadrebat.MongoDB.Interface;

namespace Tadrebat.Services
{
    public class ServiceQuestion : IQuestion
    {
        private readonly IDBQuestion _dBQuestion;
        public ServiceQuestion(IDBQuestion dBQuestion)
        {
            _dBQuestion = dBQuestion;
        }
        public async Task<Question> QuestionGetById(string Id)
        {
            return await _dBQuestion.GetById(Id);
        }
        public async Task<bool> QuestionCreate(Question obj)
        {
            await _dBQuestion.AddAsync(obj);

            return true;
        }
        public async Task<bool> QuestionUpdate(Question obj)
        {
            var quest = await QuestionGetById(obj._id);
            if (quest == null)
                return false;

            quest.Answer = new List<Answer>();
            await _dBQuestion.UpdateObj(obj._id, quest);

            await _dBQuestion.UpdateObj(obj._id, obj);

            return true;
        }
        public async Task<bool> QuestionDeActivate(string Id)
        {

            var obj = await QuestionGetById(Id);
            if (obj == null)
                return false;

            await _dBQuestion.DeactivateAsync(Id);

            return true;
        }
        public async Task<bool> QuestionActivate(string Id)
        {
            var obj = await QuestionGetById(Id);
            if (obj == null)
                return false;

            await _dBQuestion.ActivateAsync(Id);

            return true;
        }
        public async Task<List<Question>> QuestionListActive()
        {
            var sort = Builders<Question>.Sort.Descending(x => x.Name);
            var lst = await _dBQuestion.ListActive(sort);
            return lst;
        }
        public async Task<List<Question>> QuestionListActiveByTrainingCategoryId(string TrainingCategoryId)
        {
            var filter = Builders<Question>.Filter.Where(x => x.TrainingCategoryId == TrainingCategoryId
                                                            && x.IsActive == true);
            var sort = Builders<Question>.Sort.Descending(x => x.Name);
            var lst = await _dBQuestion.GetPaged(filter, null, 1, int.MaxValue);

            return lst.lstResult;
        }
        public async Task<MongoResultPaged<Question>> QuestionListAll(string filterText, string TrainingTypeId, string TrainingCategoryId, int pageNumber = 1, int PageSize = 15)
        {
            //var lst = await _dBQuestion.ListAll(pageNumber, PageSize);
            //var lst = await _dBQuestion.QuestionListAllSearch(filterText, pageNumber, PageSize);
            //var filter = Builders<Question>.Filter.Where(x => x.IsActive == true);
            var filter = Builders<Question>.Filter.Where(x => x.Name.Contains(filterText));
            //if (!string.IsNullOrEmpty(filterText))
            //{
            //    filter = filter & Builders<Question>.Filter.Where(x => x.Name.Contains(filterText));
            //}
            if (!string.IsNullOrEmpty(TrainingTypeId))
            {
                filter = filter & Builders<Question>.Filter.Where(x => x.TrainingTypeId == TrainingTypeId);
            }
            if (!string.IsNullOrEmpty(TrainingCategoryId))
            {
                filter = filter & Builders<Question>.Filter.Where(x => x.TrainingCategoryId == TrainingCategoryId);
            }

            try
            {
                var sort = Builders<Question>.Sort.Descending(x => x.CreatedAt);
                var lst = await _dBQuestion.GetPaged(filter, null, pageNumber, PageSize);

                return lst;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> IsQuestionExist(string Id)
        {
            var obj = await _dBQuestion.GetById(Id);
            return obj != null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;

namespace Tadrebat.Interface
{
    public interface IQuestion
    {
        Task<Question> QuestionGetById(string Id);
        Task<bool> QuestionCreate(Question obj);
        Task<bool> QuestionUpdate(Question obj);
        Task<bool> QuestionDeActivate(string Id);
        Task<bool> QuestionActivate(string Id);
        Task<List<Question>> QuestionListActive();
        Task<List<Question>> QuestionListActiveByTrainingCategoryId(string TrainingCategoryId);
        Task<MongoResultPaged<Question>> QuestionListAll(string filterText, string TrainingTypeId, string TrainingCategoryId, int pageNumber = 1, int PageSize = 15);
        Task<bool> IsQuestionExist(string Id);
    }
}

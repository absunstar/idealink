using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;

namespace Tadrebat.Interface
{
    public interface IExamTemplate
    {
        Task<ExamTemplate> ExamTemplateGetById(string Id);
        Task<bool> ExamTemplateCreate(ExamTemplate obj);
        Task<bool> ExamTemplateUpdate(ExamTemplate obj);
        Task<bool> ExamTemplateDeActivate(string Id);
        Task<bool> ExamTemplateActivate(string Id);
        Task<List<ExamTemplate>> ExamTemplateListActive();
        Task<MongoResultPaged<ExamTemplate>> ExamTemplateListAll(string filterText, int pageNumber = 1, int PageSize = 15);
        Task<bool> IsExamTemplateExist(string Id);
    }
}

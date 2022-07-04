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
    public class ServiceExamTemplate : IExamTemplate
    {
        private readonly IDBExamTemplate _dBExamTemplate;
        public ServiceExamTemplate(IDBExamTemplate dBExamTemplate)
        {
            _dBExamTemplate = dBExamTemplate;
        }
        public async Task<ExamTemplate> ExamTemplateGetById(string Id)
        {
            return await _dBExamTemplate.GetById(Id);
        }
        public async Task<bool> ExamTemplateCreate(ExamTemplate obj)
        {
            await _dBExamTemplate.AddAsync(obj);

            return true;
        }
        public async Task<bool> ExamTemplateUpdate(ExamTemplate obj)
        {
            var quest = await ExamTemplateGetById(obj._id);
            if (quest == null)
                return false;

            await _dBExamTemplate.UpdateObj(obj._id, obj);

            return true;
        }
        public async Task<bool> ExamTemplateDeActivate(string Id)
        {

            var obj = await ExamTemplateGetById(Id);
            if (obj == null)
                return false;

            await _dBExamTemplate.DeactivateAsync(Id);

            return true;
        }
        public async Task<bool> ExamTemplateActivate(string Id)
        {
            var obj = await ExamTemplateGetById(Id);
            if (obj == null)
                return false;

            await _dBExamTemplate.ActivateAsync(Id);

            return true;
        }
        public async Task<List<ExamTemplate>> ExamTemplateListActive()
        {
            var sort = Builders<ExamTemplate>.Sort.Descending(x => x.Name);
            var lst = await _dBExamTemplate.ListActive(sort);
            return lst;
        }
        
        public async Task<MongoResultPaged<ExamTemplate>> ExamTemplateListAll(string filterText, int pageNumber = 1, int PageSize = 15)
        {
            
            var filter = Builders<ExamTemplate>.Filter.Where(x => x.Name.Contains(filterText));
            var sort = Builders<ExamTemplate>.Sort.Descending(x => x.CreatedAt);
            var lst = await _dBExamTemplate.GetPaged(filter, null, pageNumber, PageSize);

            return lst;
        }

        public async Task<bool> IsExamTemplateExist(string Id)
        {
            var obj = await _dBExamTemplate.GetById(Id);
            return obj != null;
        }
    }
}

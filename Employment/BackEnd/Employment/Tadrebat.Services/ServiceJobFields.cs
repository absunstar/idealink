using Employment.Entity.Mongo;
using Employment.Interface;
using Employment.MongoDB.Interface;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Services
{
   public class ServiceJobFields : ServiceRepository<JobFields>, IServiceJobFields
    {
        private readonly IDBJobFields _dBJobFields;
        public ServiceJobFields(IDBJobFields dBJobFields) : base(dBJobFields)
        {
            _dBJobFields = dBJobFields;
        }
        public async Task<List<JobFields>> ListAnyJobFields()
        {
            var filter = Builders<JobFields>.Filter.Empty;
            var sort = Builders<JobFields>.Sort.Ascending(x => x.Name);
            var lst = await _dBJobFields.GetPaged(filter, sort, 1, int.MaxValue);
            return lst.lstResult;
        }
        public async Task<bool> SubCreate(string MainId, string Name)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(MainId))
                return false;

            return await _dBJobFields.SubCreate(MainId, Name);
        }
        public async Task<bool> SubUpdate(string MainId, string Id, string Name)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(MainId) || string.IsNullOrEmpty(Id))
                return false;

            await _dBJobFields.SubUpdate(MainId, Id, Name);

            return true;
        }
        public async Task<bool> SubActivate(string MainId, string Id)
        {
            if (string.IsNullOrEmpty(MainId) || string.IsNullOrEmpty(Id))
                return false;

            await _dBJobFields.SubActivate(MainId, Id);

            return true;
        }
        public async Task<bool> SubDeActivate(string MainId, string Id)
        {
            if (string.IsNullOrEmpty(MainId) || string.IsNullOrEmpty(Id))
                return false;

            await _dBJobFields.SubDeActivate(MainId, Id);

            return true;
        }

        //public async Task<bool> JobFieldSaveTranslate(List<TranslateData> Data)
        //{
        //    foreach (var item in Data)
        //    {
        //        var obj = await GetById(item._id);
        //        if (obj == null)
        //            continue;

        //        var update = Builders<JobFields>.Update.Set(s => s.Name2, item.Name2);
        //        await _dBJobFields.UpdateAsync(item._id, update);
        //    }
        //    return true;
        //}
        public async Task<bool> JobSubFieldsSaveTranslate(string JobFieldId, List<TranslateData> Data)
        {
            var jobField = await GetById(JobFieldId);
            if (jobField != null)
            {
                foreach (var item in Data)
                {
                    var obj = jobField.subItems.Where(x => x._id == item._id).FirstOrDefault();
                    if (obj == null)
                        continue;

                    obj.Name2 = item.Name2;
                }
                await _dBJobFields.UpdateObj(JobFieldId, jobField);
            }
            return true;
        }
    }
}

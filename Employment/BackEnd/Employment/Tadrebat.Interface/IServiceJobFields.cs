using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Employment.Entity.Mongo;

namespace Employment.Interface
{
    public interface IServiceJobFields : IServiceRepository<JobFields>
    {
        Task<List<JobFields>> ListAnyJobFields();
        Task<bool> SubCreate(string MainId, string Name);
        Task<bool> SubUpdate(string MainId, string Id, string Name);
        Task<bool> SubActivate(string MainId, string Id);
        Task<bool> SubDeActivate(string MainId, string Id);
        //Task<bool> JobFieldSaveTranslate(List<TranslateData> Data);
        Task<bool> JobSubFieldsSaveTranslate(string JobFieldId, List<TranslateData> Data);
    }
}

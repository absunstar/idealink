using Employment.Entity.Mongo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Interface
{
    public interface IServiceCountry : IServiceRepository<Country>
    {
        Task<bool> SubCreate(string MainId, string Name);
        Task<bool> SubUpdate(string MainId, string Id, string Name);
        Task<bool> SubActivate(string MainId, string Id);
        Task<bool> SubDeActivate(string MainId, string Id);
        //Task<bool> CountrySaveTranslate(List<TranslateData> Data);
        Task<bool> CitySaveTranslate(string CountryId, List<TranslateData> Data);
    }
}

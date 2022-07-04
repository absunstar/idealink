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
    public class ServiceCountry : ServiceRepository<Country>, IServiceCountry
    {
        private readonly IDBCountry _dBCountrys;
        public ServiceCountry(IDBCountry dBCountrys) : base(dBCountrys)
        {
            _dBCountrys = dBCountrys;
        }
        public async Task<bool> SubCreate(string MainId, string Name)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(MainId))
                return false;

            return await _dBCountrys.SubCreate(MainId, Name);
        }
        public async Task<bool> SubUpdate(string MainId, string Id, string Name)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(MainId) || string.IsNullOrEmpty(Id))
                return false;

            await _dBCountrys.SubUpdate(MainId, Id, Name);

            return true;
        }
        public async Task<bool> SubActivate(string MainId, string Id)
        {
            if (string.IsNullOrEmpty(MainId) || string.IsNullOrEmpty(Id))
                return false;

            await _dBCountrys.SubActivate(MainId, Id);

            return true;
        }
        public async Task<bool> SubDeActivate(string MainId, string Id)
        {
            if (string.IsNullOrEmpty(MainId) || string.IsNullOrEmpty(Id))
                return false;

            await _dBCountrys.SubDeActivate(MainId, Id);

            return true;
        }
        //public async Task<bool> CountrySaveTranslate(List<TranslateData> Data)
        //{
        //    foreach (var item in Data)
        //    {
        //        var obj = await GetById(item._id);
        //        if (obj == null)
        //            continue;

        //        var update = Builders<Country>.Update.Set(s => s.Name2, item.Name2);
        //        await _dBCountrys.UpdateAsync(item._id, update);
        //    }
        //    return true;
        //}
        public async Task<bool> CitySaveTranslate(string CountryId, List<TranslateData> Data)
        {
            var country = await GetById(CountryId);
            if (country != null)
            {
                foreach (var item in Data)
                {
                    var obj = country.subItems.Where(x => x._id == item._id).FirstOrDefault();
                    if (obj == null)
                        continue;

                    obj.Name2 = item.Name2;
                }
                await _dBCountrys.UpdateObj(CountryId, country);
            }
            return true;
        }
    }
}

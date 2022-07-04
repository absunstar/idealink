using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Employment.Entity.Mongo;
using Employment.Interface;

namespace Employment.Cache
{
    public class CacheData : ICacheData
    {
        private IMemoryCache _cache;
        private IDataManagement BLDataLookup;
        public CacheData(IMemoryCache cache, IDataManagement _BLDataLookup)
        {
            _cache = cache;
            BLDataLookup = _BLDataLookup;
        }
        //public async Task<List<TrainingCategory>> GetTrainingCategory()
        //{
        //    var lst = _cache.Get<List<TrainingCategory>>("TrainingCategory");
        //    if (lst == null)
        //    {
        //        var result = await BLDataLookup.TrainingCategoryListActive();
        //        _cache.Set("TrainingCategory", lst);
        //    }

        //    return lst;
        //}
    }
}

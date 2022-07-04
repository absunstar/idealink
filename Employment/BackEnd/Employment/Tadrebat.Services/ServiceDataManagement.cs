using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employment.Interface;
using Employment.Entity.Mongo;
using Employment.MongoDB.Interface;
using MongoDB.Driver;

namespace Employment.Services
{
    public class ServiceDataManagement : IDataManagement
    {
        private readonly IDBNGOType _dBNGOType;
        public ServiceDataManagement(IDBNGOType dBNGOType)
        {
            _dBNGOType = dBNGOType;
        }
        #region NGOType
        public async Task<NGOType> NGOTypeGetById(string Id)
        {
            return await _dBNGOType.GetById(Id);
        }
        public async Task<bool> NGOTypeCreate(string Name)
        {
            if (string.IsNullOrEmpty(Name))
                return false;

            var obj = new NGOType();
            obj.Name = Name;

            await _dBNGOType.AddAsync(obj);

            return true;
        }
        public async Task<bool> NGOTypeUpdate(string Id, string Name)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Id))
                return false;

            await _dBNGOType.UpdateName(Id, Name);

            return true;
        }
        public async Task<bool> NGOTypeDeActivate(string Id)
        {
            var obj = await NGOTypeGetById(Id);
            if (obj == null)
                return false;

            await _dBNGOType.DeactivateAsync(Id);

            return true;
        }
        public async Task<bool> NGOTypeActivate(string Id)
        {
            var obj = await NGOTypeGetById(Id);
            if (obj == null)
                return false;

            await _dBNGOType.ActivateAsync(Id);

            return true;
        }
        public async Task<List<NGOType>> NGOTypeListActive()
        {
            var sort = Builders<NGOType>.Sort.Descending(x => x.Name);
            var lst = await _dBNGOType.ListActive(sort);
            return lst;
        }
        public async Task<MongoResultPaged<NGOType>> NGOTypeListAll(string filterText, int pageNumber = 1, int PageSize = 15)
        {
            //var lst = await _dBNGOType.ListAll(pageNumber, PageSize);
            var lst = await _dBNGOType.NGOTypeListAllSearch(filterText, pageNumber, PageSize);
            return lst;
        }

        public async Task<bool> IsNGOTypeExist(string Id)
        {
            var obj = await _dBNGOType.GetById(Id);
            return obj != null;
        }
        #endregion

    }
}

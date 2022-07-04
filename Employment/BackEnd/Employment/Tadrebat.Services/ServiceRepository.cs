using Employment.Entity.Mongo;
using Employment.Interface;
using Employment.MongoDB.Interface;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Services
{
    public class ServiceRepository<T> : IServiceRepository<T> where T : MongoEntityNameBase, new()
    {
        private readonly IRepositoryMongo<T> _dBLayer;
        public ServiceRepository(IRepositoryMongo<T> dBLayer)
        {
            _dBLayer = dBLayer;
        }
        public async Task<T> GetById(string Id)
        {
            return await _dBLayer.GetById(Id);
        }
        public async virtual Task<string> CreateReturnId(T obj)
        {
            await _dBLayer.AddAsync(obj);

            return obj._id;
        }
        public async virtual Task<bool> Create(T obj)
        {
            await _dBLayer.AddAsync(obj);

            return true;
        }
        public async virtual Task<bool> Create(string Name)
        {
            if (string.IsNullOrEmpty(Name))
                return false;

            var obj = new T();
            obj.Name = Name;

            await _dBLayer.AddAsync(obj);

            return true;
        }
        public async virtual Task<bool> Update(T obj)
        {
            if (obj == null)
                return false;

            var update = Builders<T>.Update.Set(x => x.Name, obj.Name);
            await _dBLayer.UpdateAsync(obj._id, update);

            return true;
        }
        public async Task<bool> DeActivate(string Id)
        {
            var obj = await GetById(Id);
            if (obj == null)
                return false;

            await _dBLayer.DeactivateAsync(Id);

            return true;
        }
        public async Task<bool> Activate(string Id)
        {
            var obj = await GetById(Id);
            if (obj == null)
                return false;

            await _dBLayer.ActivateAsync(Id);

            return true;
        }
        public async Task<List<T>> ListActive()
        {
            var sort = Builders<T>.Sort.Ascending(x => x.Name);
            var lst = await _dBLayer.ListActive(sort);
            return lst;
        }
        public async Task<MongoResultPaged<T>> ListAll(string filterText, int pageNumber = 1, int PageSize = 15)
        {
            var sort = Builders<T>.Sort.Descending(x => x.CreatedAt);
            var filter = Builders<T>.Filter.Where(x => x.Name.ToLower().Contains(filterText.ToLower()));
            var lst = await _dBLayer.GetPaged(filter, sort, pageNumber, PageSize);
            return lst;
        }
        public async Task<MongoResultPaged<T>> ListAllByUserId(string filterText, string UserId, int pageNumber = 1, int PageSize = 15)
        {
            return await ListAll(filterText, pageNumber, PageSize);
        }
        public async Task<bool> IsExist(string Id)
        {
            var obj = await _dBLayer.GetById(Id);
            return obj != null;
        }
        public async Task<bool> SaveTranslate(List<TranslateData> Data)
        {
            foreach (var item in Data)
            {
                var obj = await GetById(item._id);
                if (obj == null)
                    continue;

                var update = Builders<T>.Update.Set(s => s.Name2, item.Name2);
                await _dBLayer.UpdateAsync(item._id, update);
            }
            return true;
        }
    }
}

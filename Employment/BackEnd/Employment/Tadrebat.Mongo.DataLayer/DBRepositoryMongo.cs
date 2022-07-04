using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Employment.MongoDB.Interface;
using Employment.Entity.Mongo;
using Employment.Cache;

namespace Employment.Mongo.DataLayer
{
    public class DBRepositoryMongo<T> : DBBase, IRepositoryMongo<T> where T : MongoEntityNameBase
    {
        protected IMongoCollection<T> _mongoCollection;
        public DBRepositoryMongo(IMongoDBContext mongoDBContext, string _pDBCollectionName) : base(mongoDBContext, _pDBCollectionName)
        {
            _mongoCollection = _mongoDB.GetCollection<T>(_DBCollectionName);
        }
        public async Task<T> GetById(string Id)
        {
            return await _mongoCollection.FindSync(x => x._id == Id).FirstOrDefaultAsync();
        }
        public async Task UpdateObj(string Id, T obj)
        {
            var filter = Builders<T>.Filter.Where(x => x._id == Id);
            await _mongoCollection.ReplaceOneAsync(filter, obj);
        }
        public async Task<List<T>> ListActive(SortDefinition<T> sort = null)
        {
            var filter = Builders<T>.Filter.Where(x => x.IsActive == true);
            if (sort == null)
                sort = Builders<T>.Sort.Descending(x => x._id);
            return await _mongoCollection.Find(filter).Sort(sort).ToListAsync();
        }
        public async Task<List<T>> ListActive(FilterDefinition<T> filter, SortDefinition<T> sort = null)
        {
            filter = filter & Builders<T>.Filter.Where(x => x.IsActive == true);
            if (sort == null)
                sort = Builders<T>.Sort.Descending(x => x._id);
            return await _mongoCollection.Find(filter).Sort(sort).ToListAsync();
        }
        public async Task<MongoResultPaged<T>> ListAll(int PageNumber = 1, int PageSize = 15)
        {
            var filter = Builders<T>.Filter.Empty;
            return await GetPaged(filter, null, PageNumber, PageSize);
        }
        public async Task<MongoResultPaged<T>> GetPaged(FilterDefinition<T> filter, SortDefinition<T> sort = null, int PageNumber = 1, int PageSize = 15)
        {
            int skip = PageSize * (PageNumber - 1);
            if (sort == null)
                sort = Builders<T>.Sort.Descending(x => x._id);

            var count = await _mongoCollection.CountAsync(filter);
            var lst = await _mongoCollection.Find(filter).Sort(sort).Skip(skip).Limit(PageSize).ToListAsync();
            return new MongoResultPaged<T>(count, lst, PageSize);
        }
        public async Task<T> GetOne(FilterDefinition<T> filter)
        {

            var obj = await _mongoCollection.FindSync(filter).SingleOrDefaultAsync();
            return obj;

        }
        public async Task AddAsync(T obj)
        {

            await _mongoCollection.InsertOneAsync(obj);

        }
        public async Task UpdateAsync(string Id, UpdateDefinition<T> update)
        {
            await _mongoCollection.UpdateOneAsync(x => x._id == Id, update);
        }
        public async Task UpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            await _mongoCollection.UpdateOneAsync(filter, update);
        }
        public async Task UpdateManyAsync(FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            await _mongoCollection.UpdateManyAsync(filter, update);
        }
        public async Task DeactivateAsync(string Id)
        {
            var update = UpdateActivation(false);
            await UpdateAsync(Id, update);
        }
        public async Task ActivateAsync(string Id)
        {
            var update = UpdateActivation(true);
            await UpdateAsync(Id, update);
        }
        public UpdateDefinition<T> UpdateActivation(bool flag)
        {
            return Builders<T>.Update.Set(s => s.IsActive, flag);
        }
        public async Task<bool> AddField(string Id, FieldDefinition<T> field, object value)
        {
            var filter = Builders<T>.Filter.Where(x => x._id == Id);
            var update = Builders<T>.Update.AddToSet(field, value);
            await _mongoCollection.UpdateOneAsync(filter, update);
            return true;
        }
        public async Task<bool> AddField(FilterDefinition<T> filter, FieldDefinition<T> field, object value)
        {
            var update = Builders<T>.Update.AddToSet(field, value);
            await _mongoCollection.UpdateOneAsync(filter, update);
            return true;
        }
        public async Task<bool> AddFieldList(string Id, FieldDefinition<T> field, object[] value)
        {
            var filter = Builders<T>.Filter.Where(x => x._id == Id);
            var update = Builders<T>.Update.AddToSetEach(field, value);
            await _mongoCollection.UpdateOneAsync(filter, update);
            return true;
        }
        public async Task<bool> AddFieldList(FilterDefinition<T> filter, FieldDefinition<T> field, object[] value)
        {
            var update = Builders<T>.Update.AddToSetEach(field, value);
            await _mongoCollection.UpdateOneAsync(filter, update);
            return true;
        }
        public async Task<bool> RemoveField(string Id, FieldDefinition<T> field, object value)
        {
            var filter = Builders<T>.Filter.Where(x => x._id == Id);
            var update = Builders<T>.Update.Pull(field, value);
            try { 
            await _mongoCollection.UpdateOneAsync(filter, update);
            }catch (Exception ex)
            {

            }
            return true;
        }
        public async Task<bool> RemoveField(FilterDefinition<T> filter, FieldDefinition<T> field, object value)
        {
            var update = Builders<T>.Update.Pull(field, value);
            await _mongoCollection.UpdateOneAsync(filter, update);
            return true;
        }
        public async Task<bool> RemoveFieldList(string Id, FieldDefinition<T> field, object[] value)
        {
            var filter = Builders<T>.Filter.Where(x => x._id == Id);
            var update = Builders<T>.Update.PullAll(field, value);
            await _mongoCollection.UpdateOneAsync(filter, update);
            return true;
        }
        public async Task<bool> RemoveFieldList(FilterDefinition<T> filter, FieldDefinition<T> field, object[] value)
        {
            var update = Builders<T>.Update.PullAll(field, value);
            await _mongoCollection.UpdateOneAsync(filter, update);
            return true;
        }
        public async Task<bool> RemoveField(string Id, UpdateDefinition<T> pull)
        {
            var filter = Builders<T>.Filter.Where(x => x._id == Id);
            //var update = Builders<T>.Update.Pull(field, value);

            await _mongoCollection.UpdateOneAsync(filter, pull);
            return true;
        }
        public async Task<long> Count(FilterDefinition<T> filter)
        {
            try { 
            var count = await _mongoCollection.CountAsync(filter);
            return count;
            }
            catch(Exception ex)
            {

            }
            return 0;
        }
    }
}

using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Employment.Entity.Mongo;

namespace Employment.MongoDB.Interface
{
    public interface IRepositoryMongo<T>
    {
        Task<T> GetById(string Id);
        Task UpdateObj(string Id, T obj);
        Task<List<T>> ListActive(SortDefinition<T> sort = null);
        Task<List<T>> ListActive(FilterDefinition<T> filter, SortDefinition<T> sort = null);
        Task<MongoResultPaged<T>> ListAll(int PageNumber = 1, int PageSize = 15); 
        Task<MongoResultPaged<T>> GetPaged(FilterDefinition<T> filter, SortDefinition<T> sort, int PageNumber = 1, int PageSize = 15);
        Task<T> GetOne(FilterDefinition<T> filter);
        Task AddAsync(T obj);
        Task UpdateAsync(string Id, UpdateDefinition<T> update);
        Task UpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> update);
        Task UpdateManyAsync(FilterDefinition<T> filter, UpdateDefinition<T> update);
        Task DeactivateAsync(string Id);
        Task ActivateAsync(string Id);
        UpdateDefinition<T> UpdateActivation(bool flag);
        Task<bool> AddField(string Id, FieldDefinition<T> field, object value);
        Task<bool> AddField(FilterDefinition<T> filter, FieldDefinition<T> field, object value);
        Task<bool> AddFieldList(string Id, FieldDefinition<T> field, object[] value);
        Task<bool> AddFieldList(FilterDefinition<T> filter, FieldDefinition<T> field, object[] value);
        Task<bool> RemoveField(string Id, FieldDefinition<T> field, object value);
        Task<bool> RemoveField(FilterDefinition<T> filter, FieldDefinition<T> field, object value);
        Task<bool> RemoveFieldList(string Id, FieldDefinition<T> field, object[] value);
        Task<bool> RemoveFieldList(FilterDefinition<T> filter, FieldDefinition<T> field, object[] value);
        Task<bool> RemoveField(string Id, UpdateDefinition<T> pull);
        Task<long> Count(FilterDefinition<T> filter);

    }
}

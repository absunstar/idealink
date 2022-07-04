using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;

namespace Tadrebat.MongoDB.Interface
{
    public interface IDBTrainingCategory : IRepositoryMongo<TrainingCategory>
    {
        Task UpdateName(string Id, string Name, string TrainingTypeId);
        Task<MongoResultPaged<TrainingCategory>> GetPaged(int CurrentPage, int PageSize);
        Task<MongoResultPaged<TrainingCategory>> TrainingCategoryListAllSearch(string filterText, int CurrentPage = 1, int PageSize = 15);

        Task<bool> CourseCreate(string TrainingCategoryId, string Name);
        Task<bool> CourseUpdate(string TrainingCategoryId, string CourseId, string Name);
        Task<bool> CourseDeActivate(string TrainingCategoryId, string Id);
        Task<bool> CourseActivate(string TrainingCategoryId, string Id);
    }
}

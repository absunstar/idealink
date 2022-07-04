using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;

namespace Tadrebat.Interface
{
    public interface IDataManagement
    {
        #region TrainingType
        Task<TrainingType> TrainingTypeGetById(string Id);
        Task<bool> TrainingTypeCreate(string Name);
        Task<bool> TrainingTypeUpdate(string Id, string Name);
        Task<bool> TrainingTypeDeActivate(string Id);
        Task<bool> TrainingTypeActivate(string Id);
        Task<List<TrainingType>> TrainingTypeListActive();
        Task<MongoResultPaged<TrainingType>> TrainingTypeListAll(string filterText, int pageNumber = 1, int PageSize = 15);
        Task<bool> IsTrainingTypeExist(string Id);
        Task<bool> TrainingTypeSaveTranslate(List<TranslateData> Data);
        #endregion
        #region TrainingCategory
        Task<TrainingCategory> TrainingCategoryGetById(string Id);
        Task<bool> TrainingCategoryCreate(string Name, string TrainingTypeId);
        Task<bool> TrainingCategoryUpdate(string Id, string Name, string TrainingTypeId);
        Task<bool> TrainingCategoryDeActivate(string Id);
        Task<bool> TrainingCategoryActivate(string Id);
        Task<List<TrainingCategory>> TrainingCategoryListActive();
        Task<List<TrainingCategory>> TrainingCategoryListByTrainingType(string TrainingTypeId); 
        Task<MongoResultPaged<TrainingCategory>> TrainingCategoryListAll(string filterText, int pageNumber = 1, int PageSize = 15);
        Task<bool> IsTrainingCategoryExist(string Id);
        Task<bool> TrainingCategorySaveTranslate(List<TranslateData> Data);
        #endregion
        #region Course
        Task<bool> CourseCreate(string TrainingCategoryId, string Name);
        Task<bool> CourseUpdate(string TrainingCategoryId, string Id, string Name); 
        Task<bool> CourseDeActivate(string TrainingCategoryId, string Id);
        Task<bool> CourseActivate(string TrainingCategoryId, string Id);
        Task<bool> CourseSaveTranslate(string TrainingCategoryId, List<TranslateData> Data);
        #endregion
        #region NGOType
        Task<NGOType> NGOTypeGetById(string Id);
        Task<bool> NGOTypeCreate(string Name);
        Task<bool> NGOTypeUpdate(string Id, string Name);
        Task<bool> NGOTypeDeActivate(string Id);
        Task<bool> NGOTypeActivate(string Id);
        Task<List<NGOType>> NGOTypeListActive();
        Task<MongoResultPaged<NGOType>> NGOTypeListAll(string filterText, int pageNumber = 1, int PageSize = 15);
        Task<bool> IsNGOTypeExist(string Id);
        #endregion
        #region City
        Task<City> CityGetById(string Id);
        Task<bool> CityCreate(string Name);
        Task<bool> CityUpdate(string Id, string Name);
        Task<bool> CityDeActivate(string Id);
        Task<bool> CityActivate(string Id);
        Task<List<City>> CityListActive();
        Task<MongoResultPaged<City>> CityListAll(string filterText, int pageNumber = 1, int PageSize = 15);
        Task<bool> IsCityExist(string Id);
        Task<bool> CitySaveTranslate(List<TranslateData> Data);
        #endregion
        #region Area
        Task<bool> AreaCreate(string CityId, string Name);
        Task<bool> AreaUpdate(string CityId, string Id, string Name);
        Task<bool> AreaDeActivate(string CityId, string Id);
        Task<bool> AreaActivate(string CityId, string Id);
        Task<bool> AreaSaveTranslate(string CityId, List<TranslateData> Data);
        #endregion
    }
}

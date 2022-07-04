using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Interface;
using Tadrebat.Entity.Mongo;
using Tadrebat.MongoDB.Interface;
using Tadrebat.Persistance.Interfaces;
using MongoDB.Driver;

namespace Tadrebat.Services
{
    public class ServiceDataManagement : IDataManagement
    {
        private readonly IDBTrainingType _dBTrainingType;
        private readonly IDBTrainingCategory _dBTrainingCategory;
        private readonly IDBNGOType _dBNGOType;
        private readonly IDBCity _dBCity;
        public ServiceDataManagement(IDBTrainingCategory dBTrainingCategory
                                , IDBTrainingType dBTrainingType
                                , IDBNGOType dBNGOType
                                , IDBCity dBCity)
        {
            _dBTrainingType = dBTrainingType;
            _dBTrainingCategory = dBTrainingCategory;
            _dBNGOType = dBNGOType;
            _dBCity = dBCity;
        }
        #region TrainingType
        public async Task<TrainingType> TrainingTypeGetById(string Id)
        {
            return await _dBTrainingType.GetById(Id);
        }
        public async Task<bool> TrainingTypeCreate(string Name)
        {
            if (string.IsNullOrEmpty(Name))
                return false;

            var obj = new TrainingType();
            obj.Name = Name;

            await _dBTrainingType.AddAsync(obj);

            return true;
        }
        public async Task<bool> TrainingTypeUpdate(string Id, string Name)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Id))
                return false;

            await _dBTrainingType.UpdateName(Id, Name);

            return true;
        }
        public async Task<bool> TrainingTypeDeActivate(string Id)
        {
            var obj = await TrainingTypeGetById(Id);
            if (obj == null)
                return false;

            await _dBTrainingType.DeactivateAsync(Id);

            return true;
        }
        public async Task<bool> TrainingTypeActivate(string Id)
        {
            var obj = await TrainingTypeGetById(Id);
            if (obj == null)
                return false;

            await _dBTrainingType.ActivateAsync(Id);

            return true;
        }
        public async Task<List<TrainingType>> TrainingTypeListActive()
        {
            var sort = Builders<TrainingType>.Sort.Descending(x => x.Name);
            var lst = await _dBTrainingType.ListActive(sort);
            return lst;
        }
        public async Task<MongoResultPaged<TrainingType>> TrainingTypeListAll(string filterText, int pageNumber = 1, int PageSize = 15)
        {
            //var lst = await _dBTrainingType.ListAll(pageNumber, PageSize);
            var lst = await _dBTrainingType.TrainingTypeListAllSearch(filterText, pageNumber, PageSize);
            return lst;
        }

        public async Task<bool> IsTrainingTypeExist(string Id)
        {
            var obj = await _dBTrainingType.GetById(Id);
            return obj != null;
        }
        public async Task<bool> TrainingTypeSaveTranslate(List<TranslateData> Data)
        {
            foreach (var item in Data)
            {
                var obj = await TrainingTypeGetById(item._id);
                if (obj == null)
                    continue;

                var update = Builders<TrainingType>.Update.Set(s => s.Name2, item.Name2).Set(s => s.Name3, item.Name3);
                await _dBTrainingType.UpdateAsync(item._id, update);
            }
            return true;
        }
        #endregion
        #region TrainingCategory
        public async Task<TrainingCategory> TrainingCategoryGetById(string Id)
        {
            return await _dBTrainingCategory.GetById(Id);
        }
        public async Task<bool> TrainingCategoryCreate(string Name, string TrainingTypeId)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(TrainingTypeId))
                return false;

            var obj = new TrainingCategory();
            obj.Name = Name;
            obj.TrainingTypeId = TrainingTypeId;
            await _dBTrainingCategory.AddAsync(obj);

            return true;
        }
        public async Task<bool> TrainingCategoryUpdate(string Id, string Name, string TrainingTypeId)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(TrainingTypeId))
                return false;

            await _dBTrainingCategory.UpdateName(Id, Name, TrainingTypeId);

            return true;
        }
        public async Task<bool> TrainingCategoryDeActivate(string Id)
        {
            var obj = await TrainingCategoryGetById(Id);
            if (obj == null)
                return false;

            await _dBTrainingCategory.DeactivateAsync(Id);

            return true;
        }
        public async Task<bool> TrainingCategoryActivate(string Id)
        {
            var obj = await TrainingCategoryGetById(Id);
            if (obj == null)
                return false;

            await _dBTrainingCategory.ActivateAsync(Id);

            return true;
        }
        public async Task<List<TrainingCategory>> TrainingCategoryListActive()
        {
            var sort = Builders<TrainingCategory>.Sort.Descending(x => x.Name);
            var lst = await _dBTrainingCategory.ListActive(sort);
            return lst;
        }
        public async Task<List<TrainingCategory>> TrainingCategoryListByTrainingType(string TrainingTypeId)
        {
            var filter = Builders<TrainingCategory>.Filter.Where(x => x.TrainingTypeId == TrainingTypeId);
            var sort = Builders<TrainingCategory>.Sort.Descending(x => x.Name);
            var lst = await _dBTrainingCategory.ListActive(filter, sort);
            return lst;
        }
        public async Task<MongoResultPaged<TrainingCategory>> TrainingCategoryListAll(string filterText, int pageNumber = 1, int PageSize = 15)
        {
            //var lst = await _dBTrainingCategory.ListAll(pageNumber, PageSize);
            var lst = await _dBTrainingCategory.TrainingCategoryListAllSearch(filterText, pageNumber, PageSize);
            return lst;
        }

        public async Task<bool> IsTrainingCategoryExist(string Id)
        {
            var obj = await _dBTrainingCategory.GetById(Id);
            return obj != null;
        }
        public async Task<bool> TrainingCategorySaveTranslate(List<TranslateData> Data)
        {
            foreach (var item in Data)
            {
                var obj = await TrainingCategoryGetById(item._id);
                if (obj == null)
                    continue;

                var update = Builders<TrainingCategory>.Update.Set(s => s.Name2, item.Name2).Set(s => s.Name3, item.Name3);
                await _dBTrainingCategory.UpdateAsync(item._id, update);
            }
            return true;
        }
        #endregion
        #region Course
        public async Task<bool> CourseCreate(string TrainingCategoryId, string Name)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(TrainingCategoryId))
                return false;

            return await _dBTrainingCategory.CourseCreate(TrainingCategoryId, Name);
        }
        public async Task<bool> CourseUpdate(string TrainingCategoryId, string Id, string Name)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(TrainingCategoryId) || string.IsNullOrEmpty(Id))
                return false;

            await _dBTrainingCategory.CourseUpdate(TrainingCategoryId, Id, Name);

            return true;
        }
        public async Task<bool> CourseActivate(string TrainingCategoryId, string Id)
        {
            if (string.IsNullOrEmpty(TrainingCategoryId) || string.IsNullOrEmpty(Id))
                return false;

            await _dBTrainingCategory.CourseActivate(TrainingCategoryId, Id);

            return true;
        }
        public async Task<bool> CourseDeActivate(string TrainingCategoryId, string Id)
        {
            if (string.IsNullOrEmpty(TrainingCategoryId) || string.IsNullOrEmpty(Id))
                return false;

            await _dBTrainingCategory.CourseDeActivate(TrainingCategoryId, Id);

            return true;
        }
        public async Task<bool> CourseSaveTranslate(string TrainingCategoryId, List<TranslateData> Data)
        {
            var trainingCategory = await TrainingCategoryGetById(TrainingCategoryId);
            if (trainingCategory != null)
            {
                foreach (var item in Data)
                {
                    var obj = trainingCategory.Course.Where(x => x._id == item._id).FirstOrDefault();
                    if (obj == null)
                        continue;

                    obj.Name2 = item.Name2;
                    obj.Name3 = item.Name3;
                }
                await _dBTrainingCategory.UpdateObj(TrainingCategoryId, trainingCategory);
            }
            return true;
        }

        #endregion
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
        #region City
        public async Task<City> CityGetById(string Id)
        {
            return await _dBCity.GetById(Id);
        }
        public async Task<bool> CityCreate(string Name)
        {
            if (string.IsNullOrEmpty(Name))
                return false;

            var obj = new City();
            obj.Name = Name;

            await _dBCity.AddAsync(obj);

            return true;
        }
        public async Task<bool> CityUpdate(string Id, string Name)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Id))
                return false;

            await _dBCity.UpdateName(Id, Name);

            return true;
        }
        public async Task<bool> CityDeActivate(string Id)
        {
            var obj = await CityGetById(Id);
            if (obj == null)
                return false;

            await _dBCity.DeactivateAsync(Id);

            return true;
        }
        public async Task<bool> CityActivate(string Id)
        {
            var obj = await CityGetById(Id);
            if (obj == null)
                return false;

            await _dBCity.ActivateAsync(Id);

            return true;
        }
        public async Task<List<City>> CityListActive()
        {
            var sort = Builders<City>.Sort.Descending(x => x.Name);
            var lst = await _dBCity.ListActive(sort);
            return lst;
        }
        public async Task<MongoResultPaged<City>> CityListAll(string filterText, int pageNumber = 1, int PageSize = 15)
        {
            var lst = await _dBCity.CityListAllSearch(filterText, pageNumber, PageSize);
            return lst;
        }

        public async Task<bool> IsCityExist(string Id)
        {
            var obj = await _dBCity.GetById(Id);
            return obj != null;
        }
        public async Task<bool> CitySaveTranslate(List<TranslateData> Data)
        {
            foreach (var item in Data)
            {
                var obj = await CityGetById(item._id);
                if (obj == null)
                    continue;

                var update = Builders<City>.Update.Set(s => s.Name2, item.Name2).Set(s => s.Name3, item.Name3);
                await _dBCity.UpdateAsync(item._id, update);
            }
            return true;
        }
        #endregion
        #region Area
        public async Task<bool> AreaCreate(string CityId, string Name)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(CityId))
                return false;

            return await _dBCity.AreaCreate(CityId, Name);
        }
        public async Task<bool> AreaUpdate(string CityId, string Id, string Name)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(CityId) || string.IsNullOrEmpty(Id))
                return false;

            await _dBCity.AreaUpdate(CityId, Id, Name);

            return true;
        }
        public async Task<bool> AreaActivate(string CityId, string Id)
        {
            if (string.IsNullOrEmpty(CityId) || string.IsNullOrEmpty(Id))
                return false;

            await _dBCity.AreaActivate(CityId, Id);

            return true;
        }
        public async Task<bool> AreaDeActivate(string CityId, string Id)
        {
            if (string.IsNullOrEmpty(CityId) || string.IsNullOrEmpty(Id))
                return false;

            await _dBCity.AreaDeActivate(CityId, Id);

            return true;
        }
        public async Task<bool> AreaSaveTranslate(string CityId, List<TranslateData> Data)
        {
            var city = await CityGetById(CityId);
            if (city != null)
            {
                foreach (var item in Data)
                {
                    var obj = city.areas.Where(x=>x._id == item._id).FirstOrDefault();
                    if (obj == null)
                        continue;

                    obj.Name2 = item.Name2;
                    obj.Name3 = item.Name3;
                }
                await _dBCity.UpdateObj(CityId, city);
            }
            return true;
        }
        #endregion
    }
}

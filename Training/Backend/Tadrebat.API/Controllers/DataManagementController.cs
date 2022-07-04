using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tadrebat.API.Helpers.AutoMapper;
using Tadrebat.API.Model.Model;
using Tadrebat.API.Model.Response;
using Tadrebat.Entity.Mongo;
using Tadrebat.Interface;

namespace Tadrebat.API.Controllers
{
    //[AllowAnonymous]
    [Authorize(Roles = "Admin,Partner,SubPartner, Trainer, Trainee")]
    public class DataManagementController : BaseController
    {
        private readonly IDataManagement BLDataManagement;
        private readonly HelperMapperData HLMapperData;
        private readonly HelperTranslate HLHelperTranslate;
        public string currentLang = "en";
        public DataManagementController(IDataManagement _BLDataManagement, IMapper mapper, HelperTranslate _HLHelperTranslate,
            HelperMapperData _HLMapperData) : base(mapper)
        {
            BLDataManagement = _BLDataManagement;
            HLMapperData = _HLMapperData;
            HLHelperTranslate = _HLHelperTranslate;

        }
        #region TrainingType
        public async Task<IActionResult> TrainingTypeGetById(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLDataManagement.TrainingTypeGetById(model.Id);
            var response = _mapper.Map<TrainingType, ResponseTrainingType>(result);

            currentLang = GetLanguage();
            response = await HLHelperTranslate.MapTrainingType(response, result, currentLang);

            return Ok(response);
        }
        public async Task<IActionResult> TrainingTypeListActive()
        {
            var result = await BLDataManagement.TrainingTypeListActive();
            var response = _mapper.Map<List<TrainingType>, List<ResponseTrainingType>>(result);

            currentLang = GetLanguage();
            response = await HLHelperTranslate.MapTrainingType(response, result, currentLang);
            
            return Ok(response.OrderBy(x => x.Name));
        }
        public async Task<IActionResult> TrainingTypeListAll(ModelPaged model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var result = await BLDataManagement.TrainingTypeListAll(model.filterText, model.CurrentPage, model.PageSize);
            var response = _mapper.Map<MongoResultPaged<TrainingType>, ResponsePaged<ResponseTrainingType>>(result);

            //currentLang = GetLanguage();
            //response = await HLHelperTranslate.MapTrainingType(response, result, currentLang);

            return Ok(response);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TrainingTypeCreate(ModelTrainingType model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLDataManagement.TrainingTypeCreate(model.Name);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TrainingTypeUpdate(ModelTrainingType model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (string.IsNullOrEmpty(model.Id))
                return BadRequest();

            var result = await BLDataManagement.TrainingTypeUpdate(model.Id, model.Name);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TrainingTypeActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLDataManagement.TrainingTypeActivate(model.Id);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TrainingTypeDeActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLDataManagement.TrainingTypeDeActivate(model.Id);

            return await FormatResult(result);
        }
        #endregion
        #region TrainingCategory
        public async Task<IActionResult> TrainingCategoryGetById(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLDataManagement.TrainingCategoryGetById(model.Id);
            //var response = _mapper.Map<TrainingCategory, ResponseTrainingCategory>(result);
            var response = await HLMapperData.MapTrainingCategory(result);

            //currentLang = GetLanguage();
            //response = await HLHelperTranslate.MapTrainingCategory(response, result, currentLang);

            return Ok(response);
        }
        public async Task<IActionResult> TrainingCategoryListActive()
        {
            var result = await BLDataManagement.TrainingCategoryListActive();
            //var response = _mapper.Map<List<TrainingCategory>, List<ResponseTrainingCategory>>(result);
            var response = await HLMapperData.MapTrainingCategory(result);

            currentLang = GetLanguage();
            response = await HLHelperTranslate.MapTrainingCategory(response, result, currentLang);

            return Ok(response.OrderBy(x => x.Name));
        }
        public async Task<IActionResult> TrainingCategoryListAll(ModelPaged model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var result = await BLDataManagement.TrainingCategoryListAll(model.filterText, model.CurrentPage, model.PageSize);
            var response = await HLMapperData.MapTrainingCategory(result);

            //currentLang = GetLanguage();
            //response = await HLHelperTranslate.MapTrainingCategory(response, result, currentLang);

            return Ok(response);
        }
        public async Task<IActionResult> TrainingCategoryListByTrainingType(ModelId model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var result = await BLDataManagement.TrainingCategoryListByTrainingType(model.Id);
            var response = await HLMapperData.MapTrainingCategory(result);

            currentLang = GetLanguage();
            response = await HLHelperTranslate.MapTrainingCategory(response, result, currentLang);

            return Ok(response);
        }
        public async Task<IActionResult> TrainingCategoryCreate(ModelTrainingCategory model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLDataManagement.TrainingCategoryCreate(model.Name, model.TrainingTypeId);

            return await FormatResult(result);
        }
        public async Task<IActionResult> TrainingCategoryUpdate(ModelTrainingCategory model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (string.IsNullOrEmpty(model.Id))
                return BadRequest();

            var result = await BLDataManagement.TrainingCategoryUpdate(model.Id, model.Name, model.TrainingTypeId);

            return await FormatResult(result);
        }
        public async Task<IActionResult> TrainingCategoryActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLDataManagement.TrainingCategoryActivate(model.Id);

            return await FormatResult(result);
        }
        public async Task<IActionResult> TrainingCategoryDeActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLDataManagement.TrainingCategoryDeActivate(model.Id);

            return await FormatResult(result);
        }
        #endregion
        #region Course
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CourseCreate(ModelCourse model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLDataManagement.CourseCreate(model.TrainingCategoryId, model.Name);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CourseUpdate(ModelCourse model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLDataManagement.CourseUpdate(model.TrainingCategoryId, model.Id, model.Name);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CourseActivate(ModelCourse model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLDataManagement.CourseActivate(model.TrainingCategoryId, model.Id);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CourseDeActivate(ModelCourse model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLDataManagement.CourseDeActivate(model.TrainingCategoryId, model.Id);

            return await FormatResult(result);
        }
        #endregion
        #region NGOType
        public async Task<IActionResult> NGOTypeGetById(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLDataManagement.NGOTypeGetById(model.Id);
            var response = _mapper.Map<NGOType, ResponseNGOType>(result);

            return Ok(response);
        }
        public async Task<IActionResult> NGOTypeListActive()
        {
            var result = await BLDataManagement.NGOTypeListActive();
            var response = _mapper.Map<List<NGOType>, List<ResponseNGOType>>(result);

            return Ok(response.OrderBy(x => x.Name));
        }
        public async Task<IActionResult> NGOTypeListAll(ModelPaged model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var result = await BLDataManagement.NGOTypeListAll(model.filterText, model.CurrentPage, model.PageSize);
            var response = _mapper.Map<MongoResultPaged<NGOType>, ResponsePaged<ResponseNGOType>>(result);

            return Ok(response);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> NGOTypeCreate(ModelNGOType model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLDataManagement.NGOTypeCreate(model.Name);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> NGOTypeUpdate(ModelNGOType model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (string.IsNullOrEmpty(model.Id))
                return BadRequest();

            var result = await BLDataManagement.NGOTypeUpdate(model.Id, model.Name);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> NGOTypeActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLDataManagement.NGOTypeActivate(model.Id);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> NGOTypeDeActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLDataManagement.NGOTypeDeActivate(model.Id);

            return await FormatResult(result);
        }
        #endregion
        #region City
        public async Task<IActionResult> CityGetById(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLDataManagement.CityGetById(model.Id);
            var response = _mapper.Map<City, ResponseCity>(result);

            //currentLang = GetLanguage();
            //response = await HLHelperTranslate.MapCity(response, result, currentLang);

            return Ok(response);
        }
        public async Task<IActionResult> CityListActive()
        {
            var result = await BLDataManagement.CityListActive();
            var response = _mapper.Map<List<City>, List<ResponseCity>>(result);

            currentLang = GetLanguage();
            response = await HLHelperTranslate.MapCity(response, result, currentLang);

            return Ok(response.OrderBy(x => x.Name));
        }
        public async Task<IActionResult> CityListAll(ModelPaged model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var result = await BLDataManagement.CityListAll(model.filterText, model.CurrentPage, model.PageSize);
            var response = _mapper.Map<MongoResultPaged<City>, ResponsePaged<ResponseCity>>(result);

            //currentLang = GetLanguage();
            //response = await HLHelperTranslate.MapCity(response, result, currentLang);

            return Ok(response);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CityCreate(ModelCity model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLDataManagement.CityCreate(model.Name);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CityUpdate(ModelCity model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (string.IsNullOrEmpty(model.Id))
                return BadRequest();

            var result = await BLDataManagement.CityUpdate(model.Id, model.Name);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CityActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLDataManagement.CityActivate(model.Id);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CityDeActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLDataManagement.CityDeActivate(model.Id);

            return await FormatResult(result);
        }
        #endregion
        #region Area
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AreaCreate(ModelArea model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLDataManagement.AreaCreate(model.CityId, model.Name);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AreaUpdate(ModelArea model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLDataManagement.AreaUpdate(model.CityId, model.Id, model.Name);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AreaActivate(ModelArea model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLDataManagement.AreaActivate(model.CityId, model.Id);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AreaDeActivate(ModelArea model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLDataManagement.AreaDeActivate(model.CityId, model.Id);

            return await FormatResult(result);
        }
        #endregion
    }
}
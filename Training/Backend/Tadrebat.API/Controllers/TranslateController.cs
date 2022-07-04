using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tadrebat.API.Model.Model;
using Tadrebat.API.Model.Response;
using Tadrebat.Entity.Mongo;
using Tadrebat.Interface;

namespace Tadrebat.API.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TranslateController : BaseController
    {
        private readonly IDataManagement BLDataManagement;
        public TranslateController(IMapper mapper, IDataManagement _BLDataManagement) : base(mapper)
        {
            BLDataManagement = _BLDataManagement;
        }
        public async Task<IActionResult> ListTranslationByType(ModelTranslateByType model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            switch (model.Type)
            {
                case Enum.EnumTranslateType.City:
                    var result = await BLDataManagement.CityListActive();
                    var data = _mapper.Map<List<City>, List<ResponseTranslateData>>(result);
                    var response = new ResponseTranslate(Enum.EnumTranslateType.City, data);

                    return Ok(response);
                case Enum.EnumTranslateType.Area:
                    var city = await BLDataManagement.CityGetById(model.Id);
                    var areas = city.areas.Where(x => x.IsActive == true).ToList();
                    var areasdata = _mapper.Map<List<Area>, List<ResponseTranslateData>>(areas);
                    var areasresponse = new ResponseTranslate(Enum.EnumTranslateType.Area, areasdata, model.Id);

                    return Ok(areasresponse);
                case Enum.EnumTranslateType.TrainingType:
                    var traingType = await BLDataManagement.TrainingTypeListActive();
                    var traingTypedata = _mapper.Map<List<TrainingType>, List<ResponseTranslateData>>(traingType);
                    var traingTyperesponse = new ResponseTranslate(Enum.EnumTranslateType.TrainingType, traingTypedata);

                    return Ok(traingTyperesponse);
                case Enum.EnumTranslateType.TrainingCategory:
                    var trainingCategory = await BLDataManagement.TrainingCategoryListActive();
                    var trainingCategorydata = _mapper.Map<List<TrainingCategory>, List<ResponseTranslateData>>(trainingCategory);
                    var trainingCategoryresponse = new ResponseTranslate(Enum.EnumTranslateType.TrainingCategory, trainingCategorydata);

                    return Ok(trainingCategoryresponse);
                case Enum.EnumTranslateType.Courses:
                    var objTrainingCategory = await BLDataManagement.TrainingCategoryGetById(model.Id);
                    var courses = objTrainingCategory.Course.Where(x => x.IsActive == true).ToList();
                    var coursesdata = _mapper.Map<List<Course>, List<ResponseTranslateData>>(courses);
                    var coursesresponse = new ResponseTranslate(Enum.EnumTranslateType.Courses, coursesdata, model.Id);

                    return Ok(coursesresponse);
            }
            return BadRequest();
        }
        public async Task<IActionResult> SaveListTranslation(ModelTranslate model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            switch (model.Type)
            {
                case Enum.EnumTranslateType.City:
                    var data = _mapper.Map<List<ModelTranslateData>, List<TranslateData>>(model.Data);
                    var result = await BLDataManagement.CitySaveTranslate(data);

                    return Ok(result);
                case Enum.EnumTranslateType.Area:
                    var areas = _mapper.Map<List<ModelTranslateData>, List<TranslateData>>(model.Data);
                    var resultarea = await BLDataManagement.AreaSaveTranslate(model.Id, areas);

                    return Ok(resultarea);
                case Enum.EnumTranslateType.TrainingCategory:
                    var dataTrainingCategory = _mapper.Map<List<ModelTranslateData>, List<TranslateData>>(model.Data);
                    var resultTrainingCategory = await BLDataManagement.TrainingCategorySaveTranslate(dataTrainingCategory);

                    return Ok(resultTrainingCategory);
                case Enum.EnumTranslateType.TrainingType:
                    var dataTrainingType = _mapper.Map<List<ModelTranslateData>, List<TranslateData>>(model.Data);
                    var resultTrainingType = await BLDataManagement.TrainingTypeSaveTranslate(dataTrainingType);

                    return Ok(resultTrainingType);
                case Enum.EnumTranslateType.Courses:
                    var courses = _mapper.Map<List<ModelTranslateData>, List<TranslateData>>(model.Data);
                    var resultcourses = await BLDataManagement.CourseSaveTranslate(model.Id, courses);

                    return Ok(resultcourses);
            }
            return BadRequest();
        }
        
    }
}
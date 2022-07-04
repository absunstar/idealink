using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Employment.API.Model.Model;
using Employment.API.Model.Response;
using Employment.Entity.Mongo;
using Employment.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Employment.API.Controllers
{
    public class TranslateController : BaseController
    {
        private readonly IServiceCountry _BLServiceCountry;
        private readonly IServiceJobFields _BLServiceJobFields;
        private readonly IServiceYearsOfExperience _BLServiceYearsOfExperience;
        private readonly IServiceIndustry _BLServiceIndustry;
        private readonly IServiceLanguages _BLServiceLanguages;
        private readonly IServiceQualification _BLServiceQualification;
        public TranslateController(IMapper mapper,
                                IServiceCountry BLServiceCountry,
                                IServiceJobFields BLServiceJobFields,
                                IServiceYearsOfExperience BLServiceYearsOfExperience,
                                IServiceIndustry BLServiceIndustry,
                                IServiceLanguages BLServiceLanguages,
                                IServiceQualification BLServiceQualification) : base(mapper)
        {
            _BLServiceCountry = BLServiceCountry;
            _BLServiceIndustry = BLServiceIndustry;
            _BLServiceJobFields = BLServiceJobFields;
            _BLServiceLanguages = BLServiceLanguages;
            _BLServiceQualification = BLServiceQualification;
            _BLServiceYearsOfExperience = BLServiceYearsOfExperience;
        }
        public async Task<IActionResult> ListTranslationByType(ModelTranslateByType model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            switch (model.Type)
            {
                case Enum.EnumTranslateType.Country:
                    var result = await _BLServiceCountry.ListActive();
                    var data = _mapper.Map<List<Country>, List<ResponseTranslateData>>(result);
                    var response = new ResponseTranslate(Enum.EnumTranslateType.Country, data);

                    return Ok(response);
                case Enum.EnumTranslateType.City:
                    var country = await _BLServiceCountry.GetById(model.Id);
                    var city = country.subItems.Where(x => x.IsActive == true).ToList();
                    var citydata = _mapper.Map<List<City>, List<ResponseTranslateData>>(city);
                    var cityresponse = new ResponseTranslate(Enum.EnumTranslateType.City, citydata, model.Id);

                    return Ok(cityresponse);
                case Enum.EnumTranslateType.JobField:
                    var resultJobField = await _BLServiceJobFields.ListActive();
                    var dataJobField = _mapper.Map<List<JobFields>, List<ResponseTranslateData>>(resultJobField);
                    var responseJobField = new ResponseTranslate(Enum.EnumTranslateType.JobField, dataJobField);

                    return Ok(responseJobField);
                case Enum.EnumTranslateType.JobSubFields:
                    var jobFields = await _BLServiceJobFields.GetById(model.Id);
                    var sub = jobFields.subItems.Where(x => x.IsActive == true).ToList();
                    var JobSubFieldsdata = _mapper.Map<List<JobSubFields>, List<ResponseTranslateData>>(sub);
                    var JobSubFieldresponse = new ResponseTranslate(Enum.EnumTranslateType.JobSubFields, JobSubFieldsdata, model.Id);

                    return Ok(JobSubFieldresponse);
                case Enum.EnumTranslateType.Industry:
                    var resultIndustry = await _BLServiceIndustry.ListActive();
                    var dataIndustry = _mapper.Map<List<Industry>, List<ResponseTranslateData>>(resultIndustry);
                    var responseIndustry = new ResponseTranslate(Enum.EnumTranslateType.Industry, dataIndustry);

                    return Ok(responseIndustry);
                case Enum.EnumTranslateType.Languages:
                    var resultLanguages = await _BLServiceLanguages.ListActive();
                    var dataLanguages = _mapper.Map<List<Languages>, List<ResponseTranslateData>>(resultLanguages);
                    var responseLanguages = new ResponseTranslate(Enum.EnumTranslateType.Languages, dataLanguages);

                    return Ok(responseLanguages);
                case Enum.EnumTranslateType.Qualification:
                    var resultQualification = await _BLServiceQualification.ListActive();
                    var dataQualification = _mapper.Map<List<Qualification>, List<ResponseTranslateData>>(resultQualification);
                    var responseQualification = new ResponseTranslate(Enum.EnumTranslateType.Qualification, dataQualification);

                    return Ok(responseQualification);
                case Enum.EnumTranslateType.YearsOfExperience:
                    var resultYearsOfExperience = await _BLServiceYearsOfExperience.ListActive();
                    var dataYearsOfExperience = _mapper.Map<List<YearsOfExperience>, List<ResponseTranslateData>>(resultYearsOfExperience);
                    var responseYearsOfExperience = new ResponseTranslate(Enum.EnumTranslateType.YearsOfExperience, dataYearsOfExperience);

                    return Ok(responseYearsOfExperience);
            }
            return BadRequest();
        }
        public async Task<IActionResult> SaveListTranslation(ModelTranslate model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            switch (model.Type)
            {
                case Enum.EnumTranslateType.Country:
                    var data = _mapper.Map<List<ModelTranslateData>, List<TranslateData>>(model.Data);
                    var result = await _BLServiceCountry.SaveTranslate(data);

                    return Ok(result);
                case Enum.EnumTranslateType.City:
                    var dataCity = _mapper.Map<List<ModelTranslateData>, List<TranslateData>>(model.Data);
                    var resultCity = await _BLServiceCountry.CitySaveTranslate(model.Id, dataCity);

                    return Ok(resultCity);
                case Enum.EnumTranslateType.JobField:
                    var dataJobField = _mapper.Map<List<ModelTranslateData>, List<TranslateData>>(model.Data);
                    var resultJobField = await _BLServiceJobFields.SaveTranslate(dataJobField);

                    return Ok(resultJobField);
                case Enum.EnumTranslateType.JobSubFields:
                    var dataJobSubFields = _mapper.Map<List<ModelTranslateData>, List<TranslateData>>(model.Data);
                    var resultJobSubFields = await _BLServiceJobFields.JobSubFieldsSaveTranslate(model.Id, dataJobSubFields);

                    return Ok(resultJobSubFields);
                case Enum.EnumTranslateType.Industry:
                    var dataIndustry = _mapper.Map<List<ModelTranslateData>, List<TranslateData>>(model.Data);
                    var resultIndustry = await _BLServiceIndustry.SaveTranslate(dataIndustry);

                    return Ok(resultIndustry);
                case Enum.EnumTranslateType.Languages:
                    var dataLanguages = _mapper.Map<List<ModelTranslateData>, List<TranslateData>>(model.Data);
                    var resultLanguages = await _BLServiceLanguages.SaveTranslate(dataLanguages);

                    return Ok(resultLanguages);
                case Enum.EnumTranslateType.Qualification:
                    var dataQualification = _mapper.Map<List<ModelTranslateData>, List<TranslateData>>(model.Data);
                    var resultQualification = await _BLServiceQualification.SaveTranslate(dataQualification);

                    return Ok(resultQualification);
                case Enum.EnumTranslateType.YearsOfExperience:
                    var dataYearsOfExperience = _mapper.Map<List<ModelTranslateData>, List<TranslateData>>(model.Data);
                    var resultYearsOfExperience = await _BLServiceYearsOfExperience.SaveTranslate(dataYearsOfExperience);

                    return Ok(resultYearsOfExperience);
            }
            return BadRequest();
        }
    }
}
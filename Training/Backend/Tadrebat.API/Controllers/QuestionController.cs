using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tadrebat.API.Helpers.AutoMapper;
using Tadrebat.API.Model.Model;
using Tadrebat.API.Model.Response;
using Tadrebat.Entity.Mongo;
using Tadrebat.Interface;

namespace Tadrebat.API.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QuestionController : BaseController
    {
        private readonly IQuestion BLQuestion;
        private readonly HelperMapperQuestion HLMapperQuestion;
        public string currentLang = "en";

        public QuestionController(IQuestion _BLQuestion, IMapper mapper,
            HelperMapperQuestion _HLMapperQuestion) : base(mapper)
        {
            BLQuestion = _BLQuestion;
            HLMapperQuestion = _HLMapperQuestion;
        }
        #region Question
        public async Task<IActionResult> QuestionGetById(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLQuestion.QuestionGetById(model.Id);
            //var response = _mapper.Map<Question, ResponseQuestion>(result);
            var response = await  HLMapperQuestion.MapQuestion(result);

            return Ok(response);
        }
        //public async Task<IActionResult> QuestionListActive()
        //{
        //    var result = await BLQuestion.QuestionListActive();
        //    var response = _mapper.Map<List<Question>, List<ResponseQuestion>>(result);

        //    return Ok(response);
        //}
        public async Task<IActionResult> QuestionListAll(ModelFilterQuestions model)
        {
            if (!ModelState.IsValid)
                BadRequest();
            
            currentLang = GetLanguage();

            var result = await BLQuestion.QuestionListAll(model.filterText, model.TrainingTypeId, model.TrainingCagtegoryId, model.CurrentPage, model.PageSize);
            //var response = _mapper.Map<MongoResultPaged<Question>, ResponsePaged<ResponseQuestion>>(result);
            
            var response = await HLMapperQuestion.MapQuestion(result, currentLang);

            return Ok(response);
        }
        public async Task<IActionResult> QuestionCreate(ModelQuestion model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var obj = _mapper.Map<ModelQuestion, Question>(model);
            obj.GenerateId();
            var result = await BLQuestion.QuestionCreate(obj);

            return await FormatResult(result);
        }
        public async Task<IActionResult> QuestionUpdate(ModelQuestion model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (string.IsNullOrEmpty(model.Id))
                return BadRequest();

            //var id = model._id;
            var obj = _mapper.Map<ModelQuestion, Question>(model);
            var result = await BLQuestion.QuestionUpdate(obj);

            return await FormatResult(result);
        }
        public async Task<IActionResult> QuestionActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLQuestion.QuestionActivate(model.Id);

            return await FormatResult(result);
        }
        public async Task<IActionResult> QuestionDeActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLQuestion.QuestionDeActivate(model.Id);

            return await FormatResult(result);
        }
        #endregion
    }
}
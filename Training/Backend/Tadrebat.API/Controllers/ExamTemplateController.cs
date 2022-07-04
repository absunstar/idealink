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

    public class ExamTemplateController : BaseController
    {
        private readonly IExamTemplate BLExamTemplate;
        
        public ExamTemplateController(IExamTemplate _BLExamTemplate, IMapper mapper) : base(mapper)
        {
            BLExamTemplate = _BLExamTemplate;
        }
        #region ExamTemplate
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExamTemplateGetById(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLExamTemplate.ExamTemplateGetById(model.Id);
            //var response = _mapper.Map<ExamTemplate, ResponseExamTemplate>(result);

            return Ok(result);
        }
        [Authorize(Roles = "Admin, Partner, SubPartner, Trainer")]
        public async Task<IActionResult> ExamTemplateListActive()
        {
            var result = await BLExamTemplate.ExamTemplateListActive();
            //var response = _mapper.Map<List<ExamTemplate>, List<ResponseExamTemplate>>(result);

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExamTemplateListAll(ModelPaged model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var result = await BLExamTemplate.ExamTemplateListAll(model.filterText,  model.CurrentPage, model.PageSize);
            //var response = _mapper.Map<MongoResultPaged<ExamTemplate>, ResponsePaged<ResponseExamTemplate>>(result);
            //var response = await HLMapperExamTemplate.MapExamTemplate(result);

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExamTemplateCreate(ModelExamTemplate model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var obj = _mapper.Map<ModelExamTemplate, ExamTemplate>(model);
            obj.GenerateId();
            var result = await BLExamTemplate.ExamTemplateCreate(obj);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExamTemplateUpdate(ModelExamTemplate model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (string.IsNullOrEmpty(model.Id))
                return BadRequest();

            //var id = model._id;
            var obj = _mapper.Map<ModelExamTemplate, ExamTemplate>(model);
            var result = await BLExamTemplate.ExamTemplateUpdate(obj);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExamTemplateActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLExamTemplate.ExamTemplateActivate(model.Id);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExamTemplateDeActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLExamTemplate.ExamTemplateDeActivate(model.Id);

            return await FormatResult(result);
        }
        #endregion
    }
}

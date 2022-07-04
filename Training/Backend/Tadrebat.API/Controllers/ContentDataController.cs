using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tadrebat.API.Helpers.AutoMapper;
using Tadrebat.API.Model.Model;
using Tadrebat.API.Model.Response;
using Tadrebat.Entity.Mongo;
using Tadrebat.Enum;
using Tadrebat.Interface;

namespace Tadrebat.API.Controllers
{
   
    public class ContentDataController : BaseController
    {
        private readonly IContentData BLContentData;
        private readonly HelperMapperData HLMapperData;
        public string currentLang = "en";

        public ContentDataController(IContentData _BLContentData, IMapper mapper,
            HelperMapperData _HLMapperData) : base(mapper)
        {
            BLContentData = _BLContentData;
            HLMapperData = _HLMapperData;
        }
        public async Task<IActionResult> ContentDataGetById(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLContentData.ContentDataGetById(model.Id);
            var response = _mapper.Map<ContentData, ResponseContentData>(result);

            return Ok(response);
        }
        public async Task<IActionResult> ContentDataOneGetByTypeId(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            EnumContentData type;
            System.Enum.TryParse(model.Id, out type); 

            var result = await BLContentData.ContentDataOneGetByTypeId(type);
            var response = _mapper.Map<ContentData, ResponseContentData>(result);

            return Ok(response);
        }
        //public async Task<IActionResult> ContentDataListActive()
        //{
        //    var result = await BLContentData.ContentDataListActive();
        //    var response = _mapper.Map<List<ContentData>, List<ResponseContentData>>(result);

        //    return Ok(response);
        //}
        public async Task<IActionResult> ContentDataListAll(ModelPaged model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var result = await BLContentData.ContentDataListAll(model.filterText, model.CurrentPage, model.PageSize);
            var response = _mapper.Map<MongoResultPaged<ContentData>, ResponsePaged<ResponseContentData>>(result);

            return Ok(response);
        }
        public async Task<IActionResult> ContentDataCreate(ModelContentData model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var obj = _mapper.Map<ModelContentData, ContentData>(model);
            obj.GenerateId();
            var result = await BLContentData.ContentDataCreate(obj);

            return await FormatResult(result);
        }
        public async Task<IActionResult> ContentDataUpdate(ModelContentData model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (string.IsNullOrEmpty(model._id))
                return BadRequest();

            //var id = model._id;
            var obj = _mapper.Map<ModelContentData, ContentData>(model);
            var result = await BLContentData.ContentDataUpdate(obj);

            return await FormatResult(result);
        }
        public async Task<IActionResult> ContentDataActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLContentData.ContentDataActivate(model.Id);

            return await FormatResult(result);
        }
        public async Task<IActionResult> ContentDataDeActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLContentData.ContentDataDeActivate(model.Id);

            return await FormatResult(result);
        }
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> UpdateSiteLogo()
        {
            try
            {
                if (Request.Form.Files.Count == 0 || Request.Form.Files[0].Length == 0)
                    return BadRequest();

                var file = Request.Form.Files[0];
                if (!Path.GetExtension(file.FileName).ToLower().Equals(".png"))
                    return BadRequest(GetErrorPNG());

                var result = await BLContentData.UpdateSiteLogo(file);
                
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        protected string GetErrorPNG()
        {
            currentLang = GetLanguage();
            var str = currentLang == "ar" ? "يجب أن يكون الملف المحدد هو PNG فقط" : "Selected file must be PNG only.";
            return str;
        }

    }
}
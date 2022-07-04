using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tadrebat.API.Model.Model;
using Tadrebat.Entity.Mongo;
using Tadrebat.Interface;

namespace Tadrebat.API.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LogoPartnerController : BaseController
    {
        private readonly ILogoPartner BLLogoPartner;
        public string currentLang = "en";

        public LogoPartnerController(ILogoPartner _BLLogoPartner, IMapper mapper) : base(mapper)
        {
            BLLogoPartner = _BLLogoPartner;
        }
        public async Task<IActionResult> LogoPartnerGetById(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLLogoPartner.LogoPartnerGetById(model.Id);
            //var response = _mapper.Map<LogoPartner, ResponseLogoPartner>(result);

            return Ok(result);
        }
        public async Task<IActionResult> LogoPartnerListActive()
        {
            var result = await BLLogoPartner.LogoPartnerListActive();
           

            return Ok(result);
        }
        public async Task<IActionResult> LogoPartnerListAll(ModelPaged model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var result = await BLLogoPartner.LogoPartnerListAll(model.filterText, model.CurrentPage, model.PageSize);
            //var response = _mapper.Map<MongoResultPaged<LogoPartner>, ResponsePaged<ResponseLogoPartner>>(result);

            return Ok(result);
        }
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> LogoPartnerCreate(string WebsiteURL)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            if (Request.Form.Files.Count == 0 || Request.Form.Files[0].Length == 0)
                return BadRequest();

            var file = Request.Form.Files[0];
            if (!Path.GetExtension(file.FileName).ToLower().Equals(".png")
              && !Path.GetExtension(file.FileName).ToLower().Equals(".jpg")
              && !Path.GetExtension(file.FileName).ToLower().Equals(".jpeg"))
                return BadRequest(GetErrorPNG());

            var result = await BLLogoPartner.LogoPartnerCreate(file, WebsiteURL);

            return await FormatResult(result);
        }
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> LogoPartnerUpdate(string WebsiteURL, string Id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (string.IsNullOrEmpty(Id))
                return BadRequest();

            var file = Request.Form.Files[0];
            if (!Path.GetExtension(file.FileName).ToLower().Equals(".png")
              && !Path.GetExtension(file.FileName).ToLower().Equals(".jpg")
              && !Path.GetExtension(file.FileName).ToLower().Equals(".jpeg"))
                return BadRequest(GetErrorPNG());

            var result = await BLLogoPartner.LogoPartnerUpdate(file, WebsiteURL, Id);

            return await FormatResult(result);
        }
        public async Task<IActionResult> LogoPartnerActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLLogoPartner.LogoPartnerActivate(model.Id);

            return await FormatResult(result);
        }
        public async Task<IActionResult> LogoPartnerDeActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLLogoPartner.LogoPartnerDeActivate(model.Id);

            return await FormatResult(result);
        }

        protected string GetErrorPNG()
        {
            currentLang = GetLanguage();
            var str = currentLang == "ar" ? "يجب أن يكون الملف المحدد هو PNG, JPG, JPEG فقط" : "Selected file must be PNG, JPG, JPEG only.";
            return str;
        }

    }
}
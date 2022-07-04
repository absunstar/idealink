using AutoMapper;
using Employment.API.Model.Model;
using Employment.API.Model.Response;
using Employment.Entity.Mongo;
using Employment.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Controllers
{
    [AllowAnonymous]
    public class GenericController<T> : BaseController where T:MongoEntityNameBase,new()
    {
        private string currentLang = "en";
        private readonly IServiceRepository<T> _BLService;
        public GenericController(IServiceRepository<T> BLService, IMapper mapper) : base(mapper)
        {
            _BLService = BLService;
        }
        public async Task<IActionResult> GetById(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _BLService.GetById(model.Id);

            return Ok(result);
        }
        public async Task<IActionResult>ListActive()
        {
            currentLang = GetLanguage();
            var result = await _BLService.ListActive();

            result = currentLang == "en" ? result.OrderBy(x => x.Name).ToList() : result.OrderBy(x => x.Name2).ToList();
            if (currentLang == "ar")
                result.ForEach(x => x.Name = !string.IsNullOrEmpty(x.Name2) ? x.Name2 : x.Name);
            return Ok(result); ;
        }
        public async Task<IActionResult>ListAll(ModelPaged model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var result = await _BLService.ListAll(model.filterText, model.CurrentPage, model.PageSize);

            return Ok(result);
        }
        public async Task<IActionResult>Create(ModelIdName model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _BLService.Create(model.Name);

            return await FormatResult(result);
        }
        public async Task<IActionResult>Update(ModelIdName model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (string.IsNullOrEmpty(model.Id))
                return BadRequest();

            var obj = new T();
            obj._id = model.Id;
            obj.Name = model.Name;

            var result = await _BLService.Update(obj);

            return await FormatResult(result);
        }
        public async Task<IActionResult>Activate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _BLService.Activate(model.Id);

            return await FormatResult(result);
        }
        public async Task<IActionResult>DeActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _BLService.DeActivate(model.Id);

            return await FormatResult(result);
        }
    }
}

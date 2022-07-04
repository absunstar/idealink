using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Employment.API.Model.Model;
using Employment.Entity.Mongo;
using Employment.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Employment.API.Controllers
{
    public class CountryController : GenericController<Country>
    {
        private readonly IServiceCountry _BLService;
        public CountryController(IServiceCountry BLService, IMapper mapper) : base(BLService, mapper)
        {
            _BLService = BLService;
        }
        public async Task<IActionResult> SubCreate(ModelSubEntity model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _BLService.SubCreate(model.MainId, model.Name);

            return await FormatResult(result);
        }
        public async Task<IActionResult> SubUpdate(ModelSubEntity model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _BLService.SubUpdate(model.MainId, model.Id, model.Name);

            return await FormatResult(result);
        }
        public async Task<IActionResult> SubActivate(ModelSubEntity model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _BLService.SubActivate(model.MainId, model.Id);

            return await FormatResult(result);
        }
        public async Task<IActionResult> SubDeActivate(ModelSubEntity model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _BLService.SubDeActivate(model.MainId, model.Id);

            return await FormatResult(result);
        }
    }
}
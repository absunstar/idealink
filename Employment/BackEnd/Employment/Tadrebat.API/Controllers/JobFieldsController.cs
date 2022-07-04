using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Employment.Entity.Mongo;
using Employment.Interface;
using Employment.API.Model.Model;
using Microsoft.AspNetCore.Authorization;

namespace Employment.API.Controllers
{
    public class JobFieldsController : GenericController<JobFields>
    {
        private readonly IServiceJobFields _BLService;
        public JobFieldsController(IServiceJobFields BLService, IMapper mapper) : base(BLService, mapper)
        {
            _BLService = BLService;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ListAnyJobFields()
        {
            if (!ModelState.IsValid)
                BadRequest();

            var result = await _BLService.ListAnyJobFields();

            return Ok(result);
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
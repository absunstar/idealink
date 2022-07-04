using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Employment.API.Model.Model;
using Employment.API.Model.Response;
using Employment.Entity.Mongo;
using Employment.Interface;

namespace Employment.API.Controllers
{
    [AllowAnonymous]
    //[Authorize(Roles = "Admin, Partner")]
    public class DataManagementController : BaseController
    {
        private readonly IDataManagement BLDataManagement;
        //private readonly IServiceRepository<Languages> _BLService;
        public DataManagementController(IDataManagement _BLDataManagement
            //, IServiceRepository<Languages> BLService
            , IMapper mapper) : base(mapper)
        {
            BLDataManagement = _BLDataManagement;
            //_BLService = BLService;   
        }
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

            return Ok(response);
        }
        public async Task<IActionResult> NGOTypeListAll(ModelPaged model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var result = await BLDataManagement.NGOTypeListAll(model.filterText, model.CurrentPage, model.PageSize);
            var response = _mapper.Map<MongoResultPaged<NGOType>, ResponsePaged<ResponseNGOType>>(result);

            return Ok(response);
        }
        public async Task<IActionResult> NGOTypeCreate(ModelNGOType model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLDataManagement.NGOTypeCreate(model.Name);

            return await FormatResult(result);
        }
        public async Task<IActionResult> NGOTypeUpdate(ModelNGOType model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (string.IsNullOrEmpty(model.Id))
                return BadRequest();

            var result = await BLDataManagement.NGOTypeUpdate(model.Id, model.Name);

            return await FormatResult(result);
        }
        public async Task<IActionResult> NGOTypeActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLDataManagement.NGOTypeActivate(model.Id);

            return await FormatResult(result);
        }
        public async Task<IActionResult> NGOTypeDeActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLDataManagement.NGOTypeDeActivate(model.Id);

            return await FormatResult(result);
        }
        #endregion
    }
}
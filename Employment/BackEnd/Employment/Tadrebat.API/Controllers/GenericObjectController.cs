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
    public class GenericObjectController<T, W> : BaseController where T : MongoEntityNameBase, new() where W : ResponseBase, new()
    {
        private readonly IServiceRepository<T> _BLService;
        public GenericObjectController(IServiceRepository<T> BLService, IMapper mapper) : base(mapper)
        {
            _BLService = BLService;
        }
        public async virtual Task<IActionResult> GetById(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _BLService.GetById(model.Id);
            var response = new W();
            response.Map(result);
            
            return Ok(response);
        }
        public async Task<IActionResult> ListActive()
        {
            var result = await _BLService.ListActive();

            return Ok(result);
        }
        public async Task<IActionResult> ListAll(ModelPaged model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var userId = GetUserId();

            var result = await _BLService.ListAllByUserId(model.filterText, userId,model.CurrentPage, model.PageSize);
            var response = new ResponsePaged<W>();
            
            response.pageSize = result.pageSize;
            response.totalCount = result.totalCount;
            result.lstResult.ForEach(item => {
                var obj = new W();
                obj.Map(item);
                response.lstResult.Add(obj);
            });
            return Ok(response);
        }
        //public virtual async Task<IActionResult> Create(T model)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest();

        //    var result = await _BLService.Create(model);

        //    return await FormatResult(result);
        //}
        //public async Task<IActionResult> Update(T model)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest();

        //    if (string.IsNullOrEmpty(model._id))
        //        return BadRequest();

        //    var result = await _BLService.Update(model);

        //    return await FormatResult(result);
        //}
        public async virtual Task<IActionResult> Activate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _BLService.Activate(model.Id);

            return await FormatResult(result);
        }
        public async Task<IActionResult> DeActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _BLService.DeActivate(model.Id);

            return await FormatResult(result);
        }
    }

}
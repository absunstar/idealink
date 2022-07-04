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
    public class FavouriteController : BaseController
    {
        private readonly IServiceFavourite _BLService;
        public FavouriteController(IServiceFavourite BLService, IMapper mapper) : base(mapper)
        {
            _BLService = BLService;
        }
        public async Task<IActionResult> Create(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userId = GetUserId();
            var role = GetUserRole();

            bool result = false;
            switch (role)
            {
                case Enum.EnumUserTypes.Employer:
                    result = await _BLService.AddResume(userId,model.Id);
                    break;
                case Enum.EnumUserTypes.JobSeeker:
                    result = await _BLService.AddJob(userId, model.Id);//the entity here is the user we added to favourite
                    break;
            }
            

            return await FormatResult(result);
        }
        public async Task<IActionResult> ListAll()
        {

            var userId = GetUserId();
            
            var result = await _BLService.GetMyFavourite(userId);
            var response = new List<ResponseFavourite>();

            result.ForEach(item =>
            {
                var obj = new ResponseFavourite();
                obj.Map(item);
                response.Add(obj);
            });

            return Ok(response);
        }
        public async Task<IActionResult> DeActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _BLService.DeActivate(model.Id);

            return await FormatResult(result);
        }
        public async Task<IActionResult> DeActivateByJobId(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var UserId = GetUserId();

            var result = await _BLService.DeActivateByJobId(UserId, model.Id);

            return Ok(result);
        }
        //public async Task<IActionResult> DeActivateByProfileId(ModelId model)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest();

        //    var UserId = GetUserId();
            
        //    var result = await _BLService.DeActivateByProfileId(UserId, model.Id);

        //    return Ok(result);
        //}
        public async Task<IActionResult> CheckMyFavourite(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var UserId = GetUserId();

            var result = await _BLService.CheckMyFavourite(UserId, model.Id);

            return Ok(result);
        }
    }
}
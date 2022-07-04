using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Employment.API.Model.Model;
using Employment.API.Model.Response;
using Employment.Interface;
using Employment.MongoDB.Interface;

namespace Employment.API.Controllers
{
    [AllowAnonymous]
    public class TestAAAAAController : BaseController
    {
        //private readonly IServiceLanguages BLLang;
        public TestAAAAAController( IMapper mapper) : base(mapper)//IServiceLanguages _BLLang) 
        {

            //BLLang = _BLLang;
        }
        public async Task<IActionResult> Test()
        {
            //var a = await BLLang.Create("test");

            //var x = new ResponsePaged<ResponseTrainingCategory>();
            //x.totalCount = 6;
            //x.lstResult = new List<ResponseTrainingCategory>();
            //var y = new ResponseTrainingCategory();
            //y.Id = "11";
            //y.Name = "2222";
            //x.lstResult.Add(y);
            return Ok();
        }

    }
    
}
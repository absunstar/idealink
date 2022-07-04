using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tadrebat.API.Model.Model;
using Tadrebat.API.Model.Response;
using Tadrebat.Interface;
using Tadrebat.Model.Entity;
using Tadrebat.MongoDB.Interface;

namespace Tadrebat.API.Controllers
{
    [AllowAnonymous]
    public class TestController : BaseController
    {
        private readonly IDBTrainingCategory dBTrainingCategory;
        private readonly IUserManagement BLUserManagement;
        public TestController(IDBTrainingCategory _dBTrainingCategory, IUserManagement _userManagement, IMapper mapper) : base(mapper)
        {
            dBTrainingCategory = _dBTrainingCategory;
            BLUserManagement = _userManagement;
        }
        public async Task<IActionResult> Test()
        {
            var x = new ResponsePaged<ResponseTrainingCategory>();
            x.totalCount = 6;
            x.lstResult = new List<ResponseTrainingCategory>();
            var y = new ResponseTrainingCategory();
            y.Id = "11";
            y.Name = "2222";
            x.lstResult.Add(y);
            return Ok(x);
        }

    }
    
}
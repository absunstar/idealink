using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tadrebat.API.Model.Model;
using Tadrebat.Enum;
using Tadrebat.Interface;
using Tadrebat.ModelsGlobal;

namespace Tadrebat.API.Controllers
{
    [AllowAnonymous]
    public class UserManagementController : BaseController
    {
        private readonly IUserManagement BLUserManagement;
        public UserManagementController(IUserManagement _userManagement, IMapper mapper) : base(mapper)
        {
            BLUserManagement = _userManagement;
        }
        //public async Task<object> CreateUser(ModelUser model)
        //{
        //    if (!ModelState.IsValid)
        //        return "false"; //TODO: Change to generic error function

        //    ApplicationUser user = new ApplicationUser();
        //    user.Email = model.Email;
        //    user.UserName = model.Email;
        //    //user.FullName = model.FullName;
        //    var result = await BLUserManagement.CreateApplicationUser(user, EnumUserTypes.Admin);

        //    return result;
        //}
        //public async Task<IActionResult> ConfirmAccount(string Token, Guid UserId)
        //{
        //    if (string.IsNullOrEmpty(Token) || UserId == Guid.Empty || UserId == null)
        //        return BadRequest();
        //    var result = await BLUserManagement.ConfirmAccount(Token, UserId);

        //    return Ok();
        //}
    }
}
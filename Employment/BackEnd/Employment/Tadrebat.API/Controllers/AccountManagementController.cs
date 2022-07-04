using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Employment.API.Helpers.AutoMapper;
using Employment.API.Helpers.HTTPCall;
using Employment.API.Model.Model;
using Employment.API.Model.Response;
using Employment.Entity.Mongo;
using Employment.Interface;
using Employment.Services;
using System.Text.RegularExpressions;
using Employment.Enum;

namespace Employment.API.Controllers
{
    [AllowAnonymous]
    //[Authorize(Roles = "Admin,Employer")]
    public class AccountManagementController : BaseController
    {
        private readonly HTTPCallSTS HTTPCallSTS;
        //private readonly HelperMapperUser HPMapperUser;
        private readonly IUserProfile BLServiceUserProfile;
        private readonly IServiceCompany _BLServiceCompany;
        public AccountManagementController(HTTPCallSTS _HTTPCallSTS
                                , IUserProfile _BLServiceUserProfile,
                                // , ServiceUpdateEntityConsistency _BLServiceUpdateEntityConsistency
                                IServiceCompany BLServiceCompany
                                , IMapper mapper) : base(mapper)
        {
            HTTPCallSTS = _HTTPCallSTS;
            BLServiceUserProfile = _BLServiceUserProfile;
            //HPMapperUser = _HPMapperUser;
            _BLServiceCompany = BLServiceCompany;
        }
        public async Task<IActionResult> GetById(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLServiceUserProfile.UserProfileGetById(model.Id);
            var response = _mapper.Map<UserProfile, ResponseUserProfile>(result);
            //var response = await HPMapperUser.MapUser(result);

            return Ok(response);
        }
        public async Task<IActionResult> GetMyUser()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userId = GetUserId();

            var result = await BLServiceUserProfile.UserProfileGetById(userId);
            var response = _mapper.Map<UserProfile, ResponseUserProfile>(result);

            var lst = result.MyCompanies.ToList().Select(x => new SubItemActive()
            {
                _id = x._id,
                Name = x.Name
            }).ToList();
            lst = await _BLServiceCompany.GetUserCompanies(lst);

            response.MyCompanies = _mapper.Map<List<SubItemActive>, List<ResponseSubItemActive>>(lst);
            response.MyCompanies = response.MyCompanies.OrderBy(x => x.Name).ToList();
            
            //var response = await HPMapperUser.MapUser(result);

            return Ok(response);
        }
        public async Task<IActionResult> ListActive()
        {
            var result = await BLServiceUserProfile.UserProfileListActive();
            var response = _mapper.Map<List<UserProfile>, List<ResponseUserProfile>>(result);
            //var response = await HPMapperUser.MapUser(result);

            return Ok(response);
        }
        public async Task<IActionResult> ListAll(ModelAccountSearch model)
        {
            if (!ModelState.IsValid)
                BadRequest();
            var role = GetUserRole();
            //if admin send empty userid, otherwise send partner userid
            string UserId = role == Enum.EnumUserTypes.Admin ? "" : GetUserId();

            var result = await BLServiceUserProfile.UserProfileListAll(model.filterText, model.filterType, model.CurrentPage, model.PageSize);
            var response = _mapper.Map<MongoResultPaged<UserProfile>, ResponsePaged<ResponseUserProfile>>(result);
            //var response = await HPMapperUser.MapUser(result);

            return Ok(response);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Create(ModelUserProfile model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = new UserProfile();
            user.Name = model.Name;
            user.Email = model.Email;
            user.Type = (int)model.Type;

            var MDUser = await BLServiceUserProfile.UserProfileGetByEmail(user.Email);
            if (MDUser != null)
                return BadRequest("Email already registered");

            if (model.Type != Enum.EnumUserTypes.Admin)
            {
                if (!CheckStrength(model.Password))
                    return BadRequest("Password is week. It should be min 8 char, have at least one small char, one capital, one number and one special char");
            }
            //craete user
            var result = await BLServiceUserProfile.UserProfileCreate(user);
            if (result)
            {
                MDUser = await BLServiceUserProfile.UserProfileGetByEmail(user.Email);

                result = await HTTPCallSTS.RegisterUser(model, MDUser._id, model.Password);
            }
            return await FormatResult(result);
        }
        public async Task<IActionResult> Update(ModelUserProfile model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (string.IsNullOrEmpty(model.Id))
                return BadRequest();

            if (!await BLServiceUserProfile.CheckTypePermissionByRole(GetUserRole(), model.Type))
                return BadRequest();

            var obj = _mapper.Map<ModelUserProfile, UserProfile>(model);
            var result = await BLServiceUserProfile.UserProfileUpdate(obj);

            return await FormatResult(result);
        }
        public async Task<IActionResult> Activate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLServiceUserProfile.UserProfileActivate(model.Id);

            var user = await BLServiceUserProfile.UserProfileGetById(model.Id);

            await HTTPCallSTS.UpdateAccountStatus(user.Email, true);

            return await FormatResult(result);
        }
        public async Task<IActionResult> DeActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLServiceUserProfile.UserProfileDeActivate(model.Id);

            var user = await BLServiceUserProfile.UserProfileGetById(model.Id);
            await HTTPCallSTS.UpdateAccountStatus(user.Email, false);

            return await FormatResult(result);
        }
        public async Task<IActionResult> ListActiveEmployer(ModelPaged model)
        {
            model.CurrentPage = 1;
            model.PageSize = 20;
            var result = await BLServiceUserProfile.UserProfileListActiveEmployer(model.filterText, model.CurrentPage, model.PageSize);
            var response = _mapper.Map<List<UserProfile>, List<ResponseUserProfile>>(result);
            //var response = await HPMapperUser.MapUser(result);

            return Ok(response);
        }
        public async Task<IActionResult> ListActiveJobSeeker(ModelPaged model)
        {
            model.CurrentPage = 1;
            model.PageSize = 20;
            var result = await BLServiceUserProfile.UserProfileListActiveJobSeeker(model.filterText, model.CurrentPage, model.PageSize);
            var response = _mapper.Map<List<UserProfile>, List<ResponseUserProfile>>(result);
            //var response = await HPMapperUser.MapUser(result);

            return Ok(response);
        }
        public async Task<IActionResult> UpdateRole(ModelId model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var role = GetUserRole();

            if (role != Enum.EnumUserTypes.Undefined)
                return BadRequest("Cannot change user role.");

            //if admin send empty userid, otherwise send partner userid
            string UserId = GetUserId();

            System.Enum.TryParse(model.Id, out EnumUserTypes type);

            var result = await BLServiceUserProfile.UpdateUserRole(UserId, type);

            var user = await BLServiceUserProfile.UserProfileGetById(UserId);

            result = await HTTPCallSTS.UpdateUserRole(user.Email,(int)type);

            return Ok(result);
        }
        protected bool CheckStrength(string password)
        {
            int score = 0;

            if (password.Length < 8)
                return false;

            if (!Regex.Match(password, @"\d", RegexOptions.ECMAScript).Success)
                return false;
            if (!Regex.Match(password, @"[a-z]", RegexOptions.ECMAScript).Success &&
                    Regex.Match(password, @"[A-Z]", RegexOptions.ECMAScript).Success)
                return false;
            if (!Regex.Match(password, @"[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]", RegexOptions.ECMAScript).Success)
                return false;

            return true;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Limit(ModelId model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var user = await BLServiceUserProfile.UserProfileGetById(model.Id);
            if (user.Type != (int)EnumUserTypes.Employer)
                return BadRequest("Can only be used for Employer account");

            var result = await BLServiceUserProfile.Limit(model.Id);

            return Ok(result);
        }
        public async Task<IActionResult> ChangePassword(ModelChangePassword model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if(model.NewPassword == model.OldPassword)
                return BadRequest("New password cannot be same as old password.");
            
            if (model.NewPassword != model.ConfirmPassword)
                return BadRequest("New password does not match confirm password.");

            if (!CheckStrength(model.NewPassword))
                return BadRequest("Password is week. It should be min 8 char, have at least one small char, one capital, one number and one special char");

            var userEmail = GetUserEmail();
            
            var result = await HTTPCallSTS.ChangePassword(userEmail, model.OldPassword, model.NewPassword);
            if(result.Item1)
                return Ok();

            return BadRequest(result.Item2);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResendActivationLink(ModelEmail model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await HTTPCallSTS.ResendActivationLink(model.Email);
            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResendPasswordLink(ModelEmail model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await HTTPCallSTS.ResendPasswordLink(model.Email);
            return await FormatResult(result);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Employment.API.Model.Model;
using Employment.API.Model.Response;
using Employment.Entity.Mongo;
using Employment.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employment.API.Controllers
{
    public class CompanyController : GenericObjectController<Company, ResponseCompany>
    {
        private readonly IServiceCompany _BLService;
        private readonly IUserProfile _BLServiceUserProfile;
        public CompanyController(IServiceCompany BLService, IUserProfile BLServiceUserProfile, IMapper mapper) : base(BLService, mapper)
        {
            _BLService = BLService;
            _BLServiceUserProfile = BLServiceUserProfile;
        }
        public async Task<IActionResult> Create(ModelCompany model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userId = GetUserId();
            if(! await IsCompanyLimit(userId))
            {
                return BadRequest("You have reached the allowed company limit.");
            }

            var obj = _mapper.Map<ModelCompany, Company>(model);
            
            obj.UserCanAccess.Add(userId);

            var result = await _BLService.Create(obj);

            return await FormatResult(result);
        }
        public async Task<IActionResult> Update(ModelCompany model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            
            if (string.IsNullOrEmpty(model._Id))
                return BadRequest();

            var obj = _mapper.Map<ModelCompany, Company>(model);
            var result = await _BLService.Update(obj);

            return await FormatResult(result);
        }
        public async override Task<IActionResult> Activate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userId = GetUserId();
            if (!await IsCompanyLimit(userId))
            {
                return BadRequest("You have reached the allowed company limit.");
            }

            return await base.Activate(model);
        }
        public async Task<IActionResult> AddEmployer(ModelCompanyAddEmployer model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _BLService.AssignUser(model.UserId, model.CompanyId);

            return await FormatResult(result);
        }
        public async Task<IActionResult> RemoveEmployer(ModelCompanyAddEmployer model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _BLService.RemoveUser(model.CompanyId, model.UserId);

            return await FormatResult(result);
        }
        public async Task<IActionResult> ListCompanyEmployers(ModelId model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var result = await _BLService.ListCompanyEmployers(model.Id);
            var response = _mapper.Map<List<UserProfile>, List<ResponseCompanyEmployers>>(result);
            
            return Ok(response);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ListCompany()
        {
            if (!ModelState.IsValid)
                BadRequest();

            var result = await _BLService.ListCompany();
            var response = _mapper.Map<List<Company>, List<ResponseCompanyEmployers>>(result);

            return Ok(response);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ListAnyCompany()
        {
            if (!ModelState.IsValid)
                BadRequest();

            var result = await _BLService.ListAnyCompany();
            var response = _mapper.Map<List<Company>, List<ResponseCompanyEmployers>>(result);

            return Ok(response);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ListAnyCompanyPaged(ModelPaged model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var result = await _BLService.ListAnyCompanyPaged(model.filterText, model.CurrentPage, model.PageSize);
            var response = new ResponsePaged<ResponseCompany>();

            response.pageSize = result.pageSize;
            response.totalCount = result.totalCount;
            result.lstResult.ForEach(item => {
                var obj = new ResponseCompany();
                obj.Map(item);
                response.lstResult.Add(obj);
            });
            return Ok(response);
        }
        public async Task<IActionResult> GetCompanyWaitingApproval(ModelPaged model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var userId = GetUserId();

            var result = await _BLService.GetCompanyWaitingApproval(model.filterText, model.CurrentPage, model.PageSize);
            var response = new ResponsePaged<ResponseCompany>();

            response.pageSize = result.pageSize;
            response.totalCount = result.totalCount;
            result.lstResult.ForEach(item => {
                var obj = new ResponseCompany();
                obj.Map(item);
                response.lstResult.Add(obj);
            });
            return Ok(response);
        }
        public async Task<IActionResult> GetCompanyWaitingApprovalCount()
        {
            var result = await _BLService.GetCompanyWaitingApprovalCount();
            return Ok(result);
        }
        public async Task<IActionResult> updateApproved(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var result = await _BLService.UpdateStatus(model.Id, true);

            return await FormatResult(result);
        }
        public async Task<IActionResult> updateRejected(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _BLService.UpdateStatus(model.Id, false);

            return await FormatResult(result);
        }
        protected async Task<bool> IsCompanyLimit(string UserId)
        {
            if (string.IsNullOrEmpty(UserId))
                return false;

            var user= await _BLServiceUserProfile.UserProfileGetById(UserId);
            
            //if unlimited return true
            if (!user.IsEmployerLimitedCompanies)
                return true;

            //if limited check how many companies he has active
            var count = await _BLService.GetCompanyByUserIdCount(UserId);

            return count < 1;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ReportCompanyCount(ModelReportDates model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            DateTime startDate = string.IsNullOrEmpty(model.StartDate) ? DateTime.MinValue : Convert.ToDateTime(model.StartDate);
            DateTime endDate = string.IsNullOrEmpty(model.EndDate) ? DateTime.MinValue : Convert.ToDateTime(model.EndDate);

            var result = await _BLService.ReportCompanyCount(startDate, endDate);

            return Ok(result);
        }
    }
}
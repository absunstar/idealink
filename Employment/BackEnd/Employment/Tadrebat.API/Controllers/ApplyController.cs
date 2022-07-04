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
    public class ApplyController : BaseController
    {
        private readonly IServiceApply _BLService;
        private readonly IServiceJobSeeker _BLServiceJobSeeker;
        public ApplyController(IServiceApply BLService, IServiceJobSeeker BLServiceJobSeeker, IMapper mapper) : base(mapper)
        {
            _BLService = BLService;
            _BLServiceJobSeeker = BLServiceJobSeeker;
        }
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> Create(ModelApplyCreate model)
        {
            var lang = GetLanguage();
            if (!ModelState.IsValid)
                return BadRequest();
            
            var userId = GetUserId();

            var check = await _BLService.CheckIfApplied(userId, model.Id);
            if(check)
            {
                return BadRequest(lang == "en" ? "You had already applied to this job." : "كنت قد تقدمت بالفعل لهذه الوظيفة");
            }
            var jobseeker = await _BLServiceJobSeeker.GetByUserId(userId);
            if(jobseeker != null)
            {
                if(!jobseeker.IsProfileComplete)
                    return BadRequest(lang == "en" ? "Please complete your resume before applying." : "يرجى إكمال سيرتك الذاتية قبل التقديم.");
            }
            else
            {
                return BadRequest("User not found" + userId);
            }

            var obj = new Apply();
            obj.Job._id = model.Id;
            obj.JobSeeker._id = userId;
            obj.Message = model.Message;

            var result = await _BLService.Create(obj);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Employer, Admin")]
        public async Task<IActionResult> Hire(ModelApplyHire model)
        {
            var lang = GetLanguage();
            if (!ModelState.IsValid)
                return BadRequest();
            if(string.IsNullOrEmpty(model.Id) || string.IsNullOrEmpty(model.JobSeekerId))
                return BadRequest();

            var result = await _BLService.Hire(model.JobSeekerId, model.Id, true);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Employer, Admin")]
        public async Task<IActionResult> UnHire(ModelApplyHire model)
        {
            var lang = GetLanguage();
            if (!ModelState.IsValid)
                return BadRequest();
            if (string.IsNullOrEmpty(model.Id) || string.IsNullOrEmpty(model.JobSeekerId))
                return BadRequest();

            var result = await _BLService.Hire(model.JobSeekerId, model.Id, false);

            return await FormatResult(result);
        }
        public async Task<IActionResult> ListAll(ModelApplyList model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var userId = GetUserId();
            var role = GetUserRole();

            MongoResultPaged<Apply> result = new MongoResultPaged<Apply>(0, new List<Apply>(), model.PageSize);
            switch(role)
            {
                case Enum.EnumUserTypes.Admin:
                    result = await _BLService.GetByJoId(model.JobId, model.filterText, model.CurrentPage, model.PageSize);
                    break;
                case Enum.EnumUserTypes.Employer:
                    result = await _BLService.GetByJoId(model.JobId, model.filterText, model.CurrentPage, model.PageSize);
                    break;
                case Enum.EnumUserTypes.JobSeeker:
                    result = await _BLService.GetByJobSeekerId(userId, model.filterText, model.CurrentPage, model.PageSize);
                    break;
            }

            //var response = _mapper.Map<MongoResultPaged<Apply>, ResponsePaged<ResponseApply>>(result);
            var response = new ResponsePaged<ResponseApply>();

            response.pageSize = result.pageSize;
            response.totalCount = result.totalCount;
            result.lstResult.ForEach(item =>
            {
                var obj = new ResponseApply();
                obj.Map(item);
                response.lstResult.Add(obj);
            });
            return Ok(response);
        }
        public async Task<IActionResult> CheckMyApply(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var UserId = GetUserId();

            var result = await  _BLService.CheckIfApplied(UserId, model.Id);

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ReportJobSeekerHiredCount(ModelReportDates model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            DateTime startDate = string.IsNullOrEmpty(model.StartDate) ? DateTime.MinValue : Convert.ToDateTime(model.StartDate);
            DateTime endDate = string.IsNullOrEmpty(model.EndDate) ? DateTime.MinValue : Convert.ToDateTime(model.EndDate);

            var result = await _BLService.ReportJobSeekerHiredCount(startDate, endDate);

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ReportJobSeekerAppliedPerJobCount(ModelReportDates model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            DateTime startDate = string.IsNullOrEmpty(model.StartDate) ? DateTime.MinValue : Convert.ToDateTime(model.StartDate);
            DateTime endDate = string.IsNullOrEmpty(model.EndDate) ? DateTime.MinValue : Convert.ToDateTime(model.EndDate);

            var result = await _BLService.ReportJobSeekerAppliedPerJobCount(startDate, endDate);

            return Ok(result);
        }
    }
}
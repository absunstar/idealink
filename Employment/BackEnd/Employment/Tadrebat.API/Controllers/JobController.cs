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
    public class JobController : GenericObjectController<Job, ResponseJob>
    {
        private readonly IServiceJob _BLService;
        public JobController(IServiceJob BLService, IMapper mapper) : base(BLService, mapper)
        {
            _BLService = BLService;
        }
        [AllowAnonymous]
        public async override Task<IActionResult> GetById(ModelId model)
        {
            return await base.GetById(model);
        }
        public async Task<IActionResult> Create(ModelJob model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var obj = _mapper.Map<ModelJob, Job>(model);

            var result = await _BLService.CreateReturnId(obj);

            return Ok(result);
        }
        public async Task<IActionResult> Update(ModelJob model)
        {
            return BadRequest();
            //if (!ModelState.IsValid)
            //    return BadRequest();

            //if (string.IsNullOrEmpty(model._id))
            //    return BadRequest();

            //var obj = _mapper.Map<ModelJob, Job>(model);
            //var result = await _BLService.Update(obj);

            //return await FormatResult(result);
        }
        public async Task<IActionResult> updatePublish(ModelJob model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (string.IsNullOrEmpty(model._id))
                return BadRequest();

            var obj = _mapper.Map<ModelJob, Job>(model);
            obj.Status = Enum.EnumJobStatus.Published;
            var result = await _BLService.Update(obj);

            return await FormatResult(result);
        }
        public async Task<IActionResult> updateDraft(ModelJob model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (string.IsNullOrEmpty(model._id))
                return BadRequest();

            var obj = _mapper.Map<ModelJob, Job>(model);
            obj.Status = Enum.EnumJobStatus.Draft;
            var result = await _BLService.Update(obj);

            return await FormatResult(result);
        }
        public async Task<IActionResult> updateDraftPublish(ModelJob model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var obj = _mapper.Map<ModelJob, Job>(model);
            obj.Status = Enum.EnumJobStatus.Published;

            var result = await _BLService.CreateReturnId(obj);

            return Ok(result);
        }
        
        public async Task<IActionResult> updateApproved(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var result = await _BLService.UpdateStatus(model.Id, Enum.EnumJobStatus.Approved);

            return await FormatResult(result);
        }
        public async Task<IActionResult> updateRejected(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _BLService.UpdateStatus(model.Id, Enum.EnumJobStatus.Rejected);

            return await FormatResult(result);
        }
        public async Task<IActionResult> GetJobWaitingApproval(ModelPaged model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var userId = GetUserId();

            var result = await _BLService.GetJobWaitingApproval(model.filterText, model.CurrentPage, model.PageSize);
            var response = new ResponsePaged<ResponseJob>();

            response.pageSize = result.pageSize;
            response.totalCount = result.totalCount;
            result.lstResult.ForEach(item => {
                var obj = new ResponseJob();
                obj.Map(item);
                response.lstResult.Add(obj);
            });
            return Ok(response);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetJobWaitingApprovalCount()
        {
            var result = await _BLService.GetJobWaitingApprovalCount();
            return Ok(result);
        }
        public async Task<IActionResult> ListAllByEmployerId(ModelPaged model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var userId = GetUserId();

            var result = await _BLService.ListAllByEmployerId(userId, model.filterText, model.CurrentPage, model.PageSize);
            var response = new ResponsePaged<ResponseJob>();

            response.pageSize = result.pageSize;
            response.totalCount = result.totalCount;
            result.lstResult.ForEach(item => {
                var obj = new ResponseJob();
                obj.Map(item);
                response.lstResult.Add(obj);
            });
            return Ok(response);
        }
        [AllowAnonymous]
        
        public async Task<IActionResult> Search(ModelJobSearch model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            //var userId = GetUserId();
            try { 
            var result = await _BLService.Search(model.CompanyId, model.filterText, model.ExperienceId, model.GenderId, model.Qualificationid, model.IndustryId,model.JobFieldId, model.CountryId, model.CityId, model.CurrentPage, model.PageSize);
            var response = new ResponsePaged<ResponseJobSearch>();

            response.pageSize = result.pageSize;
            response.totalCount = result.totalCount;
            result.lstResult.ForEach(item =>
            {
                var obj = new ResponseJobSearch();
                obj.Map(item);
                response.lstResult.Add(obj);
            });
            return Ok(response);
            }
            catch(Exception ex)
            {

            }return Ok();
        }

        [AllowAnonymous]

        public async Task<IActionResult> SearchforFilterTextAndCity(ModelJobSearch model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            //var userId = GetUserId();
            try
            {
                var result = await _BLService.SearchforFilterTextAndCity(model.filterText, model.CityId, model.CurrentPage, model.PageSize);
                var response = new ResponsePaged<ResponseJobSearch>();

                response.pageSize = result.pageSize;
                response.totalCount = result.totalCount;
                result.lstResult.ForEach(item =>
                {
                    var obj = new ResponseJobSearch();
                    obj.Map(item);
                    response.lstResult.Add(obj);
                });
                return Ok(response);
            }
            catch (Exception ex)
            {

            }
            return Ok();
        }
        [AllowAnonymous]
        public async Task<IActionResult> SearchCompany()
        {
            if (!ModelState.IsValid)
                BadRequest();

            //var userId = GetUserId();
            try
            {
                var result = await _BLService.SearchCompany();
                var response = new ResponsePaged<ResponseJobSearch>();

                response.pageSize = result.pageSize;
                response.totalCount = result.totalCount;
                result.lstResult.ForEach(item =>
                {
                    var obj = new ResponseJobSearch();
                    obj.Map(item);
                    response.lstResult.Add(obj);
                });
                return Ok(response);
            }
            catch (Exception ex)
            {

            }
            return Ok();
        }
        [AllowAnonymous]
        public async Task<IActionResult> ForSearchValidation(ModelJobSearch model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            //var userId = GetUserId();
            try
            {
                var result = await _BLService.ForSearchValidation(model.filterTextValidation, model.CurrentPage, model.PageSize);
                var response = new ResponsePaged<ResponseJobValidation>();

                response.pageSize = result.pageSize;
                response.totalCount = result.totalCount;
                result.lstResult.ForEach(item =>
                {
                    var obj = new ResponseJobValidation();
                    obj.Map(item);
                    response.lstResult.Add(obj);
                });
                return Ok(response);
            }
            catch (Exception ex)
            {

            }
            return Ok();
        }
     

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminJobSearch(ModelAdminJobSearch model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            //var userId = GetUserId();
            try
            {
                var result = await _BLService.AdminJobSearch(model.CompanyId,model.StatusId, model.filterText,  model.CurrentPage, model.PageSize);
                var response = new ResponsePaged<ResponseJob>();

                response.pageSize = result.pageSize;
                response.totalCount = result.totalCount;
                result.lstResult.ForEach(item =>
                {
                    var obj = new ResponseJob();
                    obj.Map(item);
                    response.lstResult.Add(obj);
                });
                return Ok(response);
            }
            catch (Exception ex)
            {

            }
            return Ok();
        }
        
        public async Task<IActionResult> GetMyJobStats()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userId = GetUserId();
            var result = await _BLService.GetMyJobStats(userId);

            return Ok(result);
        }
        public async Task<IActionResult> GetJobsByCompanyId(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _BLService.GetJobsByCompanyId(model.Id);

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ReportJobCount(ModelReportJob model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            DateTime startDate = string.IsNullOrEmpty(model.StartDate) ? DateTime.MinValue : Convert.ToDateTime(model.StartDate);
            DateTime endDate = string.IsNullOrEmpty(model.EndDate) ? DateTime.MinValue : Convert.ToDateTime(model.EndDate);

            var result = await _BLService.ReportJobCount(model.CompanyId, startDate, endDate, model.JobFieldId);

            return Ok(result);
        }
    }
}
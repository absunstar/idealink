using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Employment.API.Model;
using Employment.API.Model.Model;
using Employment.API.Model.Response;
using Employment.Entity.Model;
using Employment.Entity.Mongo;
using Employment.Interface;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Employment.API.Helpers.Files;

namespace Employment.API.Controllers
{
    public class JobSeekerController : GenericObjectController<JobSeeker, ResponseJobSeeker>
    {
        private readonly IServiceJobSeeker _BLService;
        private readonly IUserProfile BLServiceUserProfile;
        protected readonly ICacheConfig _BLCacheConfig;
        public JobSeekerController(IServiceJobSeeker BLService, IMapper mapper, IUserProfile _BLServiceUserProfile, ICacheConfig cacheConfig) : base(BLService, mapper)
        {
            _BLService = BLService;
            BLServiceUserProfile = _BLServiceUserProfile;
            _BLCacheConfig = cacheConfig;
        }
        [AllowAnonymous]
        public async override Task<IActionResult> GetById(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var UserId = GetUserId();
            try
            {
                var result = await _BLService.GetByUserId(model.Id);
                //var resCanAccess = await _BLService.ContactPermissionHasPermission(model.Id, UserId);
                int canAccess = -1;
                //switch (resCanAccess)
                //{
                //    case null: canAccess = 0; break;
                //    case -1: canAccess = -1; break;
                //    case 1: canAccess = 1; break;
                //    case 0: canAccess = 2; break;
                //}

                var response = new ResponseJobSeeker();
                response.Map(result, canAccess);

                //SS 
                var obj = (JobSeeker)result;
                string _id = obj._id;

                string FileUploadOnCloud = _BLCacheConfig.FileUploadOnCloud;
                if (FileUploadOnCloud == "false")
                {
                    response.ProfilePicture = HelperFiles.GetURLJobSeeker(_id, obj.ProfilePicture, Enum.EnumFileType.ProfilePicture);
                    response.CoverLetterFile = HelperFiles.GetURLJobSeeker(_id, obj.CoverLetterFile, Enum.EnumFileType.CoverLetter);
                    response.ResumeFile = HelperFiles.GetURLJobSeeker(_id, obj.ResumeFile, Enum.EnumFileType.Resume);
                    foreach (var item in response.Certification)
                    {
                        item.CertificatePath = string.IsNullOrEmpty(item.CertificatePath) ? "" : HelperFiles.GetURLJobSeekerCertificate(obj.UserId, item._id, item.CertificatePath);
                    }
                }


                response.Certification = response.Certification.OrderBy(x => x.StartDate).ToList();
                response.Education = response.Education.OrderBy(x => x.StartDate).ToList();
                response.WorkHistory = response.WorkHistory.OrderBy(x => x.StartDate).ToList();

                response.IsMyResume = (UserId == response._id || UserId == result.UserId);
                Console.WriteLine(response);
                return Ok(response);

            }
            catch (Exception ex)
            {

            }
            return Ok();
        }
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> UpdateInfo(ModelJobSeeker model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var UserId = GetUserId();

            var obj = _mapper.Map<ModelJobSeeker, JobSeeker>(model);
            var result = await _BLService.UpdateInfo(UserId, obj);

            return await FormatResult(result);
        }
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> UpdateDescription(ModelJobSeeker model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var UserId = GetUserId();

            var obj = _mapper.Map<ModelJobSeeker, JobSeeker>(model);
            var result = await _BLService.UpdateDescription(UserId, obj.About);

            return await FormatResult(result);
        }
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> UpdateProfile(ModelJobSeeker model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var UserId = GetUserId();

            var obj = _mapper.Map<ModelJobSeeker, JobSeeker>(model);

            obj.Languages = new List<SubItem>();
            foreach (var item in model.Languages)
            {
                obj.Languages.Add(new SubItem(item, ""));
            }

            var result = await _BLService.UpdateProfile(UserId, obj);

            return await FormatResult(result);
        }
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> UpdateSocialMedia(ModelJobSeeker model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var UserId = GetUserId();

            var obj = _mapper.Map<ModelJobSeeker, JobSeeker>(model);
            var result = await _BLService.UpdateSocialMedia(UserId, obj);

            return await FormatResult(result);
        }
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> AddEducation(ModelResumeItem model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var UserId = GetUserId();
            //if(model.EndDate.Date.ToString("dd/MM/yyyy") == "01/01/0001")
            //{
            //    model.EndDate ;
            //}
            var obj = _mapper.Map<ModelResumeItem, ResumeItem>(model);
            obj.GenerateId();
            var result = await _BLService.AddEducation(UserId, obj);

            return await FormatResult(result);
        }
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> UpdateEducation(ModelResumeItem model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var UserId = GetUserId();

            var obj = _mapper.Map<ModelResumeItem, ResumeItem>(model);
            var result = await _BLService.UpdateEducation(UserId, obj);

            return await FormatResult(result);
        }
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> RemoveEducation(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var UserId = GetUserId();

            var result = await _BLService.RemoveEducation(UserId, model.Id);

            return await FormatResult(result);
        }
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> AddWorkExperience(ModelResumeItem model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var UserId = GetUserId();

            var obj = _mapper.Map<ModelResumeItem, ResumeItem>(model);
            obj.GenerateId();
            var result = await _BLService.AddWorkExperience(UserId, obj);

            return await FormatResult(result);
        }
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> UpdateWorkExperience(ModelResumeItem model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var UserId = GetUserId();

            var obj = _mapper.Map<ModelResumeItem, ResumeItem>(model);
            var result = await _BLService.UpdateWorkExperience(UserId, obj);

            return await FormatResult(result);
        }
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> RemoveWorkExperience(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var UserId = GetUserId();

            var result = await _BLService.RemoveWorkExperience(UserId, model.Id);

            return await FormatResult(result);
        }
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> AddExtraCurricular(ModelResumeItem model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var UserId = GetUserId();

            var obj = _mapper.Map<ModelResumeItem, ResumeItem>(model);
            obj.GenerateId();
            var result = await _BLService.AddExtraCurricular(UserId, obj);

            return await FormatResult(result);
        }
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> UpdateExtraCurricular(ModelResumeItem model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var UserId = GetUserId();

            var obj = _mapper.Map<ModelResumeItem, ResumeItem>(model);
            var result = await _BLService.UpdateExtraCurricular(UserId, obj);

            return await FormatResult(result);
        }
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> RemoveExtraCurricular(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var UserId = GetUserId();

            var result = await _BLService.RemoveExtraCurricular(UserId, model.Id);

            return await FormatResult(result);
        }
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> AddCertification(ModelResumeCertification model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var UserId = GetUserId();

            var obj = _mapper.Map<ModelResumeCertification, ResumeCertification>(model);
            obj.GenerateId();
            var result = await _BLService.AddCertification(UserId, obj);

            return await FormatResult(result);
        }
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> UpdateCertification(ModelResumeCertification model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var UserId = GetUserId();

            var obj = _mapper.Map<ModelResumeCertification, ResumeCertification>(model);
            var result = await _BLService.UpdateCertification(UserId, obj);

            return await FormatResult(result);
        }
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> RemoveCertification(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var UserId = GetUserId();

            var result = await _BLService.RemoveCertification(UserId, model.Id);

            return await FormatResult(result);
        }
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> ImportCertification()
        {
            var email = GetUserEmail();
            var baseURL = _BLCacheConfig.URLTadrebatAPI;
            var fullURL = Flurl.Url.Combine(baseURL, "Trainee/GetTraineeCertificates", "?Email=" + email);
            using (var client = new HttpClient())
            {
                string Id = Guid.NewGuid().ToString();

                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(fullURL),
                    Method = HttpMethod.Get,
                };

                request.Content = new StringContent("",//JsonConvert.SerializeObject(obj),
                    Encoding.UTF8,
                            "application/x-www-form-urlencoded");//CONTENT-TYPE header

                HttpResponseMessage response = new HttpResponseMessage();
                try
                {
                    response = await client.SendAsync(request);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                // ... Check Status Code                                
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var lst = JsonConvert.DeserializeObject<IEnumerable<ResponseTraineeCertificates>>(result);
                    foreach (var obj in lst)
                    {
                        var UserId = GetUserId();

                        var cert = new ResumeCertification();
                        cert.Name = obj.TrainingName;
                        cert.StartDate = Convert.ToDateTime(obj.Date);
                        cert.Description = obj.TrainingDescription;

                        string newPath = _BLCacheConfig.FileFolderTemp;
                        if (!Directory.Exists(newPath))
                        {
                            Directory.CreateDirectory(newPath);
                        }
                        string filePath = obj.FilePath;
                        string ext = Path.GetExtension(filePath);
                        string newfileName = Guid.NewGuid().ToString() + ext;
                        string fullPath = Path.Combine(newPath, newfileName);

                        try
                        {
                            System.IO.File.Copy(filePath, fullPath, true);
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }

                        cert.CertificateTadrebatId = Path.GetFileName(filePath);
                        cert.CertificatePath = newfileName;
                        await _BLService.AddCertification(UserId, cert);
                    }
                }
                else
                {
                    return BadRequest("User doesn't exisit in Tadrebat or doesn't have certificates yet.");
                }

            }
            return Ok();
        }

        public async Task<IActionResult> Search(ModelJobSeekerSearch model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var userId = GetUserId();

            var genderList = model.GenderId != null ? model.GenderId.Select(int.Parse).ToList() : null;
            var result = await _BLService.Search(model.filterText, model.ExperienceId, genderList, model.Qualificationid, model.LanguageId, model.CountryId, model.CityId, model.CurrentPage, model.PageSize);
            var response = new ResponsePaged<ResponseJobSeeker>();

            response.pageSize = result.pageSize;
            response.totalCount = result.totalCount;
            foreach (var item in result.lstResult)
            {
                var obj = new ResponseJobSeeker();
                int canAccess = -1;
                obj.Map(item, canAccess);
                response.lstResult.Add(obj);
            }
            return Ok(response);

        }

        [Authorize(Roles = "JobSeeker")]
        // [AllowAnonymous]
        public async Task<IActionResult> UploadFile(ModelFileUpload model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var userId = GetUserId();

            var fileExtension = Path.GetExtension(model.FileName);
            string allowExtension = model.type == Enum.EnumFileType.ProfilePicture ? ".jpg,.png,.jpeg,.bmp" : ".doc,.docx,.pdf";

            if (!allowExtension.Contains(fileExtension.ToLower()))
            {
                return BadRequest("File Extension not allowed");
            }

            var result = await _BLService.UploadFIle(userId, model.FileName, model.type);

            return Ok(result);
        }
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> ContactPermissionRequest(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userId = GetUserId();

            var employer = await BLServiceUserProfile.UserProfileGetById(userId);
            if (employer.MyCompanies.Count == 0)
                return BadRequest();

            var result = await _BLService.ContactPermissionRequest(model.Id, userId, employer.MyCompanies[0]._id, employer.MyCompanies[0].Name);
            return Ok(result);
        }
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> ContactPermissionApprove(ModelId model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var userId = GetUserId();

            var result = await _BLService.ContactPermissionApprove(userId, model.Id);

            return Ok(result);
        }
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> ContactPermissionReject(ModelId model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var userId = GetUserId();

            var result = await _BLService.ContactPermissionReject(userId, model.Id);

            return Ok(result);
        }
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> ContactPermissioGetApprovalList(ModelPaged model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var userId = GetUserId();

            var result = await _BLService.ContactPermissioGetApprovalList(userId, model.CurrentPage, model.PageSize);
            var response = _mapper.Map<MongoResultPaged<CanAccessContactInformation>, ResponsePaged<ResponseContactInformationRequest>>(result);

            return Ok(response);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ReportJobSeekerCount(ModelReportDates model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            DateTime startDate = string.IsNullOrEmpty(model.StartDate) ? DateTime.MinValue : Convert.ToDateTime(model.StartDate);
            DateTime endDate = string.IsNullOrEmpty(model.EndDate) ? DateTime.MinValue : Convert.ToDateTime(model.EndDate);

            var result = await _BLService.ReportJobSeekerCount(startDate, endDate);

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ReportJobSeekerGenderCount(ModelReportDates model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            DateTime startDate = string.IsNullOrEmpty(model.StartDate) ? DateTime.MinValue : Convert.ToDateTime(model.StartDate);
            DateTime endDate = string.IsNullOrEmpty(model.EndDate) ? DateTime.MinValue : Convert.ToDateTime(model.EndDate);

            var result = await _BLService.ReportJobSeekerGenderCount(startDate, endDate);
            var response = new ResponseReportJobSeekerGender();
            response.Male = result.Item1;
            response.Female = result.Item2;
            response.Other = result.Item3;

            return Ok(response);
        }
    }
}
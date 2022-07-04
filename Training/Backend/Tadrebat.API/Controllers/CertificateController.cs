using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tadrebat.API.Helpers.AutoMapper;
using Tadrebat.API.Model.Model;
using Tadrebat.API.Model.Response;
using Tadrebat.Interface;

namespace Tadrebat.API.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CertificateController : BaseController
    {
        private readonly HelperMapperCertificate HLMapperCertificate;
        private readonly ICertificate BLCertificate;
        const string FolderCertificates = "Certificates";
        const string FolderCertificatesTemplate = "Template";
        const string FolderTrainee = "Trainee";
        const string FolderTrainer = "Trainer";
        const string FolderSystem = "System";
        public string currentLang = "en";
        public CertificateController(IMapper mapper,
            HelperMapperCertificate _HLMapperCertificate,
            ICertificate _BLCertificate) : base(mapper)
        {
            HLMapperCertificate = _HLMapperCertificate;
            BLCertificate = _BLCertificate;
        }

        #region public methods
        public async Task<IActionResult> CertificateActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLCertificate.CertificateActivate(model.Id);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CertificateDeActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLCertificate.CertificateDeActivate(model.Id);

            return await FormatResult(result);
        }
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> UploadSystemGenericFile(int fileType, string partnerId = null)
        {
            try
            {
                if (Request.Form.Files.Count == 0 || Request.Form.Files[0].Length == 0)
                    return BadRequest();

                var file = Request.Form.Files[0];
                if (!Path.GetExtension(file.FileName).ToLower().Equals(".pdf"))
                    return BadRequest(GetErrorPDF());

                string filePath = await BLCertificate.UploadCertificateFile(file, fileType, partnerId, null, null);
                bool result = false;
                if (string.IsNullOrEmpty(partnerId))
                    result = await BLCertificate.UpdateSystemGeneric((Enum.EnumCertificateType)fileType, filePath);
                else
                    result = await BLCertificate.UpdatePartnerGeneric((Enum.EnumCertificateType)fileType, partnerId, filePath);
                return Ok(new ResponseFileUpload(filePath));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> UploadCertificateCategoryFile(int fileType, string partnerId, string trainingTypeId, string trainingCategoryId)
        {
            try
            {
                if (Request.Form.Files.Count == 0 || Request.Form.Files[0].Length == 0)
                    return BadRequest();

                var file = Request.Form.Files[0];
                if (!Path.GetExtension(file.FileName).ToLower().Equals(".pdf"))
                    return BadRequest(GetErrorPDF());


                string filePath = await BLCertificate.UploadCertificateFile(file, fileType, partnerId, trainingCategoryId, null);
                bool result = await BLCertificate.UpdatePartnerTrainingCategory((Enum.EnumCertificateType)fileType, partnerId, trainingTypeId, trainingCategoryId, filePath);

                return Ok(new ResponseFileUpload(filePath));

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> UploadCertificateTrainingCenterFile(int fileType, string partnerId, string trainingCenterId, string trainingTypeId, string trainingCategoryId)
        {
            try
            {
                if (Request.Form.Files.Count == 0 || Request.Form.Files[0].Length == 0)
                    return BadRequest();

                var file = Request.Form.Files[0];
                if (!Path.GetExtension(file.FileName).ToLower().Equals(".pdf"))
                    return BadRequest(GetErrorPDF());

                string filePath = await BLCertificate.UploadCertificateFile(file, fileType, null, trainingCategoryId, trainingCenterId);
                bool result = await BLCertificate.UpdateTrainingCenterTrainingCategory((Enum.EnumCertificateType)fileType, partnerId, trainingCenterId, trainingTypeId, trainingCategoryId, filePath);

                return Ok(new ResponseFileUpload(filePath));

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        public async Task<IActionResult> CertificateListAllSystemGeneric()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLCertificate.CertificateListAllSystemGeneric();
            var response = await HLMapperCertificate.MapCertificate(result);

            return Ok(response);
        }
        
        public async Task<IActionResult> CertificateListAllGenericByPartnerId(ModelPaged model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLCertificate.CertificateListAllGenericByPartnerId("", model.CurrentPage, model.PageSize);

            currentLang = GetLanguage();
            var response = await HLMapperCertificate.MapCertificate(result, currentLang);

            return Ok(response);
        }
        public async Task<IActionResult> CertificateListAllByPartnerId(ModelPaged model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLCertificate.CertificateListAllByPartnerId("", "", model.CurrentPage, model.PageSize);

            currentLang = GetLanguage();
            var response = await HLMapperCertificate.MapCertificate(result, currentLang);

            return Ok(response);
        }
        public async Task<IActionResult> CertificateListAllByTrainingCenterId(ModelPaged model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLCertificate.CertificateListAllByTrainingCenterId("", "", model.CurrentPage, model.PageSize);

            currentLang = GetLanguage();
            var response = await HLMapperCertificate.MapCertificate(result, currentLang);

            return Ok(response);
        }
        #endregion

        protected string GetErrorPDF()
        {
            currentLang = GetLanguage();
            var str = currentLang == "ar" ? "يجب أن يكون الملف المحدد هو PDF فقط" : "Selected file must be PDF only.";
            return str;
        }

    }
}
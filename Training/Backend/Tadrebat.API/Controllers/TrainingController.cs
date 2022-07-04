using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Tadrebat.API.Helpers.AutoMapper;
using Tadrebat.API.Helpers.ExportToExcel;
using Tadrebat.API.Model.Model;
using Tadrebat.API.Model.Response;
using Tadrebat.Cache;
using Tadrebat.Entity.Mongo;
using Tadrebat.Enum;
using Tadrebat.Interface;

namespace Tadrebat.API.Controllers
{
    [Authorize(Roles = "Admin,Partner, SubPartner, Trainer, Trainee")]

    public class TrainingController : BaseController
    {

        private readonly ITraining BLServiceTraining;
        private readonly HelperMapperTraining HLMapperTraining;
        private readonly ICacheConfig _BLCacheConfig;
        private readonly ITrainee BLServiceTrainee;
        private readonly IUserProfile BLServiceTrainer;

        public string currentLang = "en";
        public TrainingController(ITraining _BLServiceTraining
                                , IMapper mapper
                                , ICacheConfig BLCacheConfig
                                , HelperMapperTraining _HLMapperTraining
                                , ITrainee _BLServiceTrainee
                                , IUserProfile _BLServiceTrainer) : base(mapper)
        {
            BLServiceTraining = _BLServiceTraining;
            HLMapperTraining = _HLMapperTraining;
            _BLCacheConfig = BLCacheConfig;
            BLServiceTrainee = _BLServiceTrainee;
            BLServiceTrainer = _BLServiceTrainer;
        }
        public async Task<IActionResult> GetById(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLServiceTraining.GetById(model.Id);
            var response = _mapper.Map<Training, ResponseTraining>(result);

            return Ok(response);
        }
        //public async Task<IActionResult> ListActive()
        //{
        //    var result = await BLServiceTraining.ListActive();
        //    var response = _mapper.Map<List<Training>, List<ResponseTraining>>(result);

        //    return Ok(response);
        //}
        //public async Task<IActionResult> ListSearch(string filter)
        //{
        //    var result = await BLServiceTraining.SearchActive(filter);
        //    var response = _mapper.Map<List<Training>, List<ResponseTraining>>(result);

        //    return Ok(response);
        //}
        public async Task<IActionResult> ListAll(ModelTrainingSearch model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            //SS -Commented because not getting proper id
            //var userId = GetUserId();
            var role = GetUserRole();
            currentLang = GetLanguage();

            string userId = string.Empty;
            if (role == EnumUserTypes.Trainee)
            {
                var userDetails = await BLServiceTrainee.GetByEmail(this.User.Identity.Name);
                userId = userDetails._id;
            }
            else
            {
                var userDetails = await BLServiceTrainer.UserProfileGetByEmail(this.User.Identity.Name);
                userId = userDetails == null ? "" : userDetails._id;
            }

            var result = await BLServiceTraining.ListAll(userId, role, model.PartnerId, model.SubPartnerId, model.TrainerId, model.TrainingTypeId, model.TrainingCategoryId, model.CurrentPage, model.PageSize);
            var response = await HLMapperTraining.MapTraining(result, currentLang);

            return Ok(response);
        }
        public async Task<IActionResult> ExportTrainingTrainee(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLServiceTraining.GetById(model.Id);
            var response = _mapper.Map<Training, ResponseTraining>(result);

            string file = Path.Combine("ExportReport", "TrainingTraineeList-" + Guid.NewGuid().ToString() + ".xlsx");

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("List");
                string tab = string.Empty;
                int count = 1;

                worksheet.Cell(1, count++).Value = "Name";
                worksheet.Cell(1, count++).Value = "Email";
                worksheet.Cell(1, count++).Value = "Mobile";
                worksheet.Cell(1, count++).Value = "NationalId";

                count = 2;
                foreach (var item in response.Trainees)
                {
                    worksheet.Cell(count, 1).Value = item.Name;
                    worksheet.Cell(count, 2).Value = item.Email;
                    worksheet.Cell(count, 3).Value = item.Mobile;
                    worksheet.Cell(count, 4).Value = item.NationalId;
                    count++;
                }
                workbook.SaveAs(Path.Combine(_BLCacheConfig.UploadFolder, file));
            }

            return Ok(file);
            return Ok(file); ;
        }
        public async Task<IActionResult> ExportTraining(ModelTrainingSearch model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var userId = GetUserId();
            var role = GetUserRole();
            currentLang = GetLanguage();

            var result = await BLServiceTraining.ListAll(userId, role, model.PartnerId, model.SubPartnerId, model.TrainerId, model.TrainingTypeId, model.TrainingCategoryId, 1, int.MaxValue);
            var response = await HLMapperTraining.MapTraining(result, currentLang);

            string file = Path.Combine("ExportReport", "TrainingList-" + Guid.NewGuid().ToString() + ".xlsx");

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("List");
                string tab = string.Empty;
                int count = 1;

                worksheet.Cell(1, count++).Value = "Trainer (Partner)";
                worksheet.Cell(1, count++).Value = "Type(Category)";
                worksheet.Cell(1, count++).Value = "Dates";

                count = 2;
                foreach (var item in response.lstResult)
                {
                    worksheet.Cell(count, 1).Value = item.TrainerCount.ToString() + " - " + item.TrainerDetails.Name + "( " + item.PartnerId.Name + " )";
                    worksheet.Cell(count, 2).Value = item.TrainingTypeId.Name + "( " + item.TrainingCategoryId.Name + " )";
                    worksheet.Cell(count, 3).Value = item.StartDate.ToString("dd/MM/yyyy") + " - " + item.EndDate.ToString("dd/MM/yyyy");
                    count++;
                }
                workbook.SaveAs(Path.Combine(_BLCacheConfig.UploadFolder, file));
            }

            return Ok(file);
        }
        [Authorize(Roles = "Admin,Partner,SubPartner, Trainer")]
        public async Task<IActionResult> Create(ModelTraining model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var obj = _mapper.Map<ModelTraining, Training>(model);
            obj = await HLMapperTraining.UpdateTraining(obj);
            var result = await BLServiceTraining.Create(obj);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin,Partner,SubPartner, Trainer")]
        public async Task<IActionResult> Update(ModelTraining model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (string.IsNullOrEmpty(model.Id) || model.EndDate.Date < DateTime.Now.Date)
                return BadRequest("Cannot edit training");

            var obj = _mapper.Map<ModelTraining, Training>(model);
            obj = await HLMapperTraining.UpdateTraining(obj);
            var result = await BLServiceTraining.Update(obj);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SaveExamTemplate(ModelEntitySubEntityIds model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (string.IsNullOrEmpty(model.MainEntityId) || string.IsNullOrEmpty(model.SubEntityId))
                return BadRequest();

            var result = await BLServiceTraining.SaveExamTemplate(model.MainEntityId, model.SubEntityId);

            return await FormatResult(result);
        }

        [Authorize(Roles = "Admin,Partner,SubPartner, Trainer")]
        public async Task<IActionResult> SetConfirmed1(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLServiceTraining.SetConfirmed1(model.Id);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin,Partner,SubPartner, Trainer")]
        public async Task<IActionResult> SetConfirmed2(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLServiceTraining.SetConfirmed2(model.Id);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin,Partner,SubPartner, Trainer")]
        public async Task<IActionResult> SetAdminApproved(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLServiceTraining.SetAdminApproved(model.Id);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin,Partner,SubPartner, Trainer")]
        public async Task<IActionResult> DeActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLServiceTraining.DeActivate(model.Id);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin,Partner,SubPartner, Trainer")]
        public async Task<IActionResult> SaveAttendnace(ModelSaveAttendance model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var att = _mapper.Map<ModelAttendance, Attendance>(model.Attendances);
            var result = await BLServiceTraining.SaveAttendnace(model.trainingId, att);

            return await FormatResult(result);
        }
    }
}
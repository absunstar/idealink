using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Employment.API.Model.Model;
using Employment.API.Model.Response;
using Employment.Entity.Mongo;
using Employment.Interface;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Employment.API.Controllers
{
    public class JobFairController : GenericObjectController<JobFair, ResponseJobFair>
    {
        private readonly IServiceJobFair _BLService;
        protected readonly ICacheConfig _BLCacheConfig;
        public JobFairController(IServiceJobFair BLService, IMapper mapper, ICacheConfig cacheConfig) : base(BLService, mapper)
        {
            _BLService = BLService;
            _BLCacheConfig = cacheConfig;
        }
        public virtual async Task<IActionResult> Create(ModelJobFair model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var obj = _mapper.Map<ModelJobFair, JobFair>(model);
            obj.GenerateId();
            var result = await _BLService.Create(obj);

            return await FormatResult(result);
        }
        public async Task<IActionResult> Update(ModelJobFair model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (string.IsNullOrEmpty(model._id))
                return BadRequest();

            var obj = _mapper.Map<ModelJobFair, JobFair>(model);
            var result = await _BLService.UpdateFair(obj);

            return await FormatResult(result);
        }
        public async Task<IActionResult> Search(ModelPaged model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var result = await _BLService.Search(model.filterText, model.CurrentPage, model.PageSize);
            var response = new ResponsePaged<ResponseJobFair>();

            response.pageSize = result.pageSize;
            response.totalCount = result.totalCount;
            result.lstResult.ForEach(item =>
            {
                var obj = new ResponseJobFair();
                obj.Map(item);
                response.lstResult.Add(obj);
            });
            return Ok(response);
        }
        public async Task<IActionResult> CheckRegister(ModelId model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var UserId = GetUserId();

            var result = await _BLService.CheckRegister(model.Id, UserId);

            return Ok(result);
        }
        public async Task<IActionResult> Register(ModelJobFairRegisteration model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var UserId = GetUserId();
            var role = GetUserRole();
            var obj = _mapper.Map<ModelJobFairRegisteration, JobFairRegisteration>(model);

            var result = await _BLService.Register(model.JobFairId, obj, role);

            return Ok(result);
        }
        public async Task<IActionResult> SetAttendance(ModelJobFairAttendance model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var result = await _BLService.SetAttendance(model.JobFairId, model.Code);

            return Ok(result);
        }
        public async Task<IActionResult> ExportRegisteredUser(ModelId model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var jobFair = await _BLService.GetById(model.Id);
            if (jobFair != null)
            {
                var lst = jobFair.Registered;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                try
                {
                    ExcelPackage excel = new ExcelPackage();
                    var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(1).Height = 20;
                    workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(1).Style.Font.Bold = true;
                    workSheet.Cells[1, 1].Value = "S.No";
                    workSheet.Cells[1, 2].Value = "Id";
                    workSheet.Cells[1, 3].Value = "Name";
                    workSheet.Cells[1, 4].Value = "JobTitle";
                    workSheet.Cells[1, 5].Value = "DOB";
                    workSheet.Cells[1, 6].Value = "IsAttendance";
                    workSheet.Cells[1, 7].Value = "Code";
                    workSheet.Cells[1, 8].Value = "Email";
                    workSheet.Cells[1, 9].Value = "Data";
                    //Body of table  
                    //  
                    int recordIndex = 2;
                    foreach (var student in lst)
                    {
                        workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                        workSheet.Cells[recordIndex, 2].Value = student._id;
                        workSheet.Cells[recordIndex, 3].Value = student.Name;
                        workSheet.Cells[recordIndex, 4].Value = student.JobTitle;
                        workSheet.Cells[recordIndex, 5].Value = student.DOB;
                        workSheet.Cells[recordIndex, 6].Value = student.IsAttendance;
                        workSheet.Cells[recordIndex, 7].Value = student.Code;
                        workSheet.Cells[recordIndex, 8].Value = student.Email;

                        string data = "";
                        if (student.data.Count > 0)
                            data = string.Join(",", student.data);

                        workSheet.Cells[recordIndex, 9].Value = data;
                        recordIndex++;
                    }
                    workSheet.Column(1).AutoFit();
                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).AutoFit();
                    workSheet.Column(4).AutoFit();
                    string excelName = "JobFairRecord-" + model.Id + ".xlsx";
                    string path = Path.Combine(_BLCacheConfig.FileFolderRoot, "ExportReport");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    excel.SaveAs(new FileInfo(Path.Combine(path, excelName)));
                    
                    return Ok(Path.Combine(_BLCacheConfig.URLCDN, "ExportReport", excelName));
                }
                catch (Exception ex)
                {

                }

                //using (var memoryStream = new MemoryStream())
                //{
                //    excel.SaveAs(memoryStream);
                //    memoryStream.Seek(0, SeekOrigin.Begin);
                //    return this.File(
                //        fileContents: memoryStream.ToArray(),
                //        contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",

                //        // By setting a file download name the framework will
                //        // automatically add the attachment Content-Disposition header
                //        fileDownloadName: "ERSheet.xlsx"
                //    );
                //}

            }
            return Ok();
        }

    }
}
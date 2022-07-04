using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using Tadrebat.API.Helpers.AutoMapper;
using Tadrebat.API.Helpers.Constants;
using Tadrebat.API.Helpers.HTTPCall;
using Tadrebat.API.Model.Model;
using Tadrebat.API.Model.Response;
using Tadrebat.Entity.Mongo;
using Tadrebat.Enum;
using Tadrebat.Interface;
using Tadrebat.ModelsGlobal;

namespace Tadrebat.API.Controllers
{
    //[Authorize(Roles = "Admin,Partner,SubPartner, Trainer,Trainee")]

    public class TraineeController : BaseController
    {

        private readonly HTTPCallSTS HTTPCallSTS;
        private readonly ITrainee BLServiceTrainee;
        private readonly ITraining BLServiceTraining;
        private readonly IUserProfile BLServiceTrainer;
        private readonly HelperTranslate HLHelperTranslate;
        private readonly ICacheConfig _BLCacheConfig;
        private readonly IDataManagement BLDataManagement;
        public string currentLang = "en";
        public TraineeController(HTTPCallSTS _HTTPCallSTS
                                , ITrainee _BLServiceTrainee
                                , ITraining _BLServiceTraining
                                , ICacheConfig BLCacheConfig
                                , IUserProfile _BLServiceTrainer
                                , HelperTranslate _HLHelperTranslate
                                , IDataManagement _BLDataManagement
                                , IMapper mapper) : base(mapper)
        {
            HTTPCallSTS = _HTTPCallSTS;
            BLServiceTrainee = _BLServiceTrainee;
            BLServiceTraining = _BLServiceTraining;
            BLServiceTrainer = _BLServiceTrainer;
            HLHelperTranslate = _HLHelperTranslate;
            _BLCacheConfig = BLCacheConfig;
            BLDataManagement = _BLDataManagement;
        }
        [Authorize(Roles = "Trainee")]
        public async Task<IActionResult> getMyProfile()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            //SS -Commented because not getting proper id
            // var Id = GetUserId();
            string userId = string.Empty;
            var role = GetUserRole();

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

            // var result = await BLServiceTrainee.GetById(Id);
            var result = await BLServiceTrainee.GetById(userId);
            var response = _mapper.Map<Trainee, ResponseTrainee>(result);

            return Ok(response);
        }

        public async Task<IActionResult> GetById(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLServiceTrainee.GetById(model.Id);
            var response = _mapper.Map<Trainee, ResponseTrainee>(result);

            return Ok(response);
        }
        [Authorize(Roles = "Admin,Partner,SubPartner, Trainer")]
        public async Task<IActionResult> ListActive()
        {
            var result = await BLServiceTrainee.ListActive();
            var response = _mapper.Map<List<Trainee>, List<ResponseTrainee>>(result);

            return Ok(response);
        }
        [Authorize(Roles = "Admin,Partner,SubPartner, Trainer")]
        public async Task<IActionResult> ListSearch(ModelPaged model)
        {
            model.PageSize = 20;
            var result = await BLServiceTrainee.SearchActive(model.filterText, model.CurrentPage, model.PageSize);
            var response = _mapper.Map<List<Trainee>, List<ResponseTrainee>>(result);

            return Ok(response);
        }
        [Authorize(Roles = "Admin,Partner,SubPartner, Trainer")]
        public async Task<IActionResult> ListAll(ModelAccountSearch model)
        {
            if (!ModelState.IsValid)
                BadRequest();
            var role = GetUserRole();
            //if admin send empty userid, otherwise send partner userid
            string UserId = role == Enum.EnumUserTypes.Admin ? "" : GetUserId();

            var result = await BLServiceTrainee.ListAll(model.filterText, model.CurrentPage, model.PageSize);
            var response = _mapper.Map<MongoResultPaged<Trainee>, ResponsePaged<ResponseTrainee>>(result);

            return Ok(response);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Create(ModelTrainee model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = new Trainee();
            user.Name = model.Name;
            user.Email = model.Email;
            user.NationalId = model.NationalId;
            user.Gender = model.Gender;
            user.Mobile = model.Mobile;
            user.DOB = model.DOB;
            user.data = model.data;
            user.IdType = model.IdType;

            //check if can add to training
            if (!string.IsNullOrEmpty(model.TrainingId))
            {
                var training = await BLServiceTraining.GetById(model.TrainingId);
                if (training == null)
                    return BadRequest("Training not found");

                if (training.StartDate < DateTime.Now.AddDays(-2))
                    return BadRequest("Cannot add Trainee to training as it passed 2 days after the start date.");

            }
            //craete user
            var result = await BLServiceTrainee.Create(user, model.TrainingId);
            if (result)
            {
                var MDUser = await BLServiceTrainee.GetByEmail(user.Email);

                result = await HTTPCallSTS.RegisterUser(model.Email, MDUser._id);
            }
            else
            {
                return BadRequest("Email already exists.");
            }
            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin,Partner,SubPartner, Trainer")]
        public async Task<IActionResult> Update(ModelTrainee model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (string.IsNullOrEmpty(model.Id))
                return BadRequest();

            var obj = _mapper.Map<ModelTrainee, Trainee>(model);
            var result = await BLServiceTrainee.Update(obj);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Trainee")]
        public async Task<IActionResult> updateMyProfile(ModelTrainee model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (string.IsNullOrEmpty(model.Id))
                return BadRequest();

            var obj = _mapper.Map<ModelTrainee, Trainee>(model);
            var result = await BLServiceTrainee.Update(obj);

            return await FormatResult(result);
        }

        public async Task<IActionResult> Activate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLServiceTrainee.Activate(model.Id);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin,Partner,SubPartner, Trainer")]
        public async Task<IActionResult> DeActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLServiceTrainee.DeActivate(model.Id);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin,Partner,SubPartner, Trainer")]
        public async Task<IActionResult> RemoveTraining(ModelTraineeTraining model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLServiceTrainee.RemoveTraining(model.TraineeId, model.TrainingId);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin,Partner,SubPartner, Trainer")]
        public async Task<IActionResult> AddTraining(ModelTraineeTraining model)
        {
            if (!ModelState.IsValid)
                return BadRequest();


            var training = await BLServiceTraining.GetById(model.TrainingId);
            if (training == null)
                return BadRequest("Training not found");

            if (training.StartDate.Date < DateTime.Now.AddDays(-2).Date)
                return BadRequest("Cannot add Trainee to training as it passed 2 days after the start date.");

            var result = await BLServiceTrainee.AddTraining(model.TraineeId, model.TrainingId);

            return await FormatResult(result);
        }
        //  [Authorize(Roles = "Trainee")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMyTraining()
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                // var userId = GetUserId();
                // var userId = "5ed0ad78c7398b4160453440";
                //string userId = "60e2a88e65223114cc112760";
                string userId = "";
                var role = GetUserRole();

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

                var user = await BLServiceTrainee.GetById(userId);
                var response = new ResponseMyTraining();
                response.Profile = _mapper.Map<Trainee, ResponseTrainee>(user);

                var result = new List<ResponseMyTrainingItems>();
                foreach (var t in user.myTrainings.ToList().OrderByDescending(x => x.TrainingId))
                {
                    var training = await BLServiceTraining.GetById(t.TrainingId);
                    if (training == null)
                        continue;

                    if (!training.IsActive.GetValueOrDefault())
                        continue;

                    if (!training.Trainees.Any(x => x.IsApproved == true && x._Id == userId))
                        continue;

                    var trainer = await BLServiceTrainer.UserProfileGetById(training.TrainerId);

                    var obj = _mapper.Map<TraineeTraining, ResponseMyTrainingItems>(t);

                    obj.Name = training.TrainerCount + " - " + trainer.Name + " (" + training.PartnerId.Name + ")";
                    obj.Date = training.StartDate.ToString("dd/MM/yyyy") + " - " + training.EndDate.ToString("dd/MM/yyyy");
                    result.Add(obj);

                }
                response.trainings = result;

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.InnerException);
            }
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetTraineeCertificates(string Email)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {

                var user = await BLServiceTrainee.GetByEmail(Email);
                if (user == null)
                {
                    return BadRequest();
                }
                var result = new List<ResponseTraineeCertificates>();

                foreach (var t in user.myTrainings.Where(x => !string.IsNullOrEmpty(x.CertificatePath)).ToList().OrderByDescending(x => x.TrainingId))
                {
                    var training = await BLServiceTraining.GetById(t.TrainingId);
                    if (training == null)
                        continue;

                    if (!training.IsActive.GetValueOrDefault())
                        continue;

                    if (!training.Trainees.Any(x => x.IsApproved == true && x._Id == user._id))
                        continue;

                    var obj = new ResponseTraineeCertificates();

                    var type = await BLDataManagement.TrainingTypeGetById(training.TrainingTypeId);
                    var cat = await BLDataManagement.TrainingCategoryGetById(training.TrainingCategoryId);

                    obj.TrainingName = type.Name + " - " + cat.Name;
                    obj.Date = training.StartDate.ToString("yyyy/MM/dd");
                    obj.FilePath = Path.Combine(_BLCacheConfig.UploadFolder, t.CertificatePath);

                    result.Add(obj);
                }



                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.InnerException);
            }
        }
        [Authorize(Roles = "Admin,Partner,SubPartner, Trainer")]
        public async Task<IActionResult> GetTraineeTraining(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();


            //userId = "5ed0ad78c7398b4160453440";
            var user = await BLServiceTrainee.GetById(model.Id);
            var response = new ResponseMyTraining();
            response.Profile = _mapper.Map<Trainee, ResponseTrainee>(user);

            var result = new List<ResponseMyTrainingItems>();
            foreach (var t in user.myTrainings.ToList().OrderByDescending(x => x.TrainingId))
            {
                var training = await BLServiceTraining.GetById(t.TrainingId);
                if (training == null)
                    continue;

                if (!training.IsActive.GetValueOrDefault())
                    continue;

                if (!training.Trainees.Any(x => x.IsApproved == true && x._Id == model.Id))
                    continue;

                var trainer = await BLServiceTrainer.UserProfileGetById(training.TrainerId);

                var obj = _mapper.Map<TraineeTraining, ResponseMyTrainingItems>(t);

                obj.Name = training.TrainerCount + " - " + trainer.Name + " (" + training.PartnerId.Name + ")";
                obj.Date = training.StartDate.ToString("dd/MM/yyyy") + " - " + training.EndDate.ToString("dd/MM/yyyy");

                result.Add(obj);

            }
            response.trainings = result;

            return Ok(response);
        }
        [Authorize(Roles = "Trainee")]
        public async Task<IActionResult> TraineeRegister(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            //SS -UserId not getting proper
            // var userId = GetUserId();

            var userDetails = await BLServiceTrainee.GetByEmail(this.User.Identity.Name);
            string userId = userDetails._id;

            var result = await BLServiceTrainee.AddTraining(userId, model.Id, false);

            return await FormatResult(result);
        }
        public async Task<IActionResult> ApproveTraineeRegister(ModelTraineeTraining model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLServiceTrainee.ApproveTraineeRegister(model.TraineeId, model.TrainingId);

            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin,Partner,SubPartner, Trainer")]
        public async Task<IActionResult> ImportTrainee(string TrainingId)
        {
            try
            {
                var response = new ResponseImportTrainee();

                var training = await BLServiceTraining.GetById(TrainingId);
                if (training == null)
                {
                    response.TraineeError("Training not found", "");
                    return Ok(response);
                }

                if (training.StartDate < DateTime.Now.AddDays(-2))
                {
                    response.TraineeError("Cannot add Trainee to training as it passed 2 days after the start date.", "");
                    return Ok(response);
                }

                string newPath = Path.Combine(_BLCacheConfig.UploadFolderTemp, "Uploaded");
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                var filePath = Path.GetTempFileName();
                var file = Request.Form.Files[0];

                using (var inputStream = new FileStream(filePath, FileMode.Create))
                {
                    // read file to stream
                    await file.CopyToAsync(inputStream);
                    // stream to byte array
                    byte[] array = new byte[inputStream.Length];
                    inputStream.Seek(0, SeekOrigin.Begin);
                    inputStream.Read(array, 0, array.Length);
                    // get file name
                    string fName = file.FileName;

                    if (file.Length > 0)
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        //FileInfo filex = new FileInfo(newPath);
                        using (ExcelPackage package = new ExcelPackage(inputStream))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                            int rowCount = worksheet.Dimension.Rows;
                            int ColCount = worksheet.Dimension.Columns;

                            var rawText = string.Empty;
                            var lstError = new List<TraineeError>();
                            for (int row = 2; row <= rowCount; row++) //start row 2 to ignore titles
                            {
                                if (worksheet.Cells[row, 1].Value == null)
                                    break;
                                var user = new Trainee();
                                user.Name = worksheet.Cells[row, 1].Value.ToString();
                                user.Email = worksheet.Cells[row, 2].Value.ToString();
                                user.Mobile = worksheet.Cells[row, 3].Value.ToString();
                                user.NationalId = worksheet.Cells[row, 4].Value.ToString();
                                user.Gender = Convert.ToInt32(worksheet.Cells[row, 5].Value.ToString());
                                try
                                {
                                    user.DOB = Convert.ToDateTime(worksheet.Cells[row, 6].Text.ToString());
                                }
                                catch (Exception ex)
                                {
                                    var objError = _mapper.Map<Trainee, TraineeError>(user);
                                    objError.Error = "Invalid DOB";
                                    lstError.Add(objError);

                                    continue;
                                }
                                var obj = await BLServiceTrainee.GetByEmail(user.Email);
                                if (obj != null) //email found
                                {
                                    var result = await BLServiceTrainee.AddTraining(obj._id, TrainingId);
                                }
                                else // add new user
                                {
                                    if (string.IsNullOrEmpty(user.Name))
                                    {
                                        var objError = _mapper.Map<Trainee, TraineeError>(user);
                                        objError.Error = "Name is Required Field";
                                        lstError.Add(objError);

                                        continue;
                                    }
                                    if (string.IsNullOrEmpty(user.Email))
                                    {
                                        var objError = _mapper.Map<Trainee, TraineeError>(user);
                                        objError.Error = "Email is Required Field";
                                        lstError.Add(objError);

                                        continue;
                                    }
                                    if (!IsValidEmail(user.Email))
                                    {
                                        var objError = _mapper.Map<Trainee, TraineeError>(user);
                                        objError.Error = "Email format is invalid";
                                        lstError.Add(objError);

                                        continue;
                                    }
                                    if (string.IsNullOrEmpty(user.Mobile))
                                    {
                                        var objError = _mapper.Map<Trainee, TraineeError>(user);
                                        objError.Error = "Mobile is Required Field";
                                        lstError.Add(objError);

                                        continue;
                                    }
                                    if (!IsDigitsOnly(user.Mobile))
                                    {
                                        var objError = _mapper.Map<Trainee, TraineeError>(user);
                                        objError.Error = "Mobile should contain numbers only";
                                        lstError.Add(objError);

                                        continue;
                                    }
                                    if (string.IsNullOrEmpty(user.NationalId))
                                    {
                                        var objError = _mapper.Map<Trainee, TraineeError>(user);
                                        objError.Error = "NationalId is Required Field";
                                        lstError.Add(objError);

                                        continue;
                                    }
                                    if (user.Gender != 1 && user.Gender != 2)
                                    {

                                        var objError = _mapper.Map<Trainee, TraineeError>(user);
                                        objError.Error = "Gender can only have value 1 for Male or 2 Female";
                                        lstError.Add(objError);
                                        continue;
                                    }

                                    //craete user
                                    var result = await BLServiceTrainee.Create(user, TrainingId);
                                    if (result)
                                    {
                                        var MDUser = await BLServiceTrainee.GetByEmail(user.Email);

                                        result = await HTTPCallSTS.RegisterUser(user.Email, MDUser._id);
                                    }
                                    else
                                    {
                                        string error = "Email already linked to a non trainee account";
                                    }
                                }
                            }

                            if (lstError.Count() > 0)
                            {
                                //export Errors
                                newPath = Path.Combine(_BLCacheConfig.UploadFolderTemp, "Errors");
                                if (!Directory.Exists(newPath))
                                {
                                    Directory.CreateDirectory(newPath);
                                }

                                using (var excel = new OfficeOpenXml.ExcelPackage())
                                {
                                    var sheet = excel.Workbook.Worksheets.Add("Error");
                                    sheet.Cells["A1"].LoadFromCollection(lstError, true);
                                    string fileName = "Error-" + Guid.NewGuid().ToString() + ".xlsx";
                                    excel.SaveAs(new FileInfo(Path.Combine(newPath, fileName)));

                                    response.TraineeError("Please check these records, they have invalid data.", @"Errors/" + fileName);
                                    return Ok(response);
                                }
                            }

                        }
                        return Ok(response);
                    }
                    else
                    {
                        response.TraineeError("Cannot upload empty file.", "");
                        return Ok(response);
                    }
                }
            }
            catch (System.Exception ex)
            {
                var response = new ResponseImportTrainee();
                response.TraineeError("Upload Failed: " + ex.Message, "");
                return Ok(response);
            }
        }
        [Authorize(Roles = "Admin,Partner,SubPartner, Trainer")]
        public async Task<IActionResult> ResendActivationLink(ModelEmail model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await HTTPCallSTS.ResendActivationLink(model.Email);
            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin,Partner,SubPartner, Trainer")]
        public async Task<IActionResult> ResendPasswordLink(ModelEmail model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await HTTPCallSTS.ResendPasswordLink(model.Email);
            return await FormatResult(result);
        }
        [Authorize(Roles = "Admin,Partner,SubPartner, Trainer")]
        public async Task<IActionResult> DownloadTrainingCertificate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            // model.Id = "5efa1e1f98adb20910fee090";

            var lst = await BLServiceTraining.GetCourseCertificates(model.Id);

            if (lst.Count == 0)
            {
                return BadRequest("No Certificates found");
            }

            // https://www.c-sharpcorner.com/article/download-multiple-files-in-compressed-format-in-asp-net-mvc-5-step-by-step/
            //https://stackoverflow.com/questions/17232414/creating-a-zip-archive-in-memory-using-system-io-compression
            using (var memoryStream = new MemoryStream())
            {
                using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    var lstCertificate = new List<TraineeTraining>();
                    lst.ForEach(user =>
                    {
                        var t = user.myTrainings.Where(x => x.TrainingId == model.Id && x.HasCertificate == true).FirstOrDefault();
                        if (t != null)
                        {
                            string strFilePath = Path.Combine(_BLCacheConfig.CertificateBaseUrl, t.CertificatePath);
                            string strfileName = user.Name + "-" + Path.GetFileName(t.CertificatePath);
                            ziparchive.CreateEntryFromFile(strFilePath, strfileName);
                        }
                    });

                    //for (int i = 0; i < lst.Count; i++)
                    //{
                    //    string strFilePath = Path.Combine(_BLCacheConfig.CertificateBaseUrl, lst[i].CertificatePath);
                    //    string strfileName = Path.GetFileName(lst[i].CertificatePath);
                    //    ziparchive.CreateEntryFromFile(strFilePath, strfileName);
                    //}
                }

                string fileName = Guid.NewGuid().ToString();
                var path = Path.Combine(_BLCacheConfig.CertificateBaseUrl, "Certificates", "Compressed");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                path = Path.Combine(path, fileName + ".zip");

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    memoryStream.CopyTo(fileStream);
                }
                return Ok(Path.Combine("Certificates", "Compressed", fileName + ".zip"));

            }
        }

    }
}
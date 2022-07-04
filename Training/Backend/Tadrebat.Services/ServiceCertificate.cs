using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;
using Tadrebat.Interface;
using Tadrebat.MongoDB.Interface;

namespace Tadrebat.Services
{
    public class CertificateFieldName
    {
        public const string UnquieIdentifier = "UnquieIdentifier";
        public const string Name = "Name";
        public const string TrainingName = "TrainingName";
        public const string TrainingDetails = "TrainingDetails";
        public const string TrainingCategories = "TrainingCategories";
        public const string StartDate = "StartDate";
        public const string EndDate = "EndDate";
        //public const string Achievement = "Achievement";
        //public const string Month = "Month";
        //public const string Day = "Day";
        //public const string Year = "Year";
        //public const string SignatoryName = "SignatoryName";
        //public const string SignatoryTitle = "SignatoryTitle";


    }
    public class ServiceCertificate : ICertificate
    {
        const string FolderCertificates = "Certificates";
        const string FolderCertificatesTemplate = "Template";
        const string FolderCertificatesGenerated = "Generated";
        const string FolderTrainee = "Trainee";
        const string FolderTrainer = "Trainer";
        const string FolderSystem = "System";

        private readonly IDBCertificate _dBCertificate;
        private readonly ICacheConfig _cacheConfig;
        private readonly IDataManagement _BLDataManagement;
        private readonly ITrainee _BLTrainee;
        private readonly ITraining _BLTraining;
        private readonly IUserProfile _BLUserProfile;
        public ServiceCertificate(IDBCertificate dBCertificate, ICacheConfig cacheConfig, IDataManagement BLDataManagement, ITrainee BLTrainee, ITraining BLTraining, IUserProfile BLUserProfile)
        {
            _dBCertificate = dBCertificate;
            _cacheConfig = cacheConfig;
            _BLDataManagement = BLDataManagement;
            _BLTrainee = BLTrainee;
            _BLTraining = BLTraining;
            _BLUserProfile = BLUserProfile;
        }
        public async Task<Certificate> CertificateGetById(string Id)
        {
            return await _dBCertificate.GetById(Id);
        }
        public async Task<bool> CertificateCreate(Certificate obj)
        {
            await _dBCertificate.AddAsync(obj);

            return true;
        }
        public async Task<bool> CertificateUpdate(Certificate obj)
        {
            var q = await CertificateGetById(obj._id);
            if (q == null)
                return false;

            obj.CreatedAt = q.CreatedAt;
            await _dBCertificate.UpdateObj(obj._id, obj);

            return true;
        }
        public async Task<bool> CertificateDeActivate(string Id)
        {

            var obj = await CertificateGetById(Id);
            if (obj == null)
                return false;

            await _dBCertificate.DeactivateAsync(Id);

            return true;
        }
        public async Task<bool> CertificateActivate(string Id)
        {
            var obj = await CertificateGetById(Id);
            if (obj == null)
                return false;

            await _dBCertificate.ActivateAsync(Id);

            return true;
        }
        public async Task<List<Certificate>> CertificateListActive()
        {
            var sort = Builders<Certificate>.Sort.Descending(x => x.CreatedAt);
            var lst = await _dBCertificate.ListActive(sort);
            return lst;
        }
        public async Task<MongoResultPaged<Certificate>> CertificateListAll(int pageNumber = 1, int PageSize = 15)
        {
            var lst = await _dBCertificate.ListAll(pageNumber, PageSize);
            return lst;
        }
        public async Task<MongoResultPaged<Certificate>> CertificateListAllByTrainingCenterId(string PartnerId, string TrainingTypeId, int pageNumber = 1, int PageSize = 15)
        {
            //if (!string.IsNullOrEmpty(PartnerId))
            //    return new MongoResultPaged<Certificate>(0, new List<Certificate>(), PageSize);

            var filter = Builders<Certificate>.Filter.Where(x => x.IsPartnerGeneric == false
                                                         && x.IsSystemGeneric == false
                                                         && x.TrainingCenterId != null
                                                        );
            if (!string.IsNullOrEmpty(PartnerId))
            {
                filter = filter & Builders<Certificate>.Filter.Where(x => x.PartnerId == PartnerId);
            }
            if (!string.IsNullOrEmpty(TrainingTypeId))
            {
                filter = filter & Builders<Certificate>.Filter.Where(x => x.TrainingTypeId == TrainingTypeId);
            }
            var sort = Builders<Certificate>.Sort.Descending(x => x.CreatedAt);

            var lst = await _dBCertificate.GetPaged(filter, sort, pageNumber, PageSize);
            return lst;
        }
        public async Task<MongoResultPaged<Certificate>> CertificateListAllByPartnerId(string PartnerId, string TrainingTypeId, int pageNumber = 1, int PageSize = 15)
        {
            //if (!string.IsNullOrEmpty(PartnerId))
            //    return new MongoResultPaged<Certificate>(0, new List<Certificate>(), PageSize);

            var filter = Builders<Certificate>.Filter.Where(x => x.IsPartnerGeneric == false
                                                         && x.IsSystemGeneric == false
                                                         && x.TrainingCenterId == null
                                                         );
            if (!string.IsNullOrEmpty(PartnerId))
            {
                filter = filter & Builders<Certificate>.Filter.Where(x => x.PartnerId == PartnerId);
            }
            if (!string.IsNullOrEmpty(TrainingTypeId))
            {
                filter = filter & Builders<Certificate>.Filter.Where(x => x.TrainingTypeId == TrainingTypeId);
            }
            var sort = Builders<Certificate>.Sort.Descending(x => x.CreatedAt);

            var lst = await _dBCertificate.GetPaged(filter, sort, pageNumber, PageSize);
            return lst;
        }
        public async Task<MongoResultPaged<Certificate>> CertificateListAllGenericByPartnerId(string PartnerId, int pageNumber = 1, int PageSize = 15)
        {
            //if (!string.IsNullOrEmpty(PartnerId))
            //    return new MongoResultPaged<Certificate>(0,new List<Certificate>(),PageSize);

            var filter = Builders<Certificate>.Filter.Where(x => x.IsPartnerGeneric == true
                                                         && x.IsSystemGeneric == false
                                                         );

            if (!string.IsNullOrEmpty(PartnerId))
            {
                filter = filter & Builders<Certificate>.Filter.Where(x => x.PartnerId == PartnerId);
            }
            var sort = Builders<Certificate>.Sort.Descending(x => x.CreatedAt);

            var lst = await _dBCertificate.GetPaged(filter, sort, pageNumber, PageSize);
            return lst;
        }
        public async Task<MongoResultPaged<Certificate>> CertificateListAllSystemGeneric()
        {
            var filter = Builders<Certificate>.Filter.Where(x => x.IsPartnerGeneric == false
                                                         && x.IsSystemGeneric == true
                                                         );

            var sort = Builders<Certificate>.Sort.Descending(x => x.CreatedAt);

            var lst = await _dBCertificate.GetPaged(filter, sort, 1, int.MaxValue);
            return lst;
        }

        public async Task<bool> IsCertificateExist(string Id)
        {
            var obj = await _dBCertificate.GetById(Id);
            return obj != null;
        }

        public async Task<Certificate> CertificateGetSystemGeneric(Enum.EnumCertificateType type)
        {
            var filter = Builders<Certificate>.Filter.Where(x => x.Type == type
                                                            && x.IsSystemGeneric == true
                                                           && x.IsActive == true);
            var obj = await _dBCertificate.GetOne(filter);

            return obj;
        }
        public async Task<Certificate> CertificateGetPartnerGeneric(string PartnerId, Enum.EnumCertificateType type)
        {
            if (string.IsNullOrEmpty(PartnerId))
                return null;

            var filter = Builders<Certificate>.Filter.Where(x => x.Type == type
                                                            && x.PartnerId == PartnerId
                                                            && x.IsPartnerGeneric == true
                                                            && x.IsActive == true);

            var obj = await _dBCertificate.GetOne(filter);

            return obj;
        }
        public async Task<Certificate> CertificateGetPartnerTrainingCategory(string PartnerId, string TrainingCategoryId, Enum.EnumCertificateType type)
        {
            if (string.IsNullOrEmpty(PartnerId) || string.IsNullOrEmpty(TrainingCategoryId))
                return null;

            var filter = Builders<Certificate>.Filter.Where(x => x.Type == type
                                                            && x.PartnerId == PartnerId
                                                            && x.TrainingCategoryId == TrainingCategoryId
                                                            && (x.TrainingCenterId == null || x.TrainingCenterId == "")
                                                            && x.IsActive == true);

            var obj = await _dBCertificate.GetOne(filter);

            return obj;
        }
        public async Task<Certificate> CertificateGetTrainingCenterTrainingCategory(string PartnerId, string TrainingCenterId, string TrainingCategoryId, Enum.EnumCertificateType type)
        {
            if (string.IsNullOrEmpty(PartnerId) || string.IsNullOrEmpty(TrainingCategoryId) || string.IsNullOrEmpty(TrainingCenterId))
                return null;

            var filter = Builders<Certificate>.Filter.Where(x => x.Type == type
                                                            && x.PartnerId == PartnerId
                                                            && x.TrainingCenterId == TrainingCenterId
                                                            && x.TrainingCategoryId == TrainingCategoryId
                                                            && x.IsActive == true);

            var obj = await _dBCertificate.GetOne(filter);

            return obj;
        }

        public async Task<bool> UpdateSystemGeneric(Enum.EnumCertificateType type, string FileName)
        {
            var obj = await CertificateGetSystemGeneric(type);
            if (obj != null)
            {
                obj.IsActive = true;
                obj.FileName = FileName;
                await CertificateUpdate(obj);
            }
            else
            {
                obj = new Certificate();
                obj.Type = type;
                obj.IsSystemGeneric = true;
                obj.FileName = FileName;
                await CertificateCreate(obj);
            }
            return true;
        }
        public async Task<bool> UpdatePartnerGeneric(Enum.EnumCertificateType type, string PartnerId, string FileName)
        {
            var obj = await CertificateGetPartnerGeneric(PartnerId, type);
            if (obj != null)
            {
                obj.IsActive = true;
                obj.FileName = FileName;
                await CertificateUpdate(obj);
            }
            else
            {
                obj = new Certificate();
                obj.Type = type;
                obj.IsPartnerGeneric = true;
                obj.PartnerId = PartnerId;
                obj.FileName = FileName;
                await CertificateCreate(obj);
            }
            return true;
        }
        public async Task<bool> UpdatePartnerTrainingCategory(Enum.EnumCertificateType type, string PartnerId, string TrainingTypeId, string TrainingCategoryId, string FileName)
        {
            var obj = await CertificateGetPartnerTrainingCategory(PartnerId, TrainingCategoryId, type);
            if (obj != null)
            {
                obj.IsActive = true;
                obj.FileName = FileName;
                await CertificateUpdate(obj);
            }
            else
            {
                obj = new Certificate();
                obj.Type = type;
                obj.TrainingTypeId = TrainingTypeId;
                obj.TrainingCategoryId = TrainingCategoryId;
                obj.PartnerId = PartnerId;
                obj.FileName = FileName;
                await CertificateCreate(obj);
            }
            return true;
        }
        public async Task<bool> UpdateTrainingCenterTrainingCategory(Enum.EnumCertificateType type, string PartnerId, string TrainingCenterId, string TrainingTypeId, string TrainingCategoryId, string FileName)
        {
            var obj = await CertificateGetTrainingCenterTrainingCategory(PartnerId, TrainingCenterId, TrainingCategoryId, type);
            if (obj != null)
            {
                obj.IsActive = true;
                obj.FileName = FileName;
                await CertificateUpdate(obj);
            }
            else
            {
                obj = new Certificate();
                obj.Type = type;
                obj.TrainingTypeId = TrainingTypeId;
                obj.TrainingCategoryId = TrainingCategoryId;
                obj.PartnerId = PartnerId;
                obj.TrainingCenterId = TrainingCenterId;
                obj.FileName = FileName;
                await CertificateCreate(obj);
            }
            return true;
        }
        public async Task<string> UploadCertificateFile(IFormFile File, int Type, string PartnerId, string TrainingCategoryId, string TrainingCenterId)
        {
            string folderName = "";
            string fileName = "";
            if (Type == (int)Enum.EnumCertificateType.Trainer)
            {
                folderName = Path.Combine(FolderCertificates, FolderCertificatesTemplate, FolderTrainer);
            }
            else
            {
                folderName = Path.Combine(FolderCertificates, FolderCertificatesTemplate, FolderTrainee);
            }


            if (!string.IsNullOrEmpty(TrainingCenterId))
            {
                folderName = Path.Combine(folderName, TrainingCenterId);
                fileName = TrainingCategoryId + ".pdf";
            }
            else if (!string.IsNullOrEmpty(PartnerId))
            {
                folderName = Path.Combine(folderName, PartnerId);
                if (!string.IsNullOrEmpty(TrainingCategoryId))
                {
                    fileName = TrainingCategoryId + ".pdf";
                }
                else
                {
                    fileName = "Generic.pdf";
                }
            }
            else
            {
                folderName = Path.Combine(folderName, FolderSystem);
                fileName = "Generic.pdf";
            }

            var pathToSave = Path.Combine(_cacheConfig.CertificateBaseUrl, folderName);
            ValidateDirectoryIsExits(pathToSave);

            var fullPath = Path.Combine(pathToSave, fileName);
            var dbPath = Path.Combine(folderName, fileName);

            string UpoadFileOnCloud = _cacheConfig.UpoadFileOnCloud;
            if (UpoadFileOnCloud == "false")
            {
                //upload on server
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    File.CopyTo(stream);
                }
            }
            else
            {
                //SS - added store data in azure storage
                using (var memoryStream = new MemoryStream())
                {
                    File.CopyTo(memoryStream);
                    var fileBytes = memoryStream.ToArray();
                    string base64file = Convert.ToBase64String(fileBytes);
                    byte[] bytes = System.Convert.FromBase64String(base64file);
                    string fullFileName = folderName + "/" + fileName;
                    dbPath = await UploadFileToBlobAsync(fullFileName, bytes, File.ContentType);
                }
            }
            return dbPath;
        }
        public async Task<string> GetCertificate(Enum.EnumCertificateType Type, string PartnerId, string TrainingCategoryId, string TrainingCenterId)
        {
            string certifiatePath = _cacheConfig.CertificateBaseUrl;

            var trainingCenter = await CertificateGetTrainingCenterTrainingCategory(PartnerId, TrainingCenterId, TrainingCategoryId, Type);
            if (trainingCenter != null)
            {
                return Path.Combine(certifiatePath, trainingCenter.FileName);
            }

            var partnerCategory = await CertificateGetPartnerTrainingCategory(PartnerId, TrainingCategoryId, Type);
            if (partnerCategory != null)
            {
                return Path.Combine(certifiatePath, partnerCategory.FileName);
            }

            var partnerGeneric = await CertificateGetPartnerGeneric(PartnerId, Type);
            if (partnerGeneric != null)
            {
                return Path.Combine(certifiatePath, partnerGeneric.FileName);
            }

            var systemGeneric = await CertificateGetSystemGeneric(Type);
            if (systemGeneric != null)
            {
                return Path.Combine(certifiatePath, systemGeneric.FileName);
            }

            ServiceHelper.Log("Trying to generate certifiate and no certificates found");
            //throw new Exception("Trying to generate certifiate and no certificates found");

            return "";
        }

        public async Task<bool> GenerateTraineePassedExamCertificate(string TraineeId, string TrainingId)
        {
            var trainee = await _BLTrainee.GetById(TraineeId);
            var training = await _BLTraining.GetById(TrainingId);

            var trainingCategory = await _BLDataManagement.TrainingCategoryGetById(training.TrainingCategoryId);

            //get certificate template
            //get certifcate saving path

            string certificateTemplate = await GetCertificate(Enum.EnumCertificateType.Trainee, training.PartnerId._id, training.TrainingCategoryId, training.TrainingCenterId._id);

            if (string.IsNullOrEmpty(certificateTemplate))
                return false;

            string newCertificate = Path.Combine(FolderCertificates, FolderCertificatesGenerated, FolderTrainee, trainee._id);
            string newCertificateFullPath = Path.Combine(_cacheConfig.CertificateBaseUrl, newCertificate);

            if (!Directory.Exists(newCertificateFullPath))
            {
                Directory.CreateDirectory(newCertificateFullPath);
            }

            newCertificateFullPath = Path.Combine(newCertificateFullPath, trainingCategory._id + ".pdf");

            var CertificateNumber = Guid.NewGuid().ToString();
            await _BLTrainee.UpdateCertificate(trainee._id, training._id, Path.Combine(newCertificate, trainingCategory._id + ".pdf"), CertificateNumber);

            using (var templateFileStream = new FileStream(certificateTemplate, FileMode.Open))
            using (var generatedFileStream = new FileStream(newCertificateFullPath, FileMode.Create))
            {
                var pdfReader = new PdfReader(templateFileStream);
                var stamper = new PdfStamper(pdfReader, generatedFileStream);

                var form = stamper.AcroFields;
                var fieldKeys = form.Fields.Keys;

                var trainingName = string.Format("Has successfully completed training in basic skills on");


                var trainingCategories = new StringBuilder("(");

                foreach (var c in trainingCategory.Course.Where(x => x.IsActive == true))
                {
                    trainingCategories.Append(c.Name + ", ");
                }
                if (trainingCategories.Length > 1)
                    trainingCategories.Replace(',', ')', trainingCategories.ToString().LastIndexOf(','), 2);
                else
                    trainingCategories = trainingCategories.Clear();


                foreach (string fieldKey in fieldKeys)
                {
                    if (fieldKey == CertificateFieldName.Name)
                    {
                        form.SetField(fieldKey, trainee.Name);
                    }
                    if (fieldKey == CertificateFieldName.TrainingName)
                    {
                        form.SetField(fieldKey, trainingName);
                    }
                    if (fieldKey == CertificateFieldName.TrainingCategories)
                    {
                        form.SetField(fieldKey, trainingCategories.ToString());
                    }
                    if (fieldKey == CertificateFieldName.StartDate)
                    {
                        form.SetField(fieldKey, training.StartDate.ToString("MMMM dd, yyyy", new CultureInfo("en-US")));
                    }
                    if (fieldKey == CertificateFieldName.EndDate)
                    {
                        form.SetField(fieldKey, training.EndDate.ToString("MMMM dd, yyyy", new CultureInfo("en-US")));
                    }
                    if (fieldKey == CertificateFieldName.UnquieIdentifier)
                    {
                        form.SetField(fieldKey, CertificateNumber);
                    }
                }

                stamper.FormFlattening = true;

                stamper.Close();
                pdfReader.Close();
            }
            return true;
        }
        public async Task<bool> GenerateTrainerCertificate(string trainerId, string PartnerId, string TrainingCategoryId)
        {
            var trainer = await _BLUserProfile.UserProfileGetById(trainerId);

            string certificateTemplate = "";
            //foreach(var partnerId in trainer.MyPartnerListIds)
            //{
            //    certificateTemplate = await GetCertificate(Enum.EnumCertificateType.Trainer, partnerId, TrainingCategoryId, "-1");
            //    if (!string.IsNullOrEmpty(certificateTemplate))
            //        break;
            //}
            certificateTemplate = await GetCertificate(Enum.EnumCertificateType.Trainer, PartnerId, TrainingCategoryId, "-1");
            if (string.IsNullOrEmpty(certificateTemplate))
                return false;

            string newCertificate = Path.Combine(FolderCertificates, FolderCertificatesGenerated, FolderTrainer, trainer._id);
            string newCertificateFullPath = Path.Combine(_cacheConfig.CertificateBaseUrl, newCertificate);

            if (!Directory.Exists(newCertificateFullPath))
            {
                Directory.CreateDirectory(newCertificateFullPath);
            }

            newCertificateFullPath = Path.Combine(newCertificateFullPath, TrainingCategoryId + ".pdf");

            await _BLUserProfile.ApproveTrainerCertificate(trainer._id, PartnerId, TrainingCategoryId, Path.Combine(newCertificate, TrainingCategoryId + ".pdf"));

            using (var templateFileStream = new FileStream(certificateTemplate, FileMode.Open))
            using (var generatedFileStream = new FileStream(newCertificateFullPath, FileMode.Create))
            {
                var pdfReader = new PdfReader(templateFileStream);
                var stamper = new PdfStamper(pdfReader, generatedFileStream);

                var form = stamper.AcroFields;
                var fieldKeys = form.Fields.Keys;

                foreach (string fieldKey in fieldKeys)
                {
                    if (fieldKey == CertificateFieldName.Name)
                    {
                        form.SetField(fieldKey, trainer.Name);
                    }
                    if (fieldKey == CertificateFieldName.TrainingDetails)
                    {
                        if (!string.IsNullOrEmpty(trainer.TrainerTrainingDetails))
                        {
                            form.SetField(fieldKey, trainer.TrainerTrainingDetails);
                        }
                    }
                    if (fieldKey == CertificateFieldName.StartDate)
                    {
                        if (trainer.TrainerStartDate != null)
                            form.SetField(fieldKey, trainer.TrainerStartDate.AddDays(1).ToString("MMMM dd, yyyy", new CultureInfo("en-US")));
                    }
                    if (fieldKey == CertificateFieldName.EndDate)
                    {
                        if (trainer.TrainerEndDate != null)
                            form.SetField(fieldKey, trainer.TrainerEndDate.AddDays(1).ToString("MMMM dd, yyyy", new CultureInfo("en-US")));
                    }

                }

                stamper.FormFlattening = true;

                stamper.Close();
                pdfReader.Close();
            }
            return true;
        }
        #region private methods
        private void ValidateDirectoryIsExits(string folderFullPath)
        {
            if (!Directory.Exists(folderFullPath))
            {
                Directory.CreateDirectory(folderFullPath);
            }
            //string[] foldersAndSubfoldersPaths = folderFullPath.Split("\\");
            //string folderPath = "";
            //foreach (string folderName in foldersAndSubfoldersPaths)
            //{
            //    folderPath += folderName + "\\";
            //    if (!Directory.Exists(Path.Combine(_cacheConfig.CertificateBaseUrl, folderPath)))
            //    {
            //        Directory.CreateDirectory(Path.Combine(_cacheConfig.CertificateBaseUrl, folderPath));
            //    }
            //}
        }
        private async Task<string> UploadFileToBlobAsync(string strFileName, byte[] fileData, string fileMimeType)
        {
            try
            {
                string accessKey = _cacheConfig.BlobAccessKey;
                string strContainerName = _cacheConfig.BlobContainerName;
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName.ToLower());
                string fileName = strFileName;

                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                }

                if (fileName != null && fileData != null)
                {
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                    cloudBlockBlob.Properties.ContentType = fileMimeType;
                    await cloudBlockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
                    return cloudBlockBlob.Uri.AbsoluteUri;
                }
                return "";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion
    }
}

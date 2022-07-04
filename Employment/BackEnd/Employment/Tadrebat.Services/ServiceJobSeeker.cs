using Employment.Entity.Mongo;
using Employment.Interface;
using Employment.MongoDB.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MongoDB.Driver;
using System.IO;
using System.Reflection.Emit;
using Employment.Entity.Model;
using Employment.Persistance.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;


namespace Employment.Services
{
    public class ServiceJobSeeker : ServiceRepository<JobSeeker>, IServiceJobSeeker
    {
        private readonly IDBJobSeeker _dBJobSeeker;
        private readonly IDBFavourite _dBFavourite;
        private readonly IServiceCountry _BLCountry;
        private readonly IServiceQualification _BLQualification;
        private readonly IServiceYearsOfExperience _BLYearsOfExperience;
        private readonly IServiceLanguages _BLLanguages;
        private readonly IFile _BLFile;
        private readonly IDBApply _dBApply;
        private IAsyncRepository<ReportJobSeeker> _repositoryReport;
        private IAsyncRepository<ReportJobSeekerLanguage> _repositoryReportLang;
        private IAsyncRepository<ReportJobSeekerResumeItem> _repositoryReportResumeItem;
        private readonly ICacheConfig _BLCacheConfig;
        private string CertificatePath = string.Empty;
        public ServiceJobSeeker(IDBJobSeeker dBJobSeeker
                            , IServiceCountry BLCountry,
                            IServiceQualification BLQualification,
                            IServiceLanguages BLLanguages,
                            IServiceYearsOfExperience BLYearsOfExperience,
                            IFile BLFile,
                            IDBApply dBApply,
                            ICacheConfig cacheConfig,
                            IAsyncRepository<ReportJobSeeker> repositoryReport,
                            IAsyncRepository<ReportJobSeekerLanguage> repositoryReportLang,
                            IAsyncRepository<ReportJobSeekerResumeItem> repositoryReportResumeItem,
                            IDBFavourite dBFavourite) : base(dBJobSeeker)
        {
            _dBJobSeeker = dBJobSeeker;
            _BLCountry = BLCountry;
            _BLYearsOfExperience = BLYearsOfExperience;
            _BLQualification = BLQualification;
            _BLLanguages = BLLanguages;
            _BLFile = BLFile;
            _dBFavourite = dBFavourite;
            _dBApply = dBApply;
            _repositoryReport = repositoryReport;
            _repositoryReportLang = repositoryReportLang;
            _repositoryReportResumeItem = repositoryReportResumeItem;
            _BLCacheConfig = cacheConfig;

        }
        public async Task<JobSeeker> GetByUserId(string UserId)
        {
            try
            {
                var filter = Builders<JobSeeker>.Filter.Where(x => x.UserId == UserId || x._id == UserId);
                return await _dBJobSeeker.GetOne(filter);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<bool> Create(string UserId, string Email, string Name)
        {
            var seeker = new JobSeeker();
            seeker.UserId = UserId;
            seeker.Email = Email;
            seeker.Name = Name;

            var res = await Create(seeker);

            await CreateReportJobSeeker(seeker);
            return res;
        }
        public async Task<bool> UpdateInfo(string userId, JobSeeker obj)
        {
            var seeker = await GetByUserId(userId);
            if (seeker == null)
                return false;

            seeker.Name = obj.Name == "" ? seeker.Name : obj.Name;
            seeker.JobTitle = obj.JobTitle == "" ? seeker.JobTitle : obj.JobTitle;
            seeker.Email = obj.Email;
            seeker.Phone = obj.Phone;
            seeker.Website = obj.Website;
            seeker.Gender = obj.Gender;
            seeker.SocialLinkedin = obj.SocialLinkedin;

            if (!string.IsNullOrEmpty(obj.Country._id))
            {
                var country = await _BLCountry.GetById(obj.Country._id);
                seeker.Country.Name = country?.Name;
                seeker.Country._id = country?._id;

                if (!string.IsNullOrEmpty(obj.City._id))
                {
                    var city = country.subItems.Where(x => x._id == obj.City._id).FirstOrDefault();
                    seeker.City.Name = city?.Name;
                    seeker.City._id = city?._id;
                }
            }

            await _dBJobSeeker.UpdateObj(obj._id, seeker);
            await updateIsComplete(seeker);
            await updateReportJobSeeker(seeker);
            return true;
        }
        public async Task<bool> UpdateDescription(string userId, string About)
        {
            var seeker = await GetByUserId(userId);
            if (seeker == null)
                return false;

            var update = Builders<JobSeeker>.Update.Set(x => x.About, About);
            await _dBJobSeeker.UpdateAsync(seeker._id, update);
            await updateIsComplete(seeker);
            await updateReportJobSeeker(seeker);
            return true;
        }
        public async Task<bool> UpdateProfile(string userId, JobSeeker obj)
        {
            var seeker = await GetByUserId(userId);
            if (seeker == null)
                return false;

            seeker.DOB = obj.DOB;

            if (!string.IsNullOrEmpty(obj.Qualification._id))
            {
                var Qualification = await _BLQualification.GetById(obj.Qualification._id);
                seeker.Qualification.Name = Qualification.Name;
                seeker.Qualification._id = Qualification._id;
            }
            if (!string.IsNullOrEmpty(obj.Experience._id))
            {
                var Experience = await _BLYearsOfExperience.GetById(obj.Experience._id);
                seeker.Experience.Name = Experience.Name;
                seeker.Experience._id = Experience._id;
            }

            if (obj.Languages.Count() > 0)
            {
                seeker.Languages.Clear();
                var lst = await _BLLanguages.ListActive();
                foreach (var item in obj.Languages)
                {
                    var lang = lst.Where(x => x._id == item._id).FirstOrDefault();
                    if (lang != null)
                        seeker.Languages.Add(new SubItem(lang._id, lang.Name));
                }
            }

            await _dBJobSeeker.UpdateObj(obj._id, seeker);
            await updateReportJobSeeker(seeker);
            return true;
        }
        public async Task<bool> UpdateSocialMedia(string userId, JobSeeker obj)
        {
            var seeker = await GetByUserId(userId);
            if (seeker == null)
                return false;

            seeker.SocialFacebook = obj.Email;
            seeker.SocialTwitter = obj.SocialTwitter;
            seeker.SocialLinkedin = obj.SocialLinkedin;
            seeker.SocialGooglePlus = obj.SocialGooglePlus;

            await _dBJobSeeker.UpdateObj(obj._id, seeker);
            await updateReportJobSeeker(seeker);
            return true;
        }

        public async Task<bool> AddEducation(string userId, ResumeItem obj)
        {
            var seeker = await GetByUserId(userId);
            if (seeker == null)
                return false;

            FieldDefinition<JobSeeker> field = "Education";
            await _dBJobSeeker.AddField(seeker._id, field, obj);
            await updateIsComplete(seeker);

            seeker = await GetByUserId(userId);
            await updateReportJobSeeker(seeker);
            return true;
        }
        public async Task<bool> UpdateEducation(string userId, ResumeItem obj)
        {
            var seeker = await GetByUserId(userId);
            if (seeker == null)
                return false;

            var item = seeker.Education.Find(f => f._id == obj._id);

            var filter = Builders<JobSeeker>.Filter.Where(x => x._id == seeker._id && x.Education.Any(y => y._id == obj._id));
            var update = Builders<JobSeeker>.Update.Set(x => x.Education[-1], obj);

            await _dBJobSeeker.UpdateAsync(filter, update);

            seeker = await GetByUserId(userId);
            await updateReportJobSeeker(seeker);
            return true;
        }
        public async Task<bool> RemoveEducation(string userId, string EducationId)
        {
            var seeker = await GetByUserId(userId);
            if (seeker == null)
                return false;

            FieldDefinition<JobSeeker> field = "Education";
            var update = Builders<JobSeeker>.Update.PullFilter("Education", Builders<ResumeItem>.Filter.Eq("_id", EducationId));
            await _dBJobSeeker.RemoveField(seeker._id, update);
            await updateIsComplete(seeker);

            seeker = await GetByUserId(userId);
            await updateReportJobSeeker(seeker);
            return true;
        }
        public async Task<bool> AddWorkExperience(string userId, ResumeItem obj)
        {
            var seeker = await GetByUserId(userId);
            if (seeker == null)
                return false;

            FieldDefinition<JobSeeker> field = "WorkHistory";
            await _dBJobSeeker.AddField(seeker._id, field, obj);

            seeker = await GetByUserId(userId);
            await updateReportJobSeeker(seeker);
            return true;
        }
        public async Task<bool> UpdateWorkExperience(string userId, ResumeItem obj)
        {
            var seeker = await GetByUserId(userId);
            if (seeker == null)
                return false;

            var item = seeker.WorkHistory.Find(f => f._id == obj._id);

            var filter = Builders<JobSeeker>.Filter.Where(x => x._id == seeker._id && x.WorkHistory.Any(y => y._id == obj._id));
            var update = Builders<JobSeeker>.Update.Set(x => x.WorkHistory[-1], obj);

            await _dBJobSeeker.UpdateAsync(filter, update);

            seeker = await GetByUserId(userId);
            await updateReportJobSeeker(seeker);
            return true;
        }
        public async Task<bool> RemoveWorkExperience(string userId, string WorkExperienceId)
        {
            var seeker = await GetByUserId(userId);
            if (seeker == null)
                return false;

            FieldDefinition<JobSeeker> field = "WorkHistory";
            var update = Builders<JobSeeker>.Update.PullFilter("WorkHistory", Builders<ResumeItem>.Filter.Eq("_id", WorkExperienceId));
            await _dBJobSeeker.RemoveField(seeker._id, update);

            seeker = await GetByUserId(userId);
            await updateReportJobSeeker(seeker);
            return true;
        }
        public async Task<bool> AddExtraCurricular(string userId, ResumeItem obj)
        {
            var seeker = await GetByUserId(userId);
            if (seeker == null)
                return false;

            FieldDefinition<JobSeeker> field = "ExtraCurricular";
            await _dBJobSeeker.AddField(seeker._id, field, obj);

            seeker = await GetByUserId(userId);
            await updateReportJobSeeker(seeker);
            return true;
        }
        public async Task<bool> UpdateExtraCurricular(string userId, ResumeItem obj)
        {
            var seeker = await GetByUserId(userId);
            if (seeker == null)
                return false;

            var item = seeker.ExtraCurricular.Find(f => f._id == obj._id);

            var filter = Builders<JobSeeker>.Filter.Where(x => x._id == seeker._id && x.ExtraCurricular.Any(y => y._id == obj._id));
            var update = Builders<JobSeeker>.Update.Set(x => x.ExtraCurricular[-1], obj);

            await _dBJobSeeker.UpdateAsync(filter, update);

            seeker = await GetByUserId(userId);
            await updateReportJobSeeker(seeker);
            return true;
        }
        public async Task<bool> RemoveExtraCurricular(string userId, string ExtraCurricularId)
        {
            var seeker = await GetByUserId(userId);
            if (seeker == null)
                return false;

            FieldDefinition<JobSeeker> field = "ExtraCurricular";
            var update = Builders<JobSeeker>.Update.PullFilter("ExtraCurricular", Builders<ResumeItem>.Filter.Eq("_id", ExtraCurricularId));
            await _dBJobSeeker.RemoveField(seeker._id, update);

            seeker = await GetByUserId(userId);
            await updateReportJobSeeker(seeker);
            return true;
        }
        public async Task<bool> AddCertification(string userId, ResumeCertification obj)
        {
            var seeker = await GetByUserId(userId);
            if (seeker == null)
                return false;

            if (!string.IsNullOrEmpty(obj.CertificateTadrebatId))
            {
                var res = seeker.Certification.Where(x => x.CertificateTadrebatId == obj.CertificateTadrebatId).ToList();
                if (res.Count() > 0)
                    return true;
            }
            if (await UploadCertificate(seeker.UserId, obj))
            {
                //SS set file path for certificate
                string FileUploadOnCloud = _BLCacheConfig.FileUploadOnCloud;
                if (FileUploadOnCloud == "false")
                {
                    obj.CertificatePath = obj._id + Path.GetExtension(obj.CertificatePath);
                }
                else
                {
                    if (!CertificatePath.Equals(string.Empty))
                        obj.CertificatePath = CertificatePath;
                }


            }
            FieldDefinition<JobSeeker> field = "Certification";
            await _dBJobSeeker.AddField(seeker._id, field, obj);
            seeker = await GetByUserId(userId);
            await updateReportJobSeeker(seeker);
            return true;
        }
        public async Task<bool> UpdateCertification(string userId, ResumeCertification obj)
        {
            var seeker = await GetByUserId(userId);
            if (seeker == null)
                return false;
            if (await UploadCertificate(userId, obj))
            {
                //SS set file path for certificate
                string FileUploadOnCloud = _BLCacheConfig.FileUploadOnCloud;
                if (FileUploadOnCloud == "false")
                {
                    obj.CertificatePath = obj._id + Path.GetExtension(obj.CertificatePath);
                }
                else
                {
                    if (!CertificatePath.Equals(string.Empty))
                        obj.CertificatePath = CertificatePath;
                }


            }
            var item = seeker.Certification.Find(f => f._id == obj._id);

            var filter = Builders<JobSeeker>.Filter.Where(x => x._id == seeker._id && x.Certification.Any(y => y._id == obj._id));
            var update = Builders<JobSeeker>.Update.Set(x => x.Certification[-1], obj);

            await _dBJobSeeker.UpdateAsync(filter, update);
            seeker = await GetByUserId(userId);
            await updateReportJobSeeker(seeker);
            return true;
        }
        public async Task<bool> RemoveCertification(string userId, string CertificationId)
        {
            var seeker = await GetByUserId(userId);
            if (seeker == null)
                return false;

            FieldDefinition<JobSeeker> field = "Certification";
            var update = Builders<JobSeeker>.Update.PullFilter("Certification", Builders<ResumeCertification>.Filter.Eq("_id", CertificationId));
            await _dBJobSeeker.RemoveField(seeker._id, update);
            seeker = await GetByUserId(userId);
            await _BLFile.DeleteCertificate(userId, CertificationId);

            await updateReportJobSeeker(seeker);
            return true;
        }
        public async Task<MongoResultPaged<JobSeeker>> Search(string filterText, List<string> ExperienceId, List<int> GenderId, List<string> Qualificationid, List<string> LanguageId, string CountryId, string CityId, int pageNumber = 1, int PageSize = 15)
        {

            var filter = Builders<JobSeeker>.Filter.Where(x => x.IsActive == true && x.IsProfileComplete == true);

            if (!string.IsNullOrEmpty(filterText))
            {
                filter = filter & Builders<JobSeeker>.Filter.Where(x => x.Name.ToLower().Contains(filterText.ToLower())
                                                                        || x.JobTitle.ToLower().Contains(filterText.ToLower()));
            }
            if (!string.IsNullOrEmpty(CountryId))
            {
                filter = filter & Builders<JobSeeker>.Filter.Where(x => x.Country._id == CountryId);
            }
            if (!string.IsNullOrEmpty(CityId))
            {
                filter = filter & Builders<JobSeeker>.Filter.Where(x => x.City._id == CityId);
            }
            if (ExperienceId != null && ExperienceId.Count > 0)
            {
                filter = filter & Builders<JobSeeker>.Filter.Where(x => ExperienceId.Contains(x.Experience._id));
            }

            if (GenderId != null && GenderId.Count > 0)
            {
                filter = filter & Builders<JobSeeker>.Filter.Where(x => GenderId.Contains(x.Gender));
            }

            if (Qualificationid != null && Qualificationid.Count > 0)
            {
                filter = filter & Builders<JobSeeker>.Filter.Where(x => Qualificationid.Contains(x.Qualification._id));
            }

            if (LanguageId != null && LanguageId.Count > 0)
            {
                filter = filter & Builders<JobSeeker>.Filter.Where(x => x.Languages.Any(y => LanguageId.Contains(y._id)));
            }
            var sort = Builders<JobSeeker>.Sort.Descending(x => x.Name);
            var lst = await _dBJobSeeker.GetPaged(filter, sort, pageNumber, PageSize);
            return lst;

        }
        public async Task<bool> UploadFIle(string userId, string fileName, Enum.EnumFileType type)
        {
            var obj = await GetByUserId(userId);
            if (obj == null)
                return false;

            if (string.IsNullOrEmpty(fileName))
                return false;
            string FileUploadOnCloud = _BLCacheConfig.FileUploadOnCloud;
            if (FileUploadOnCloud == "false")
            {
                //SS - upload file on server
                await _BLFile.UploadJobSeekerFile(obj._id, fileName, type);
                switch (type)
                {
                    case Enum.EnumFileType.CoverLetter:
                        obj.CoverLetterFile = obj._id + Path.GetExtension(fileName);
                        break;
                    case Enum.EnumFileType.Resume:
                        obj.ResumeFile = obj._id + Path.GetExtension(fileName);
                        break;
                    case Enum.EnumFileType.ProfilePicture:
                        obj.ProfilePicture = obj._id + Path.GetExtension(fileName);
                        break;
                }
            }
            else
            {
                string accesskey = _BLCacheConfig.BlobAccessKey;
                string ContainerName = _BLCacheConfig.BlobContainerName;

                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(accesskey);

                string fullUrl = fileName;
                string[] strName = fileName.Split('/');
                // fileNametempfolder name / filename
                fileName = strName[strName.Length - 2] + "/" + strName[strName.Length - 1];
                string newFilename = obj._id + Path.GetExtension(fileName);

                //copy from tempfolder to actual folder
                string FileUrl = await CopyBlob(cloudStorageAccount, ContainerName, fileName, newFilename, type.ToString());

                if (FileUrl == "")
                {
                    await DeleteBlob(cloudStorageAccount, ContainerName, fileName);
                    return false;
                }

                switch (type)
                {
                    case Enum.EnumFileType.CoverLetter:
                        obj.CoverLetterFile = FileUrl;
                        break;
                    case Enum.EnumFileType.Resume:
                        obj.ResumeFile = FileUrl;
                        break;
                    case Enum.EnumFileType.ProfilePicture:
                        obj.ProfilePicture = FileUrl;
                        break;
                }
                //after copy delete file from tempfolder
                await DeleteBlob(cloudStorageAccount, ContainerName, fileName);

            }

            await _dBJobSeeker.UpdateObj(obj._id, obj);
            return true;
        }
        protected async Task<bool> UploadCertificate(string UserId, ResumeCertification obj)
        {
            if (string.IsNullOrEmpty(obj.CertificatePath))
                return false;

            string FileUploadOnCloud = _BLCacheConfig.FileUploadOnCloud;
            if (FileUploadOnCloud == "false")
            {
                return await _BLFile.UploadCertificate(UserId, obj._id, obj.CertificatePath);
            }
            else
            {
                string accesskey = _BLCacheConfig.BlobAccessKey;
                string ContainerName = _BLCacheConfig.BlobContainerName;

                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(accesskey);

                string fullUrl = obj.CertificatePath;
                string[] strName = fullUrl.Split('/');
                // fileName =tempfolder name / filename
                string fileName = strName[strName.Length - 2] + "/" + strName[strName.Length - 1];
                string newFilename = obj._id + Path.GetExtension(fileName);
                //copy from tempfolder to actual folder
                string FileUrl = await CopyBlob(cloudStorageAccount, ContainerName, fileName, newFilename, "Certificate");
                CertificatePath = FileUrl;
                if (FileUrl == "")
                {
                    //after copy delete file from tempfolder
                    await DeleteBlob(cloudStorageAccount, ContainerName, fileName);
                    return false;
                }
                return true;
            }



        }
        protected async Task<bool> updateFavourite(JobSeeker obj)
        {
            var filter = Builders<Favourite>.Filter.Where(x => x.EntityId == obj._id);
            var update = Builders<Favourite>.Update.Set(x => x.Name, obj.Name).Set(y => y.Title, obj.JobTitle).Set(z => z.ImageURL, obj.ProfilePicture);

            await _dBFavourite.UpdateManyAsync(filter, update);
            return true;
        }
        protected async Task<bool> updateApply(JobSeeker obj)
        {
            var filter = Builders<Apply>.Filter.Where(x => x.Job._id == obj._id);
            var update = Builders<Apply>.Update.Set(x => x.JobSeeker.Name, obj.Name).Set(y => y.JobSeeker.SubName, obj.JobTitle).Set(z => z.Job.URL, obj.ProfilePicture);

            await _dBApply.UpdateManyAsync(filter, update);
            return true;
        }
        protected async Task<bool> updateIsComplete(JobSeeker obj)
        {
            if (obj == null)
                return false;

            if (obj.IsProfileComplete == true)
                return true;

            if (!string.IsNullOrEmpty(obj.About) && obj.Gender != 0 && obj.Education.Count() > 0)
            {
                obj.IsProfileComplete = true;
                await _dBJobSeeker.UpdateObj(obj._id, obj);
            }

            return true;
        }
        protected async Task<bool> CreateReportJobSeeker(JobSeeker obj)
        {
            var report = await ConvertToReportJobSeeker(obj, null);

            try
            {
                report = await _repositoryReport.AddAsync(report);
                await UpdateReportSubs(obj, report);
            }
            catch (Exception ex)
            {


            }
            return true;
        }
        protected async Task<bool> updateReportJobSeeker(JobSeeker obj)
        {
            try
            {
                var report = await _repositoryReport.GetQueryableFirstorDefaultAsync(x => x.JobSeekerId == obj._id);
                report = await ConvertToReportJobSeeker(obj, report);
                await _repositoryReport.UpdateAsync(report);
                await UpdateReportSubs(obj, report);
            }
            catch (Exception ex)
            {


            }

            return true;
        }
        protected async Task<bool> UpdateReportSubs(JobSeeker obj, ReportJobSeeker report)
        {
            try
            {
                //update language
                var lstLang = await _repositoryReportLang.GetQueryableTolistAsync(x => x.JobSeekerId == obj._id);
                foreach (var item in lstLang)
                {
                    await _repositoryReportLang.DeleteAsync(item);
                }
                foreach (var item in obj.Languages)
                {
                    var lang = await ConvertToReportJobSeekerLang(item, obj._id, report.Id);
                    await _repositoryReportLang.AddAsync(lang);
                }

                //update education
                var lstItem = await _repositoryReportResumeItem.GetQueryableTolistAsync(x => x.JobSeekerId == obj._id);
                foreach (var item in lstItem)
                {
                    await _repositoryReportResumeItem.DeleteAsync(item);
                }
                foreach (var itemedu in obj.Education)
                {
                    var edu = await ConvertToReportJobSeekerResumeItem(itemedu, obj._id, report.Id, 1);
                    await _repositoryReportResumeItem.AddAsync(edu);
                }
                foreach (var itemWork in obj.WorkHistory)
                {
                    var work = await ConvertToReportJobSeekerResumeItem(itemWork, obj._id, report.Id, 2);
                    await _repositoryReportResumeItem.AddAsync(work);
                }
                foreach (var itemExtra in obj.ExtraCurricular)
                {
                    var extra = await ConvertToReportJobSeekerResumeItem(itemExtra, obj._id, report.Id, 4);
                    await _repositoryReportResumeItem.AddAsync(extra);
                }
                foreach (var itemCert in obj.Certification)
                {
                    var cert = await ConvertToReportJobSeekerCert(itemCert, obj._id, report.Id, 3);
                    await _repositoryReportResumeItem.AddAsync(cert);
                }
            }
            catch (Exception ex)
            {


            }
            return true;
        }
        protected async Task<ReportJobSeeker> ConvertToReportJobSeeker(JobSeeker obj, ReportJobSeeker report)
        {
            if (report == null)
                report = new ReportJobSeeker();

            report.JobSeekerId = obj._id;
            report.CreatedAt = obj.CreatedAt;
            report.Name = obj.Name;
            report.JobTitle = obj.JobTitle;
            report.Email = obj.Email;
            report.Phone = obj.Phone;
            report.Website = obj.Website;
            report.QualificationId = obj.Qualification._id;
            report.QualificationName = obj.Qualification.Name;
            report.ExperienceId = obj.Experience._id;
            report.ExperienceName = obj.Experience.Name;
            report.About = obj.About;
            report.SocialFacebook = obj.SocialFacebook;
            report.SocialTwitter = obj.SocialTwitter;
            report.SocialLinkedin = obj.SocialLinkedin;
            report.SocialGooglePlus = obj.SocialGooglePlus;
            report.CountryId = obj.Country._id;
            report.CountryName = obj.Country.Name;
            report.CityId = obj.City._id;
            report.CityName = obj.City.Name;
            if (obj.DOB.Date.ToString() != "01/01/0001 00:00:00" && obj.DOB.Year > 1900)
                report.DOB = obj.DOB;

            return report;
        }
        protected async Task<ReportJobSeekerLanguage> ConvertToReportJobSeekerLang(SubItem obj, string JobSeekerId, long FK_JobSeekerId)
        {
            var report = new ReportJobSeekerLanguage();
            report.JobSeekerId = JobSeekerId;
            report.FK_JobSeekerId = FK_JobSeekerId;
            report.LanguageId = obj._id;
            report.LanguageName = obj.Name;

            return report;
        }
        protected async Task<ReportJobSeekerResumeItem> ConvertToReportJobSeekerResumeItem(ResumeItem obj, string JobSeekerId, long FK_JobSeekerId, int typeId)
        {
            var report = new ReportJobSeekerResumeItem();
            report.JobSeekerId = JobSeekerId;
            report.FK_JobSeekerId = FK_JobSeekerId;
            report.Name = obj.Name;
            report.Title = obj.SubTitle;
            report.Description = obj.Description;
            if (obj.StartDate.Date.ToString() != "01/01/0001 00:00:00" && obj.StartDate.Year > 1900)
                report.StartDate = obj.StartDate;
            if (obj.EndDate.Date.ToString() != "01/01/0001 00:00:00" && obj.EndDate.Year > 1900)
                report.EndDate = obj?.EndDate;
            report.FK_ResumeItemTypeId = typeId;
            return report;
        }
        protected async Task<ReportJobSeekerResumeItem> ConvertToReportJobSeekerCert(ResumeCertification obj, string JobSeekerId, long FK_JobSeekerId, int typeId)
        {
            var report = new ReportJobSeekerResumeItem();
            report.JobSeekerId = JobSeekerId;
            report.FK_JobSeekerId = FK_JobSeekerId;
            report.Name = obj.Name;
            report.Description = obj.Description;
            if (obj.StartDate.Date.ToString() != "01/01/0001 00:00:00" && obj.StartDate.Year > 1900)
            {
                report.StartDate = obj.StartDate;
                report.EndDate = obj.StartDate;
            }
            report.FK_ResumeItemTypeId = typeId;
            return report;
        }

        public async Task<MongoResultPaged<CanAccessContactInformation>> ContactPermissioGetApprovalList(string UserId, int pageNumber = 1, int PageSize = 15)
        {
            var seeker = await GetByUserId(UserId);
            if (seeker == null)
                return new MongoResultPaged<CanAccessContactInformation>(0, new List<CanAccessContactInformation>(), PageSize);

            var lst = seeker.ContactPermission.OrderByDescending(x => x.CreatedAt).Skip(PageSize * (pageNumber - 1)).Take(pageNumber).ToList();
            var result = new MongoResultPaged<CanAccessContactInformation>(seeker.ContactPermission.Count(), lst, PageSize);
            return result;

        }
        public async Task<bool> ContactPermissionRequest(string UserId, string EmployerId, string CompanyId, string CompanyName)
        {
            var seeker = await GetByUserId(UserId);
            if (seeker == null)
                return false;

            var res = await ContactPermissionHasPermission(UserId, EmployerId);
            if (res != null)
                return false;

            var requset = new CanAccessContactInformation();
            requset.EmployerId = EmployerId;
            requset.CompanyId = CompanyId;
            requset.CompanyName = CompanyName;

            seeker.ContactPermission.Add(requset);

            await _dBJobSeeker.UpdateObj(seeker._id, seeker);
            return true;
        }
        public async Task<bool> ContactPermissionApprove(string UserId, string EmployerId)
        {
            return await ContactPermissionUpdateStatus(UserId, EmployerId, true);
        }
        public async Task<bool> ContactPermissionReject(string UserId, string EmployerId)
        {
            return await ContactPermissionUpdateStatus(UserId, EmployerId, false);
        }
        protected async Task<bool> ContactPermissionUpdateStatus(string UserId, string EmployerId, bool flag)
        {
            var seeker = await GetByUserId(UserId);
            if (seeker == null)
                return false;

            var obj = seeker.ContactPermission.Where(x => x.EmployerId == EmployerId).FirstOrDefault();
            obj.IsApproved = Convert.ToInt32(flag);

            await _dBJobSeeker.UpdateObj(seeker._id, seeker);

            return true;
        }
        public async Task<int?> ContactPermissionHasPermission(string UserId, string EmployerId)
        {
            var seeker = await GetByUserId(UserId);
            if (seeker == null)
                return -1;

            var obj = seeker.ContactPermission.Where(x => x.EmployerId == EmployerId).FirstOrDefault();
            //if (obj == null)
            //    return false;

            return obj?.IsApproved;
        }

        public async Task<long> ReportJobSeekerCount(DateTime StartDate, DateTime EndDate)
        {
            var filter = Builders<JobSeeker>.Filter.Empty;

            if (StartDate != DateTime.MinValue)
            {
                filter = filter & Builders<JobSeeker>.Filter.Where(x => x.CreatedAt >= StartDate);
            }
            if (EndDate != DateTime.MinValue)
            {
                filter = filter & Builders<JobSeeker>.Filter.Where(x => x.CreatedAt <= EndDate);
            }

            return await _dBJobSeeker.Count(filter);
        }
        public async Task<(long, long, long)> ReportJobSeekerGenderCount(DateTime StartDate, DateTime EndDate)
        {
            var filtermain = Builders<JobSeeker>.Filter.Empty;

            if (StartDate != DateTime.MinValue)
            {
                filtermain = filtermain & Builders<JobSeeker>.Filter.Where(x => x.CreatedAt >= StartDate);
            }
            if (EndDate != DateTime.MinValue)
            {
                filtermain = filtermain & Builders<JobSeeker>.Filter.Where(x => x.CreatedAt <= EndDate);
            }

            var filter = filtermain & Builders<JobSeeker>.Filter.Where(x => x.Gender == 1);
            var male = await _dBJobSeeker.Count(filter);

            filter = filtermain & Builders<JobSeeker>.Filter.Where(x => x.Gender == 2);
            var female = await _dBJobSeeker.Count(filter);

            filter = filtermain & Builders<JobSeeker>.Filter.Where(x => x.Gender != 2 && x.Gender != 1);
            var other = await _dBJobSeeker.Count(filter);
            return (male, female, other);
        }

        public async Task<string> CopyBlob(CloudStorageAccount cloudStorageAccount, string ContainerName, string sourceFileName, string NewFileName, string Type)
        {
            try
            {
                string destinationFileName = Type + "/" + NewFileName;

                CloudBlobClient blobStorageClient = cloudStorageAccount.CreateCloudBlobClient();
                var sourceContainer = blobStorageClient.GetContainerReference(ContainerName);
                var destinationContainer = blobStorageClient.GetContainerReference(ContainerName);

                if (await destinationContainer.CreateIfNotExistsAsync())
                {
                    await destinationContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                }

                var sourceBlob = sourceContainer.GetBlobReference(sourceFileName);

                var destinationBlob = destinationContainer.GetBlobReference(destinationFileName);

                var result = await destinationBlob.StartCopyAsync(sourceBlob.Uri);

                string copyURL = destinationBlob.Uri.ToString();

                return copyURL;
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        public async Task<bool> DeleteBlob(CloudStorageAccount cloudStorageAccount, string containerName, string blobFileName)
        {
            try
            {
                CloudBlobClient blobStorageClient = cloudStorageAccount.CreateCloudBlobClient();
                var container = blobStorageClient.GetContainerReference(containerName);
                var blob = container.GetBlobReference(blobFileName);
                await blob.DeleteAsync();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }

        }

    }
}

using Employment.Entity.Mongo;
using Employment.Interface;
using Employment.MongoDB.Interface;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MongoDB.Driver;
using Employment.Enum;
using Employment.Persistance.Interfaces;
using Employment.Entity.Model;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Employment.Entity.Mongo;

namespace Employment.Services
{
    public class ServiceJob : ServiceRepository<Job>, IServiceJob
    {
        private readonly IDBJob _dBJobs;
        private readonly IDBCompany _dBCompany;
        private readonly IDBIndustry _dbIndustry;
        private readonly IDBCountry _dbCountry;
        private readonly IDBQualification _dBQualification;
        private readonly IDBJobFields _dBJobFields;
        private readonly IDBYearsOfExperience _dBYearsOfExperience;
        private readonly IDBApply _dBApply;
        private readonly IDBFavourite _dBFavourite;
        private readonly IUserProfile _BLUserProfile;
        private readonly ICacheConfig _ICacheConfig;
        private IAsyncRepository<ReportJob> _repositoryReport;
        protected string ServiceName = string.Empty;
        protected string IndexName = string.Empty;
        protected string ApiKey = string.Empty;
        public ServiceJob(IDBJob dBJobs
                        , IDBIndustry dBIndustry
                        , IDBYearsOfExperience dBYearsOfExperience
                        , IDBQualification dBQualification
                        , IDBJobFields dBJobFields
                        , IDBCompany dBCompany
                        , IDBCountry dBCountry
                        , IDBApply dBApply
                        , IUserProfile BLUserProfile
                        , ICacheConfig CacheConfig
                        , IAsyncRepository<ReportJob> repositoryReport
                        , IDBFavourite dBFavourite) : base(dBJobs)
        {
            _dBJobs = dBJobs;
            _dBCompany = dBCompany;
            _dbCountry = dBCountry;
            _dbIndustry = dBIndustry;
            _dBQualification = dBQualification;
            _dBYearsOfExperience = dBYearsOfExperience;
            _dBJobFields = dBJobFields;
            _dBApply = dBApply;
            _BLUserProfile = BLUserProfile;
            _ICacheConfig = CacheConfig;
            _dBFavourite = dBFavourite;
            _repositoryReport = repositoryReport;

            ServiceName = _ICacheConfig.ServiceName;
            IndexName = _ICacheConfig.IndexName;
            ApiKey = _ICacheConfig.AdminApiKey;

        }
        public async Task<bool> IncrementApplicantCounter(string JobId)
        {

            var job = await GetById(JobId);
            if (job == null)
                return false;

            job.ApplicantCount += 1;

            await _dBJobs.UpdateObj(JobId, job);

            return true;
        }
        public async Task<MongoResultPaged<Job>> GetJobWaitingApproval(string filterText, int pageNumber = 1, int PageSize = 15)
        {
            var sort = Builders<Job>.Sort.Ascending(x => x.Name);

            var filter = Builders<Job>.Filter.Where(x => (x.Name.ToLower().Contains(filterText.ToLower())
                                                    || x.Company.Name.ToLower().Contains(filterText.ToLower()))
                                                    && x.Status == Enum.EnumJobStatus.Published
                                                    && x.IsActive == true);

            var lst = await _dBJobs.GetPaged(filter, sort, pageNumber, PageSize);
            return lst;
        }
        public async Task<long> GetJobWaitingApprovalCount()
        {
            var sort = Builders<Job>.Sort.Ascending(x => x.Name);

            var filter = Builders<Job>.Filter.Where(x => x.Status == Enum.EnumJobStatus.Published
                                                    && x.IsActive == true);

            var count = await _dBJobs.Count(filter);
            return count;
        }

        public async Task<MongoResultPaged<Job>> ListAllByEmployerId(string UserId, string filterText, int pageNumber = 1, int PageSize = 15)
        {
            var user = await _BLUserProfile.UserProfileGetById(UserId);
            if (user == null)
                return new MongoResultPaged<Job>(0, new List<Job>(), PageSize);

            var lstCompanyId = user.MyCompanies.Select(x => x._id);

            var sort = Builders<Job>.Sort.Descending(x => x.CreatedAt);

            var filter = Builders<Job>.Filter.Where(x => x.Name.ToLower().Contains(filterText.ToLower()));
            filter = filter & Builders<Job>.Filter.Where(x => lstCompanyId.Contains(x.Company._id));

            var lst = await _dBJobs.GetPaged(filter, sort, pageNumber, PageSize);
            return lst;
        }
        public async override Task<string> CreateReturnId(Job obj)
        {
            obj = await UpdateJobSubItems(obj);
            var res = await base.CreateReturnId(obj);
            await CreateReportJob(obj);

            //SS -Added data in AzureIndex for Search
            ServiceAzureSearch AS = new ServiceAzureSearch(_dBJobs, _ICacheConfig);
            await AS.UploadDataOnIndex(res);

            return res;
        }
        public async Task<List<Job>> GetJobsByCompanyId(string CompanyId)
        {
            var filter = Builders<Job>.Filter.Where(x => x.Company._id == CompanyId
                                                       && x.IsActive == true
                                                       && x.Status == EnumJobStatus.Approved);
            var lst = await _dBJobs.GetPaged(filter, null, 1, int.MaxValue);

            return lst.lstResult;
        }
        public async Task<JobStats> GetMyJobStats(string userId)
        {
            var result = new JobStats();

            var user = await _BLUserProfile.UserProfileGetById(userId);

            if (user == null)
                return result;

            var companyIds = user.MyCompanies.Select(x => x._id).ToList();
            var filter = Builders<Job>.Filter.Where(x => companyIds.Contains(x.Company._id));

            var lst = await _dBJobs.GetPaged(filter, null, 1, int.MaxValue);
            if (lst.totalCount == 0)
                return result;

            var jobIds = lst.lstResult.Select(x => x._id).ToList();
            var filterApply = Builders<Apply>.Filter.Where(x => x.IsActive == true && jobIds.Contains(x.Job._id));

            var apply = await _dBApply.GetPaged(filterApply, null, 1, int.MaxValue);

            result.JobCount = lst.totalCount;
            result.PostedJobCount = lst.lstResult.Count(x => x.Status != EnumJobStatus.Draft && x.IsActive == true);
            result.ActiveJobCount = lst.lstResult.Count(x => x.Status == EnumJobStatus.Approved && x.IsActive == true);
            result.ApplicantCount = apply.totalCount;

            return result;
        }
        public async Task<bool> UpdateExpiredJobs()
        {
            return await _dBJobs.UpdateExpiredJobs();
        }
        public async Task<bool> UpdateStatus(string Id, EnumJobStatus status)
        {
            if (string.IsNullOrEmpty(Id))
                return false;

            var obj = await GetById(Id);
            obj.Status = status;
            await _dBJobs.UpdateObj(obj._id, obj);

            return true;
        }
        public async Task<bool> Update(Job obj)
        {
            if (obj == null)
                return false;

            var job = await GetById(obj._id);
            if (job == null)// || job.Status == EnumJobStatus.Closed)
                return false;

            obj = await UpdateJobSubItems(obj);
            obj.ApplicantCount = job.ApplicantCount;
            obj.IsActive = job.IsActive;
            obj.CreatedAt = job.CreatedAt;

            await _dBJobs.UpdateObj(obj._id, obj);
            await updateFavourite(obj);
            await updateApply(obj);

            await updateReportJob(obj);

            //SS -Added data in AzureIndex for Search
            ServiceAzureSearch AS = new ServiceAzureSearch(_dBJobs, _ICacheConfig);
            await AS.UploadDataOnIndex(obj._id.ToString());

            return true;
        }
        protected async Task<Job> UpdateJobSubItems(Job obj)
        {
            if (obj == null)
                return obj;
            if (!string.IsNullOrEmpty(obj.Company._id))
            {
                var Company = await _dBCompany.GetById(obj.Company._id);
                obj.Company.Name = Company.Name;
                obj.Company.URL = Company.CompanyLogo;
            }
            if (!string.IsNullOrEmpty(obj.Industry._id))
            {
                var industry = await _dbIndustry.GetById(obj.Industry._id);
                obj.Industry.Name = industry.Name;
            }
            if (!string.IsNullOrEmpty(obj.Country._id))
            {
                var country = await _dbCountry.GetById(obj.Country._id);
                obj.Country.Name = country.Name;

                if (!string.IsNullOrEmpty(obj.City._id))
                {
                    var city = country.subItems.Where(x => x._id == obj.City._id).FirstOrDefault();
                    obj.City.Name = city.Name;
                }
            }
            if (!string.IsNullOrEmpty(obj.Qualification._id))
            {
                var Qualification = await _dBQualification.GetById(obj.Qualification._id);
                obj.Qualification.Name = Qualification.Name;
            }
            if (!string.IsNullOrEmpty(obj.Experience._id))
            {
                var Experience = await _dBYearsOfExperience.GetById(obj.Experience._id);
                obj.Experience.Name = Experience.Name;
            }
            if (!string.IsNullOrEmpty(obj.JobField._id))
            {
                var JobField = await _dBJobFields.GetById(obj.JobField._id);
                obj.JobField.Name = JobField.Name;

                if (!string.IsNullOrEmpty(obj.JobSubField._id))
                {
                    var JobSubField = JobField.subItems.Where(x => x._id == obj.JobSubField._id).FirstOrDefault();
                    obj.JobSubField.Name = JobSubField.Name;
                }
            }
            return obj;
        }

        public async Task<MongoResultPaged<JobSearch>> Search(string[] CompanyId, string filterText, List<string> ExperienceId, List<int> GenderId, List<string> QualificationId, List<string> IndustryId, List<string> JobFieldId, List<string> CountryId, List<string> CityId, int pageNumber = 1, int PageSize = 15)
        {
            IEnumerable<JobSearch> filter = new List<JobSearch>();



            if (CityId != null && filterText == null)
            {
                List<JobSearch> filterResult = new List<JobSearch>();
                IEnumerable<JobSearch> filterrs = new List<JobSearch>();
                for (int j = 0; j < CityId.Count(); j++)
                {


                    string url = $"https://{ServiceName}.search.windows.net/indexes/{IndexName}/docs?api-version=2020-06-30-Preview&search=" + "City:" + CityId[j] + "&speler=lexicon&queryLanguage=en-us&queryType=full";
                    string data = GetDataIndex(url);
                    var json1 = JObject.Parse((data));
                    string data1 = json1["value"].ToString();
                    List<JobSearch> Azurefilter = getAllJob(data1);
                    //IEnumerable<JobSearch> filter1 = new List<JobSearch>();

                    List<JobSearch> AddMongoData = new List<JobSearch>();
                    //var filter = Builders<Job>.Filter.Where(x => x.IsActive == true && x.Status == EnumJobStatus.Approved);
                    for (int i = 0; i < Azurefilter.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(Azurefilter[i].id))
                        {
                            JobSearch AzureFilterMonago = new JobSearch();
                            //AzureFilterMonago = filter.Where(x => x.id == Azurefilter[i].id);
                            //string resultdata = "ObjectId("+Azurefilter[i].id+")";
                            var Iddata = await _dBJobs.GetById(Azurefilter[i].id);
                            AzureFilterMonago.id = Iddata._id;
                            AzureFilterMonago.Name = Iddata.Name;
                            AzureFilterMonago.JobField = Iddata.JobField.Name;
                            AzureFilterMonago.JobSubField = Iddata.JobSubField.Name;
                            AzureFilterMonago.Qualification = Iddata.Qualification.Name;
                            AzureFilterMonago.Company = Iddata.Company.Name;
                            AzureFilterMonago.Experience = Iddata.Experience.Name;
                            AzureFilterMonago.Country = Iddata.Country.Name;
                            AzureFilterMonago.City = Iddata.City.Name;
                            filterResult.Add(AzureFilterMonago);
                            filter = filterResult;

                        }
                    }
                }
                filter = filter.Where(x => x.IsActive == true && x.Status == "Approved").ToList();
                //filterrs = filterResult.Where(x => CityId.Contains(x.City));
                //filterrs = filterResult.Where(x => x.Name.ToLower().Contains(filterText.ToLower())
                //                             || x.Company.ToLower().Contains(filterText.ToLower()));

            }
            if (!string.IsNullOrEmpty(filterText) && CityId == null)
            {
                string url = $"https://{ServiceName}.search.windows.net/indexes/{IndexName}/docs?api-version=2020-06-30-Preview&search=" + filterText + "&speller=lexicon&queryLanguage=en-us&queryType=full&searchMode=any&select=Name,Company,JobField";
                string data = GetDataIndex(url);
                var json = JObject.Parse((data));
                string data1 = json["value"].ToString();
                List<JobSearch> Azurefilter = getAllJob(data1);
                List<JobSearch> filterResult = new List<JobSearch>();
                IEnumerable<JobSearch> filterrs = new List<JobSearch>();
                List<JobSearch> AddMongoData = new List<JobSearch>();
                for (int i = 0; i < Azurefilter.Count(); i++)
                {
                    if (!string.IsNullOrEmpty(Azurefilter[i].id))
                    {
                        JobSearch AzureFilterMonago = new JobSearch();
                        var Iddata = await _dBJobs.GetById(Azurefilter[i].id);
                        AzureFilterMonago.id = Iddata._id;
                        AzureFilterMonago.Name = Iddata.Name;
                        AzureFilterMonago.JobField = Iddata.JobField.Name;
                        AzureFilterMonago.JobSubField = Iddata.JobSubField.Name;
                        AzureFilterMonago.Qualification = Iddata.Qualification.Name;
                        AzureFilterMonago.Company = Iddata.Company.Name;
                        AzureFilterMonago.Experience = Iddata.Experience.Name;
                        AzureFilterMonago.Country = Iddata.Country.Name;
                        AzureFilterMonago.City = Iddata.City.Name;
                        filterResult.Add(AzureFilterMonago);
                    }
                }


                filter = filterResult;

                filter = filter.Where(x => x.IsActive == true && x.Status == "Approved").ToList();

            }
            if (CityId != null && filterText != null)
            // if (CityId != null && filterText != null) ///////If City Id is not equal to null and Filter text is not null then go Inside
            {

                //string url = $"https://{ServiceName}.search.windows.net/indexes/{IndexName}/docs?api-version=2020-06-30-Preview&search=" + filterText + "&speller=lexicon&queryLanguage=en-us&queryType=full&searchMode=any&select=Name,Company,JobField";
                List<JobSearch> filterResult = new List<JobSearch>();
                IEnumerable<JobSearch> filterrs = new List<JobSearch>();
                for (int j = 0; j < CityId.Count(); j++)
                {
                    string url = $"https://{ServiceName}.search.windows.net/indexes/{IndexName}/docs?api-version=2020-06-30&search=" + filterText + "&$filter=City eq " + "'" + CityId[j] + "'";
                    string data = GetDataIndex(url);
                    var json = JObject.Parse((data));
                    string data1 = json["value"].ToString();
                    List<JobSearch> Azurefilter = getAllJob(data1);

                    List<JobSearch> citywithfiltertect = new List<JobSearch>();

                    IEnumerable<JobSearch> filtertextresult = new List<JobSearch>();

                    for (int i = 0; i < Azurefilter.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(Azurefilter[i].id))
                        {
                            JobSearch AzureFilterMonago = new JobSearch();
                            var Iddata = await _dBJobs.GetById(Azurefilter[i].id);
                            AzureFilterMonago.id = Iddata._id;
                            AzureFilterMonago.Name = Iddata.Name;
                            AzureFilterMonago.JobField = Iddata.JobField.Name;
                            AzureFilterMonago.JobSubField = Iddata.JobSubField.Name;
                            AzureFilterMonago.Qualification = Iddata.Qualification.Name;
                            AzureFilterMonago.Company = Iddata.Company.Name;
                            AzureFilterMonago.Experience = Iddata.Experience.Name;
                            AzureFilterMonago.Country = Iddata.Country.Name;
                            AzureFilterMonago.City = Iddata.City.Name;
                            filterResult.Add(AzureFilterMonago);
                        }
                    }
                }

                filter = filterResult;



                filter = filter.Where(x => x.IsActive == true && x.Status == "Approved").ToList();
                IEnumerable<JobSearch> filtercity = new List<JobSearch>();
                List<JobSearch> filterResultcity = new List<JobSearch>();


            }
            if (CompanyId != null)
            {
                List<JobSearch> filterResult = new List<JobSearch>();
                for (int j = 0; j < CompanyId.Count(); j++)
                {


                    string url = $"https://{ServiceName}.search.windows.net/indexes/{IndexName}/docs?api-version=2020-06-30-Preview&search=*" + "&$filter=Company eq " + "'" + CompanyId[j] + "'";

                    //   string url = $"https://{ServiceName}.search.windows.net/indexes/{IndexName}/docs?api-version=2020-06-30-Preview&search=Company:" + CompanyId[j];
                    string data = GetDataIndex(url);
                    var json = JObject.Parse((data));
                    string data1 = json["value"].ToString();
                    List<JobSearch> Azurefilter = getAllJob(data1);
                    //List<JobSearch> filterResult = new List<JobSearch>();

                    IEnumerable<JobSearch> filterrs = new List<JobSearch>();
                    List<JobSearch> AddMongoData = new List<JobSearch>();
                    for (int i = 0; i < Azurefilter.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(Azurefilter[i].id))
                        {
                            JobSearch AzureFilterMonago = new JobSearch();
                            var Iddata = await _dBJobs.GetById(Azurefilter[i].id);
                            AzureFilterMonago.id = Iddata._id;
                            AzureFilterMonago.Name = Iddata.Name;
                            AzureFilterMonago.JobField = Iddata.JobField.Name;
                            AzureFilterMonago.JobSubField = Iddata.JobSubField.Name;
                            AzureFilterMonago.Qualification = Iddata.Qualification.Name;
                            AzureFilterMonago.Company = Iddata.Company.Name;
                            AzureFilterMonago.Experience = Iddata.Experience.Name;
                            AzureFilterMonago.Country = Iddata.Country.Name;
                            AzureFilterMonago.City = Iddata.City.Name;
                            filterResult.Add(AzureFilterMonago);
                        }
                    }

                }
                //filterrs = filterResult.Where(x => CompanyId.Contains(x.Company));
                filter = filter.Concat(filterResult);
                //filter = filterResult;
                filter = filter.Where(x => x.IsActive == true && x.Status == "Approved").ToList();
            }
            if (ExperienceId != null && ExperienceId.Count() > 0)
            {
                IEnumerable<JobSearch> filterrs = new List<JobSearch>();
                List<JobSearch> filterResult = new List<JobSearch>();
                for (int j = 0; j < ExperienceId.Count(); j++)
                {
                    string url = $"https://{ServiceName}.search.windows.net/indexes/{IndexName}/docs?api-version=2020-06-30-Preview&search=*" + "&$filter=Experience eq " + "'" + ExperienceId[j] + "'";
                    string data = GetDataIndex(url);
                    var json = JObject.Parse((data));
                    string data1 = json["value"].ToString();
                    List<JobSearch> Azurefilter = getAllJob(data1);
                    //List<JobSearch> filterResult = new List<JobSearch>();
                    List<JobSearch> AddMongoData = new List<JobSearch>();
                    for (int i = 0; i < Azurefilter.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(Azurefilter[i].id))
                        {
                            JobSearch AzureFilterMonago = new JobSearch();
                            var Iddata = await _dBJobs.GetById(Azurefilter[i].id);
                            AzureFilterMonago.id = Iddata._id;
                            AzureFilterMonago.Name = Iddata.Name;
                            AzureFilterMonago.JobField = Iddata.JobField.Name;
                            AzureFilterMonago.JobSubField = Iddata.JobSubField.Name;
                            AzureFilterMonago.Qualification = Iddata.Qualification.Name;
                            AzureFilterMonago.Company = Iddata.Company.Name;
                            AzureFilterMonago.Experience = Iddata.Experience.Name;
                            AzureFilterMonago.Country = Iddata.Country.Name;
                            AzureFilterMonago.City = Iddata.City.Name;
                            filterResult.Add(AzureFilterMonago);
                        }
                    }
                }

                filter = filter.Concat(filterResult);
                //    filterrs = filterResult.Where(x => ExperienceId.Contains(x.Experience));
                //  filter = filterrs;
                filter = filter.Where(x => x.IsActive == true && x.Status == "Approved").ToList();

            }
            if (QualificationId != null && QualificationId.Count() > 0)
            {
                List<JobSearch> filterResult = new List<JobSearch>();
                for (int j = 0; j < QualificationId.Count(); j++)
                {

                    string url = $"https://{ServiceName}.search.windows.net/indexes/{IndexName}/docs?api-version=2020-06-30-Preview&search=*" + "&$filter=Qualification eq " + "'" + QualificationId[j] + "'";
                    string data = GetDataIndex(url);
                    var json = JObject.Parse((data));
                    string data1 = json["value"].ToString();
                    List<JobSearch> Azurefilter = getAllJob(data1);

                    IEnumerable<JobSearch> filterrs = new List<JobSearch>();
                    List<JobSearch> AddMongoData = new List<JobSearch>();
                    for (int i = 0; i < Azurefilter.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(Azurefilter[i].id))
                        {
                            JobSearch AzureFilterMonago = new JobSearch();
                            var Iddata = await _dBJobs.GetById(Azurefilter[i].id);
                            AzureFilterMonago.id = Iddata._id;
                            AzureFilterMonago.Name = Iddata.Name;
                            AzureFilterMonago.JobField = Iddata.JobField.Name;
                            AzureFilterMonago.JobSubField = Iddata.JobSubField.Name;
                            AzureFilterMonago.Qualification = Iddata.Qualification.Name;
                            AzureFilterMonago.Company = Iddata.Company.Name;
                            AzureFilterMonago.Experience = Iddata.Experience.Name;
                            AzureFilterMonago.Country = Iddata.Country.Name;
                            AzureFilterMonago.City = Iddata.City.Name;
                            filterResult.Add(AzureFilterMonago);
                        }
                    }


                }
                filter = filter.Concat(filterResult);
                //filter = filterResult;
                filter = filter.Where(x => x.IsActive == true && x.Status == "Approved").ToList();
            }
            if (IndustryId != null && IndustryId.Count > 0)
            {
                IEnumerable<JobSearch> filterrs = new List<JobSearch>();
                List<JobSearch> filterResult = new List<JobSearch>();
                for (int j = 0; j < IndustryId.Count(); j++)
                {

                    string url = $"https://{ServiceName}.search.windows.net/indexes/{IndexName}/docs?api-version=2020-06-30-Preview&search=*" + "&$filter=Industry eq " + "'" + IndustryId[j] + "'";
                    //  string url = $"https://{ServiceName}.search.windows.net/indexes/{IndexName}/docs?api-version=2020-06-30-Preview&search=Industry:" + IndustryId[j];
                    string data = GetDataIndex(url);
                    var json = JObject.Parse((data));
                    string data1 = json["value"].ToString();
                    List<JobSearch> Azurefilter = getAllJob(data1);
                    List<JobSearch> AddMongoData = new List<JobSearch>();
                    for (int i = 0; i < Azurefilter.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(Azurefilter[i].id))
                        {
                            JobSearch AzureFilterMonago = new JobSearch();
                            var Iddata = await _dBJobs.GetById(Azurefilter[i].id);
                            AzureFilterMonago.id = Iddata._id;
                            AzureFilterMonago.Name = Iddata.Name;
                            AzureFilterMonago.JobField = Iddata.JobField.Name;
                            AzureFilterMonago.JobSubField = Iddata.JobSubField.Name;
                            AzureFilterMonago.Qualification = Iddata.Qualification.Name;
                            AzureFilterMonago.Company = Iddata.Company.Name;
                            AzureFilterMonago.Experience = Iddata.Experience.Name;
                            AzureFilterMonago.Country = Iddata.Country.Name;
                            AzureFilterMonago.Industry = Iddata.Industry.Name;
                            AzureFilterMonago.City = Iddata.City.Name;
                            filterResult.Add(AzureFilterMonago);
                        }
                    }

                }
                //filterrs = filterResult.Where(x => IndustryId.Contains(x.Industry));
                filter = filter.Concat(filterResult);
                //filter = filterResult;
                filter = filter.Where(x => x.IsActive == true && x.Status == "Approved").ToList();
            }
            if (JobFieldId != null && JobFieldId.Count > 0)
            {
                List<JobSearch> filterResult = new List<JobSearch>();
                IEnumerable<JobSearch> filterrs = new List<JobSearch>();
                for (int j = 0; j < JobFieldId.Count(); j++)
                {
                    string url = $"https://{ServiceName}.search.windows.net/indexes/{IndexName}/docs?api-version=2020-06-30-Preview&search=*" + "&$filter=JobField eq " + "'" + JobFieldId[j] + "'"; string data = GetDataIndex(url);
                    var json = JObject.Parse((data));
                    string data1 = json["value"].ToString();
                    List<JobSearch> Azurefilter = getAllJob(data1);
                    // List<JobSearch> filterResult = new List<JobSearch>();
                    List<JobSearch> AddMongoData = new List<JobSearch>();
                    for (int i = 0; i < Azurefilter.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(Azurefilter[i].id))
                        {
                            JobSearch AzureFilterMonago = new JobSearch();
                            //AzureFilterMonago = filter.Where(x => x.id == Azurefilter[i].id);
                            //string resultdata = "ObjectId("+Azurefilter[i].id+")";
                            var Iddata = await _dBJobs.GetById(Azurefilter[i].id);
                            AzureFilterMonago.id = Iddata._id;
                            AzureFilterMonago.Name = Iddata.Name;
                            AzureFilterMonago.JobField = Iddata.JobField.Name;
                            AzureFilterMonago.JobSubField = Iddata.JobSubField.Name;
                            AzureFilterMonago.Qualification = Iddata.Qualification.Name;
                            AzureFilterMonago.Company = Iddata.Company.Name;
                            AzureFilterMonago.Experience = Iddata.Experience.Name;
                            AzureFilterMonago.Country = Iddata.Country.Name;
                            AzureFilterMonago.City = Iddata.City.Name;
                            filterResult.Add(AzureFilterMonago);

                        }
                    }
                    //filterrs = filterResult.Where(x => JobFieldId.Contains(x.JobField));

                }

                filter = filter.Concat(filterResult);
                //filter = filterResult;
                filter = filter.Where(x => x.IsActive == true && x.Status == "Approved").ToList();
            }
            var sort = filter.OrderByDescending(x => x.Name);
            var lst = await GetAzureIndexPaged(filter, sort, 1, 15);
            //var lst = await _dBJobs.GetPaged(filter, sort, pageNumber, PageSize);
            return lst;

        }



        public async Task<MongoResultPaged<JobSearch>> SearchforFilterTextAndCity(string filterText, List<string> CityId, int pageNumber = 1, int PageSize = 15)
        {

            IEnumerable<JobSearch> filter = new List<JobSearch>();
            if (CityId != null && filterText != null) ///////If City Id is not equal to null and Filter text is not null then go Inside
            {

                string url = $"https://{ServiceName}.search.windows.net/indexes/{IndexName}/docs?api-version=2020-06-30-Preview&search=" + filterText + "&speller=lexicon&queryLanguage=en-us&queryType=full&searchMode=any&select=Name,Company,JobField";
                string data = GetDataIndex(url);
                var json = JObject.Parse((data));
                string data1 = json["value"].ToString();
                List<JobSearch> Azurefilter = getAllJob(data1);
                List<JobSearch> filterResult = new List<JobSearch>();
                List<JobSearch> citywithfiltertect = new List<JobSearch>();
                IEnumerable<JobSearch> filterrs = new List<JobSearch>();
                IEnumerable<JobSearch> filtertextresult = new List<JobSearch>();

                for (int i = 0; i < Azurefilter.Count(); i++)
                {
                    if (!string.IsNullOrEmpty(Azurefilter[i].id))
                    {
                        JobSearch AzureFilterMonago = new JobSearch();
                        var Iddata = await _dBJobs.GetById(Azurefilter[i].id);
                        AzureFilterMonago.id = Iddata._id;
                        AzureFilterMonago.Name = Iddata.Name;
                        AzureFilterMonago.JobField = Iddata.JobField.Name;
                        AzureFilterMonago.JobSubField = Iddata.JobSubField.Name;
                        AzureFilterMonago.Qualification = Iddata.Qualification.Name;
                        AzureFilterMonago.Company = Iddata.Company.Name;
                        AzureFilterMonago.Experience = Iddata.Experience.Name;
                        AzureFilterMonago.Country = Iddata.Country.Name;
                        AzureFilterMonago.City = Iddata.City.Name;
                        filterResult.Add(AzureFilterMonago);
                    }
                }
                filtertextresult = filterResult;
                filter = filtertextresult.Where(x => x.IsActive == true && x.Status == "Approved").ToList();
                IEnumerable<JobSearch> filtercity = new List<JobSearch>();
                List<JobSearch> filterResultcity = new List<JobSearch>();
                for (int j = 0; j < CityId.Count(); j++)
                {
                    string url1 = $"https://{ServiceName}.search.windows.net/indexes/{IndexName}/docs?api-version=2020-06-30-Preview&search=" + "City:" + CityId[j] + "&speler=lexicon&queryLanguage=en-us&queryType=full";
                    string datacity = GetDataIndex(url1);
                    var json1 = JObject.Parse((datacity));
                    string data1city = json1["value"].ToString();
                    List<JobSearch> Azurefiltercity = getAllJob(data1city);

                    for (int i = 0; i < Azurefiltercity.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(Azurefiltercity[i].id))
                        {
                            JobSearch AzureFilterMonago = new JobSearch();
                            var Iddata = await _dBJobs.GetById(Azurefiltercity[i].id);
                            AzureFilterMonago.id = Iddata._id;
                            AzureFilterMonago.Name = Iddata.Name;
                            AzureFilterMonago.JobField = Iddata.JobField.Name;
                            AzureFilterMonago.JobSubField = Iddata.JobSubField.Name;
                            AzureFilterMonago.Qualification = Iddata.Qualification.Name;
                            AzureFilterMonago.Company = Iddata.Company.Name;
                            AzureFilterMonago.Experience = Iddata.Experience.Name;
                            AzureFilterMonago.Country = Iddata.Country.Name;
                            AzureFilterMonago.City = Iddata.City.Name;
                            filterResultcity.Add(AzureFilterMonago);
                        }
                    }
                }

                filtercity = filterResultcity;
                filter = filtercity.Where(x => x.IsActive == true && x.Status == "Approved").ToList();
            }
            var sort = filter.OrderByDescending(x => x.Name);
            var lst = await GetAzureIndexPaged(filter, sort, 1, 15);
            return lst;

        }
        public async Task<MongoResultPaged<JobSearch>> SearchCompany(int pageNumber = 1, int PageSize = 15)
        {
            string Url = "";
            //string data = GetDataIndexalllist();
            //  $top = 1000 &$count = true
            string data = GetDataIndex($"https://{ServiceName}.search.windows.net/indexes/{IndexName}/docs?api-version=2020-06-30-Preview&search=*&$top=1000&$count=true");
            var json = JObject.Parse((data));
            string data1 = json["value"].ToString();
            IEnumerable<JobSearch> filter = getAllJob(data1);
            filter = filter.Where(x => x.IsActive == true && x.Status == "Approved");
            var sort = filter.OrderByDescending(x => x.Name);
            var lst = await GetAzureIndexPaged(filter, sort, 1, 15);
            //var lst = await _dBJobs.GetPaged(filter, sort, pageNumber, PageSize);
            return lst;
        }


        public async Task<MongoResultPaged<JobSearch>> ForSearchValidation(string filterTextValidation, int pageNumber = 1, int PageSize = 15)
        {
            //start aj23072021
            List<JobSearch> filter = new List<JobSearch>();
            string url = "";
            // string data = GetDataIndex(url);
            string data = GetDataIndex($"https://{ServiceName}.search.windows.net/indexes/{IndexName}/docs?api-version=2020-06-30-Preview&search=*&$top=1000&$count=true");
            //string data = GetDataIndexalllist();
            var json = JObject.Parse((data));
            string data1 = json["value"].ToString();
            IEnumerable<JobSearch> filterdatacname = getAllJob(data1);
            IEnumerable<JobSearch> filterdatacompany = getAllJob(data1);
            IEnumerable<JobSearch> filterdatajob = getAllJob(data1);

            IEnumerable<JobSearch> filterdataname;
            IEnumerable<JobSearch> filterdataCompany;
            IEnumerable<JobSearch> filterdataJobField;

            List<JobSearch> fn = filterdatacname.ToList();
            List<JobSearch> fc = filterdatacompany.ToList();
            List<JobSearch> fj = filterdatajob.ToList();

            if (!string.IsNullOrEmpty(filterTextValidation))
            {

                filterdataname = filterdatacname.Where(x => x.Name.ToLower().Contains(filterTextValidation.ToLower()));
                fn = filterdataname.ToList();
                filterdataCompany = filterdatacompany.Where(x => x.Company.ToLower().Contains(filterTextValidation.ToLower()));
                fc = filterdataCompany.ToList();
                filterdataJobField = filterdatajob.Where(x => x.JobField.ToLower().Contains(filterTextValidation.ToLower()));
                fj = filterdataJobField.ToList();
            }
            List<JobSearch> list = getAllJob(data1);
            List<JobSearch> filterlist = new List<JobSearch>();
            List<JobSearch> filterlist1 = new List<JobSearch>();
            for (int i = 0; i < fn.Count(); i++)
            {
                string rs = fn[i].Name;
                fn[i].Company = null;
                fn[i].JobField = null;
                fn[i].Name = rs;
                filterlist.Add(fn[i]);
            }
            for (int i = 0; i < fc.Count(); i++)
            {
                string rs = fc[0].Company;
                fc[i].Company = rs;
                fc[i].JobField = null;
                fc[i].Name = null;
                filterlist.Add(fc[i]);
            }
            for (int i = 0; i < fj.Count(); i++)
            {
                string rs = fj[i].JobField;
                fj[i].Company = null;
                fj[i].JobField = rs;
                fj[i].Name = null;
                filterlist.Add(fj[i]);
            }



            filter = filterlist;
            var sort = filter.OrderByDescending(x => x.Name);
            var lst = await GetAzureIndexPaged(filter, sort, 1, 15);
            return lst;

        }

        public async Task<MongoResultPaged<Job>> AdminJobSearch(string CompanyId, int StatusId, string filterText, int pageNumber = 1, int PageSize = 15)
        {
            var filter = Builders<Job>.Filter.Where(x => x.IsActive == true);

            if (!string.IsNullOrEmpty(filterText))
            {
                filter = filter & Builders<Job>.Filter.Where(x => x.Name.ToLower().Contains(filterText.ToLower()));
            }
            if (!string.IsNullOrEmpty(CompanyId) && CompanyId != "-1")
            {
                filter = filter & Builders<Job>.Filter.Where(x => x.Company._id == CompanyId);
            }
            if (StatusId != 0 && StatusId != -1)
            {
                filter = filter & Builders<Job>.Filter.Where(x => x.Status == (EnumJobStatus)StatusId);
            }

            var sort = Builders<Job>.Sort.Descending(x => x.Name);
            var lst = await _dBJobs.GetPaged(filter, sort, pageNumber, PageSize);
            return lst;
        }
        public async Task<bool> UpdateCompanyInfo(Company obj)
        {
            var filter = Builders<Job>.Filter.Where(x => x.Company._id == obj._id);
            var update = Builders<Job>.Update.Set(x => x.Company.Name, obj.Name).Set(y => y.Company.URL, obj.CompanyLogo);

            await _dBJobs.UpdateManyAsync(filter, update);
            await updateFavouriteByCompany(obj);
            return true;
        }
        protected async Task<bool> updateFavourite(Job obj)
        {
            var filter = Builders<Favourite>.Filter.Where(x => x.EntityId == obj._id);
            var update = Builders<Favourite>.Update.Set(x => x.Name, obj.Name).Set(y => y.Title, obj.Company.Name).Set(z => z.ImageURL, obj.Company.URL);

            await _dBFavourite.UpdateManyAsync(filter, update);
            return true;
        }
        protected async Task<bool> updateApply(Job obj)
        {
            var filter = Builders<Apply>.Filter.Where(x => x.Job._id == obj._id);
            var update = Builders<Apply>.Update.Set(x => x.Job.Name, obj.Name).Set(y => y.Job.SubName, obj.Company.Name).Set(z => z.Job.URL, obj.Company.URL);

            await _dBApply.UpdateManyAsync(filter, update);
            return true;
        }
        protected async Task<bool> updateFavouriteByCompany(Company obj)
        {
            var filter = Builders<Favourite>.Filter.Where(x => x.EntityId == obj._id);
            var update = Builders<Favourite>.Update.Set(y => y.Title, obj.Name).Set(z => z.ImageURL, obj.CompanyLogo);

            await _dBFavourite.UpdateManyAsync(filter, update);
            return true;

        }
        protected async Task<bool> CreateReportJob(Job obj)
        {
            var report = await ConvertToReportJob(obj, null);

            try
            {
                await _repositoryReport.AddAsync(report);
            }
            catch (Exception ex)
            {


            }
            return true;
        }
        protected async Task<bool> updateReportJob(Job obj)
        {
            try
            {
                var report = await _repositoryReport.GetQueryableFirstorDefaultAsync(x => x.JobId == obj._id);
                report = await ConvertToReportJob(obj, report);
                await _repositoryReport.UpdateAsync(report);
            }
            catch (Exception ex)
            {


            }

            return true;
        }
        protected async Task<ReportJob> ConvertToReportJob(Job obj, ReportJob report)
        {
            if (report == null)
                report = new ReportJob();

            report.JobId = obj._id;
            report.CreatedAt = obj.CreatedAt;
            report.Name = obj.Name;
            report.CompanyId = obj.Company._id;
            report.CompanyName = obj.Company.Name;
            report.Description = obj.Description;
            report.Skills = obj.Skills;
            report.Benefits = obj.Benefits;
            report.Gender = obj.Gender;
            report.Deadline = obj.Deadline;
            report.JobFieldId = obj.JobField._id;
            report.JobFieldName = obj.JobField.Name;
            report.JobSubFieldId = obj.JobSubField._id;
            report.JobSubFieldName = obj.JobSubField.Name;
            report.typeId = ((int)obj.type).ToString();
            report.typeName = System.Enum.GetName(typeof(JobType), obj.type);
            report.ExperienceId = obj.Experience._id;
            report.ExperienceName = obj.Experience.Name;
            report.IndustryId = obj.Industry._id;
            report.IndustryName = obj.Industry.Name;
            report.QualificationId = obj.Qualification._id;
            report.QualificationName = obj.Qualification.Name;
            report.CountryId = obj.Country._id;
            report.CountryName = obj.Country.Name;
            report.CityId = obj.City._id;
            report.CityName = obj.City.Name;
            report.Address = obj.Address;
            return report;
        }
        public async Task<List<ReportJobCount>> ReportJobCount(string CompanyId, DateTime StartDate, DateTime EndDate, string JobFieldId)
        {
            return await _dBJobs.ReportJobPerCompany(CompanyId, StartDate, EndDate, JobFieldId);
        }


        public string GetDataIndex(string IndexUrl)
        {
            try
            {
                //WebRequest request = WebRequest.Create($"https://{ServiceName}.search.windows.net/indexes/{IndexName}/docs?api-version=2020-06-30-Preview&search=*&speller=lexicon&queryLanguage=en-us&queryType=full&searchMode=any");
                WebRequest request = WebRequest.Create(IndexUrl);
                request.ContentType = "application/json";
                request.Headers["api-key"] = ApiKey;
                request.Method = "GET";


                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                string result = reader.ReadToEnd();
                return result;
            }
            catch (WebException ex)
            {
                string error = "";
                if (ex.Response == null)
                {
                    error = "AzureIndex, " + "PostData; Error Msg=" + ex.Message + "; DateTime: " + DateTime.Now.ToString("MM - dd - yyyy hh mm");

                    return error;
                }
                else
                {
                    using (var stream = ex.Response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            error = "AzureIndex, " + "PostData; Error Msg=" + ex.Message + "; DateTime: " + DateTime.Now.ToString("MM - dd - yyyy hh mm");
                            return error;
                        }
                    }
                }
            }
        }


        public string GetDataIndexalllist()
        {
            try
            {
                // WebRequest request = WebRequest.Create($"https://{ServiceName}.search.windows.net/indexes/{IndexName}/docs?api-version=2020-06-30-Preview&search=*&speller=lexicon&queryLanguage=en-us&queryType=full&searchMode=any");
                WebRequest request = WebRequest.Create($"https://{ServiceName}.search.windows.net/indexes/{IndexName}/docs?api-version=2020-06-30-Preview&search=*&$skip=50&$count=true");

                //  WebRequest request = WebRequest.Create($"https://{ServiceName}.search.windows.net/indexes"+"('{IndexName}')/$metadata#docs(*)");


                request.ContentType = "application/json";
                request.Headers["api-key"] = ApiKey;
                request.Method = "GET";


                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                string result = reader.ReadToEnd();
                return result;
            }
            catch (WebException ex)
            {
                string error = "";
                if (ex.Response == null)
                {
                    error = "AzureIndex, " + "PostData; Error Msg=" + ex.Message + "; DateTime: " + DateTime.Now.ToString("MM - dd - yyyy hh mm");

                    return error;
                }
                else
                {
                    using (var stream = ex.Response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            error = "AzureIndex, " + "PostData; Error Msg=" + ex.Message + "; DateTime: " + DateTime.Now.ToString("MM - dd - yyyy hh mm");
                            return error;
                        }
                    }
                }
            }
        }


        public static List<JobSearch> getAllJob(string data)
        {
            data = data.Replace("@search.score", "search_score");
            var obj = JsonConvert.DeserializeObject<List<JobSearch>>(data);
            List<JobSearch> listJob = new List<JobSearch>();
            for (int i = 0; i < obj.Count; i++)
            {
                var jsonData = new JobSearch();
                jsonData.id = obj[i].id;
                jsonData.search_job = obj[i].search_job;
                jsonData.Name = obj[i].Name.Trim();
                jsonData.Company = obj[i].Company.Trim();
                jsonData.JobField = obj[i].JobField;
                jsonData.JobSubField = obj[i].JobSubField;
                jsonData.Experience = obj[i].Experience;
                jsonData.Industry = obj[i].Industry;
                jsonData.Qualification = obj[i].Qualification;
                jsonData.Country = obj[i].Country;
                jsonData.City = obj[i].City;

                listJob.Add(jsonData);

            }

            return listJob;
        }
        public async Task<MongoResultPaged<JobSearch>> GetAzureIndexPaged(IEnumerable<JobSearch> filter, IEnumerable<JobSearch> sort = null, int PageNumber = 1, int PageSize = 15)
        {
            int skip = PageSize * (PageNumber - 1);
            var count = filter.Count();
            var lst = filter.ToList();
            return new MongoResultPaged<JobSearch>(count, lst, PageSize);
        }
        public async void FilterCall(string data)
        {
            var json = JObject.Parse((data));
            string data1 = json["value"].ToString();
            //IEnumerable<JobSearch> filter1 = new List<JobSearch>();
            List<JobSearch> Azurefilter = getAllJob(data1);
            List<JobSearch> filterResult = new List<JobSearch>();
            List<JobSearch> AddMongoData = new List<JobSearch>();
            for (int i = 0; i < Azurefilter.Count(); i++)
            {
                if (!string.IsNullOrEmpty(Azurefilter[i].id))
                {
                    JobSearch AzureFilterMonago = new JobSearch();
                    var Iddata = await _dBJobs.GetById(Azurefilter[i].id);
                    AzureFilterMonago.id = Iddata._id;
                    AzureFilterMonago.Name = Iddata.Name;
                    AzureFilterMonago.JobField = Iddata.JobField.Name;
                    AzureFilterMonago.JobSubField = Iddata.JobSubField.Name;
                    AzureFilterMonago.Qualification = Iddata.Qualification.Name;
                    AzureFilterMonago.Company = Iddata.Company.Name;
                    AzureFilterMonago.Experience = Iddata.Experience.Name;
                    AzureFilterMonago.Country = Iddata.Country.Name;
                    AzureFilterMonago.City = Iddata.City.Name;
                    filterResult.Add(AzureFilterMonago);
                }
            }
            //return filterResult;
        }


    }
}



//if (ExperienceId != null && ExperienceId.Count > 0)
//{
//    IEnumerable<JobSearch> filterrs = new List<JobSearch>();
//    for (int j = 0; j < ExperienceId.Count(); j++)
//    {
//        // filter = filter & Builders<Job>.Filter.Where(x => ExperienceId.Contains(x.Experience._id));
//        // filter = filter.Where(x => ExperienceId.Contains(x.Experience));
//        string url = $"https://{ServiceName}.search.windows.net/indexes/employment3/docs?api-version=2020-06-30-Preview&search=" + "Experience=" + ExperienceId[j] + "&speller=lexicon&queryLanguage=en-us&queryType=full&searchMode=any";
//        string data = GetDataIndex(url);
//        var json = JObject.Parse((data));
//        string data1 = json["value"].ToString();
//        //IEnumerable<JobSearch> filter1 = new List<JobSearch>();
//        List<JobSearch> Azurefilter = getAllJob(data1);
//        List<JobSearch> filterResult = new List<JobSearch>();
//        List<JobSearch> AddMongoData = new List<JobSearch>();
//        //var filter = Builders<Job>.Filter.Where(x => x.IsActive == true && x.Status == EnumJobStatus.Approved);
//        for (int i = 0; i < Azurefilter.Count(); i++)
//        {
//            if (!string.IsNullOrEmpty(Azurefilter[i].id))
//            {
//                JobSearch AzureFilterMonago = new JobSearch();
//                //AzureFilterMonago = filter.Where(x => x.id == Azurefilter[i].id);
//                //string resultdata = "ObjectId("+Azurefilter[i].id+")";
//                var Iddata = await _dBJobs.GetById(Azurefilter[i].id);
//                AzureFilterMonago.id = Iddata._id;
//                AzureFilterMonago.Name = Iddata.Name;
//                AzureFilterMonago.JobField = Iddata.JobField.Name;
//                AzureFilterMonago.JobSubField = Iddata.JobSubField.Name;
//                AzureFilterMonago.Qualification = Iddata.Qualification.Name;
//                AzureFilterMonago.Company = Iddata.Company.Name;
//                AzureFilterMonago.Experience = Iddata.Experience.Name;
//                AzureFilterMonago.Country = Iddata.Country.Name;
//                AzureFilterMonago.City = Iddata.City.Name;
//                filterResult.Add(AzureFilterMonago);
//                filterrs = filterResult.Where(x => ExperienceId.Contains(x.Experience));
//            }
//        }

//        filter = filterrs;
//        filter = filter.Where(x => x.IsActive == true && x.Status == "Approved").ToList();
//    }
//}

//if (!string.IsNullOrEmpty(CityId))
//{
//    //  filter = filter & Builders<Job>.Filter.Where(x => x.City._id == CityId);
//   

//}
//if (CityId != null && CityId.Count() > 0)
//{
//    // filter = filter & Builders<Job>.Filter.Where(x => ExperienceId.Contains(x.Experience._id));
//    filter = filter.Where(x => CityId.Contains(x.City));
//}





// filterText id search by method
//if (!string.IsNullOrEmpty(filterText))
//{
//    string url = $"https://{ServiceName}.search.windows.net/indexes/employment2/docs?api-version=2020-06-30-Preview&search=" + filterText + "&speller=lexicon&queryLanguage=en-us&queryType=full&searchMode=any";
//    string data = GetDataIndex(url);
//    FilterCall(data);
//   //filter = filterResult;
//    filter = filter.Where(x => x.IsActive == true && x.Status == "Approved").ToList();
//}

// company id search by method
//if (CompanyId != null && CompanyId.Count() > 0)
//{
//    for (int j = 0; j < CompanyId.Count();j++) {

//        string url = $"https://{ServiceName}.search.windows.net/indexes/employment2/docs?api-version=2020-06-30-Preview&search=" + "Company=" + CompanyId[j] + "&speller=lexicon&queryLanguage=en-us&queryType=full&searchMode=any";
//        string data = GetDataIndex(url);
//        FilterCall(data);

//        //filter = filterResult;
//        filter = filter.Where(x => x.IsActive == true && x.Status == "Approved").ToList();
//        // filter = filter.Where(x => CompanyId.Contains(x.Company));
//    }
//}


//if (QualificationId != null && QualificationId.Count > 0)
//{

//    //filter = filter & Builders<Job>.Filter.Where(x => QualificationId.Contains(x.Qualification._id));
//    //filter = filter.Where(x => QualificationId.Contains(x.Qualification));
//    for (int j = 0; j < QualificationId.Count(); j++)
//    {

//        string url = $"https://{ServiceName}.search.windows.net/indexes/employment2/docs?api-version=2020-06-30-Preview&search=" + "Qualification=" + QualificationId[j];
//        string data = GetDataIndex(url);
//        var json = JObject.Parse((data));
//        string data1 = json["value"].ToString();
//        //IEnumerable<JobSearch> filter1 = new List<JobSearch>();
//        List<JobSearch> Azurefilter = getAllJob(data1);
//        List<JobSearch> filterResult = new List<JobSearch>();
//        List<JobSearch> AddMongoData = new List<JobSearch>();
//        //var filter = Builders<Job>.Filter.Where(x => x.IsActive == true && x.Status == EnumJobStatus.Approved);
//        for (int i = 0; i < Azurefilter.Count(); i++)
//        {
//            if (!string.IsNullOrEmpty(Azurefilter[i].id))
//            {
//                JobSearch AzureFilterMonago = new JobSearch();
//                //AzureFilterMonago = filter.Where(x => x.id == Azurefilter[i].id);
//                //string resultdata = "ObjectId("+Azurefilter[i].id+")";
//                var Iddata = await _dBJobs.GetById(Azurefilter[i].id);
//                AzureFilterMonago.id = Iddata._id;
//                AzureFilterMonago.Name = Iddata.Name;
//                AzureFilterMonago.JobField = Iddata.JobField.Name;
//                AzureFilterMonago.JobSubField = Iddata.JobSubField.Name;
//                AzureFilterMonago.Qualification = Iddata.Qualification.Name;
//                AzureFilterMonago.Company = Iddata.Company.Name;
//                AzureFilterMonago.Experience = Iddata.Experience.Name;
//                AzureFilterMonago.Country = Iddata.Country.Name;
//                AzureFilterMonago.City = Iddata.City.Name;
//                filterResult.Add(AzureFilterMonago);
//            }
//        }
//        //filterrs = filterResult.Where(x => JobFieldId.Contains(x.JobField));
//        filter = filterResult;
//        filter = filter.Where(x => x.IsActive == true && x.Status == "Approved").ToList();
//    }
//}
// using System.Collections.Generic;
// public List findNeighbours()
//{
//    List<GameObject> localFish = new List<GameObject>();
//    foreach (GameObject fish in fishSchool)
//    {
//        float distance = Vector3.Distance(transform.position, fish.transform.position);
//        if (distance <= nDistance)
//        {
//            localFish.Add(fish);
//        }
//    }
//    return localFish;
//}

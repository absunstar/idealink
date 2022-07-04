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

using Microsoft.Azure.ServiceBus;

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
        private IAsyncRepository<ReportJob> _repositoryReport;
        public ServiceJob(IDBJob dBJobs
                        , IDBIndustry dBIndustry
                        , IDBYearsOfExperience dBYearsOfExperience
                        , IDBQualification dBQualification
                        , IDBJobFields dBJobFields
                        , IDBCompany dBCompany
                        , IDBCountry dBCountry
                        , IDBApply dBApply
                        , IUserProfile BLUserProfile
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
            _dBFavourite = dBFavourite;
            _repositoryReport = repositoryReport;
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

            var filter = Builders<Job>.Filter.Where(x=>x.Status == Enum.EnumJobStatus.Published
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
            var res =  await base.CreateReturnId(obj);
            await CreateReportJob(obj);
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
        public async Task<MongoResultPaged<JobSearch>> Search(string CompanyId, string filterText, List<string> ExperienceId, List<int> GenderId, List<string> QualificationId, List<string> IndustryId, List<string> JobFieldId, string CountryId, string CityId, int pageNumber = 1, int PageSize = 15)
        {
            //start aj05072021
            
            string data= GetDataIndex();
            var json = JObject.Parse((data));
            string data1 = json["value"].ToString();
            IEnumerable<JobSearch> filter = getAllJob(data1);
            
            // var filter = Builders<Job>.Filter.Where(x => x.IsActive == true && x.Status == EnumJobStatus.Approved);
             filter = filter.Where(x => x.IsActive == true && x.Status == "Approved");
            if (!string.IsNullOrEmpty(filterText))
            {
                //filter = filter & Builders<Job>.Filter.Where(x => x.Name.ToLower().Contains(filterText.ToLower())
                //                                            || x.Company.Name.ToLower().Contains(filterText.ToLower()));
                filter= filter.Where(x => x.Name.ToLower().Contains(filterText.ToLower())
                                                          || x.Company.ToLower().Contains(filterText.ToLower()));

            }
            if (!string.IsNullOrEmpty(CompanyId))
            {
                //filter = filter & Builders<Job>.Filter.Where(x => x.Country._id == CountryId);
                filter = filter.Where(x => x.Company == CompanyId);
            }
            if (!string.IsNullOrEmpty(CountryId))
            {
                //filter = filter & Builders<Job>.Filter.Where(x => x.Country._id == CountryId);
                filter = filter.Where(x => x.Country == CountryId);
            }
            if (!string.IsNullOrEmpty(CityId))
            {
                //  filter = filter & Builders<Job>.Filter.Where(x => x.City._id == CityId);
                filter = filter.Where(x => x.City.ToLower() == CityId.ToLower());

            }
            if (ExperienceId != null && ExperienceId.Count > 0)
            {
                // filter = filter & Builders<Job>.Filter.Where(x => ExperienceId.Contains(x.Experience._id));
                filter = filter.Where(x => ExperienceId.Contains(x.Experience));
            }

            if (GenderId != null && GenderId.Count > 0)
            {
                //filter = filter & Builders<Job>.Filter.Where(x => GenderId.Contains(x.Gender) || x.Gender == 3);
                filter = filter.Where(x => GenderId.Contains(x.Gender) || x.Gender == 3);
            }

            if (QualificationId != null && QualificationId.Count > 0)
            {
                //filter = filter & Builders<Job>.Filter.Where(x => QualificationId.Contains(x.Qualification._id));
                filter = filter.Where(x => QualificationId.Contains(x.Qualification));
            }

            if (IndustryId != null && IndustryId.Count > 0)
            {
                //filter = filter & Builders<Job>.Filter.Where(x => IndustryId.Contains(x.Industry._id));
                filter = filter.Where(x => IndustryId.Contains(x.Industry));
            }

            if (JobFieldId != null && JobFieldId.Count > 0)
            {
                //filter = filter & Builders<Job>.Filter.Where(x => JobFieldId.Contains(x.JobField._id));
                filter = filter.Where(x => JobFieldId.Contains(x.JobField));
            }

            //var sort = Builders<Job>.Sort.Descending(x => x.Name);
            var sort = filter.OrderByDescending(x => x.Name);
            var lst = await GetAzureIndexPaged(filter, sort,1,15);
            //var lst = await _dBJobs.GetPaged(filter, sort, pageNumber, PageSize);
            return lst;
        }
        public async Task<MongoResultPaged<JobSearch>> SearchCompany( int pageNumber = 1, int PageSize = 15)
        {


            int initialCapacity = 82765;
            int maxEditDistanceDictionary = 2; //maximum edit distance per dictionary precalculation
            var symSpell = new SymSpell(initialCapacity, maxEditDistanceDictionary);

            //load dictionary
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string dictionaryPath = baseDirectory + "SymSpell/frequency_dictionary_en_82_765.txt";
            int termIndex = 2; //column of the term in the dictionary text file
            int countIndex = 1; //column of the term frequency in the dictionary text file
           

            //lookup suggestions for single-word input strings
            string inputTerm = "marcket";
            int maxEditDistanceLookup = 1; //max edit distance per lookup (maxEditDistanceLookup<=maxEditDistanceDictionary)
            var suggestionVerbosity = SymSpell.Verbosity.Closest; //Top, Closest, All
            var suggestions = symSpell.Lookup(inputTerm, suggestionVerbosity, maxEditDistanceLookup);

            //display suggestions, edit distance and term frequency
            foreach (var suggestion in suggestions)
            {
                Console.WriteLine(suggestion.term + " " + suggestion.distance.ToString() + " " + suggestion.count.ToString("N0"));
            }


            //load bigram dictionary
            //string dictionaryPath = baseDirectory + "../../../../SymSpell/frequency_bigramdictionary_en_243_342.txt";
            //int termIndex = 0; //column of the term in the dictionary text file
            //int countIndex = 2; //column of the term frequency in the dictionary text file
            if (!symSpell.LoadBigramDictionary(dictionaryPath, termIndex, countIndex))
            {
                Console.WriteLine("File not found!");
             }




















            string data = GetDataIndex();
            var json = JObject.Parse((data));
            string data1 = json["value"].ToString();
            IEnumerable<JobSearch> filter = getAllJob(data1);
            filter = filter.Where(x => x.IsActive == true && x.Status == "Approved");
            var sort = filter.OrderByDescending(x => x.Name);
            var lst = await GetAzureIndexPaged(filter, sort, 1, 15);
            //var lst = await _dBJobs.GetPaged(filter, sort, pageNumber, PageSize);
            return lst;
        }
        public async Task<MongoResultPaged<JobSearch>> ForSearchValidation(string filterText, int pageNumber = 1, int PageSize = 15)
        {
            //start aj23072021
            List<JobSearch> filter = new List<JobSearch>();
          
         
            string data = GetDataIndex();
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

            if (!string.IsNullOrEmpty(filterText))
            {
                filterdataname = filterdatacname.Where(x => x.Name.ToLower().Contains(filterText.ToLower()));
                fn=filterdataname.ToList();
                filterdataCompany = filterdatacompany.Where(x => x.Company.ToLower().Contains(filterText.ToLower()));
                fc = filterdataCompany.ToList();
                filterdataJobField = filterdatajob.Where(x => x.JobField.ToLower().Contains(filterText.ToLower()));
                fj = filterdataJobField.ToList();
            }
            List<JobSearch> list = getAllJob(data1);
            List<JobSearch> filterlist = new List<JobSearch>();
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
            var report = await ConvertToReportJob(obj,null);

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
            if(report == null)
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
        public async Task<List<ReportJobCount>> ReportJobCount(string CompanyId, DateTime StartDate, DateTime EndDate, string JobFieldId )
        {
            return await _dBJobs.ReportJobPerCompany(CompanyId, StartDate, EndDate, JobFieldId);
        }


        public string GetDataIndex()
        {
            try
            {
                WebRequest request = WebRequest.Create($"https://cognitivesmartsearch.search.windows.net/indexes/employment3/docs?api-version=2020-06-30-Preview&search=*");
                request.ContentType = "application/json";
                request.Headers["api-key"] = "B30CFCE22E1AFE521B74B0AC005C7C33";
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
      

    }
}

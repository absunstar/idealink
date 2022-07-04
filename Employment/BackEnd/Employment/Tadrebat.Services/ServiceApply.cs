using Employment.Entity.Model;
using Employment.Entity.Mongo;
using Employment.Interface;
using Employment.MongoDB.Interface;
using Employment.Persistance.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Services
{
    public class ServiceApply : ServiceRepository<Apply>, IServiceApply
    {
        private readonly IDBApply _dBApply;
        private readonly IServiceJob _BLJob;
        private readonly IServiceJobSeeker _BLJobSeeker;
        private IAsyncRepository<ReportJobApply> _repositoryReport;
        public ServiceApply(IDBApply dBApply,
                            IServiceJob BLJob,
                            IAsyncRepository<ReportJobApply> repositoryReport,
                            IServiceJobSeeker BLJobSeeker) : base(dBApply)
        {
            _dBApply = dBApply;
            _BLJobSeeker = BLJobSeeker;
            _BLJob = BLJob;
            _repositoryReport = repositoryReport;
        }
        public async override Task<bool> Create(Apply obj)
        {
            var job = await _BLJob.GetById(obj.Job._id);
            if(job != null)
            {
                obj.Job._id = job._id;
                obj.Job.Name = job.Name;
                obj.Job.SubName = job.Company.Name;
                obj.Job.URL = job.Company.URL;
                obj.Job.EntityId = job.Company._id;
            }

            var jobSeeker = await _BLJobSeeker.GetByUserId(obj.JobSeeker._id);
            if(jobSeeker != null)
            {
                obj.JobSeeker._id = obj.JobSeeker._id;
                obj.JobSeeker.Name = jobSeeker.Name;
                obj.JobSeeker.SubName = jobSeeker.JobTitle;
                obj.JobSeeker.URL = jobSeeker.ProfilePicture;
                obj.JobSeeker.EntityId = jobSeeker._id;
            }
            var result = await base.Create(obj);

            if (result)
            {
                //await CreateReportJob(obj.Job._id, obj.JobSeeker._id);
                await CreateReportJob(obj.Job._id, jobSeeker._id);
                return await _BLJob.IncrementApplicantCounter(obj.Job._id);
            }

            return false;
        }
        public async Task<long> CountApplicants(List<string> jobIds)
        {
            var filter = Builders<Apply>.Filter.Where(x => x.IsActive == true && jobIds.Contains(x.Job._id));

            var result = await _dBApply.GetPaged(filter,null,1,int.MaxValue);
            return result.totalCount;
        }
        public async Task<bool> CheckIfApplied(string UserId, string JobId)
        {
            var filter = Builders<Apply>.Filter.Where(x => x.IsActive == true && x.JobSeeker._id == UserId && x.Job._id == JobId);
            
            var obj = await _dBApply.GetOne(filter);
            return obj != null;
        }
        public async Task<MongoResultPaged<Apply>> GetByJobSeekerId(string JobSeekerId,string filterText, int pageNumber = 1, int PageSize = 15)
        {
            var filter = Builders<Apply>.Filter.Where(x => x.IsActive == true && x.JobSeeker._id == JobSeekerId);
            if(!string.IsNullOrEmpty(filterText))
            {
                filter = filter & Builders<Apply>.Filter.Where(x => x.Job.Name.ToLower().Contains(filterText.ToLower()));
            }
            var sort = Builders<Apply>.Sort.Descending(x => x.Name);
            var lst = await _dBApply.GetPaged(filter, sort, pageNumber, PageSize);
            return lst;
        }
        public async Task<MongoResultPaged<Apply>> GetByJoId(string JobId, string filterText, int pageNumber = 1, int PageSize = 15)
        {
            var filter = Builders<Apply>.Filter.Where(x => x.IsActive == true && x.Job._id == JobId);
            if (!string.IsNullOrEmpty(filterText))
            {
                filter = filter & Builders<Apply>.Filter.Where(x => x.JobSeeker.Name.ToLower().Contains(filterText.ToLower()));
            }
            var sort = Builders<Apply>.Sort.Descending(x => x.Name);
            var lst = await _dBApply.GetPaged(filter, sort, pageNumber, PageSize);
            return lst;
        }
        public async Task<bool> CheckMyApply(string UserId, string JobId)
        {
            return await CheckIfApplied(UserId, JobId);
        }
        protected async Task<bool> CreateReportJob(string JobId, string JobSeekerId)
        {
            var report = new ReportJobApply(JobId,JobSeekerId);

            try
            {
                await _repositoryReport.AddAsync(report);
            }
            catch (Exception ex)
            {


            }
            return true;
        }
        public async Task<bool> Hire(string JobSeekerId, string JobId, bool Status)
        {
            var filter = Builders<Apply>.Filter.Where(x => x.JobSeeker._id == JobSeekerId && x.Job._id == JobId);

            var obj = await _dBApply.GetOne(filter);
            if (obj == null)
                return false;

            obj.IsHired = Status;

            await _dBApply.UpdateObj(obj._id, obj);
            return true;
        }
        public async Task<List<ReportApply>> ReportJobSeekerAppliedPerJobCount(DateTime StartDate, DateTime EndDate)
        {
            return await _dBApply.ReportJobSeekerAppliedPerJobCount(StartDate, EndDate);
        }
        public async Task<long> ReportJobSeekerHiredCount(DateTime StartDate, DateTime EndDate)
        {
            var filter = Builders<Apply>.Filter.Where(x => x.IsHired == true);

            if (StartDate != DateTime.MinValue)
            {
                filter = filter & Builders<Apply>.Filter.Where(x => x.CreatedAt >= StartDate);
            }
            if (EndDate != DateTime.MinValue)
            {
                filter = filter & Builders<Apply>.Filter.Where(x => x.CreatedAt <= EndDate);
            }

            var count = await _dBApply.Count(filter);
            return count;
        }
    }
}

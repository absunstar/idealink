using Employment.Entity.Mongo;
using Employment.Interface;
using Employment.MongoDB.Interface;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using Employment.Entity.Model;
using Employment.Persistance.Interfaces;

namespace Employment.Services
{
    public class ServiceCompany : ServiceRepository<Company>, IServiceCompany
    {
        private readonly IDBCompany _dBCompany;
        private readonly IDBIndustry _dbIndustry;
        private readonly IDBCountry _dbCountry;
        private readonly IUserProfile _BLUserProfile;
        private readonly IServiceJob _BLServiceJob;
        private readonly IFile _BLFile;
        private IAsyncRepository<ReportCompany> _repositoryReport;

        public ServiceCompany(IDBCompany dBCompany
                            ,IDBIndustry dBIndustry
                            ,IUserProfile BLUserProfile
                            ,IDBCountry dBCountry,
                            IFile BLFile,
                            IAsyncRepository<ReportCompany> repositoryReport,
                            IServiceJob BLServiceJob) : base(dBCompany)
        {
            _dBCompany = dBCompany;
            _dbCountry = dBCountry;
            _dbIndustry = dBIndustry;
            _BLUserProfile = BLUserProfile;
            _BLFile = BLFile;
            _BLServiceJob = BLServiceJob;
            _repositoryReport = repositoryReport;
        }
        public async override Task<bool> Create(Company obj)
        {
            obj = await UpdateCompanySubItems(obj);
            var userId = obj.UserCanAccess.FirstOrDefault();

            await base.Create(obj);
            await AssignUser(userId, obj._id);

            if (!string.IsNullOrEmpty(obj.CompanyLogo))
            {
                await _BLFile.UploadCompanyLogo(obj._id, obj.CompanyLogo);
                obj.CompanyLogo = obj._id + Path.GetExtension(obj.CompanyLogo);
                await _dBCompany.UpdateObj(obj._id, obj);
            }
            await _BLServiceJob.UpdateCompanyInfo(obj);

            await CreateReportCompany(obj);
            return true;
        }
        public async override Task<bool> Update(Company obj)
        {
            if (obj == null)
                return false;
            
            var company = await GetById(obj._id);

            obj.CreatedAt = company.CreatedAt;
            obj.IsApproved = false;
            obj.IsActive = company.IsActive;
            obj.UserCanAccess = new List<string>();
            obj.UserCanAccess.AddRange(company.UserCanAccess);
            obj = await UpdateCompanySubItems(obj);

            var isUpdateFile = false;
            if (!string.IsNullOrEmpty(obj.CompanyLogo))
            {
                await _BLFile.UploadCompanyLogo(obj._id, obj.CompanyLogo);
                obj.CompanyLogo = obj._id + Path.GetExtension(obj.CompanyLogo);
                isUpdateFile = true;
            }
            else
            {
                obj.CompanyLogo = company.CompanyLogo;
            }
            await _dBCompany.UpdateObj(obj._id, obj);

            
            //check if name changed obj.name != company.name then update other tables
            if(company.Name != obj.Name || isUpdateFile)
            {
                await _BLServiceJob.UpdateCompanyInfo(obj);
            }
            await updateReportCompany(obj);
            return true;
        }
        public async Task<MongoResultPaged<Company>> ListAllByUserId(string filterText, string UserId, int pageNumber = 1, int PageSize = 15)
        {
            var sort = Builders<Company>.Sort.Descending(x => x.CreatedAt);
            var filter = Builders<Company>.Filter.Where(x => x.Name.ToLower().Contains(filterText.ToLower()));

            if (!string.IsNullOrEmpty(UserId))
                filter = filter & Builders<Company>.Filter.Where(x => x.UserCanAccess.Contains(UserId));
            
            var lst = await _dBCompany.GetPaged(filter, sort, pageNumber, PageSize);
            return lst;
        }
        protected async Task<Company> UpdateCompanySubItems(Company obj)
        {
            if (obj == null)
                return obj;

            if(!string.IsNullOrEmpty(obj.Industry._id))
            {
                var industry = await _dbIndustry.GetById(obj.Industry._id);
                obj.Industry.Name = industry.Name;
            }
            if (!string.IsNullOrEmpty(obj.Country._id))
            {
                var country = await _dbCountry.GetById(obj.Country._id);
                obj.Country.Name = country.Name;

                if(!string.IsNullOrEmpty(obj.City._id))
                {
                    var city = country.subItems.Where(x => x._id == obj.City._id).FirstOrDefault();
                    obj.City.Name = city.Name;
                }
            }
            return obj;
        }
        public async Task<bool> AssignUser(string UserId, string CompanyId)
        {
            FieldDefinition<Company> field = "UserCanAccess";
            await _dBCompany.AddField(CompanyId, field, UserId);

            var company = await GetById(CompanyId);
            if(company != null)
                await _BLUserProfile.AssignCompany(UserId, new SubItem(company._id, company.Name));

            return true;
        }
        public async Task<bool> RemoveUser(string CompanyId, string UserId)
        {
            FieldDefinition<Company> field = "UserCanAccess";
            await _dBCompany.RemoveField(CompanyId, field, UserId);
            
            await _BLUserProfile.RemoveCompany(UserId, CompanyId);
            
            return true;
        }

        public async Task<List<UserProfile>> ListCompanyEmployers(string CompanyId)
        {
            if (string.IsNullOrEmpty(CompanyId))
                return new List<UserProfile>();

            var compnay = await GetById(CompanyId);
            if(compnay == null)
                return new List<UserProfile>();

            var lst = await _BLUserProfile.ListByIds(compnay.UserCanAccess);
            return lst;
        }
        public async Task<List<Company>> ListCompany()
        {
            var filter = Builders<Company>.Filter.Where(x => x.IsActive == true && x.IsApproved == true);
            var sort = Builders<Company>.Sort.Ascending(x => x.Name);
            var lst = await _dBCompany.GetPaged(filter, sort, 1, int.MaxValue);
            return lst.lstResult ;
        }
        public async Task<List<Company>> ListAnyCompany()
        {
            var filter = Builders<Company>.Filter.Empty;
            var sort = Builders<Company>.Sort.Ascending(x => x.Name);
            var lst = await _dBCompany.GetPaged(filter, sort, 1, int.MaxValue);
            return lst.lstResult;
        }
        public async Task<MongoResultPaged<Company>> ListAnyCompanyPaged(string filterText, int pageNumber = 1, int PageSize = 15)
        {
            var filter = Builders<Company>.Filter.Where(x => x.Name.ToLower().Contains(filterText.ToLower()));
            var sort = Builders<Company>.Sort.Ascending(x => x.Name);
            var lst = await _dBCompany.GetPaged(filter, sort, pageNumber, PageSize);
            return lst;
        }
        public async Task<List<SubItemActive>> GetUserCompanies(List<SubItemActive> lstSource)
        {
            var lstIds = lstSource.Select(x => x._id);
            var sort = Builders<Company>.Sort.Ascending(x => x.Name);
            var filter = Builders<Company>.Filter.Where(x => lstIds.Contains(x._id) && x.IsActive == true);

            var lst = await _dBCompany.GetPaged(filter, sort, 1, int.MaxValue);
            return lst.lstResult.Select(x=> new SubItemActive() { 
                _id = x._id,
                Name = x.Name,
                IsApproved = x.IsApproved
            }).ToList();
        }
        public async Task<MongoResultPaged<Company>> GetCompanyWaitingApproval(string filterText, int pageNumber = 1, int PageSize = 15)
        {
            var sort = Builders<Company>.Sort.Ascending(x => x.Name);

            var filter = Builders<Company>.Filter.Where(x => x.Name.ToLower().Contains(filterText.ToLower())
                                                    && x.IsApproved == false
                                                    && x.IsActive == true);

            var lst = await _dBCompany.GetPaged(filter, sort, pageNumber, PageSize);
            return lst;
        }
        public async Task<long> GetCompanyWaitingApprovalCount()
        {
            var filter = Builders<Company>.Filter.Where(x =>  x.IsApproved == false
                                                    && x.IsActive == true);

            var count = await _dBCompany.Count(filter);
            return count;
        }
        public async Task<bool> UpdateStatus(string Id, bool status)
        {
            if (string.IsNullOrEmpty(Id))
                return false;

            var obj = await GetById(Id);
            obj.IsApproved= status;
            await _dBCompany.UpdateObj(obj._id, obj);

            return true;
        }

        protected async Task<bool>CreateReportCompany(Company obj)
        {
            var report = await ConvertToReportCompany(obj, null);

            try
            {
                await _repositoryReport.AddAsync(report);
            }
            catch (Exception ex)
            {

                
            }
            return true;
        }
        protected async Task<bool> updateReportCompany(Company obj)
        {
            
            try
            {
                var report = await _repositoryReport.GetQueryableFirstorDefaultAsync(x => x.CompanyId == obj._id);
                report = await ConvertToReportCompany(obj, report);
                await _repositoryReport.UpdateAsync(report);
            }
            catch (Exception ex)
            {


            }
            
            return true;
        }
        protected async Task<ReportCompany> ConvertToReportCompany(Company obj, ReportCompany report)
        {
            if(report == null)
                 report = new ReportCompany();

            report.CompanyId = obj._id;
            report.CreatedAt = obj.CreatedAt;
            report.Name = obj.Name;
            report.Email = obj.Email;
            report.Phone = obj.Phone;
            report.Website = obj.Website;
            report.Establish = obj.Establish;
            report.IndustryId = obj.Industry._id;
            report.IndustryName = obj.Industry.Name;
            report.About = obj.About;
            report.SocialFacebook = obj.SocialFacebook;
            report.SocialTwitter = obj.SocialTwitter;
            report.SocialLinkedin = obj.SocialLinkedin;
            report.SocialGooglePlus = obj.SocialGooglePlus;
            report.CountryId = obj.Country._id;
            report.CountryName = obj.Country.Name;
            report.CityId = obj.City._id;
            report.CityName = obj.City.Name;
            report.Address = obj.Address;

            return report;
        }
        public async Task<long> GetCompanyByUserIdCount(string UserId)
        {
            var filter = Builders<Company>.Filter.Where(x => x.IsApproved == false
                                                   && x.IsActive == true
                                                   && x.UserCanAccess.Contains(UserId));

            var count = await _dBCompany.Count(filter);
            return count;
        }
        public async Task<long> ReportCompanyCount(DateTime StartDate, DateTime EndDate)
        {
            var filter = Builders<Company>.Filter.Empty;

            if (StartDate != DateTime.MinValue)
            {
                filter = filter & Builders<Company>.Filter.Where(x=> x.CreatedAt >= StartDate);
            }
            if (EndDate != DateTime.MinValue)
            {
                filter = filter & Builders<Company>.Filter.Where(x => x.CreatedAt <= EndDate);
            }

            return await _dBCompany.Count(filter);
        }
    }
}

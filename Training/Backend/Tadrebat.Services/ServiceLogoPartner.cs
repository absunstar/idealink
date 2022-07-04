using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;
using Tadrebat.Interface;
using Tadrebat.MongoDB.Interface;

namespace Tadrebat.Services
{
    public class ServiceLogoPartner : ILogoPartner
    {
        private readonly IDBLogoPartner _dBLogoPartner;
        private readonly ICacheConfig _cacheConfig;
        private const string folderName = "LogoPartner";
        public ServiceLogoPartner(IDBLogoPartner dBLogoPartner, ICacheConfig cacheConfig)
        {
            _dBLogoPartner = dBLogoPartner;
            _cacheConfig = cacheConfig;
        }
        public async Task<LogoPartner> LogoPartnerGetById(string Id)
        {
            return await _dBLogoPartner.GetById(Id);
        }
        public async Task<bool> LogoPartnerCreate(IFormFile File, string WebsiteURL)
        {
            var obj = new LogoPartner();
            var strPath = await UploadFile(File, obj._id);

            obj.ImagePath = strPath;
            obj.WebsiteURL = WebsiteURL;
            await _dBLogoPartner.AddAsync(obj);

            return true;
        }
        public async Task<bool> LogoPartnerUpdate(IFormFile File, string WebsiteURL, string Id)
        {
            var obj = await LogoPartnerGetById(Id);
            if (obj == null)
                return false;

            var strPath = await UploadFile(File, obj._id);

            obj.ImagePath = strPath;
            obj.WebsiteURL = WebsiteURL;
            await _dBLogoPartner.UpdateObj(obj._id, obj);

            return true;
        }
        public async Task<bool> LogoPartnerDeActivate(string Id)
        {

            var obj = await LogoPartnerGetById(Id);
            if (obj == null)
                return false;

            await _dBLogoPartner.DeactivateAsync(Id);

            return true;
        }
        public async Task<bool> LogoPartnerActivate(string Id)
        {
            var obj = await LogoPartnerGetById(Id);
            if (obj == null)
                return false;

            await _dBLogoPartner.ActivateAsync(Id);

            return true;
        }
        public async Task<List<LogoPartner>> LogoPartnerListActive()
        {
            var sort = Builders<LogoPartner>.Sort.Descending(x => x.Name);
            var lst = await _dBLogoPartner.ListActive(sort);
            return lst;
        }
        public async Task<MongoResultPaged<LogoPartner>> LogoPartnerListAll(string filterText, int pageNumber = 1, int PageSize = 15)
        {
            try { 
            //var filter = Builders<LogoPartner>.Filter.Where(x => x._id.Contains(filterText)
            //                                                && x.IsActive == true);
            //var sort = Builders<LogoPartner>.Sort.Descending(x => x.CreatedAt);
            var lst = await _dBLogoPartner.ListAll(pageNumber, PageSize);
            return lst;
            }
            catch(Exception ex)
            {
                return new MongoResultPaged<LogoPartner>(0,new List<LogoPartner>(),PageSize);
            }
        }

        public async Task<bool> IsLogoPartnerExist(string Id)
        {
            var obj = await _dBLogoPartner.GetById(Id);
            return obj != null;
        }
        protected async Task<string> UploadFile(IFormFile File, string Id)
        {
            string fileName = Id + Path.GetExtension(File.FileName);

            var pathToSave = Path.Combine(_cacheConfig.UploadFolder, folderName);
            ValidateDirectoryIsExits(pathToSave);

            pathToSave = Path.Combine(pathToSave, fileName);

            if (System.IO.File.Exists(pathToSave))
            {
                System.IO.File.Delete(pathToSave);
            }
            using (var stream = new FileStream(pathToSave, FileMode.Create))
            {
                File.CopyTo(stream);
            }
            return Path.Combine(folderName, fileName);
        }
        private void ValidateDirectoryIsExits(string folderFullPath)
        {
            if (!Directory.Exists(folderFullPath))
            {
                Directory.CreateDirectory(folderFullPath);
            }
        }
    }
}
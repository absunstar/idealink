using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;
using Tadrebat.Enum;
using Tadrebat.Interface;
using Tadrebat.MongoDB.Interface;

namespace Tadrebat.Services
{
    public class ServiceContentData : IContentData
    {
        private readonly IDBContentData _dBContentData;
        private readonly ICacheConfig _cacheConfig;
        public ServiceContentData(IDBContentData dBContentData, ICacheConfig cacheConfig)
        {
            _dBContentData = dBContentData;
            _cacheConfig = cacheConfig;
        }
        public async Task<ContentData> ContentDataOneGetByTypeId(EnumContentData type)
        {
            var filter = Builders<ContentData>.Filter.Where(x => x.Type == type
                                                            && x.IsActive == true);
            var obj = await _dBContentData.GetOne(filter);

            if (obj == null)
            {
                obj = new ContentData();
                obj.Name = "Title";
                obj.Data = "Sample Data";
                obj.Type = type;
                await ContentDataCreate(obj);
            }

            return obj;
        }
        public async Task<ContentData> ContentDataGetById(string Id)
        {
            return await _dBContentData.GetById(Id);
        }
        public async Task<bool> ContentDataCreate(ContentData obj)
        {
            await _dBContentData.AddAsync(obj);

            return true;
        }
        public async Task<bool> ContentDataUpdate(ContentData obj)
        {
            var q = await ContentDataGetById(obj._id);
            if (q == null)
                return false;

            obj.CreatedAt = q.CreatedAt;
            await _dBContentData.UpdateObj(obj._id, obj);

            return true;
        }
        public async Task<bool> ContentDataDeActivate(string Id)
        {

            var obj = await ContentDataGetById(Id);
            if (obj == null)
                return false;

            await _dBContentData.DeactivateAsync(Id);

            return true;
        }
        public async Task<bool> ContentDataActivate(string Id)
        {
            var obj = await ContentDataGetById(Id);
            if (obj == null)
                return false;

            await _dBContentData.ActivateAsync(Id);

            return true;
        }
        public async Task<List<ContentData>> ContentDataListActive()
        {
            var sort = Builders<ContentData>.Sort.Descending(x => x.Name);
            var lst = await _dBContentData.ListActive(sort);
            return lst;
        }
        public async Task<List<ContentData>> ContentDataListActiveByType(EnumContentData type)
        {
            var filter = Builders<ContentData>.Filter.Where(x => x.Type == type
                                                            && x.IsActive == true);
            var sort = Builders<ContentData>.Sort.Descending(x => x.Name);
            var lst = await _dBContentData.GetPaged(filter, null, 1, int.MaxValue);

            return lst.lstResult;
        }
        public async Task<MongoResultPaged<ContentData>> ContentDataListAll(string filterText, int pageNumber = 1, int PageSize = 15)
        {
            //var lst = await _dBContentData.ListAll(pageNumber, PageSize);
            var filter = Builders<ContentData>.Filter.Where(x => x.Name.Contains(filterText)
                                                            && x.IsActive == true);
            var sort = Builders<ContentData>.Sort.Descending(x => x.CreatedAt);
            var lst = await _dBContentData.GetPaged(filter, sort, pageNumber, PageSize);
            return lst;
        }

        public async Task<bool> IsContentDataExist(string Id)
        {
            var obj = await _dBContentData.GetById(Id);
            return obj != null;
        }
        public async Task<bool> UpdateSiteLogo(IFormFile File)
        {
            string folderName = "Logo";
            string fileName = "logo.png";

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
            return true;
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

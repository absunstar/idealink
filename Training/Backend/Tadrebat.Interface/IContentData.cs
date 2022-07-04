using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;
using Tadrebat.Enum;

namespace Tadrebat.Interface
{
    public interface IContentData
    {
        Task<ContentData> ContentDataOneGetByTypeId(EnumContentData Id); 
        Task<ContentData> ContentDataGetById(string Id);
        Task<bool> ContentDataCreate(ContentData obj);
        Task<bool> ContentDataUpdate(ContentData obj);
        Task<bool> ContentDataDeActivate(string Id);
        Task<bool> ContentDataActivate(string Id);
        Task<List<ContentData>> ContentDataListActive();
        Task<List<ContentData>> ContentDataListActiveByType(EnumContentData Type);
        Task<MongoResultPaged<ContentData>> ContentDataListAll(string filterText, int pageNumber = 1, int PageSize = 15);
        Task<bool> IsContentDataExist(string Id);
        Task<bool> UpdateSiteLogo(IFormFile File);
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;

namespace Tadrebat.Interface
{
    public interface ILogoPartner
    {
        Task<LogoPartner> LogoPartnerGetById(string Id);
        Task<bool> LogoPartnerCreate(IFormFile File, string WebsiteURL);
        Task<bool> LogoPartnerUpdate(IFormFile File, string WebsiteURL, string Id);
        Task<bool> LogoPartnerDeActivate(string Id);
        Task<bool> LogoPartnerActivate(string Id);
        Task<List<LogoPartner>> LogoPartnerListActive();
        Task<MongoResultPaged<LogoPartner>> LogoPartnerListAll(string filterText, int pageNumber = 1, int PageSize = 15);
        Task<bool> IsLogoPartnerExist(string Id);
        
    }
}

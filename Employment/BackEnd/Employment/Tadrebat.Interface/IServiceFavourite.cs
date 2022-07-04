using Employment.Entity.Mongo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Interface
{
    public interface IServiceFavourite : IServiceRepository<Favourite>
    {
        Task<bool> AddJob(string UserId, string EntityId);
        Task<bool> AddResume(string UserId, string EntityId);
        Task<List<Favourite>> GetMyFavourite(string UserId);
        Task<bool> CheckMyFavourite(string UserId, string JobId);
        Task<bool> DeActivateByJobId(string UserId, string JobId);
        //Task<bool> DeActivateByProfileId(string UserId, string ProfileId); 
    }
}

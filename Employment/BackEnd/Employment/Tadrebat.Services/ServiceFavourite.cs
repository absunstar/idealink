using Employment.Entity.Mongo;
using Employment.Interface;
using Employment.MongoDB.Interface;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Services
{
    public class ServiceFavourite : ServiceRepository<Favourite>, IServiceFavourite
    {
        private readonly IDBFavourite _dBFavourite;
        private readonly IServiceJob _BLJob;
        private readonly IServiceJobSeeker _BLJobSeeker;
        public ServiceFavourite(IDBFavourite dBFavourite,
                                IServiceJob BLJob,
                                IServiceJobSeeker BLJobSeeker) : base(dBFavourite)
        {
            _dBFavourite = dBFavourite;
            _BLJob = BLJob;
            _BLJobSeeker = BLJobSeeker;
        }
        public async Task<bool> AddJob(string UserId, string EntityId)
        {
            var job = await _BLJob.GetById(EntityId);
            if (job == null)
                return false;

            //if (await CheckIfExist(UserId, EntityId))
            //    return true;

            var filter = Builders<Favourite>.Filter.Where(x => x.UserId == UserId && x.EntityId == EntityId);
            var obj = await _dBFavourite.GetOne(filter);

            if (obj != null)
            {
                if (obj.IsActive.GetValueOrDefault())
                    return true;
                else
                {
                    obj.IsActive = true;
                    await _dBFavourite.UpdateObj(obj._id, obj);
                    return true;
                }
            }

            var fav = new Favourite();

            fav.UserId = UserId;
            fav.EntityId = EntityId;
            fav.Name = job.Name;
            fav.Title = job.Company.Name;
            fav.ImageURL = job.Company.URL;
            fav.CompanyId = job.Company._id;
            fav.Type = Enum.EnumFavouriteType.Job;

            return await base.Create(fav);
        }
        public async Task<bool> AddResume(string UserId, string EntityId)
        {
            var seeker = await _BLJobSeeker.GetById(EntityId);
            if (seeker == null)
                return false;

            var filter = Builders<Favourite>.Filter.Where(x => x.UserId == UserId && x.EntityId == EntityId);
            var obj = await _dBFavourite.GetOne(filter);

            if (obj != null)
            {
                if (obj.IsActive.GetValueOrDefault())
                    return true;
                else
                {
                    obj.IsActive = true;
                    await _dBFavourite.UpdateObj(obj._id, obj);
                    return true;
                }
            }
                

            var fav = new Favourite();

            fav.UserId = UserId;
            fav.EntityId = EntityId;
            fav.Name = seeker.Name;
            fav.Title = seeker.JobTitle;
            fav.ResumeURL = seeker.ResumeFile;
            fav.ImageURL = seeker.ProfilePicture;
            fav.Type = Enum.EnumFavouriteType.JobSeeker;

            return await base.Create(fav);
        }
        public async Task<bool> CheckIfExist(string UserId, string EntityId)
        {
            var filter = Builders<Favourite>.Filter.Where(x => x.UserId == UserId && x.EntityId == EntityId);
            var sort = Builders<Favourite>.Sort.Descending(x => x.CreatedAt);

            var lst = await _dBFavourite.ListActive(filter, sort);
            return lst.Count > 0;
        }
        public async Task<List<Favourite>> GetMyFavourite(string UserId)
        {
            var filter = Builders<Favourite>.Filter.Where(x => x.UserId == UserId);
            var sort = Builders<Favourite>.Sort.Descending(x => x.CreatedAt);

            var lst = await _dBFavourite.ListActive(filter, sort);
            return lst;
        }
        public async Task<bool> CheckMyFavourite(string UserId, string JobId)
        {
            var filter = Builders<Favourite>.Filter.Where(x => x.UserId == UserId && x.EntityId == JobId && x.IsActive == true);
            var sort = Builders<Favourite>.Sort.Descending(x => x.CreatedAt);

            var lst = await _dBFavourite.ListActive(filter, sort);
            return lst.Count > 0;
        }
        public async Task<bool> DeActivateByJobId(string UserId, string JobId)
        {
            try
            {
                var filter = Builders<Favourite>.Filter.Where(x => x.UserId == UserId && x.EntityId == JobId);
                //var sort = Builders<Favourite>.Sort.Descending(x => x.CreatedAt);

                var obj = await _dBFavourite.GetOne(filter);
                if (obj != null)
                {
                    await DeActivate(obj._id);
                }
            }
            catch (Exception ex)

            { }
            return true;
        }
        //public async Task<bool> DeActivateByProfileId(string UserId, string ProfileId)
        //{
        //    try
        //    {
        //        var filter = Builders<Favourite>.Filter.Where(x => x.UserId == UserId && x.EntityId == ProfileId);

        //        var obj = await _dBFavourite.GetOne(filter);
        //        if (obj != null)
        //        {
        //            await DeActivate(obj._id);
        //        }
        //    }
        //    catch (Exception ex)

        //    { }
        //    return true;
        //}
    }
}

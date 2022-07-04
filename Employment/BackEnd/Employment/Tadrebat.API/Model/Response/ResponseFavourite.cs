using Employment.API.Helpers.Files;
using Employment.Entity.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model.Response
{
    public class ResponseFavourite : ResponseBase
    {
        public string Title { get; set; }
        public string EntityId { get; set; }
        public string ImageURL { get; set; }
        public string ResumeURL { get; set; }
        public override void Map(object model)
        {
            var obj = (Favourite)model;
            _id = obj._id;
            Name = obj.Name;
            CreatedAt = obj.CreatedAt;
            Title = obj.Title;
            EntityId = obj.EntityId;
            ImageURL = obj.Type == Enum.EnumFavouriteType.Job ? HelperFiles.GetURLCompanyLogo(obj.CompanyId, obj.ImageURL) : HelperFiles.GetURLJobSeeker(obj.EntityId, obj.ImageURL,Enum.EnumFileType.ProfilePicture);
            ResumeURL = HelperFiles.GetURLJobSeeker(obj.EntityId, obj.ResumeURL, Enum.EnumFileType.Resume);
        }
    }
}

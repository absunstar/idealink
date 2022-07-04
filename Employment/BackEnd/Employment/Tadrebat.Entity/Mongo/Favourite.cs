using Employment.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Entity.Mongo
{
    public class Favourite : MongoEntityNameBase
    {
        public EnumFavouriteType Type { get; set; }
        public string UserId { get; set; }
        public string EntityId { get; set; }
        public string CompanyId { get; set; }
        public string Title { get; set; }
        public string ResumeURL { get; set; }
        public string ImageURL { get; set; }
    }
}

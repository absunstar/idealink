using Employment.Entity.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model.Response
{
    public class ResponseJobFair : ResponseBase
    {
        public DateTime EventDate { get; set; }
        public string Location { get; set; }
        public Boolean IsOnline { get; set; }
        public string Field { get; set; }
        public string ShortDescription { get; set; }
        public Dictionary<string, string> data { get; set; }
        public override void Map(object model)
        {
            var obj = (JobFair)model;
            _id = obj._id;
            Name = obj.Name;
            IsActive = obj.IsActive.GetValueOrDefault();
            CreatedAt = obj.CreatedAt;
            EventDate = obj.EventDate;
            Location = obj.Location;
            ShortDescription = obj.ShortDescription;
            data = obj.data;
            Field = obj.Field;
            IsOnline = obj.IsOnline;
        }
    }
}

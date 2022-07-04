using Employment.API.Helpers.Files;
using Employment.Entity.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model.Response
{
    public class ResponseApply
    {
        public ResponseApply()
        {
            Job = new ResponseApplySubItem();
            JobSeeker = new ResponseApplySubItem();
        }
        public string _id { get; set; }
        public string Message { get; set; }
        public DateTime CraetedAt { get; set; }
        public ResponseApplySubItem Job { get; set; }
        public ResponseApplySubItem JobSeeker { get; set; }
        public bool IsHired { get; set; }
        public void Map(Apply obj)
        {
            _id = obj._id;
            Message = obj.Message;
            CraetedAt = obj.CreatedAt;
            Job = new ResponseApplySubItem(obj.Job, 1);
            JobSeeker = new ResponseApplySubItem(obj.JobSeeker, 2);
            IsHired = obj.IsHired;
        }
    }
    public class ResponseApplySubItem
    {
        public ResponseApplySubItem()
        {

        }
        public ResponseApplySubItem(ApplySubItem obj, int type)
        {
            _id = obj._id;
            Name = obj.Name;
            SubName = obj.SubName;
            URL = type == 1 ? HelperFiles.GetURLCompanyLogo(obj.EntityId, obj.URL) : HelperFiles.GetURLJobSeeker(obj.EntityId, obj.URL, Enum.EnumFileType.ProfilePicture);
        }
        public string _id { get; set; }
        public string Name { get; set; }
        public string SubName { get; set; }
        public string URL { get; set; }

    }
}

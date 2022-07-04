using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Model.Response
{
    public class ResponseUserProfile
    {
        public ResponseUserProfile()
        {
            MyPartnerListIds = new List<ResponseAssignedObj>();
            MySubPartnerListIds = new List<ResponseAssignedObj>();
            //MyAssignedToAccount = new List<ResponseAssignedToAccount>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public int Type { get; set; }
        public string CityId { get; set; }
        public string AreaId { get; set; }
        public string TrainerTrainingDetails { get; set; }
        public DateTime TrainerStartDate { get; set; }
        public DateTime TrainerEndDate { get; set; }
        public List<ResponseAssignedObj> MyPartnerListIds { get; set; }
        public List<ResponseAssignedObj> MySubPartnerListIds { get; set; }
    }
    public class ResponseAssignedObj
    {
        public ResponseAssignedObj(string id, string name)
        {
            Id = id;
            Name = name;
        }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}

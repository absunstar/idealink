using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Model.Response
{
    public class ResponseEntitySubPartner
    {
        public ResponseEntitySubPartner()
        {
            Partners = new List<ResponseEntityPartner>();
            TrainingCenters = new List<ResponseEntityTrainingCenter>();
            MemberSubPartners = new List<ResponseUserProfile>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public List<ResponseEntityPartner> Partners { get; set; }
        public List<ResponseEntityTrainingCenter> TrainingCenters { get; set; }
        public List<ResponseUserProfile> MemberSubPartners { get; set; }


    }
}

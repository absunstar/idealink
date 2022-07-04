using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Model.Response
{
    public class ResponseEntityPartner
    {
        public ResponseEntityPartner()
        {
            Members = new List<ResponseUserProfile>();
            TrainingCenters = new List<ResponseEntityTrainingCenter>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public int MinHours { get; set; }
        public int MaxHours { get; set; }
        public List<ResponseUserProfile> Members { get; set; }
        public List<ResponseEntityTrainingCenter> TrainingCenters { get; set; }
    }
}

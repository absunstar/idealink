using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Model.Model
{
    public class ModelEntitySubPartner
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public List<string> TrainingCenterIds { get; set; }
        public List<string> PartnerIds { get; set; }
    }
}

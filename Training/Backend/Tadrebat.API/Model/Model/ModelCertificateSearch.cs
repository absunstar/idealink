using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Model.Model
{
    public class ModelCertificateSearch : ModelPaged
    {
        public string PartnerId { get; set; }
        public string TrainingTypeId { get; set; }
    }
}

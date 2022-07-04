using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tadrebat.Enum;

namespace Tadrebat.API.Model.Model
{
    public class ModelCertificateUpload
    {
        public EnumCertificateType Type { get; set; }
        public string partnerId { get; set; }
        public string TrainingCenterId { get; set; }
        public string TrainingTypeId { get; set; }
        public string TrainingCategoryId { get; set; }

    }
}

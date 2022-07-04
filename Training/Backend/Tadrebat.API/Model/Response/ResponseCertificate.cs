using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tadrebat.Enum;

namespace Tadrebat.API.Model.Response
{
    public class ResponseCertificate
    {
        public string _id { get; set; }
        public bool IsActive { get; set; }
        public EnumCertificateType Type { get; set; }
        public string PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string TrainingCenterId { get; set; }
        public string TrainingCenterName { get; set; }
        public string TrainingCategoryId { get; set; }
        public string TrainingCategoryName { get; set; }
        public string TrainingTypeId { get; set; }
        public string TrainingTypeName { get; set; }
        public string FileName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Tadrebat.Enum;

namespace Tadrebat.Entity.Mongo
{
    public class Certificate : MongoEntityNameBase
    {
        public EnumCertificateType Type { get; set; }
        public bool IsSystemGeneric { get; set; }
        public bool IsPartnerGeneric { get; set; }
        public string PartnerId { get; set; }
        public string TrainingCenterId { get; set; }
        public string TrainingCategoryId { get; set; }
        public string TrainingTypeId { get; set; }
        public string FileName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Model.Response
{
    public class ResponseUserProfileTrainerCertificate
    {
        public ResponseUserProfileTrainerCertificate()
        {
        }
        public string TrainerId { get; set; }
        public string TrainerName { get; set; }
        public string PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string TrainingCategoryId { get; set; }
        public string TrainingCategoryName { get; set; }
        public string TrainingTypeId { get; set; }
        public string TrainingTypeName { get; set; }
        public int ExamCount { get; set; }
        public string CertificatePath { get; set; }
    }
    public class ResponseTrainerCertificateWithProfile
    {
        public ResponseTrainerCertificateWithProfile()
        {
            lstResult = new List<ResponseUserProfileTrainerCertificate>();
        }
        public string TrainerName { get; set; }
        public List<ResponseUserProfileTrainerCertificate> lstResult { get; set; }
    }
}

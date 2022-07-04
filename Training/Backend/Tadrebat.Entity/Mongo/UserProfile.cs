using System;
using System.Collections.Generic;
using System.Text;

namespace Tadrebat.Entity.Mongo
{
    public class UserProfile : MongoEntityBase
    {
        public UserProfile()
        {
            MyPartnerListIds = new List<string>();
            MySubPartnerListIds = new List<string>();
            MyTrainerCertificates = new List<TrainerTraining>();
        }
        public string Email { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string CityId { get; set; }
        public string AreaId { get; set; }
        public string TrainerTrainingDetails { get; set; }
        public DateTime TrainerStartDate { get; set; }
        public DateTime TrainerEndDate { get; set; }
        public List<string> MyPartnerListIds { get; set; }
        public List<string> MySubPartnerListIds { get; set; }
        public List<TrainerTraining> MyTrainerCertificates { get; set; }
    }
    public class TrainerTraining
    {
        public TrainerTraining()
        {

        }
        public string PartnerId { get; set; }
        public string TrainingCategoryId { get; set; }
        public bool HasCertificate { get; set; }
        public bool IsApproved { get; set; }
        public int ExamCount { get; set; }
        public string CertificatePath { get; set; }
    }
}

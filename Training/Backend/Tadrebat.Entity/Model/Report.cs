using System;
using System.Collections.Generic;
using System.Text;
using Tadrebat.Model.Entity;

namespace Tadrebat.Entity.Model
{
    public class Report : ReportTrainingTrainee
    {
        public Report()
        {
            CreatedAt = DateTime.Now;
        }
        //public long Id { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public string PartnerId { get; set; }
        //public string PartnerName { get; set; }
        //public string SubPartnerId { get; set; }
        //public string SubPartnerName { get; set; }
        //public string TrainingCenterId { get; set; }
        //public string TrainingCenterName { get; set; }
        //public string TrainerId { get; set; }
        //public string TrainerName { get; set; }
        //public string TrainerCityId { get; set; }
        //public string TrainerCityName { get; set; }
        //public string TrainingTypeId { get; set; }
        //public string TrainingTypeName { get; set; }
        //public string TrainingCategoryName { get; set; }
        //public string CityId { get; set; }
        //public string CityName { get; set; }
        //public string AreaId { get; set; }
        //public string AreaName { get; set; }
        //public DateTime StartDate { get; set; }
        //public DateTime EndDate { get; set; }
        //public string TrainingId { get; set; }
        //public string TraineeId { get; set; }
        //public string TraineeName { get; set; }
        //public string Email { get; set; }
        //public string NationalId { get; set; }
        //public string Gender { get; set; }
        public DateTime ExamDate { get; set; }
        public string ExamId { get; set; }
        public int Score { get; set; }
        public string IsPassExam { get; set; }
        //public bool IsOnline { get; set; }
        //public DateTime DOB { get; set; }
        //public DateTime TraineeRegisterDate { get; set; }
        //public long TrainerCount { get; set; }
        //public int IdType { get; set; }
    }
    public class ReportTrainingTrainee : BaseEntity
    {
        public ReportTrainingTrainee()
        {
            CreatedAt = DateTime.Now;
        }
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string SubPartnerId { get; set; }
        public string SubPartnerName { get; set; }
        public string TrainingCenterId { get; set; }
        public string TrainingCenterName { get; set; }
        public string TrainerId { get; set; }
        public string TrainerName { get; set; }
        public string TrainerCityId { get; set; }
        public string TrainerCityName { get; set; }
        public string TrainingTypeId { get; set; }
        public string TrainingTypeName { get; set; }
        public string TrainingCategoryName { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string AreaId { get; set; }
        public string AreaName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TrainingId { get; set; }
        public string TraineeId { get; set; }
        public string TraineeName { get; set; }
        public string Email { get; set; }
        public string NationalId { get; set; }
        public string Gender { get; set; }
        //public DateTime ExamDate { get; set; }
        //public string ExamId { get; set; }
        //public int Score { get; set; }
        //public string IsPassExam { get; set; }
        public bool IsOnline { get; set; }
        public DateTime DOB { get; set; }
        public DateTime TraineeRegisterDate { get; set; }
        public long TrainerCount { get; set; }
        public int IdType { get; set; }
    }
}

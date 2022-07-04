using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tadrebat.Enum;

namespace Tadrebat.API.Model.Response
{
    public class ResponseTraining
    {
        public ResponseTraining()
        {
            days = new List<string>();
            TrainerDetails = new ResponseItemDetails();
        }
        public string Id { get; set; }
        public ResponseItemDetails PartnerId { get; set; }
        public ResponseItemDetails SubPartnerId { get; set; }
        public ResponseItemDetails TrainingCenterId { get; set; }
        public EnumTrainingType Type { get; set; }
        public string TrainerId { get; set; }
        public ResponseItemDetails TrainerDetails { get; set; }
        public long TrainerCount { get; set; }
        public ResponseItemDetails TrainingTypeId { get; set; }
        public ResponseItemDetails TrainingCategoryId { get; set; }
        public string CityId { get; set; }
        public string AreaId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<string> days { get; set; }
        public bool CanEdit { get; set; }
        public bool IsAdminApproved { get; set; }
        public bool IsConfirm1 { get; set; }
        public bool IsConfirm2 { get; set; }
        public List<ResponseSessions> Sessions { get; set; }
        public List<ResponseTraineeInfo> Trainees { get; set; }
        public List<ResponseAttendance> Attendances { get; set; }
        public string ExamTemplateId { get; set; }
        public bool IsOnline { get; set; }
    }
    public class ResponseItemDetails
    {
        public ResponseItemDetails()
        {

        }
        public ResponseItemDetails(string id, string name)
        {
            Id = id;
            Name = name;
        }
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class ResponseSessions
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Day { get; set; }
        public bool IsAttendanceFilled { get; set; }
    }
    public class ResponseTraineeInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string NationalId { get; set; }
        public int Gender { get; set; }
        public bool IsApproved { get; set; }

    }
    public class ResponseAttendance
    {
        public ResponseAttendance()
        {
            Attendances = new List<ResponseAttendanceTrainee>();
        }
        public string SessionId { get; set; }
        public List<ResponseAttendanceTrainee> Attendances { get; set; }
    }
    public class ResponseAttendanceTrainee
    {
        public string TraineeId { get; set; }
        public bool IsAttendant { get; set; }
    }
}

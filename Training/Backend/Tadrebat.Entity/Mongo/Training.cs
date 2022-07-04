using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Tadrebat.Enum;

namespace Tadrebat.Entity.Mongo
{
    public class Training : MongoEntityBase
    {
        public Training()
        {
            days = new List<string>();
            Sessions = new List<Sessions>();
            IsAdminApproved = false;
            IsConfirm1 = false;
            IsConfirm2 = false;
            Trainees = new List<TraineeInfo>();
            Attendances = new List<Attendance>();
        }
        public ItemDetails PartnerId { get; set; }
        public ItemDetails SubPartnerId { get; set; }
        public ItemDetails TrainingCenterId { get; set; }
        public EnumTrainingType Type { get; set; }
        public long TrainerCount { get; set; }
        public string TrainerId { get; set; }
        public string TrainingTypeId { get; set; }
        public string TrainingCategoryId { get; set; }
        public string CityId { get; set; }
        public string AreaId { get; set; }
        public string ExamTemplateId { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime StartDate { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime EndDate { get; set; }

        public List<string> days { get; set; }
        public List<Sessions> Sessions { get; set; }

        public bool IsAdminApproved { get; set; }
        public bool IsConfirm1 { get; set; }
        public bool IsConfirm2 { get; set; }

        public List<TraineeInfo> Trainees { get; set; }
        public List<Attendance> Attendances { get; set; }
        public bool IsOnline { get; set; }

        public bool IsCopiedToReportDB { get; set; }
        //trainees/
        //attendance/
    }
    public class ItemDetails
    {
        public ItemDetails()
        {
            _id = ObjectId.GenerateNewId().ToString();
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string Name { get; set; }
    }
    public class Sessions
    {
        public Sessions()
        {
            _id = ObjectId.GenerateNewId().ToString();
            IsAttendanceFilled = false;
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string Name { get; set; }
        public DateTime Day { get; set; }
        public bool IsAttendanceFilled { get; set; }
    }
    //public class TraineeTraining
    //{
    //    public TraineeTraining()
    //    {
    //        info = new TraineeInfo();
    //    }
    //    public TraineeInfo info { get; set; }
    //}
    public class TraineeInfo
    {
        public TraineeInfo()
        {
            IsApproved = true;
        }
        public TraineeInfo(string id)
        {
            _Id = id;
            IsApproved = true;
        }
        public string _Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string NationalId { get; set; }
        public int Gender { get; set; }
        public bool IsApproved { get; set; }
    }
    public class Attendance
    {
        public Attendance()
        {
            Attendances = new List<AttendanceTrainee>();
        }
        public string SessionId { get; set; }
        public List<AttendanceTrainee> Attendances { get; set; }
    }
    public class AttendanceTrainee
    {
        public string TraineeId { get; set; }
        public bool IsAttendant { get; set; }
    }
}

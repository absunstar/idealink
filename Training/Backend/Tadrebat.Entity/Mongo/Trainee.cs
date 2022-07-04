using System;
using System.Collections.Generic;
using System.Text;

namespace Tadrebat.Entity.Mongo
{
    public class Trainee:MongoEntityBase
    {
        public Trainee()
        {
            myTrainings = new List<TraineeTraining>();
            data = new Dictionary<string, string>();
            IdType = 1; //NationalID
        }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string NationalId { get; set; }
        public int Gender { get; set; }
        public int IdType { get; set; }
        public DateTime DOB { get; set; }
        public List<TraineeTraining> myTrainings { get; set; }
        public Dictionary<string, string> data { get; set; }
    }
    public class TraineeTraining
    {
        public TraineeTraining()
        {

        }
        public TraineeTraining(string trainingId)
        {
            TrainingId = trainingId;
        }
        public string TrainingId { get; set; }
        public bool HasCertificate { get; set; }
        public bool CanExam { get; set; }
        public int ExamCount { get; set; }
        public string CertificatePath { get; set; }
        public string CertificateNumber { get; set; }
    }
    public class TraineeError 
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string NationalId { get; set; }
        public int Gender { get; set; }
        public DateTime DOB { get; set; }
        public string Error { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Model.Response
{
    public class ResponseTrainee
    {
        public ResponseTrainee()
        {
            data = new Dictionary<string, string>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string NationalId { get; set; }
        public int Gender { get; set; }
        public int IdType { get; set; }
        public DateTime DOB { get; set; }
        public Dictionary<string, string> data { get; set; }
    }
    public class ResponseMyTraining
    {
        public ResponseTrainee Profile { get; set; }
        public List<ResponseMyTrainingItems> trainings { get; set; }
    }
    public class ResponseMyTrainingItems
    {
        public string TrainingId { get; set; }
        public string Name { get; set; }
        public bool HasCertificate { get; set; }
        public bool CanExam { get; set; }
        public int ExamCount { get; set; }
        public string CertificatePath { get; set; }
        public string Date { get; set; }
    }
    public class ResponseTraineeCertificates
    {
        public string TrainingName { get; set; }
        public string Date { get; set; }
        public string TrainingDescription { get; set; }
        public string FilePath { get; set; }

    }
}


using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;

namespace Tadrebat.Interface
{
    public enum ExamResult
    {
        TrainingNotOver = 1,
        ExamPassed = 2,
        TrailExceeded = 3,
        AttendanceFailed = 4,
        Success = 5,
        TrainingNotFound = 6,
        NeedAddQuestions = 7,
        Generic = 8
    }
   
    public class ExamResponse
    {
        public ExamResponse()
        {
            questions = new List<QuestionTemplate>();
        }
        public ExamResult result { get; set; }
        public List<QuestionTemplate> questions { get; set; }
        public string ExamId { get; set; }
    }
    public interface IExam
    {
        Task<ExamResponse> TakeExam(string TrainingId, string TraineeId);
        
         Task<bool> SubmitExam(string ExamId, List<QuestionTemplate> questionTemplates);
    }
}

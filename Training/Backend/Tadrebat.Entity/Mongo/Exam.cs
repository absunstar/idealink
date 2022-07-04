using System;
using System.Collections.Generic;
using System.Text;

namespace Tadrebat.Entity.Mongo
{
    public class Exam : MongoEntityBase
    {
        public Exam()
        {
            ExamTemplate = new List<QuestionTemplate>();
        }
        public string TrainingId { get; set; }
        public string TraineeId { get; set; }
        public int Score { get; set; }
        public bool IsPass { get; set; }
        public List<QuestionTemplate> ExamTemplate { get; set; }
    }
    public class QuestionTemplate : Question
    {
        public string SelectedAnswer { get; set; }
    }
    public class AnswerTemplate : Answer
    {
        
    }
}

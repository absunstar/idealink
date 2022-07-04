using System;
using System.Collections.Generic;
using System.Text;

namespace Tadrebat.Entity.Mongo
{
    public class Question : MongoEntityBase
    {
        public Question()
        {
            Answer = new List<Answer>();
        }
        public string Name { get; set; }
        public int Difficulty { get; set; }
        public string TrainingCategoryId { get; set; }
        public string TrainingTypeId { get; set; }
        public List<Answer> Answer {get;set;}
    }
    public class Answer : MongoEntityBase
    {
        public string Name { get; set; }
        public bool IsCorrectAnswer { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Model.Model
{
    public class ModelTrainee
    {
        public ModelTrainee()
        {
            data = new Dictionary<string, string>();
        }
        public string Id { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string NationalId { get; set; }
        public int Gender { get; set; }
        public int IdType { get; set; }
        public DateTime DOB { get; set; }
        public string TrainingId { get; set; }
        public Dictionary<string, string> data { get; set; }
    }
}

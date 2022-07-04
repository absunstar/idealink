using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Tadrebat.Enum;

namespace Tadrebat.API.Model.Model
{
    public class ModelUserProfile
    {
        public string Id { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public EnumUserTypes Type { get; set; }
        public string CityId { get; set; }
        public string AreaId { get; set; }
        public string TrainerTrainingDetails { get; set; }
        public DateTime TrainerStartDate { get; set; }
        public DateTime TrainerEndDate { get; set; }
        public List<string> SelectedPartnerEntityId { get; set; }
        public List<string> SelectedSubPartnerEntityId { get; set; }
    }
}

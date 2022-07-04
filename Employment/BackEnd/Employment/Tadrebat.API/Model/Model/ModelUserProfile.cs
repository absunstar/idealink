using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Employment.Enum;

namespace Employment.API.Model.Model
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
        public string Password { get; set; }
    }
}

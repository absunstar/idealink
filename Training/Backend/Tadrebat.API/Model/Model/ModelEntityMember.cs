using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Model.Model
{
    public class ModelEntityMember
    {
        [Required]
        public string EntityId { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}

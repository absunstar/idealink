using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Model.Model
{
    public class ModelEntitySubEntityIds
    {
        [Required]
        public string MainEntityId { get; set; }
        [Required]
        public string SubEntityId { get; set; }
    }
}

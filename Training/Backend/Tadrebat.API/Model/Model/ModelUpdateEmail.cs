using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tadrebat.API.Model.Model
{
    public class ModelUpdateEmail
    {
        [Required]
        public string EmailOld { get; set; }
        [Required]
        public string EmailNew { get; set; }
    }
}

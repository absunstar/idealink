using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model.Model
{
    public class ModelChangePassword
    {
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
    public class ModelEmail
    {
        public string Email { get; set; }
    }
}

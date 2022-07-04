using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentityServer4.Quickstart.UI
{
    public class ModelSetPassword
    {
        [Required(ErrorMessage = "New password required", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage ="New password and confirm password does not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string ResetToken { get; set; }
        [Required]
        public Guid UserID { get; set; }
        public string tokenConfirm { get; set; }
    }
}
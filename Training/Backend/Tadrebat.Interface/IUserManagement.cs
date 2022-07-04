using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using Tadrebat.Enum;
using Tadrebat.ModelsGlobal;

namespace Tadrebat.Interface
{
    public class ResponseCreateUser
    {
        public ResponseCreateUser(bool succeeded, string error)
        {
            Succeeded = succeeded;
            Error = error;
        }
        public bool Succeeded { get; set; }
        public string Error { get; set; }
    }
    public interface IUserManagement
    {
        Task<string> GenerateUserPasswordToken(Guid UserId);
        Task<bool> IsEmailConfirmed(Guid UserId);
        Task<bool> ForgetPassword(string Email);
        Task<bool> ResetPassword(string Email, string NewPassword, string OldPassword);
        Task<ResponseCreateUser> CreateApplicationUser(ApplicationUser user, EnumUserTypes userType);
        Task<ResponseCreateUser> AddUserToRole(ApplicationUser user, EnumUserTypes userType);
        Task<bool> ConfirmAccount(string Token, Guid UserId);
        Task<IdentityResult> SetPassword(string Password, Guid UserId, string Token);
        Task<bool> ResendVerify(string Email);
    }
}

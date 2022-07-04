using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using Employment.Enum;
using Employment.ModelsGlobal;

namespace Employment.Interface
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
    public enum ResponseForgetPassword
    {
        Success = 1,
        Failed = 2,
        ConfirmResend = 3
    }
    public interface IUserManagement
    {
        Task<bool> ResendVerify(string Email);
        Task<string> GenerateUserPasswordToken(Guid UserId);
        Task<bool> IsEmailConfirmed(Guid UserId);
        Task<ResponseForgetPassword> ForgetPassword(string Email);
        Task<bool> ResetPassword(string Email, string NewPassword, string OldPassword);
        Task<ResponseCreateUser> CreateApplicationUser(ApplicationUser user, EnumUserTypes userType, bool IsGenerateConfirmEmail = true);
        Task<ResponseCreateUser> CreateApplicationUser(ApplicationUser user, EnumUserTypes userType, string Password);
        Task<ResponseCreateUser> AddUserToRole(ApplicationUser user, EnumUserTypes userType);
        Task<bool> ConfirmAccount(string Token, Guid UserId);
        Task<IdentityResult> SetPassword(string Password, Guid UserId, string Token);
        Task<IdentityResult> ChangePassword(string OldPassword, string NewPassword, string Email);
        Task<ResponseCreateUser> UpdateUndefinedUserToRole(ApplicationUser user, EnumUserTypes userType);
    }
}

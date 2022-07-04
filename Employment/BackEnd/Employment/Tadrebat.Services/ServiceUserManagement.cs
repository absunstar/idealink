using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using Employment.Enum;
using Employment.Interface;
using Employment.ModelsGlobal;
using System.Linq;
using System.Web;

namespace Employment.Services
{
    //http://www.binaryintellect.net/articles/6c463905-ed70-4b61-a05d-94083bfbec66.aspx
    public class ServiceUserManagement : IUserManagement
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationEmail _notificationEmail;
        public ServiceUserManagement(UserManager<ApplicationUser> userManager, INotificationEmail notificationEmail)
        {
            _userManager = userManager;
            _notificationEmail = notificationEmail;
        }
        public async Task<bool> IsEmailConfirmed(Guid UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            if (user == null)
                return true;

            return user.EmailConfirmed;
        }
        public async Task<bool> ResendVerify(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
                return false;

            if (user.EmailConfirmed)
                return true;

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = HttpUtility.UrlEncode(code);
            await _notificationEmail.EmailConfirmation(code, user.Id, user.Email);

            return true;
        }
        public async Task<ResponseForgetPassword> ForgetPassword(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
                return ResponseForgetPassword.Failed;

            if (!user.EmailConfirmed)
            {
                //create confirm email
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = HttpUtility.UrlEncode(code);
                await _notificationEmail.EmailConfirmation(code, user.Id, user.Email);

                return ResponseForgetPassword.ConfirmResend;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = HttpUtility.UrlEncode(token);
            await _notificationEmail.EmailForgetPassword(token, user.Id, Email);

            return ResponseForgetPassword.Success;
        }
        public async Task<bool> ResetPassword(string Email, string NewPassword, string OldPassword)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(OldPassword))
                return false;

            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
                return false;

            if (!user.EmailConfirmed)
                return false;

            var result = await _userManager.ChangePasswordAsync(user, OldPassword, NewPassword);
            return result.Succeeded;
        }
        public async Task<string> GenerateUserPasswordToken(Guid UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            if (user == null)
                return "";

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return token;
        }
        public async Task<ResponseCreateUser> CreateApplicationUser(ApplicationUser user, EnumUserTypes userType, string Password)
        {
            //create identity user
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
                return new ResponseCreateUser(result.Succeeded, result.Errors.First().Code.ToString());

            if (!string.IsNullOrEmpty(Password))
            {
                var token = await GenerateUserPasswordToken(new Guid(user.Id));
                await SetPassword(Password, new Guid(user.Id), token);
            }

            //create confirm email
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = HttpUtility.UrlEncode(code);
            await _notificationEmail.EmailConfirmation(code, user.Id, user.Email);

            // add user roles
            return await AddUserToRole(user, userType);

        }
        public async Task<ResponseCreateUser> CreateApplicationUser(ApplicationUser user, EnumUserTypes userType, bool IsGenerateConfirmEmail = true)
        {
            //create identity user
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
                return new ResponseCreateUser(result.Succeeded, result.Errors.First().Code.ToString());

            if (IsGenerateConfirmEmail)
            {
                //create confirm email
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = HttpUtility.UrlEncode(code);

                await _notificationEmail.EmailConfirmation(code, user.Id, user.Email);
            }
            // add user roles
            return await AddUserToRole(user, userType);
        }
        public async Task<ResponseCreateUser> AddUserToRole(ApplicationUser user, EnumUserTypes userType)
        {
            var result = await _userManager.AddToRoleAsync(user, userType.ToString());

            return new ResponseCreateUser(true, "");
        }
        public async Task<ResponseCreateUser>UpdateUndefinedUserToRole(ApplicationUser user, EnumUserTypes userType)
        {
            try { 
            await _userManager.RemoveFromRoleAsync(user, EnumUserTypes.Undefined.ToString());
            }
            catch(Exception ex)
            {
                int x = 0;
            }
            return await AddUserToRole(user, userType);
        }
        public async Task<bool> ConfirmAccount(string Token, Guid UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            var result = await _userManager.ConfirmEmailAsync(user, Token);
            return result.Succeeded;
        }
        public async Task<IdentityResult> SetPassword(string Password, Guid UserId, string token)
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            var result = await _userManager.ResetPasswordAsync(user, token, Password);
            return result;
        }
        public async Task<IdentityResult> ChangePassword(string OldPassword, string NewPassword ,string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var result = await _userManager.ChangePasswordAsync(user, OldPassword, NewPassword);
            return result;
        }
    }
}

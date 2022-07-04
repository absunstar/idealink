using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using Tadrebat.Enum;
using Tadrebat.Interface;
using Tadrebat.ModelsGlobal;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace Tadrebat.Services
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
        public async Task<bool> ForgetPassword(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
                return false;
            
            if (!user.EmailConfirmed) //if user not confirm resend confirm email
            {
                //create confirm email
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = HttpUtility.UrlEncode(code);
                await _notificationEmail.EmailConfirmation(code, user.Id, user.Email);
                return true;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = HttpUtility.UrlEncode(token);
            await _notificationEmail.EmailForgetPassword(token, user.Id, user.Email);
            return true;
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
            if(user == null)
                return "";

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return token;
        }
        public async Task<ResponseCreateUser> CreateApplicationUser(ApplicationUser user, EnumUserTypes userType)
        {
            //create identity user
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
                return new ResponseCreateUser(result.Succeeded, result.Errors.First().Code.ToString());

            //create confirm email
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = HttpUtility.UrlEncode(code);
            await _notificationEmail.EmailConfirmation(code, user.Id, user.Email);

            // add user roles
            return await AddUserToRole(user, userType);
        }
        public async Task<ResponseCreateUser> AddUserToRole(ApplicationUser user, EnumUserTypes userType)
        {
            //switch(userType)
            //{
            //    case EnumUserTypes.Admin:
            //        await _userManager.AddToRoleAsync(user, userType.ToString());
            //        break;
            //    case EnumUserTypes.Partner:
            //        break;
            //    case EnumUserTypes.SubPartner:
            //        break;
            //    case EnumUserTypes.Trainer:
            //        break;
            //    case EnumUserTypes.Trainee:
            //        break;
            //    default:
            //        return false;
            //}
            try
            { 
            var result =  await _userManager.AddToRoleAsync(user, userType.ToString());
            }
            catch(Exception ex)
            {
                int x = 0;
            }
            //if (!result.Succeeded)
            //    return new ResponseCreateUser(result.Succeeded, result.Errors.First().Code.ToString());

            return new ResponseCreateUser(true, "");
        }
        public async Task<bool> ConfirmAccount(string Token, Guid UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            var result = await _userManager.ConfirmEmailAsync(user, Token);
            return result.Succeeded;
        }
        public async Task<IdentityResult> SetPassword(string Password, Guid UserId,string token)
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            //if (! await _userManager.HasPasswordAsync(user))
            {
                var result = await _userManager.ResetPasswordAsync(user, token, Password);
                return result;
            }
            return null;
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
    }
}

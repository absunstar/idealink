using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecuringAngularApps.API.Model;
using Tadrebat.API.Helpers.HTTPCall;
using Tadrebat.API.Model.Model;
using Tadrebat.Enum;
using Tadrebat.Interface;

namespace Tadrebat.API.Controllers
{
    //[Authorize()]
    public class AccountController : BaseController
    {
        private readonly IUserProfile BLServiceUserProfile;
        private readonly ITrainee BLTrainee;
        private readonly HTTPCallSTS HTTPCallSTS;
        public AccountController(IUserProfile _BLServiceUserProfile,
                            HTTPCallSTS _HTTPCallSTS,
                            ITrainee _BLTrainee,
                            IMapper mapper) : base(mapper)
        {
            BLServiceUserProfile = _BLServiceUserProfile;
            HTTPCallSTS = _HTTPCallSTS;
            BLTrainee = _BLTrainee;

            LogWrite("Controller - ");
        }

        public async Task<IActionResult> AuthContext()
        {
            try
            {
                LogWrite("Start -AuthContext");

                LogWrite("setp -2 -" + JwtClaimTypes.Subject);

                var userId = this.User.FindFirstValue(JwtClaimTypes.Subject);
                var userName = "";
                var role = GetUserRole();
                LogWrite("setp -3 -" + role);
                if (role == EnumUserTypes.Trainee)
                {
                    var user = await BLTrainee.GetByEmail(this.User.Identity.Name);
                    userName = user.Name;

                    LogWrite("setp -4 -userName" + userName);
                }
                else
                {
                    var user = await BLServiceUserProfile.UserProfileGetByEmail(this.User.Identity.Name);
                    if (user == null) return NotFound();
                    userName = user.Name;

                    LogWrite("setp -4 -userName" + userName);
                }

                LogWrite("setp -5 -user id" + userId);
                var profile = new UserProfile();
                profile.Id = userId;
                profile.FullName = userName;
                profile.Role = role.ToString(); //((EnumUserTypes)Convert.ToInt32(user.Type)).ToString();

                if (profile == null) return NotFound();

                var context = new AuthContext
                {
                    userProfile = profile,
                    claims = User.Claims.Select(c => new SimpleClaim { Type = c.Type, Value = c.Value }).ToList()
                };

                LogWrite("setp -6 -" );
                return Ok(context);
            }
            catch (Exception ex)
            {
                LogWrite("Exception-" + ex);
                var context = new AuthContext();
                return Ok(context);

            }

        }
        public async Task<IActionResult> ResetPassword(ModelResetPassword model)
        {
            var email = this.GetUserEmail();
            if (string.IsNullOrEmpty(email))
                return BadRequest();
            var result = await HTTPCallSTS.ResetPassword(email, model.NewPassword, model.OldPassword);

            return await FormatResult(result);
        }

        public void LogWrite(string logMessage)
        {
            string m_exePath = string.Empty;
            m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                using (StreamWriter w = new StreamWriter(m_exePath + "\\" + "log.txt", true))
                {
                    Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {

                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :");
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
            }
        }
    }
}
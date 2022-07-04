using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecuringAngularApps.API.Model;
using Employment.API.Helpers.HTTPCall;
using Employment.API.Model.Model;
using Employment.Enum;
using Employment.Interface;
using System.Reflection;
using System.IO;

namespace Employment.API.Controllers
{
    [Authorize()]
    public class AccountController : BaseController
    {
        private readonly IUserProfile BLServiceUserProfile;
        private readonly HTTPCallSTS HTTPCallSTS;
        public AccountController(IUserProfile _BLServiceUserProfile,
                            HTTPCallSTS _HTTPCallSTS,
                            IMapper mapper) : base(mapper)
        {
            BLServiceUserProfile = _BLServiceUserProfile;
            HTTPCallSTS = _HTTPCallSTS;
        }

        public async Task<IActionResult> AuthContext()
        {
            var userId = this.User.FindFirstValue(JwtClaimTypes.Subject);
            var user = await BLServiceUserProfile.UserProfileGetByEmail(this.User.Identity.Name);
            //var user = await BLServiceUserProfile.UserProfileGetByEmail("mahmoud@test.com");

            if (user == null) return NotFound();

            var profile = new UserProfile();
            profile.Id = userId;
            profile.FullName = user.Name;
            profile.Role = ((EnumUserTypes)Convert.ToInt32(user.Type)).ToString();

            if (profile == null) return NotFound();
            var context = new AuthContext
            {
                userProfile = profile,
                claims = User.Claims.Select(c => new SimpleClaim { Type = c.Type, Value = c.Value }).ToList()
            };

            return Ok(context);
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
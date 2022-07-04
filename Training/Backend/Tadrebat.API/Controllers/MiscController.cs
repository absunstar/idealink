using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecuringAngularApps.API.Model;
using Tadrebat.API.Helpers.AutoMapper;
using Tadrebat.API.Model.Model;
using Tadrebat.API.Model.Response;
using Tadrebat.Entity.Mongo;
using Tadrebat.Enum;
using Tadrebat.Interface;
using Tadrebat.Services;

namespace Tadrebat.API.Controllers
{
    [AllowAnonymous]
    public class MiscController : BaseController
    {
        private readonly IUserProfile _BLUserProfile;
        private readonly ITrainee _BLTrainee;
        private readonly ITraining _BLTraining;
        private readonly ServiceEntityManagement _BLEntityManagement;
        private readonly IContentData _BLContentData;
        private readonly INotificationEmail _BLNotificationEmail;
        private readonly IServiceConfigForm _BLServiceConfig;
        private readonly HelperMapperEntity HLMapperEntity;
        private readonly ILogoPartner _BLLogoPartner;
        public MiscController(IMapper mapper,
                        IContentData BLContentData,
                        ServiceEntityManagement BLEntityManagement,
                        HelperMapperEntity _HLMapperEntity,
                        IServiceConfigForm BLServiceConfig,
                        IUserProfile BLUserProfile,
                        ITrainee BLTrainee,
                        ITraining BLTraining,
                        ILogoPartner BLLogoPartner,
                        INotificationEmail BLNotificationEmail) : base(mapper)
        {
            _BLNotificationEmail = BLNotificationEmail;
            _BLContentData = BLContentData;
            _BLEntityManagement = BLEntityManagement;
            HLMapperEntity = _HLMapperEntity;
            _BLServiceConfig = BLServiceConfig;
            _BLUserProfile = BLUserProfile;
            _BLTrainee = BLTrainee;
            _BLTraining = BLTraining;
            _BLLogoPartner = BLLogoPartner;
        }
        public async Task<IActionResult> RequestRegister(ModelRequestRegister model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _BLNotificationEmail.EmailRequestRegister(model.Type, model.PartnerName, model.Name, model.Email, model.Phone, model.Message, model.data);

            return Ok(result);
        }
        public async Task<IActionResult> ContentDataOneGetByTypeId(ModelId model)
        {

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                EnumContentData type;
                System.Enum.TryParse(model.Id, out type);

                var result = await _BLContentData.ContentDataOneGetByTypeId(type);

                var response = _mapper.Map<ContentData, ResponseContentData>(result);


                return Ok(response);
            }
            catch (Exception ex)
            {

                return Ok("");
            }

        }

        [Authorize()]
        //Testing 
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
                    var user = await _BLTrainee.GetByEmail(this.User.Identity.Name);
                    userName = user.Name;

                    LogWrite("setp -4 -userName" + userName);
                }
                else
                {
                    var user = await _BLUserProfile.UserProfileGetByEmail(this.User.Identity.Name);
                    if (user == null) return NotFound();
                    userName = user.Name;

                    LogWrite("setp -4 -userName" + userName);
                }

                LogWrite("setp -5 -user id" + userId);
                var profile = new SecuringAngularApps.API.Model.UserProfile();
                profile.Id = userId;
                profile.FullName = userName;
                profile.Role = role.ToString(); //((EnumUserTypes)Convert.ToInt32(user.Type)).ToString();

                if (profile == null) return NotFound();

                var context = new SecuringAngularApps.API.Model.AuthContext
                {
                    userProfile = profile,
                    claims = User.Claims.Select(c => new SimpleClaim { Type = c.Type, Value = c.Value }).ToList()
                };

                LogWrite("setp -6 -");
                return Ok(context);
            }
            catch (Exception ex)
            {
                LogWrite("Exception-" + ex);
                var context = new AuthContext();
                return Ok(context);

            }

        }
        public async Task<IActionResult> EntityPartnerListAllActiveAnonymous(string query)
        {

            var result = await _BLEntityManagement.EntityPartnerListAllActiveAnonymous();
            var response = await HLMapperEntity.MapPartner(result);

            return Ok(response);
        }
        public async Task<IActionResult> GetByType(ModelConfigFormGet model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (model.type == 0)
                return BadRequest();

            var result = await _BLServiceConfig.GetByType(model.type);

            return Ok(result != null ? result.Form : new List<FieldConfig>());
        }
        public async Task<IActionResult> GetStats()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var response = new ResponseStats();

            response.TraineeCount = await _BLTrainee.GetTraineeCount();
            response.TrainerCount = await _BLUserProfile.GetTrainerCount();
            response.TrainingCount = await _BLTraining.GetTrainingCount();
            return Ok(response);
        }
        public async Task<IActionResult> SetLanguage(ModelLanguage model)
        {
            switch (model.lang)
            {
                case EnumLanguage.English:
                    SetLanguage("en");
                    break;
                case EnumLanguage.Arabic:
                    SetLanguage("ar");
                    break;
                case EnumLanguage.French:
                    SetLanguage("fr");
                    break;
            }
            return Ok();
        }
        public async Task<IActionResult> LogoPartnerListActive()
        {
            var result = await _BLLogoPartner.LogoPartnerListActive();


            return Ok(result);
        }
        protected void SetLanguage(string lang)
        {
            HttpContext.Session.SetString("Lang", lang);
            //currentLang = HttpContext.Session.GetString("Lang");
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
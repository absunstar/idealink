using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tadrebat.API.Helpers.AutoMapper;
using Tadrebat.API.Helpers.HTTPCall;
using Tadrebat.API.Model.Model;
using Tadrebat.API.Model.Response;
using Tadrebat.Entity.Mongo;
using Tadrebat.Interface;
using Tadrebat.Services;

namespace Tadrebat.API.Controllers
{
    //[AllowAnonymous]
    [Authorize(Roles = "Admin,Partner,SubPartner, Trainer")]
    public class AccountManagementController : BaseController
    {
        private readonly HTTPCallSTS HTTPCallSTS;
        private readonly HelperMapperUser HPMapperUser;
        private readonly IUserProfile BLServiceUserProfile;
        private readonly ICertificate BLCertificate;
        private readonly ServiceUpdateEntityConsistency BLServiceUpdateEntityConsistency;
        private readonly ICacheConfig _BLCacheConfig;
        public AccountManagementController(HTTPCallSTS _HTTPCallSTS
                                , IUserProfile _BLServiceUserProfile
                                , HelperMapperUser _HPMapperUser
                                , ServiceUpdateEntityConsistency _BLServiceUpdateEntityConsistency,
                                ICertificate _BLCertificate,
                                ICacheConfig BLCacheConfig
                                , IMapper mapper) : base(mapper)
        {
            HTTPCallSTS = _HTTPCallSTS;
            BLServiceUserProfile = _BLServiceUserProfile;
            HPMapperUser = _HPMapperUser;
            BLServiceUpdateEntityConsistency = _BLServiceUpdateEntityConsistency;
            BLCertificate = _BLCertificate;
            _BLCacheConfig = BLCacheConfig;
        }
        public async Task<IActionResult> GetById(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLServiceUserProfile.UserProfileGetById(model.Id);
            //var response = _mapper.Map<UserProfile, ResponseUserProfile>(result);
            var response = await HPMapperUser.MapUser(result);

            return Ok(response);
        }
        public async Task<IActionResult> ListActive()
        {
            var result = await BLServiceUserProfile.UserProfileListActive();
            //var response = _mapper.Map<List<UserProfile>, List<ResponseUserProfile>>(result);
            var response = await HPMapperUser.MapUser(result);

            return Ok(response);
        }
        public async Task<IActionResult> ListAll(ModelAccountSearch model)
        {
            if (!ModelState.IsValid)
                BadRequest();
            var role = GetUserRole();
            //if admin send empty userid, otherwise send partner userid
            string UserId = role == Enum.EnumUserTypes.Admin ? "" : GetUserId();

            var result = await BLServiceUserProfile.UserProfileListAll(model.filterText, model.filterType, UserId, role, model.CurrentPage, model.PageSize);
            //var response = _mapper.Map<MongoResultPaged<UserProfile>, ResponsePaged<ResponseUserProfile>>(result);
            var response = await HPMapperUser.MapUser(result);

            return Ok(response);
        }
        public async Task<IActionResult> Create(ModelUserProfile model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (!await BLServiceUserProfile.CheckTypePermissionByRole(GetUserRole(), model.Type))
                return BadRequest();

            var user = new UserProfile();
            user.Name = model.Name;
            user.Email = model.Email;
            user.Type = (int)model.Type;
            user.CityId = model.CityId;
            user.AreaId = model.AreaId;
            user.TrainerTrainingDetails = model.TrainerTrainingDetails;
            user.TrainerStartDate = model.TrainerStartDate;
            user.TrainerEndDate = model.TrainerEndDate;
            //user.MyPartnerListIds = model.SelectedPartnerEntityId != null ? model.SelectedPartnerEntityId  : new List<string>();
            //user.MySubPartnerListIds = model.SelectedSubPartnerEntityId != null ? model.SelectedSubPartnerEntityId : new List<string>(); ;
            //craete user
            var result = await BLServiceUserProfile.UserProfileCreate(user);
            if (result)
            {
                var MDUser = await BLServiceUserProfile.UserProfileGetByEmail(user.Email);

                var MyPartnerListIds = model.SelectedPartnerEntityId != null ? model.SelectedPartnerEntityId : new List<string>();
                var MySubPartnerListIds = model.SelectedSubPartnerEntityId != null ? model.SelectedSubPartnerEntityId : new List<string>(); ;
                await BLServiceUpdateEntityConsistency.RegisterAccountToEntity(MDUser._id, model.Type, MyPartnerListIds, MySubPartnerListIds);

                result = await HTTPCallSTS.RegisterUser(model, MDUser._id);
            }
            else
            {
                return BadRequest("Email already exists.");
            }
            return await FormatResult(result);
        }
        public async Task<IActionResult> Update(ModelUserProfile model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (string.IsNullOrEmpty(model.Id))
                return BadRequest();

            if (!await BLServiceUserProfile.CheckTypePermissionByRole(GetUserRole(), model.Type))
                return BadRequest();

            var obj = _mapper.Map<ModelUserProfile, UserProfile>(model);
            obj.MyPartnerListIds = model.SelectedPartnerEntityId;
            obj.MySubPartnerListIds = model.SelectedSubPartnerEntityId;
            var result = await BLServiceUserProfile.UserProfileUpdate(obj);

            return await FormatResult(result);
        }
        public async Task<IActionResult> Activate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLServiceUserProfile.UserProfileActivate(model.Id);

            return await FormatResult(result);
        }
        public async Task<IActionResult> DeActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLServiceUserProfile.UserProfileDeActivate(model.Id);

            return await FormatResult(result);
        }
        public async Task<IActionResult> GetPartnerSearch(ModelPaged model)
        {
            if (!ModelState.IsValid)
                BadRequest();
            model.PageSize = 20;
            var result = await BLServiceUserProfile.GetPartnerSearch(model.filterText, model.CurrentPage, model.PageSize);
            var response = _mapper.Map<MongoResultPaged<UserProfile>, ResponsePaged<ResponseUserProfile>>(result);

            return Ok(response.lstResult);
        }
        public async Task<IActionResult> GetSubPartnerSearch(ModelPaged model)
        {
            if (!ModelState.IsValid)
                BadRequest();
            model.PageSize = 20;
            var result = await BLServiceUserProfile.GetSubPartnerSearch(model.filterText, model.CurrentPage, model.PageSize);
            var response = _mapper.Map<MongoResultPaged<UserProfile>, ResponsePaged<ResponseUserProfile>>(result);

            return Ok(response.lstResult);
        }
        public async Task<IActionResult> GetMyTrainersBySubPartnerId(ModelId model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var UserId = GetUserId();
            var Role = GetUserRole();
            var result = await BLServiceUserProfile.GetMyTrainersBySubPartnerId(UserId, Role, model.Id);
            var response = _mapper.Map<MongoResultPaged<UserProfile>, ResponsePaged<ResponseUserProfile>>(result);

            return Ok(response.lstResult.OrderBy(x => x.Name));
        }
        public async Task<IActionResult> GetMyTrainers()
        {
            if (!ModelState.IsValid)
                BadRequest();

            var UserId = GetUserId();
            var Role = GetUserRole();
            var result = await BLServiceUserProfile.GetMyTrainers(UserId, Role);
            var response = _mapper.Map<MongoResultPaged<UserProfile>, ResponsePaged<ResponseUserProfile>>(result);

            return Ok(response.lstResult);
        }
        public async Task<IActionResult> GetTrainerCertificate(ModelPaged model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var result = await BLServiceUserProfile.GetTrainerCertificate(model.filterText, model.CurrentPage, model.PageSize);
            foreach (var obj in result.lstResult)
            {
                obj.MyTrainerCertificates = obj.MyTrainerCertificates.Where(y => y.ExamCount >= _BLCacheConfig.TrainerExamCountCertificate
                                                                                                    && y.HasCertificate == false
                                                                                                    && y.IsApproved == false).ToList();
            }
            var response = await HPMapperUser.MapUserTrainerApproval(result);

            return Ok(response);
        }

        public async Task<IActionResult> CertificateApprove(ModelTrainerCertificateApproval model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLCertificate.GenerateTrainerCertificate(model.TrainerId, model.PartnerId, model.TrainingCategoryId);

            return await FormatResult(result);
        }
        public async Task<IActionResult> MyTrainerCertificate(ModelId model)
        {
            if (!ModelState.IsValid)
                BadRequest();


            var Role = GetUserRole();
            var UserId = Role == Enum.EnumUserTypes.Trainer ? GetUserId() : model.Id;

            var trainer = await BLServiceUserProfile.UserProfileGetById(UserId);

            if (trainer == null)
                return BadRequest();

            trainer.MyTrainerCertificates = trainer.MyTrainerCertificates.Where(x => x.HasCertificate == true && x.IsApproved == true).ToList();
            var response = new ResponseTrainerCertificateWithProfile();
            response.TrainerName = trainer.Name;
            response.lstResult = await HPMapperUser.MapUserTrainerApproval(trainer);

            return Ok(response);
        }
        public async Task<IActionResult> ResendActivationLink(ModelEmail model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await HTTPCallSTS.ResendActivationLink(model.Email);
            return await FormatResult(result);
        }
        public async Task<IActionResult> ResendPasswordLink(ModelEmail model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await HTTPCallSTS.ResendPasswordLink(model.Email);
            return await FormatResult(result);
        }

        public async Task<IActionResult> UpdateUserEmail(ModelUpdateEmail model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLServiceUserProfile.UpdateUserEmail(model.EmailOld, model.EmailNew);
          
            return await FormatResult(result);

        }
    }
}
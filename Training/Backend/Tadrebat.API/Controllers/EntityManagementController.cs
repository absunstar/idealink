using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tadrebat.API.Helpers.AutoMapper;
using Tadrebat.API.Model.Model;
using Tadrebat.API.Model.Response;
using Tadrebat.Entity.Mongo;
using Tadrebat.Enum;
using Tadrebat.Interface;
using Tadrebat.Services;

namespace Tadrebat.API.Controllers
{
    //[AllowAnonymous]
    [Authorize(Roles = "Admin, Partner,SubPartner, Trainer")]
    public class EntityManagementController : BaseController
    {
        private readonly ServiceEntityManagement BLEntityManagement;
        private readonly ServiceUpdateEntityConsistency BLServiceUpdateEntityConsistency;
        private readonly HelperMapperEntity HLMapperEntity;
        public EntityManagementController(ServiceEntityManagement _BLEntityManagement
                                    ,ServiceUpdateEntityConsistency _BLServiceUpdateEntityConsistency
                                    ,IMapper mapper
                                    ,HelperMapperEntity _HLMapperEntity) : base(mapper)
        {
            BLEntityManagement = _BLEntityManagement;
            HLMapperEntity = _HLMapperEntity;
            BLServiceUpdateEntityConsistency = _BLServiceUpdateEntityConsistency;
        }
        #region Partner
        public async Task<IActionResult> EntityPartnerGetById(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLEntityManagement.PartnerGetById(model.Id);
            var response = await HLMapperEntity.MapPartner(result, addPartnerAccount: true);

            return Ok(response);
        }
        
        public async Task<IActionResult> EntityPartnerListActive(string query)
        {
            var role = GetUserRole();
            //if admin send empty userid, otherwise send partner userid
            string UserId = role == Enum.EnumUserTypes.Admin ? "" : GetUserId();

            var result = await BLEntityManagement.PartnerListActive(UserId, query);
            var response = await HLMapperEntity.MapPartner(result);

            return Ok(response.OrderBy(x=>x.Name));
        }
        
        public async Task<IActionResult> EntityPartnerListAll(ModelPaged model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var role = GetUserRole();
            //if admin send empty userid, otherwise send partner userid
            string UserId = role == Enum.EnumUserTypes.Admin ? "" : GetUserId();

            var result = await BLEntityManagement.PartnerListAll(model.filterText, UserId, model.CurrentPage, model.PageSize );
            var response = await HLMapperEntity.MapPartner(result, addPartnerAccount:true);

            return Ok(response);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EntityPartnerReportListAll(ModelPaged model)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var role = GetUserRole();
            //if admin send empty userid, otherwise send partner userid
            string UserId = role == Enum.EnumUserTypes.Admin ? "" : GetUserId();

            var result = await BLEntityManagement.PartnerListAll(model.filterText, UserId, model.CurrentPage, model.PageSize);
            //var response = await HLMapperEntity.MapPartner(result);
            var response = _mapper.Map<MongoResultPaged<EntityPartner>, ResponsePaged<ResponseEntityPartnerReport>>(result);

            return Ok(response);
        }
        public async Task<IActionResult> EntityPartnerCreate(ModelEntityPartner model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var destination = new EntityPartner();
            var destId = destination._id;

            var obj = _mapper.Map<ModelEntityPartner, EntityPartner>(model, destination);
            destination._id = destId;
            var result = await BLEntityManagement.PartnerCreate(obj);

            var role = GetUserRole();
            //if admin send empty userid, otherwise send partner userid
            if (role == Enum.EnumUserTypes.Partner)
            {
                //incase of partner add him directery to the new partner Entity
                string UserId =  GetUserId();
                await BLServiceUpdateEntityConsistency.AddPartnerAccountToPartnerEntity(UserId, destId);

            }
            return await FormatResult(result);
        }
        public async Task<IActionResult> EntityPartnerUpdate(ModelEntityPartner model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (string.IsNullOrEmpty(model.Id))
                return BadRequest();
            
            var obj = _mapper.Map<ModelEntityPartner, EntityPartner>(model);
            var result = await BLEntityManagement.PartnerUpdate(obj);

            return await FormatResult(result);
        }
        public async Task<IActionResult> EntityPartnerActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLEntityManagement.PartnerActivate(model.Id);

            return await FormatResult(result);
        }
        public async Task<IActionResult> EntityPartnerDeActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLEntityManagement.PartnerDeActivate(model.Id);
            return await FormatResult(result);
        }
        public async Task<IActionResult> EntityPartnerAddMember(ModelEntityMember model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            //var result = await BLEntityManagement.PartnerAddMember(model.EntityId, model.UserId);
            var result = await BLServiceUpdateEntityConsistency.AddPartnerAccountToPartnerEntity(model.UserId,model.EntityId);

            return await FormatResult(result);
        }
        public async Task<IActionResult> EntityPartnerRemoveMember(ModelEntityMember model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            //var result = await BLEntityManagement.PartnerRemoveMember(model.EntityId, model.UserId);
            var result = await BLServiceUpdateEntityConsistency.RemovePartnerAccountToPartnerEntity(model.UserId, model.EntityId);

            return await FormatResult(result);
        }
        public async Task<IActionResult> EntityPartnerGetMy()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userId = GetUserId();
            var role = GetUserRole();            
                
            var result = role == EnumUserTypes.Admin ? await BLEntityManagement.PartnerListActive("") : await BLEntityManagement.EntityPartnerGetMy(userId);
            var response = await HLMapperEntity.MapPartner(result);

            return Ok(response.OrderBy(x => x.Name));
        }

        #endregion
        #region SubPartner
        public async Task<IActionResult> EntitySubPartnerGetById(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var role = GetUserRole();
            //if admin send empty userid, otherwise send partner userid
            string UserId = role == Enum.EnumUserTypes.Admin ? "" : GetUserId();

            var result = await BLEntityManagement.SubPartnerGetById(model.Id, UserId);
            var response = await HLMapperEntity.MapSubParnter(result);

            return Ok(response);
        }
        public async Task<IActionResult> EntitySubPartnerListActive(ModelSubPartnerActiveSearch model)
        {
            var role = GetUserRole();
            //if admin send empty userid, otherwise send partner userid
            string UserId = role == Enum.EnumUserTypes.Admin ? "" : GetUserId();
            if (model.PartnerIds == null)
                model.PartnerIds = new List<string>();

            var result = await BLEntityManagement.SubPartnerListActive(UserId, model.query, model.PartnerIds);
            var response = await HLMapperEntity.MapSubParnter(result);

            return Ok(response.OrderBy(x => x.Name));
        }
        public async Task<IActionResult> EntitySubPartnerListAll(ModelPaged model)
        {
            if (!ModelState.IsValid)
                BadRequest();
            var role = GetUserRole();
            //if admin send empty userid, otherwise send partner userid
            string UserId = role == Enum.EnumUserTypes.Admin ? "" : GetUserId();
            var result = await BLEntityManagement.SubPartnerListAll(model.filterText, UserId, model.CurrentPage, model.PageSize);
            var response = await HLMapperEntity.MapSubParnter(result);

            return Ok(response);
        }
        public async Task<IActionResult> EntitySubPartnerCreate(ModelEntitySubPartner model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            //var destination = new EntityPartner();
            //var destId = destination._id;

            //var obj = _mapper.Map<ModelEntitySubPartner, EntitySubPartner>(model);
            //var result = await BLEntityManagement.SubPartnerCreate(obj);

            var destination = new EntitySubPartner();
            var destId = destination._id;

            var obj = _mapper.Map<ModelEntitySubPartner, EntitySubPartner>(model, destination);
            destination._id = destId;
            var result = await BLEntityManagement.SubPartnerCreate(obj);

            var role = GetUserRole();
            //if admin send empty userid, otherwise send partner userid
            if (role == Enum.EnumUserTypes.Partner)
            {
                //incase of partner add him directery to the new partner Entity
                string UserId = GetUserId();
                result = await BLServiceUpdateEntityConsistency.AddSubPartnerAccountToSubPartnerEntity(UserId, destId);

            }
            return await FormatResult(result);
        }
        public async Task<IActionResult> EntitySubPartnerUpdate(ModelEntitySubPartner model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (string.IsNullOrEmpty(model.Id))
                return BadRequest();

            var obj = _mapper.Map<ModelEntitySubPartner, EntitySubPartner>(model);
            var result = await BLEntityManagement.SubPartnerUpdate(obj);

            return await FormatResult(result);
        }
        public async Task<IActionResult> EntitySubPartnerActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLEntityManagement.SubPartnerActivate(model.Id);

            return await FormatResult(result);
        }
        public async Task<IActionResult> EntitySubPartnerDeActivate(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLEntityManagement.SubPartnerDeActivate(model.Id);

            return await FormatResult(result);
        }
        public async Task<IActionResult> EntitySubPartnerAddMember(ModelEntityMember model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            //var result = await BLEntityManagement.SubPartnerAddMember(model.EntityId, model.UserId);
            var result = await BLServiceUpdateEntityConsistency.AddSubPartnerAccountToSubPartnerEntity(model.UserId, model.EntityId);

            return await FormatResult(result);
        }
        public async Task<IActionResult> EntitySubPartnerRemoveMember(ModelEntityMember model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            //var result = await BLEntityManagement.SubPartnerRemoveMember(model.EntityId, model.UserId);
            var result = await BLServiceUpdateEntityConsistency.RemoveSubPartnerAccountToSubPartnerEntity(model.UserId, model.EntityId);

            return await FormatResult(result);
        }
        public async Task<IActionResult> EntitySubPartnerAddEntityPartner(ModelEntityMember model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            //UserId is Partner Id, EntityId Is subparnterId
            var result = await BLServiceUpdateEntityConsistency.AddSubPartnerEntityToPartnerEntity(model.EntityId, model.UserId);

            return await FormatResult(result);
        }
        public async Task<IActionResult> EntitySubPartnerRemoveEntityPartner(ModelEntityMember model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            //UserId is Partner Id, EntityId Is subparnterId
            var result = await BLServiceUpdateEntityConsistency.RemoveSubPartnerEntityToPartnerEntity(model.EntityId, model.UserId);

            return await FormatResult(result);
        }
        public async Task<IActionResult> EntitySubPartnerGetMyByPartnerId(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var role = GetUserRole();
            var userId = role == EnumUserTypes.Admin ? "" : GetUserId();

            var result = await BLEntityManagement.EntitySubPartnerGetMyByPartnerId(userId, model.Id);
            var response = await HLMapperEntity.MapSubParnter(result);

            return Ok(response.OrderBy(x => x.Name));
        }
        public async Task<IActionResult> EntitySubPartnerGetMy(ModelId model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var role = GetUserRole();
            var userId = role == EnumUserTypes.Admin ? "" : GetUserId();

            var result = await BLEntityManagement.EntitySubPartnerGetMy(userId);
            var response = await HLMapperEntity.MapSubParnter(result);

            return Ok(response);
        }
        #endregion
        #region TrainingCenter
        //public async Task<IActionResult> EntityTrainingCenterGetById(ModelId model)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest();

        //    var result = await BLEntityManagement.TrainingCenterGetById(model.Id);
        //    var response = await HLMapperEntity.MapTrainingCenter(result);

        //    return Ok(response);
        //}
        //public async Task<IActionResult> EntityTrainingCenterListActive()
        //{
        //    var result = await BLEntityManagement.TrainingCenterListActive();
        //    var response = await HLMapperEntity.MapTrainingCenter(result);

        //    return Ok(response);
        //}
        //public async Task<IActionResult> EntityTrainingCenterListAll(ModelPaged model)
        //{
        //    if (!ModelState.IsValid)
        //        BadRequest();

        //    var result = await BLEntityManagement.TrainingCenterListAll(model.filterText, model.CurrentPage, model.PageSize);
        //    var response = await HLMapperEntity.MapTrainingCenter(result);
        //    return Ok(response);
        //}
        public async Task<IActionResult> EntityTrainingCenterCreate(ModelEntityTrainingCenter model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            
            var obj = _mapper.Map<ModelEntityTrainingCenter, EntityTrainingCenter>(model);
            var result = await BLEntityManagement.PartnerAddTrainingCenter(obj, model.PartnerId);

            return await FormatResult(result);
        }
        public async Task<IActionResult> EntityTrainingCenterUpdate(ModelEntityTrainingCenter model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (string.IsNullOrEmpty(model.Id))
                return BadRequest();
            var obj = _mapper.Map<ModelEntityTrainingCenter, EntityTrainingCenter>(model);
            var result = await BLEntityManagement.TrainingCenterUpdate(model.PartnerId, obj);

            return await FormatResult(result);
        }
        public async Task<IActionResult> EntityTrainingCenterActivate(ModelEntitySubEntityIds model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            
            var result = await BLEntityManagement.TrainingCenterActivate(model.MainEntityId,model.SubEntityId);

            return await FormatResult(result);
        }
        public async Task<IActionResult> EntityTrainingCenterDeActivate(ModelEntitySubEntityIds model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLEntityManagement.TrainingCenterDeActivate(model.MainEntityId, model.SubEntityId);

            return await FormatResult(result);
        }
        public async Task<IActionResult> AddEntityTrainingCenterToSubPartner(ModelEntitySubEntityIds model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLServiceUpdateEntityConsistency.AddTrainigCenterEntityToSubPartnerEntity(model.MainEntityId, model.SubEntityId);

            return await FormatResult(result);
        }
        public async Task<IActionResult> RemoveEntityTrainingCenterToSubPartner(ModelEntitySubEntityIds model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLServiceUpdateEntityConsistency.RemoveTrainigCenterEntityToSubPartnerEntity(model.MainEntityId, model.SubEntityId);

            return await FormatResult(result);
        }
        public async Task<IActionResult> AddEntityTrainingCenterToSubPartnerByPartnerID(ModelEntitySubEntityIds model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLServiceUpdateEntityConsistency.AddTrainigCenterEntityToSubPartnerEntityByPartnerId(model.MainEntityId, model.SubEntityId);

            return await FormatResult(result);
        }
        public async Task<IActionResult> RemoveEntityTrainingCenterToSubPartnerByPartnerID(ModelEntitySubEntityIds model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await BLServiceUpdateEntityConsistency.RemoveTrainigCenterEntityToSubPartnerEntityByPartnerId(model.MainEntityId, model.SubEntityId);

            return await FormatResult(result);
        }
        #endregion
    }
}
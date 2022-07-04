using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Interface;
using System.Linq;
using Tadrebat.Enum;

namespace Tadrebat.Services
{
    public class ServiceUpdateEntityConsistency
    {
        private readonly ServiceEntityManagement BLEntityManagement;
        private readonly IUserProfile BLUserProfile;
        public ServiceUpdateEntityConsistency(ServiceEntityManagement _BLEntityManagement
                                , IUserProfile _BLUserProfile)
        {
            BLEntityManagement = _BLEntityManagement;
            BLUserProfile = _BLUserProfile;
        }
        public async Task<bool> RegisterAccountToEntity(string AccountId,EnumUserTypes type, List<string> lstPartnerId, List<string> lstSubPartnerId)
        {
            switch (type)
            {
                case EnumUserTypes.Partner:
                    foreach(var obj in lstPartnerId)
                    {
                        await AddPartnerAccountToPartnerEntity(AccountId, obj);
                    }
                    break;
                case EnumUserTypes.SubPartner:
                case EnumUserTypes.Trainer:
                    foreach (var obj in lstPartnerId)
                    {
                        await AddPartnerAccountToPartnerEntityFromAccount(AccountId, obj);
                    }
                    foreach (var obj in lstSubPartnerId)
                    {
                        await AddSubPartnerAccountToSubPartnerEntity(AccountId, obj);
                    }
                    break;
            }
            return true;
        }
        public async Task<bool> AddPartnerAccountToPartnerEntity(string AccountId, string PartnerId)
        {
            //Add Acount to Entity Partner
            await BLEntityManagement.PartnerAddMember(PartnerId, AccountId);
            //Update Acount with new Partner Entity Id
            await BLUserProfile.AddMyPartnerListIds(AccountId, PartnerId);

            //Get All SubPartner Entity and Assign Account to them
            //get all subpartner that has this partnerId, then add the AccountId (Current UserId) to can access
            //Update userProfile with all these subPartners
            //await BLEntityManagement.SubPartnerAddMemberByPartnerId(PartnerId, AccountId);
            var subPartners = await BLEntityManagement.SubPartnerGetByPartnerId(PartnerId);
            await BLEntityManagement.SubPartnerAddMemberByPartnerId(PartnerId, AccountId);
            await BLUserProfile.AddMySubPartnerListIds(AccountId, subPartners.Select(x=>x._id).ToList());
            return true;
        }

        public async Task<bool> AddPartnerAccountToPartnerEntityFromAccount(string AccountId, string PartnerId)
        {
            //Add Acount to Entity Partner
            await BLEntityManagement.PartnerAddMember(PartnerId, AccountId);
            //Update Acount with new Partner Entity Id
            await BLUserProfile.AddMyPartnerListIds(AccountId, PartnerId);

            //Get All SubPartner Entity and Assign Account to them
            //get all subpartner that has this partnerId, then add the AccountId (Current UserId) to can access
            //Update userProfile with all these subPartners
            //await BLEntityManagement.SubPartnerAddMemberByPartnerId(PartnerId, AccountId);
            //var subPartners = await BLEntityManagement.SubPartnerGetByPartnerId(PartnerId);
            //await BLEntityManagement.SubPartnerAddMemberByPartnerId(PartnerId, AccountId);
            //await BLUserProfile.AddMySubPartnerListIds(AccountId, subPartners.Select(x => x._id).ToList());
            return true;
        }
        public async Task<bool> RemovePartnerAccountToPartnerEntity(string AccountId, string PartnerId)
        {
            // Get All SubPartner Entity and Assign Account to them
            //get all subpartner that has this partnerId, then Remove the AccountId (Current UserId) to can access
            //Update userProfile with all these subPartners
            var subPartners = await BLEntityManagement.SubPartnerGetByPartnerId(PartnerId);
            await BLEntityManagement.SubPartnerRemoveMemberByPartnerId(PartnerId, AccountId);
            await BLUserProfile.RemoveMySubPartnerListIds(AccountId, subPartners.Select(x => x._id).ToList());
            
            //Remove Acount to Entity Partner
            await BLEntityManagement.PartnerRemoveMember(PartnerId, AccountId);
            //Update Acount with removing Partner Entity Id
            await BLUserProfile.RemoveMyPartnerListIds(AccountId, PartnerId);

            return true;
        }
        public async Task<bool> AddSubPartnerEntityToPartnerEntity(string SubPartnerId, string PartnerId)
        {
            //add Partner entity to subpartner entity
            await BLEntityManagement.SubPartnerAddPartner(SubPartnerId, PartnerId);
            //Get all userprofile who have access to this partner, and give them access to subpartner as well
            var partner = await BLEntityManagement.PartnerGetById(PartnerId);

            foreach (var obj in partner.MemberCanAccessIds)
            {
                await AddSubPartnerAccountToSubPartnerEntity(obj, SubPartnerId);
            }
            return true;
        }
        public async Task<bool> RemoveSubPartnerEntityToPartnerEntity(string SubPartnerId, string PartnerId)
        {
            //remove Partner entity to subpartner entity
            await BLEntityManagement.SubPartnerRemovePartner(SubPartnerId, PartnerId);
            //Get all userprofile who have access to this partner, and remove them access to subpartner as well
            var partner = await BLEntityManagement.PartnerGetById(PartnerId);
            //await BLEntityManagement.SubPartnerRemoveMember(SubPartnerId, partner.MemberCanAccessIds);
            foreach (var obj in partner.MemberCanAccessIds)
            {
                await RemoveSubPartnerAccountToSubPartnerEntity(obj, SubPartnerId);
            }
            //remove all training centers
            await RemoveTrainigCenterEntityToSubPartnerEntityByPartnerId(PartnerId, SubPartnerId);
            return true;
        }
        public async Task<bool> AddSubPartnerAccountToSubPartnerEntity(string AccountId, string SubPartnerId)
        {
            //Add Acount to Entity Sub Partner
            await BLEntityManagement.SubPartnerAddMember(SubPartnerId, AccountId);

            //update UserProfile with SubPartnerEntityId
            await BLUserProfile.AddMySubPartnerListIds(AccountId, SubPartnerId);

            //Add all Subpartner Partners to my userProfile
            var subpartner = await BLEntityManagement.SubPartnerGetById(SubPartnerId);
            await BLUserProfile.AddMyPartnerListIds(AccountId, subpartner.PartnerIds.ToList());
            return true;
        }
        public async Task<bool> RemoveSubPartnerAccountToSubPartnerEntity(string AccountId, string SubPartnerId)
        {
            //Add Acount to Entity Partner
            await BLEntityManagement.SubPartnerRemoveMember(SubPartnerId, AccountId);

            //update UserProfile with SubPartnerEntityId
            await BLUserProfile.RemoveMySubPartnerListIds(AccountId, SubPartnerId);

            //Add all Subpartner Partners to my userProfile
            var subpartner = await BLEntityManagement.SubPartnerGetById(SubPartnerId);
            await BLUserProfile.RemoveMyPartnerListIds(AccountId, subpartner.PartnerIds.ToList());
            return true;
        }
        public async Task<bool> AddTrainigCenterEntityToSubPartnerEntity(string TrainingCenterId, string SubPartnerId)
        {
            //Add Acount to Entity Partner
            await BLEntityManagement.SubPartnerAddTrainingCenter(TrainingCenterId, SubPartnerId);
            return true;
        }
        public async Task<bool> RemoveTrainigCenterEntityToSubPartnerEntity(string TrainingCenterId, string SubPartnerId)
        {
            //Add Acount to Entity Partner
            await BLEntityManagement.SubPartnerRemoveTrainingCenter(TrainingCenterId, SubPartnerId);
            return true;
        }
        public async Task<bool> AddTrainigCenterEntityToSubPartnerEntityByPartnerId(string PartnerId, string SubPartnerId)
        {
            //Add Acount to Entity Partner
            await BLEntityManagement.SubPartnerAddPartnerTrainingCenter(SubPartnerId, PartnerId);
            return true;
        }
        public async Task<bool> RemoveTrainigCenterEntityToSubPartnerEntityByPartnerId(string PartnerId, string SubPartnerId)
        {
            //Add Acount to Entity Partner
            await BLEntityManagement.SubPartnerRemovePartnerTrainingCenter(SubPartnerId, PartnerId);
            return true;
        }
    }
}

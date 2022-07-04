using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;

namespace Tadrebat.Interface
{
    public interface IEntityManagement
    {
        #region Partner
        Task<EntityPartner> PartnerGetById(string Id);
        Task<bool> PartnerCreate(EntityPartner obj);
        Task<bool> PartnerUpdate(EntityPartner obj);
        Task<bool> PartnerDeActivate(string Id);
        Task<bool> PartnerActivate(string Id);
        Task<List<EntityPartner>> PartnerListActive(string UserId);
        Task<MongoResultPaged<EntityPartner>> PartnerListAll(string filterText, int pageNumber = 1, int PageSize = 15);
        Task<bool> IsPartnerExist(string Id);
        Task<bool> PartnerAddMember(string ParterId, string UserId);
        Task<bool> PartnerRemoveMember(string ParterId, string UserId);
        #endregion
        #region SubPartner
        Task<EntitySubPartner> SubPartnerGetById(string Id);
        Task<bool> SubPartnerCreate(EntitySubPartner obj);
        Task<bool> SubPartnerUpdate(EntitySubPartner obj);
        Task<bool> SubPartnerDeActivate(string Id);
        Task<bool> SubPartnerActivate(string Id);
        Task<List<EntitySubPartner>> SubPartnerListActive();
        Task<MongoResultPaged<EntitySubPartner>> SubPartnerListAll(string filterText, string UserId, int pageNumber = 1, int PageSize = 15);
        Task<bool> IsSubPartnerExist(string Id);
        Task<bool> SubPartnerAddMember(string ParterId, string UserId);
        Task<bool> SubPartnerRemoveMember(string ParterId, string UserId);
        Task<bool> SubPartnerAddPartner(string SubPartnerId, string PartnerId);
        Task<bool> SubPartnerAddMember(string SubPartnerId, List<string> UserId);
        Task<bool> SubPartnerRemovePartner(string SubPartnerId, string PartnerId);
        #endregion
        #region TrainingCenter
        Task<EntityTrainingCenter> TrainingCenterGetById(string Id);
        Task<bool> TrainingCenterCreate(EntityTrainingCenter obj);
        Task<bool> TrainingCenterUpdate(EntityTrainingCenter obj);
        Task<bool> TrainingCenterDeActivate(string Id);
        Task<bool> TrainingCenterActivate(string Id);
        Task<List<EntityTrainingCenter>> TrainingCenterListActive();
        Task<MongoResultPaged<EntityTrainingCenter>> TrainingCenterListAll(string filterText, int pageNumber = 1, int PageSize = 15);
        Task<bool> IsTrainingCenterExist(string Id);
        #endregion
    }
}

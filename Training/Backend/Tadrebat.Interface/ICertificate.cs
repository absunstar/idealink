using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Entity.Mongo;

namespace Tadrebat.Interface
{
    public interface ICertificate
    {
        Task<Certificate> CertificateGetById(string Id);
        Task<bool> CertificateCreate(Certificate obj);
        Task<bool> CertificateUpdate(Certificate obj);
        Task<bool> CertificateDeActivate(string Id);
        Task<bool> CertificateActivate(string Id);
        Task<List<Certificate>> CertificateListActive();
        Task<MongoResultPaged<Certificate>> CertificateListAll(int pageNumber = 1, int PageSize = 15);
        Task<bool> IsCertificateExist(string Id);
        Task<MongoResultPaged<Certificate>> CertificateListAllByPartnerId(string PartnerId, string TrainingTypeId, int pageNumber = 1, int PageSize = 15);
        Task<MongoResultPaged<Certificate>> CertificateListAllByTrainingCenterId(string PartnerId, string TrainingTypeId, int pageNumber = 1, int PageSize = 15);
        Task<MongoResultPaged<Certificate>> CertificateListAllGenericByPartnerId(string PartnerId, int pageNumber = 1, int PageSize = 15);
        Task<MongoResultPaged<Certificate>> CertificateListAllSystemGeneric(); 
        Task<Certificate> CertificateGetSystemGeneric(Enum.EnumCertificateType type);
        Task<Certificate> CertificateGetPartnerGeneric(string PartnerId, Enum.EnumCertificateType type);
        Task<Certificate> CertificateGetPartnerTrainingCategory(string PartnerId, string TrainingCategoryId, Enum.EnumCertificateType type);
        Task<bool> UpdateSystemGeneric(Enum.EnumCertificateType type, string FileName);
        Task<bool> UpdatePartnerGeneric(Enum.EnumCertificateType type, string PartnerId, string FileName);
        Task<bool> UpdatePartnerTrainingCategory(Enum.EnumCertificateType type, string PartnerId, string TrainingTypeId, string TrainingCategoryId, string FileName);
        Task<bool> UpdateTrainingCenterTrainingCategory(Enum.EnumCertificateType type, string PartnerId, string TrainingCenterId, string TrainingTypeId, string TrainingCategoryId, string FileName);
        Task<string> UploadCertificateFile(IFormFile File, int Type, string PartnerId, string TrainingCategoryId, string TrainingCenterId);
        Task<bool> GenerateTraineePassedExamCertificate(string TraineeId, string  TrainingId);
        Task<bool> GenerateTrainerCertificate(string TrainerId, string PartnerId, string TrainingCategoryId);
    }
}

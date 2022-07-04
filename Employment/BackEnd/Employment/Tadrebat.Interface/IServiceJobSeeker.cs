using Employment.Entity.Mongo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Employment.Interface
{
    public interface IServiceJobSeeker : IServiceRepository<JobSeeker>
    {
        Task<JobSeeker> GetByUserId(string UserId);
        Task<bool> Create(string UserId, string Email, string Name);
        Task<bool> UpdateInfo(string userId, JobSeeker obj);
        Task<bool> UpdateDescription(string userId, string About);
        Task<bool> UpdateProfile(string userId, JobSeeker obj);
        Task<bool> UpdateSocialMedia(string userId, JobSeeker obj);
        Task<bool> AddEducation(string userId, ResumeItem obj);
        Task<bool> UpdateEducation(string userId, ResumeItem obj);
        Task<bool> RemoveEducation(string userId, string EducationId);
        Task<bool> AddWorkExperience(string userId, ResumeItem obj);
        Task<bool> UpdateWorkExperience(string userId, ResumeItem obj);
        Task<bool> RemoveWorkExperience(string userId, string WorkExperienceId);
        Task<bool> AddExtraCurricular(string userId, ResumeItem obj);
        Task<bool> UpdateExtraCurricular(string userId, ResumeItem obj);
        Task<bool> RemoveExtraCurricular(string userId, string WorkExperienceId);
        Task<bool> AddCertification(string userId, ResumeCertification obj);
        Task<bool> UpdateCertification(string userId, ResumeCertification obj);
        Task<bool> RemoveCertification(string userId, string CertificationId);
        Task<MongoResultPaged<JobSeeker>> Search(string filterText, List<string> ExperienceId, List<int> GenderId, List<string> Qualificationid, List<string> LanguageId, string CountryId, string CityId, int pageNumber = 1, int PageSize = 15);
        Task<bool> UploadFIle(string userId, string fileName, Enum.EnumFileType type);
        Task<MongoResultPaged<CanAccessContactInformation>> ContactPermissioGetApprovalList(string UserId, int pageNumber = 1, int PageSize = 15);
        Task<bool> ContactPermissionApprove(string UserId, string EmployerId);
        Task<bool> ContactPermissionReject(string UserId, string EmployerId);
        Task<int?> ContactPermissionHasPermission(string UserId, string EmployerId);
        Task<bool> ContactPermissionRequest(string UserId, string EmployerId, string CompanyId, string CompanyName);

        Task<long> ReportJobSeekerCount(DateTime StartDate, DateTime EndDate);
        Task<(long, long, long)> ReportJobSeekerGenderCount(DateTime StartDate, DateTime EndDate);

        Task<string> CopyBlob(CloudStorageAccount cloudStorageAccount, string ContainerName, string sourceFileName, string NewFileName, string Type);

        Task<bool> DeleteBlob(CloudStorageAccount cloudStorageAccount, string containerName, string blobFileName);
    }
}

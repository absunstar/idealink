using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Interface
{
    public interface IFile
    {
        Task<bool> DeleteCertificate(string UserId, string CertificateId);
        Task<bool> UploadCertificate(string UserId, string CertificateId, string FileName);
        Task<bool> UploadCompanyLogo(string fileId, string fileName);
        Task<bool> UploadJobSeekerFile(string fileId, string fileName, Enum.EnumFileType type);
    }
}

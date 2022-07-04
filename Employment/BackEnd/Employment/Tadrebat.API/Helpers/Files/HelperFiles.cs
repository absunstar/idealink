using Employment.API.Helpers.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Helpers.Files
{
    public static class HelperFiles
    {
        const string FolderCompanyLogo = "CompanyLogo";
        const string FolderCompanyLogoDefault = "Default";
        const string FolderJobSeekerCoverLetter = "CoverLetter";
        const string FolderJobSeekerResume = "Resume";
        const string FolderJobSeekerProfilePicture = "ProfilePicture";
        const string FolderJobSeekerCertificate = "Certificate";

        public static string GetURLCompanyLogo(string Id, string FileName)
        {
            if (string.IsNullOrEmpty(FileName))
                return GetURLCompanyLogoDefault();

            return Flurl.Url.Combine(ConfigConstant.urlCDN, FolderCompanyLogo, Id, FileName, "?", DateTime.Now.ToString("yyyyMMddHHMMss").ToString());
        }
        public static string GetURLCompanyLogoDefault()
        {
            return Flurl.Url.Combine(ConfigConstant.urlCDN, FolderCompanyLogoDefault, "CompanyLogo.jpg", "?", DateTime.Now.ToString("yyyyMMddHHMMss").ToString());
        }
        public static string GetURLJobSeekerDefault()
        {
            return Flurl.Url.Combine(ConfigConstant.urlCDN, FolderCompanyLogoDefault, "Profile.jpg", "?", DateTime.Now.ToString("yyyyMMddHHMMss").ToString());
        }
        public static string GetURLJobSeekerCertificate(string UserId, string CertificateId, string FileName)
        {
            if (string.IsNullOrEmpty(FileName))
                return "";

            return Flurl.Url.Combine(ConfigConstant.urlCDN, FolderJobSeekerCertificate, UserId, CertificateId, FileName, "?", DateTime.Now.ToString("yyyyMMddHHMMss").ToString());
        }
        public static string GetURLJobSeeker(string Id, string FileName, Enum.EnumFileType type)
        {
            switch (type)
            {
                case Enum.EnumFileType.CoverLetter:
                    if (string.IsNullOrEmpty(FileName))
                        return "";

                    return Flurl.Url.Combine(ConfigConstant.urlCDN, FolderJobSeekerCoverLetter, Id, FileName, "?", DateTime.Now.ToString("yyyyMMddHHMMss").ToString());
                case Enum.EnumFileType.Resume:
                    if (string.IsNullOrEmpty(FileName))
                        return "";

                    return Flurl.Url.Combine(ConfigConstant.urlCDN, FolderJobSeekerResume, Id, FileName, "?", DateTime.Now.ToString("yyyyMMddHHMMss").ToString());
                case Enum.EnumFileType.ProfilePicture:
                    if (string.IsNullOrEmpty(FileName))
                        return GetURLJobSeekerDefault();

                    return Flurl.Url.Combine(ConfigConstant.urlCDN, FolderJobSeekerProfilePicture, Id, FileName, "?", DateTime.Now.ToString("yyyyMMddHHMMss").ToString());
            }
            return "";
        }
    }
}

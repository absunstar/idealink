using Employment.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
//using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Employment.Services
{
    public class ServiceFile : IFile
    {
        protected readonly ICacheConfig _BLCacheConfig;
        protected readonly string RootFolder;

        const string FolderCompanyLogo = "CompanyLogo";
        const string FolderJobSeekerCoverLetter = "CoverLetter";
        const string FolderJobSeekerResume = "Resume";
        const string FolderJobSeekerProfilePicture = "ProfilePicture";
        const string FolderJobSeekerCertificate = "Certificate";
        const string fileOriginal = "Orignial";
        readonly int imgQuality = 75;

        public ServiceFile(ICacheConfig cacheConfig)
        {
            _BLCacheConfig = cacheConfig;
            RootFolder = _BLCacheConfig.FileFolderRoot;
            imgQuality = _BLCacheConfig.ImageQuality;
        }
        public async Task<bool> DeleteCertificate(string UserId, string CertificateId)
        {
            string dest = Path.Combine(RootFolder, FolderJobSeekerCertificate, UserId, CertificateId);

            try
            {
                if (Directory.Exists(dest))
                    Directory.Delete(dest, true);
            }
            catch (Exception ex)
            {

                throw;
            }
            
            return true;
        }
        public async Task<bool> UploadCertificate(string UserId, string CertificateId, string FileName)
        {
            return await CopyFiles(CertificateId, FileName, Path.Combine(FolderJobSeekerCertificate, UserId));
        }
        public async Task<bool> UploadCompanyLogo(string fileId, string fileName)
        {
            return await CopyFiles(fileId, fileName, FolderCompanyLogo, true, _BLCacheConfig.CompanyProfileImageSize);
        }
        public async Task<bool> UploadJobSeekerFile(string fileId, string fileName, Enum.EnumFileType type)
        {
            switch (type)
            {
                case Enum.EnumFileType.CoverLetter:
                    return await CopyFiles(fileId, fileName, FolderJobSeekerCoverLetter);
                case Enum.EnumFileType.Resume:
                    return await CopyFiles(fileId, fileName, FolderJobSeekerResume);
                case Enum.EnumFileType.ProfilePicture:
                    return await CopyFiles(fileId, fileName, FolderJobSeekerProfilePicture, true, _BLCacheConfig.JobSeekerProfileImageSize);
            }
            return await CopyFiles(fileId, fileName, FolderCompanyLogo);
        }
        protected async Task<bool> CopyFiles(string fileId, string fileName, string FolderName, bool IsResize = false, int ImageSize = 150)
        {
            string newPath = _BLCacheConfig.FileFolderTemp;
            string source = Path.Combine(newPath, fileName);
            string dest = Path.Combine(RootFolder, FolderName, fileId);
            string newFileName = fileId + Path.GetExtension(fileName);
            if (!File.Exists(source))
                return false;

            if (!Directory.Exists(dest))
                Directory.CreateDirectory(dest);

            if (IsResize)
            {
                var orgFileName = fileOriginal + Path.GetExtension(fileName);
                var destOrg = Path.Combine(dest, orgFileName);

                try { 
                File.Move(source, destOrg, true);
                }
                catch(Exception ex)
                {

                }
                await ImageResize(dest, orgFileName, newFileName, ImageSize);
            }
            else
            {
                dest = Path.Combine(dest, newFileName);
                File.Move(source, dest, true);
            }

            return true;
        }
        //https://devblogs.microsoft.com/dotnet/net-core-image-processing/
        protected async Task<bool> ImageResize(string filePath, string orgFileName, string outFileName, int size)
        {
            var inputPath = Path.Combine(filePath, orgFileName);
            var outputPath = Path.Combine(filePath, outFileName);

            using (FileStream fs = new FileStream(inputPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var image = new Bitmap(Image.FromStream(fs)))
                {
                    int width, height;
                    if (image.Width > image.Height)
                    {
                        width = size;
                        height = Convert.ToInt32(image.Height * size / (double)image.Width);
                    }
                    else
                    {
                        width = Convert.ToInt32(image.Width * size / (double)image.Height);
                        height = size;
                    }
                    var resized = new Bitmap(width, height);

                    using (var graphics = Graphics.FromImage(resized))
                    {
                        graphics.CompositingQuality = CompositingQuality.HighSpeed;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.CompositingMode = CompositingMode.SourceCopy;
                        graphics.DrawImage(image, 0, 0, width, height);

                        if (File.Exists(outputPath))
                            File.Delete(outputPath);

                        using (var output = File.Open(outputPath, FileMode.Create))
                        {
                            var qualityParamId = Encoder.Quality;
                            var encoderParameters = new EncoderParameters(1);
                            encoderParameters.Param[0] = new EncoderParameter(qualityParamId, imgQuality);
                            var codec = ImageCodecInfo.GetImageDecoders()
                                .FirstOrDefault(codec => codec.FormatID == ImageFormat.Jpeg.Guid);

                            resized.Save(output, codec, encoderParameters);
                        }
                    }
                    image.Dispose();
                }
            }
            return true;
        }
    }
}

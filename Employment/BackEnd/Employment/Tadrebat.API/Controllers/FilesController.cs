using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Employment.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Net;

namespace Employment.API.Controllers
{
    public class FilesController : BaseController
    {
        protected readonly ICacheConfig _BLCacheConfig;
        string accessKey = string.Empty;
        string strContainerName = string.Empty;
        string FileUploadOnCloud = "false";
        public FilesController(IMapper mapper,
                                ICacheConfig cacheConfig) : base(mapper)
        {
            _BLCacheConfig = cacheConfig;

        }
        //https://www.npmjs.com/package/ngx-file-drop
        //[AllowAnonymous]
        public ActionResult UploadFile()
        {
            try
            {
                accessKey = _BLCacheConfig.BlobAccessKey;
                strContainerName = _BLCacheConfig.BlobContainerName;
                FileUploadOnCloud = _BLCacheConfig.FileUploadOnCloud;

                var file = Request.Form.Files[0];
                string newPath = _BLCacheConfig.FileFolderTemp;
                var fileExtension = Path.GetExtension(file.FileName);
                string allowExtension = ".jpg,.png,.jpeg,.bmp,.doc,.docx,.pdf";


                if (!allowExtension.Contains(fileExtension.ToLower()))
                {
                    return Ok(file.FileName);
                }

                if (FileUploadOnCloud == "true")
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        file.CopyTo(memoryStream);
                        var fileBytes = memoryStream.ToArray();
                        string base64file = Convert.ToBase64String(fileBytes);
                        byte[] bytes = System.Convert.FromBase64String(base64file);
                        string fileName = GenerateFileName(file.FileName);

                        var fileurl = UploadFileToBlob(fileName, bytes, file.ContentType);

                        return Ok(fileurl);
                    }
                }
                else
                {
                    // upload file on server using 
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string ext = Path.GetExtension(fileName);
                    string newfileName = Guid.NewGuid().ToString() + ext;

                    byte[] bytes;
                    using (var memoryStream = new MemoryStream())
                    {
                        file.CopyTo(memoryStream);
                        var fileBytes = memoryStream.ToArray();
                        string base64file = Convert.ToBase64String(fileBytes);
                        bytes = System.Convert.FromBase64String(base64file);
                    }

                    UploadFileOnServer(bytes, newfileName, newPath);
                    return Ok(newfileName);
                }

                //if (!Directory.Exists(newPath))
                //{
                //	Directory.CreateDirectory(newPath);
                //}
                //if (file.Length > 0)
                //{
                //	string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                //	string ext = Path.GetExtension(fileName);
                //	string newfileName = Guid.NewGuid().ToString() + ext;
                //	string fullPath = Path.Combine(newPath, newfileName);

                //	using (var stream = new FileStream(fullPath, FileMode.Create))
                //	{
                //		file.CopyTo(stream);
                //	}
                //	return Ok(newfileName);
                //}
                //return BadRequest("Cannot upload empty file.");
            }
            catch (System.Exception ex)
            {
                return Ok("Upload Failed: " + ex.Message);
            }
        }

        public string UploadFileToBlob(string strFileName, byte[] fileData, string fileMimeType)
        {
            try
            {

                var _task = Task.Run(() => this.UploadFileToBlobAsync(strFileName, fileData, fileMimeType));
                _task.Wait();
                string fileUrl = _task.Result;
                return fileUrl;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private string GenerateFileName(string fileName)
        {
            string strFileName = string.Empty;
            string[] strName = fileName.Split('.');
            string newfilename = Guid.NewGuid().ToString();
            strFileName = newfilename + "." + strName[strName.Length - 1];
            return strFileName;
        }

        private async Task<string> UploadFileToBlobAsync(string strFileName, byte[] fileData, string fileMimeType)
        {
            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);
                //string fileName = this.GenerateFileName(strFileName);
                // first store file in temp folder after that move those file in actual folders
                string fileName = "tempfolder/" + strFileName;

                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                }

                if (fileName != null && fileData != null)
                {
                    //DateTimeOffset.UtcNow.AddMinutes(10);
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                    cloudBlockBlob.Properties.ContentType = fileMimeType;
                    await cloudBlockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
                    return cloudBlockBlob.Uri.AbsoluteUri;
                }
                return "";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public void UploadFileOnServer(byte[] filebytes, string filename, string newPath)
        {
            try
            {
                //string foldername = @"\\ilnas1\FTPROOT";
                //string foldername = @"\\ILSTAGING_NEW\employmentfiles.idealake.com\";
                //string pathstring = System.IO.Path.Combine(newPath, filename);

                string pathstring = "";
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                pathstring = System.IO.Path.Combine(newPath, filename);
                if (!System.IO.File.Exists(pathstring))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(pathstring))
                    {
                        fs.Write(filebytes, 0, filebytes.Length);
                    }
                }
            }
            catch (Exception x)
            {
                throw x;
            }
        }
    }
}
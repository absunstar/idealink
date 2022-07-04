using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using Employment.Interface;
using System.IO;

namespace Employment.Cache
{
    public class CacheConfig : ICacheConfig
    {
        private IMemoryCache _cache;
        private IConfiguration _config;
        public CacheConfig(IMemoryCache cache, IConfiguration config)
        {
            _cache = cache;
            _config = config;
        }
        public string URLMCT
        {
            get
            {
                var obj = _cache.Get<string>("URLMCT");
                if (obj == null)
                {
                    var val = _config.GetValue<string>("URLMCT");
                    _cache.Set("URLMCT", val);
                    obj = val;
                }

                return obj;
            }
        }
        public string URLTadrebatAPI
        {
            get
            {
                var obj = _cache.Get<string>("URLTadrebatAPI");
                if (obj == null)
                {
                    var val = _config.GetValue<string>("URLTadrebatAPI");
                    _cache.Set("URLTadrebatAPI", val);
                    obj = val;
                }

                return obj;
            }
        }

        public string URLSTS
        {
            get
            {
                var obj = _cache.Get<string>("STSAuthorityURL");
                if (obj == null)
                {
                    var val = _config.GetValue<string>("STSAuthorityURL");
                    _cache.Set("STSAuthorityURL", val);
                    obj = val;
                }

                return obj;
            }
        }
        public string URLAPI
        {
            get
            {
                var obj = _cache.Get<string>("APIURL");
                if (obj == null)
                {
                    var val = _config.GetValue<string>("APIURL");
                    _cache.Set("APIURL", val);
                    obj = val;
                }

                return obj;
            }
        }
        public string URLSPAClient
        {
            get
            {
                var obj = _cache.Get<string>("SPAClientURL");
                if (obj == null)
                {
                    var val = _config.GetValue<string>("SPAClientURL");
                    _cache.Set("SPAClientURL", val);
                    obj = val;
                }

                return obj;
            }
        }
        public string URLCDN
        {
            get
            {
                var obj = _cache.Get<string>("FilesCDN");
                if (obj == null)
                {
                    var val = _config.GetValue<string>("FilesCDN");
                    _cache.Set("FilesCDN", val);
                    obj = val;
                }

                return obj;
            }
        }
        public string AccountConfirmationURL
        {
            get
            {
                var obj = _cache.Get<string>("AccountConfirmationURL");
                if (obj == null)
                {
                    var val = _config.GetValue<string>("AccountConfirmationURL");
                    _cache.Set("AccountConfirmationURL", val);
                    obj = val;
                }

                return obj;
            }
        }
        public int PageSize
        {
            get
            {
                var obj = _cache.Get<int>("PageSize");
                if (obj == 0)
                {
                    var val = _config.GetValue<int>("PageSize");
                    if (val == 0)
                        val = 15;

                    _cache.Set("PageSize", val);
                    obj = val;
                }

                return obj;
            }
        }
        public string FileFolderRoot
        {
            get
            {
                var obj = _cache.Get<string>("FileFolder");
                if (string.IsNullOrEmpty(obj))
                {
                    var val = _config.GetValue<string>("FileFolder");

                    _cache.Set("FileFolder", val);
                    obj = val;
                }

                return obj;
            }
        }
        public string FileFolderTemp
        {
            get
            {
                return Path.Combine(FileFolderRoot, "_Temp");
            }
        }
        public int JobSeekerProfileImageSize
        {
            get
            {
                var obj = _cache.Get<int>("JobSeekerProfileImageSize");
                if (obj == 0)
                {
                    var val = _config.GetValue<int>("JobSeekerProfileImageSize");

                    _cache.Set("JobSeekerProfileImageSize", val);
                    obj = val;
                }

                return obj;
            }
        }
        public int CompanyProfileImageSize
        {
            get
            {
                var obj = _cache.Get<int>("CompanyProfileImageSize");
                if (obj == 0)
                {
                    var val = _config.GetValue<int>("CompanyProfileImageSize");

                    _cache.Set("CompanyProfileImageSize", val);
                    obj = val;
                }

                return obj;
            }
        }
        public int ImageQuality
        {
            get
            {
                var obj = _cache.Get<int>("ImageQuality");
                if (obj == 0)
                {
                    var val = _config.GetValue<int>("ImageQuality");

                    _cache.Set("ImageQuality", val);
                    obj = val;
                }

                return obj;
            }
        }

        public string EmailUserName
        {
            get
            {
                var obj = _cache.Get<string>("EmailUserName");
                if (string.IsNullOrEmpty(obj))
                {
                    var val = _config.GetValue<string>("EmailUserName");
                    if (string.IsNullOrEmpty(val))
                        throw new Exception("No EmailPassword is set");

                    _cache.Set("EmailUserName", val);
                    obj = val;
                }

                return obj;
            }
        }
        public string EmailPassword
        {
            get
            {
                var obj = _cache.Get<string>("EmailPassword");
                if (string.IsNullOrEmpty(obj))
                {
                    var val = _config.GetValue<string>("EmailPassword");
                    if (string.IsNullOrEmpty(val))
                        throw new Exception("No EmailPassword is set");

                    _cache.Set("EmailPassword", val);
                    obj = val;
                }

                return obj;
            }
        }
        public string EmailSMTP
        {
            get
            {
                var obj = _cache.Get<string>("EmailSMTP");
                if (string.IsNullOrEmpty(obj))
                {
                    var val = _config.GetValue<string>("EmailSMTP");
                    if (string.IsNullOrEmpty(val))
                        throw new Exception("No EmailSMTP is set");

                    _cache.Set("EmailSMTP", val);
                    obj = val;
                }

                return obj;
            }
        }
        public int EmailPort
        {
            get
            {
                var obj = _cache.Get<int>("EmailPort");
                if (obj == 0)
                {
                    var val = _config.GetValue<int>("EmailPort");

                    _cache.Set("EmailPort", val);
                    obj = val;
                }

                return obj;
            }
        }

        public string BlobAccessKey
        {
            get
            {
                var obj = _cache.Get<string>("BlobAccessKey");
                if (string.IsNullOrEmpty(obj))
                {
                    var val = _config.GetValue<string>("BlobAccessKey");

                    _cache.Set("BlobAccessKey", val);
                    obj = val;
                }

                return obj;
            }
        }
        public string BlobContainerName
        {
            get
            {
                var obj = _cache.Get<string>("BlobContainerName");
                if (string.IsNullOrEmpty(obj))
                {
                    var val = _config.GetValue<string>("BlobContainerName");

                    _cache.Set("BlobContainerName", val);
                    obj = val;
                }

                return obj;
            }
        }

        public string ServiceName
        {
            get
            {
                var obj = _cache.Get<string>("ServiceName");
                if (string.IsNullOrEmpty(obj))
                {
                    var val = _config.GetValue<string>("ServiceName");

                    _cache.Set("ServiceName", val);
                    obj = val;
                }

                return obj;
            }
        }

        public string IndexName
        {
            get
            {
                var obj = _cache.Get<string>("IndexName");
                if (string.IsNullOrEmpty(obj))
                {
                    var val = _config.GetValue<string>("IndexName");

                    _cache.Set("IndexName", val);
                    obj = val;
                }

                return obj;
            }
        }

        public string AdminApiKey
        {
            get
            {
                var obj = _cache.Get<string>("AdminApiKey");
                if (string.IsNullOrEmpty(obj))
                {
                    var val = _config.GetValue<string>("AdminApiKey");

                    _cache.Set("AdminApiKey", val);
                    obj = val;
                }

                return obj;
            }
        }

        public string FileUploadOnCloud
        {
            get
            {
                var obj = _cache.Get<string>("FileUploadOnCloud");
                if (string.IsNullOrEmpty(obj))
                {
                    var val = _config.GetValue<string>("FileUploadOnCloud");

                    _cache.Set("FileUploadOnCloud", val);
                    obj = val;
                }

                return obj;
            }
        }

    }
}

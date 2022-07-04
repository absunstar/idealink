using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using Tadrebat.Interface;

namespace Tadrebat.Cache
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
        public int AttendancePercent
        {
            get
            {
                var obj = _cache.Get<int>("AttendancePercent");
                if (obj == 0)
                {
                    var val = _config.GetValue<int>("AttendancePercent");
                    //if (val == 0)
                    //    val = 80;

                    _cache.Set("AttendancePercent", val);
                    obj = val;
                }

                return obj;
            }
        }
        public int ExamTrailCount
        {
            get
            {
                var obj = _cache.Get<int>("ExamTrailCount");
                if (obj == 0)
                {
                    var val = _config.GetValue<int>("ExamTrailCount");
                    if (val == 0)
                        val = 2;

                    _cache.Set("ExamTrailCount", val);
                    obj = val;
                }

                return obj;
            }
        }
        public int ExamQuestionCount
        {
            get
            {
                var obj = _cache.Get<int>("ExamQuestionCount");
                if (obj == 0)
                {
                    var val = _config.GetValue<int>("ExamQuestionCount");
                    if (val == 0)
                        val = 15;

                    _cache.Set("ExamQuestionCount", val);
                    obj = val;
                }

                return obj;
            }
        }
        public int ExamPassingPercent
        {
            get
            {
                var obj = _cache.Get<int>("ExamPassingPercent");
                if (obj == 0)
                {
                    var val = _config.GetValue<int>("ExamPassingPercent");
                    if (val == 0)
                        val = 70;

                    _cache.Set("ExamPassingPercent", val);
                    obj = val;
                }

                return obj;
            }
        }
        public string CertificateBaseUrl
        {
            get
            {
                var obj = _cache.Get<string>("CertificateBaseUrl");
                if (string.IsNullOrEmpty(obj))
                {
                    var val = _config.GetValue<string>("CertificateBaseUrl");
                    if (string.IsNullOrEmpty(val))
                        throw new Exception("No Certificate File is set");

                    _cache.Set("CertificateBaseUrl", val);
                    obj = val;
                }

                return obj;
            }
        }
        public int TrainerExamCountCertificate
        {
            get
            {
                var obj = _cache.Get<int>("TrainerExamCountCertificate");
                if (obj == 0)
                {
                    var val = _config.GetValue<int>("TrainerExamCountCertificate");
                    if (val == 0)
                        val = 50;

                    _cache.Set("TrainerExamCountCertificate", val);
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
        public string UploadFolder
        {
            get
            {
                var obj = _cache.Get<string>("UploadFolder");
                if (string.IsNullOrEmpty(obj))
                {
                    var val = _config.GetValue<string>("UploadFolder");
                    if (string.IsNullOrEmpty(val))
                        obj = @"c:\_NRG\Upload";

                    _cache.Set("UploadFolder", val);
                    obj = val;
                }

                return obj;
            }
        }
        public string UploadFolderTemp
        {
            get
            {
                var obj = _cache.Get<string>("UploadFolderTemp");
                if (string.IsNullOrEmpty(obj))
                {
                    var val = _config.GetValue<string>("UploadFolderTemp");
                    if (string.IsNullOrEmpty(val))
                        obj = @"c:\_NRG\Upload";

                    _cache.Set("UploadFolderTemp", val);
                    obj = val;
                }

                return obj;
            }
        }
        public string STSConnection
        {
            get
            {
                var obj = _cache.Get<string>("STSConnection");
                if (obj == null)
                {
                    var val = _config.GetValue<string>("ConnectionStrings:STSConnection");
                    _cache.Set("STSConnection", val);
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

        public string UpoadFileOnCloud
        {
            get
            {
                var obj = _cache.Get<string>("UpoadFileOnCloud");
                if (string.IsNullOrEmpty(obj))
                {
                    var val = _config.GetValue<string>("UpoadFileOnCloud");

                    _cache.Set("UpoadFileOnCloud", val);
                    obj = val;
                }

                return obj;
            }
        }

        public string FilesCDN
        {
            get
            {
                var obj = _cache.Get<string>("FilesCDN");
                if (string.IsNullOrEmpty(obj))
                {
                    var val = _config.GetValue<string>("FilesCDN");

                    _cache.Set("FilesCDN", val);
                    obj = val;
                }

                return obj;
            }
        }
    }
}

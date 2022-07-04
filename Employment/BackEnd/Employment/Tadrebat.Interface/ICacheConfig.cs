using System;
using System.Collections.Generic;
using System.Text;

namespace Employment.Interface
{
    public interface ICacheConfig
    {
        string URLMCT { get; }

        string URLTadrebatAPI { get; }
        string URLSTS { get; }
        string URLAPI { get; }
        string URLSPAClient { get; }
        string URLCDN { get; }
        string AccountConfirmationURL { get; }
        string FileFolderRoot { get; }
        string FileFolderTemp { get; }

        int JobSeekerProfileImageSize { get; }
        int CompanyProfileImageSize { get; }
        int ImageQuality { get; }

        public string EmailUserName { get; }
        public string EmailPassword { get; }
        public string EmailSMTP { get; }
        public int EmailPort { get; }

        public string BlobAccessKey { get; }

        public string BlobContainerName { get; }

        public string ServiceName { get; }

        public string IndexName { get; }

        public string AdminApiKey { get; }

        public string FileUploadOnCloud { get; }

    }
}

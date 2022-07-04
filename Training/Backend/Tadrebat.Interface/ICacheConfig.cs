using System;
using System.Collections.Generic;
using System.Text;

namespace Tadrebat.Interface
{
    public interface ICacheConfig
    {
        string URLSTS { get; }
        string URLAPI { get; }
        string URLSPAClient { get; }
        string AccountConfirmationURL { get; }

        int AttendancePercent { get; }
        int ExamTrailCount { get; }

        int ExamQuestionCount { get; }

        int ExamPassingPercent { get; }
        string CertificateBaseUrl { get; }
        public int TrainerExamCountCertificate { get; }

        public string EmailUserName { get; }
        public string EmailPassword { get; }
        public string EmailSMTP { get; }
        public int EmailPort { get; }

        public string UploadFolder { get; }
        public string UploadFolderTemp { get; }
        public string STSConnection { get; }

        public string BlobAccessKey { get; }
        public string BlobContainerName { get; }

        public string UpoadFileOnCloud { get; }

        public string FilesCDN { get; }

    }
}

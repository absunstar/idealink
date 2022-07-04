using Employment.API.Helpers.Files;
using Employment.Entity.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model.Response
{
    public class ResponseJobSeeker : ResponseBase
    {
        public ResponseJobSeeker()
        {
            Experience = new ResponseSubItem();
            Qualification = new ResponseSubItem();
            Country = new ResponseSubItem();
            City = new ResponseSubItem();
            Languages = new List<ResponseSubItem>();
            Education = new List<ResponseResumeItem>();
            WorkHistory = new List<ResponseResumeItem>();
            ExtraCurricular = new List<ResponseResumeItem>();
            Certification = new List<ResponseResumeCertification>();
        }
        //public string UserId { get; set; }
        public bool IsMyResume { get; set; }
        public string JobTitle { get; set; }
        public int Gender { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public ResponseSubItem Experience { get; set; }
        public ResponseSubItem Qualification { get; set; }
        public string About { get; set; }
        public string SocialFacebook { get; set; }
        public string SocialTwitter { get; set; }
        public string SocialLinkedin { get; set; }
        public string SocialGooglePlus { get; set; }
        public List<ResponseSubItem> Languages { get; set; }
        public ResponseSubItem Country { get; set; }
        public ResponseSubItem City { get; set; }
        public List<ResponseResumeItem> Education { get; set; }
        public List<ResponseResumeItem> WorkHistory { get; set; }
        public List<ResponseResumeItem> ExtraCurricular { get; set; }
        public List<ResponseResumeCertification> Certification { get; set; }
        public string CoverLetterFile { get; set; }
        public string ResumeFile { get; set; }
        public string ProfilePicture { get; set; }
        public int ContactPermissionHasPermission { get; set; }
        public virtual void Map(object model, int canAccess)
        {
            if (model == null)
                return;
            var obj = (JobSeeker)model;
            ContactPermissionHasPermission = canAccess;
            Name = obj.Name;
            _id = obj._id;
            IsActive = obj.IsActive.GetValueOrDefault();
            //UserId = obj.UserId;
            JobTitle = obj.JobTitle;
            DOB = obj.DOB;
            Gender = obj.Gender;
            Email = obj.Email;
            Website = obj.Website;
            Phone = obj.Phone;
            Experience = new ResponseSubItem(obj.Experience._id, obj.Experience.Name);
            Qualification = new ResponseSubItem(obj.Qualification._id, obj.Qualification.Name);
            About = obj.About;
            SocialFacebook = obj.SocialFacebook;
            SocialGooglePlus = obj.SocialGooglePlus;
            SocialLinkedin = obj.SocialLinkedin;
            SocialGooglePlus = obj.SocialGooglePlus;
            SocialTwitter = obj.SocialTwitter;
            Country = new ResponseSubItem(obj.Country._id, obj.Country.Name);
            City = new ResponseSubItem(obj.City._id, obj.City.Name);
            //SS - save file name in db
            //CoverLetterFile = HelperFiles.GetURLJobSeeker(_id, obj.CoverLetterFile, Enum.EnumFileType.CoverLetter);
            //ResumeFile = HelperFiles.GetURLJobSeeker(_id, obj.ResumeFile, Enum.EnumFileType.Resume);
            //ProfilePicture = HelperFiles.GetURLJobSeeker(_id, obj.ProfilePicture, Enum.EnumFileType.ProfilePicture);
            //SS -Added 
            CoverLetterFile = obj.CoverLetterFile;
            ResumeFile = obj.ResumeFile;
            ProfilePicture = obj.ProfilePicture;
            foreach (var item in obj.Languages)
            {
                Languages.Add(new ResponseSubItem(item._id, item.Name));
            }
            foreach (var item in obj.Education)
            {
                Education.Add(new ResponseResumeItem()
                {
                    _id = item._id,
                    Name = item.Name,
                    Description = item.Description,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    SubTitle = item.SubTitle
                });
            }
            foreach (var item in obj.WorkHistory)
            {
                WorkHistory.Add(new ResponseResumeItem()
                {
                    _id = item._id,
                    Name = item.Name,
                    Description = item.Description,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    SubTitle = item.SubTitle
                });
            }
            foreach (var item in obj.ExtraCurricular)
            {
                ExtraCurricular.Add(new ResponseResumeItem()
                {
                    _id = item._id,
                    Name = item.Name,
                    Description = item.Description,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    SubTitle = item.SubTitle
                });
            }
            foreach (var item in obj.Certification)
            {
                Certification.Add(new ResponseResumeCertification()
                {
                    _id = item._id,
                    Name = item.Name,
                    Description = item.Description,
                    StartDate = item.StartDate,
                    // CertificatePath = string.IsNullOrEmpty(item.CertificatePath) ? "" : HelperFiles.GetURLJobSeekerCertificate(obj.UserId, item._id, item.CertificatePath)
                    CertificatePath= item.CertificatePath
                });
            }
        }
    }
    public class ResponseResumeItem : ResponseBase
    {
        public string SubTitle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
    }
    public class ResponseResumeCertification : ResponseBase
    {
        public ResponseResumeCertification()
        {
        }
        public DateTime StartDate { get; set; }
        public string Description { get; set; }
        public string CertificatePath { get; set; }
    }
    public class ResponseContactInformationRequest
    {
        public ResponseContactInformationRequest()
        {
        }
        public string EmployerId { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

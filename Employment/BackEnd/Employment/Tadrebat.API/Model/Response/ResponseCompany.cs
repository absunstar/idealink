using Employment.API.Helpers.Files;
using Employment.Entity.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employment.API.Model.Response
{
    public class ResponseCompany : ResponseBase
    {
        public ResponseCompany()
        {
            Country = new ResponseSubItem();
            City = new ResponseSubItem();
            Industry = new ResponseSubItem();
        }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public DateTime Establish { get; set; }
        public ResponseSubItem Industry { get; set; }
        public string About { get; set; }
        public string SocialFacebook { get; set; }
        public string SocialTwitter { get; set; }
        public string SocialLinkedin { get; set; }
        public string SocialGooglePlus { get; set; }
        public ResponseSubItem Country { get; set; }
        public ResponseSubItem City { get; set; }
        public string Address { get; set; }
        public string CompanyLogo { get; set; }
        public bool IsApproved { get; set; }
        public Dictionary<string, string> data { get; set; }
        public override void Map(object model)
        {
            if (model == null)
                return;

            var obj = (Company)model;
            data = obj.data;
            IsApproved = obj.IsApproved;
            About = obj.About;
            Address = obj.Address;
            City = new ResponseSubItem(obj.City._id, obj.City.Name);
            Country = new ResponseSubItem(obj.Country._id, obj.Country.Name);
            Email = obj.Email;
            Establish = obj.Establish;
            Industry = new ResponseSubItem(obj.Industry._id, obj.Industry.Name);
            Name = obj.Name;
            Phone = obj.Phone;
            SocialFacebook = obj.SocialFacebook;
            SocialGooglePlus = obj.SocialGooglePlus;
            SocialLinkedin = obj.SocialLinkedin;
            SocialGooglePlus = obj.SocialGooglePlus;
            SocialTwitter = obj.SocialTwitter;
            Website = obj.Website;
            _id = obj._id;
            IsActive = obj.IsActive.GetValueOrDefault();
            CompanyLogo = HelperFiles.GetURLCompanyLogo(_id, obj.CompanyLogo);
        }
    }

    public class ResponseSubItem
    {
        public ResponseSubItem()
        {

        }
        public ResponseSubItem(string id, string name)
        {
            _id = id;
            Name = name;
        }
        public string _id { get; set; }
        public string Name { get; set; }
    }
    public class ResponseSubItemURL : ResponseSubItem
    {
        public ResponseSubItemURL()
        {

        }
        public ResponseSubItemURL(string id, string name, string url) : base(id,name)
        {
            URL = HelperFiles.GetURLCompanyLogo(id, url);
        }
        public string URL { get; set; }
    }
    public class ResponseSubItemActive : ResponseSubItem
    {
        public bool IsApproved { get; set; }
    }
}

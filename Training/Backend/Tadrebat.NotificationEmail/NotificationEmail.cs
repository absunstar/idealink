using IdentityServer4.Models;
using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using Tadrebat.Interface;
using Flurl;
using System.Collections.Generic;

namespace Tadrebat.Notification
{
    public class NotificationEmail : INotificationEmail
    {
        private ICacheConfig _cacheConfig;
        public NotificationEmail(ICacheConfig cacheConfig)
        {
            _cacheConfig = cacheConfig;
        }
        public async Task<bool> EmailConfirmation(string Code, string UserId, string Email)
        {
            string url = Url.Combine(_cacheConfig.URLSTS, _cacheConfig.AccountConfirmationURL);
            url = string.Format("{0}?Token={1}&userId={2}", url, Code, UserId);
            
            MailMessage message = new MailMessage();
            #region formatter
            string text = string.Format("Please click on this link to confirm your Tadrebat Acount "+Email+": " + url);
            string html = "Please confirm your account <b>"+Email+ "</b> by clicking this link: <a href=\"" + url + "\">link</a><br/>";

            html += HttpUtility.HtmlEncode(@" Or click on the copy the following link on the browser: " + url);
            #endregion
            
            message.Subject = "Tadrebat Account Confirmation Email";
            message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
            message.Body = html;

            return await SendMail(message, Email);
        }
        public async Task<bool> EmailForgetPassword(string Code, string UserId, string email)
        {
            string url = Url.Combine(_cacheConfig.URLSTS, "Account/SetPassword");
            url = string.Format("{0}?Token={1}&UserId={2}", url, Code, UserId);

            MailMessage message = new MailMessage();
            #region formatter
            string text = string.Format("Please click on this link to reset a new password for  your Tadrebat Acount: " + url);
            string html = "Please processed by clicking this link: <a href=\"" + url + "\">link</a><br/>";

            html += HttpUtility.HtmlEncode(@"Or click on the copy the following link on the browser: " + url);
            #endregion

            message.Subject = "Tadrebat Forgot Password";
            message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
            message.Body = html;

            return await SendMail(message, email);
        }
        public async Task<bool> EmailRequestRegister(string Type, string Partner, string Name, string Email, string Phone, string Message, Dictionary<string, string> data)
        {
            MailMessage message = new MailMessage();
            #region formatter
           // string text = string.Format("Please click on this link to confirm your Tadrebat Acount " + Email + ": " + url);
            string html = "A new User Requested to register <br />";
            html += "Type: <b>" + Type + "</b> <br />";
            html += "Partner: <b>" + Partner + "</b> <br />";
            html += "Name: <b>" + Name + "</b> <br />";
            html += "Email: <b>" + Email + "</b> <br />";
            html += "Phone: <b>" + Phone + "</b> <br />";
            html += "Message: <b>" + Message + "</b> <br />";
            foreach(var d in data)
            {
                html += d.Key + ": <b>" + d.Value + "</b> <br />";
            }
            #endregion

            message.Subject = "Tadrebat Request Registeration";
            //message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
            message.Body = html;

            return await SendMail(message, "");
        }
        protected async Task<bool> SendMail(MailMessage message, string email)
        {
            
            //TODO:TO REMOVE Fixed email 
            message.To.Clear();
           // message.To.Add("mhk.tadrebat@gmail.com");
            if(!string.IsNullOrEmpty(email))
                message.To.Add(email);

            //MailMessage msg = new MailMessage();
            MailMessage msg = message;
            msg.From = new MailAddress(_cacheConfig.EmailUserName);

            SmtpClient smtpClient = new SmtpClient(_cacheConfig.EmailSMTP, _cacheConfig.EmailPort);
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(_cacheConfig.EmailUserName, _cacheConfig.EmailPassword);
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = true;
            try
            {
                await smtpClient.SendMailAsync(msg);
            }
            catch (Exception ex)
            {
                //throw ex;
                return false;
            }

            return true;
        }
    }
}

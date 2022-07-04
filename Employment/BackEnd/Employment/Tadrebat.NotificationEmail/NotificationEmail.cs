using IdentityServer4.Models;
using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using Employment.Interface;
using Flurl;

namespace Employment.Notification
{
    public class NotificationEmail : INotificationEmail
    {
        private ICacheConfig _cacheConfig;
        public NotificationEmail(ICacheConfig cacheConfig)
        {
            _cacheConfig = cacheConfig;
        }
        public async Task<bool> EmailAttendanceCode(long Code, string UserName, string UserEmail, string JobFairName, string JobFairLocation, DateTime JobFairDate, bool IsOnline)
        {
            MailMessage message = new MailMessage();
            var strOnline = IsOnline ? "Online" : "Offline";

            string text = string.Format("You have registered successfully for Fair " + JobFairName + " being held at " + JobFairLocation + " on " + JobFairDate.ToString("dd/MM/YYYY")
                                        + "You will need your code " + Code.ToString() + " for enterance. Please save it.");
            string html = "You have registered successfully for Fair " + JobFairName + " being held at " + JobFairLocation + " on " + JobFairDate.ToString("dd/MM/YYYY")
                                        + ". This Fair will be " + strOnline
                                        + "<br>You will need your code " + Code.ToString() + " for enterance. Please save it.";


            message.Subject = "Employment - Job Fair " + JobFairName + " Registeration";
            message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
            message.Body = html;

            return await SendMail(message, UserEmail);
        }
        public async Task<bool> EmailConfirmation(string Code, string UserId, string Email)
        {
            string url = Url.Combine(_cacheConfig.URLSTS, _cacheConfig.AccountConfirmationURL);
            url = string.Format("{0}?Token={1}&userId={2}", url, Code, UserId);

            MailMessage message = new MailMessage();
            #region formatter
            string text = string.Format("Please click on this link to confirm your Employment Acount " + Email + ": " + url);
            string html = "Please confirm your account <B>" + Environment.NewLine + Email + "</B> by clicking this link: <a href=\"" + url + "\">link</a><br/>";

            html += HttpUtility.HtmlEncode(@"Or click on the copy the following link on the browser: " + Environment.NewLine + url);
            #endregion

            message.Subject = "Employment Account Confirmation Email";
            message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
            message.Body = html;

            return await SendMail(message, Email);
        }
        public async Task<bool> EmailForgetPassword(string Code, string UserId, string Email)
        {
            string url = Url.Combine(_cacheConfig.URLSTS, "Account/SetPassword");
            url = string.Format("{0}?Token={1}&UserId={2}", url, Code, UserId);

            MailMessage message = new MailMessage();
            #region formatter
            string text = string.Format("Please click on this link to reset a new password for your Employment Acount: " + url);
            string html = "Please processed by clicking this link: <a href=\"" + url + "\"> link</a><br/>";

            html += HttpUtility.HtmlEncode(@"Or click on the copy the following link on the browser: " + Environment.NewLine + url);
            #endregion

            message.Subject = "Employment Forgot Password";
            message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
            message.Body = html;

            return await SendMail(message, Email);
        }
        protected async Task<bool> SendMail(MailMessage message, string email)
        {

            message.To.Clear();
            if (!string.IsNullOrEmpty(email))
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

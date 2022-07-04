using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Employment.API.Helpers.Constants;
using Employment.API.Model.Model;
using Employment.Enum;

namespace Employment.API.Helpers.HTTPCall
{
    public class HTTPCallSTS
    {
        public async Task<bool> ResendPasswordLink(string email)
        {
            string param = string.Format("?Email={0}", email);
            var result = await CallSTS("Account/ResendPasswordLink", param);
            return result.Item1;
        }

        public async Task<bool> ResendActivationLink(string email)
        {
            string param = string.Format("?Email={0}", email);
            var result = await CallSTS("Account/ResendActivationLink", param);
            return result.Item1;
        }
        public async Task<bool> RegisterUser(ModelUserProfile user, string UserId, string password)
        {
            string param = string.Format("?Email={0}&Type={1}&MDID={2}&Password={3}", user.Email, (int)user.Type, UserId,password);
            var result = await CallSTS("Account/CreateSTSUser", param);
            return result.Item1;
        }
        public async Task<(bool, string)> ChangePassword(string Email, string OldPassword, string NewPassword)
        {
            string param = string.Format("?Email={0}&OldPassword={1}&NewPassword={2}", Email, OldPassword, NewPassword);
            var result = await CallSTS("Account/ChangeUserPassword", param);
            return result;
        }
        public async Task<bool> ResetPassword(string UserEmail, string NewPassword, string OldPassword)
        {
            string param = string.Format("?Email={0}&NewPassword={1}&OldPassword={2}", UserEmail, NewPassword, OldPassword);
            var result = await CallSTS("Account/ResetSTSPassword", param);
            return result.Item1;
        }
        public async Task<bool> UpdateAccountStatus(string Email, bool Status)
        {
            string param = string.Format("?Email={0}&Status={1}", Email, Status);
            var result = await CallSTS("Account/AccountStatus", param);
            return result.Item1;
        }
        public async Task<bool> UpdateUserRole( string Email, int UserType)
        {
            string param = string.Format("?Email={0}&Type={1}", Email, UserType);
            var result = await CallSTS("Account/UpdateUserRole", param);
            return result.Item1;
        }
        protected async Task<(bool,string)> CallSTS(string functionName, string param)
        {
            var baseURL = ConfigConstant.urlstsAuthority;
            var fullURL = Flurl.Url.Combine(baseURL, functionName);
            fullURL += param;
            //var objResponse = new ResponseHttpClient();

            using (var client = new HttpClient())
            {
                string Id = Guid.NewGuid().ToString();
                client.BaseAddress = new Uri(fullURL);

                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(fullURL),
                    Method = HttpMethod.Get,
                };

                request.Content = new StringContent("",//JsonConvert.SerializeObject(obj),
                    Encoding.UTF8,
                    "application/x-www-form-urlencoded");//CONTENT-TYPE header

                HttpResponseMessage response = new HttpResponseMessage();
                try
                {
                    response = await client.SendAsync(request);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                
                // ... Check Status Code                                
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return (true,"");
                }
                
                var reason = await response.Content.ReadAsStringAsync();
                return (false, reason);
            }
        }
    }
}

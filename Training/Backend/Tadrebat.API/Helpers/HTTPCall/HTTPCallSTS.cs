using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.API.Helpers.Constants;
using Tadrebat.API.Model.Model;
using Tadrebat.Enum;

namespace Tadrebat.API.Helpers.HTTPCall
{
    public class HTTPCallSTS
    {
        public async Task<bool> ResendPasswordLink(string email)
        {
            string param = string.Format("?Email={0}", email);
            var result = await CallSTS("Account/ResendPasswordLink", param);
            return result;
        }
        
        public async Task<bool> ResendActivationLink(string email)
        {
            string param = string.Format("?Email={0}", email);
            var result = await CallSTS("Account/ResendActivationLink", param);
            return result;
        }
        public async Task<bool> RegisterUser(ModelUserProfile user, string UserId)
        {
            string param = string.Format("?Email={0}&Type={1}&MDID={2}", user.Email, (int)user.Type, UserId);
            var result = await CallSTS("Account/CreateSTSUser", param);
            return result;
        }
        public async Task<bool> RegisterUser(string email, string UserId)
        {
            string param = string.Format("?Email={0}&Type={1}&MDID={2}", email, (int)EnumUserTypes.Trainee, UserId);
            var result = await CallSTS("Account/CreateSTSUser", param);
            return result;
        }
        public async Task<bool> ResetPassword(string UserEmail, string NewPassword, string OldPassword)
        {
            string param = string.Format("?Email={0}&NewPassword={1}&OldPassword={2}", UserEmail, NewPassword, OldPassword);
            var result = await CallSTS("Account/ResetSTSPassword", param);
            return result;
        }
        protected async Task<bool> CallSTS(string functionName, string param)
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
                HttpContent content = response.Content;

                // ... Check Status Code                                
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                    //objResponse.IsSuccess = true;
                    //// ... Read the string.
                    //objResponse.Response = await content.ReadAsStringAsync();
                }
                else
                {
                    //objResponse.IsSuccess = false;
                    //// ... Read the string.
                    //objResponse.Response = response.ToString(); ;
                }
            }
            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tadrebat.Interface
{
    public interface INotificationEmail
    {
        Task<bool> EmailConfirmation(string Code, string UserId, string Email);
        Task<bool> EmailForgetPassword(string Code, string UserId, string Email);
        Task<bool> EmailRequestRegister(string Type, string Partner, string Name, string Email, string Phone, string Message, Dictionary<string, string> data);
    }
}

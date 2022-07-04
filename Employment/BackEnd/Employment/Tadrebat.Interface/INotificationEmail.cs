using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Interface
{
    public interface INotificationEmail
    {
        Task<bool> EmailAttendanceCode(long Code, string UserName, string UserEmail, string JobFairName, string JobFairLocation, DateTime JobFairDate, bool IsOnline);
        Task<bool> EmailConfirmation(string Code, string UserId, string Email);
        Task<bool> EmailForgetPassword(string Code, string UserId, string Email);
    }
}

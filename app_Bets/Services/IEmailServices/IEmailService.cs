using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bets.Domain.Services.EmailServices
{
    public interface IEmailService
    {
        Task SendEmailService(string subject, string toEmail, string userName, string message, bool isHtml = false);
    }
}

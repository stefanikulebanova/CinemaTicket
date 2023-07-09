using KinoIS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KinoIS.Service.Interface
{
    public interface EmailMessageService
    {
        Task SendEmailAsync(List<EmailMessage> allMails);
    }
}

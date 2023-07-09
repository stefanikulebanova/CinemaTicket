using KinoIS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinoIs.Repository.Interface
{
    public interface EmailMessageRepository
    {
        EmailMessage Insert(EmailMessage mail);
        List<EmailMessage> getAll();
    }
}

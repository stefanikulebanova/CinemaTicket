using KinoIs.Repository.Interface;
using KinoIS.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinoIS.Service.Implementation
{
    public class BackgroundEmailSenderImpl : BackgroundEmailSender
    {
        private readonly EmailMessageService _emailService;
        private readonly EmailMessageRepository _mailRepository;

        public BackgroundEmailSenderImpl(EmailMessageService emailService, EmailMessageRepository mailRepository)
        {
            _emailService = emailService;
            _mailRepository = mailRepository;
        }
        public async Task DoWork()
        {
            await _emailService.SendEmailAsync(_mailRepository.getAll().Where(z => !z.Status).ToList());
        }
    }
}

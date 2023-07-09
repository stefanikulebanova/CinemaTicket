using KinoIs.Repository.Interface;
using KinoIS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinoIs.Repository.Implementation
{
    public class EmailMessageRepositoryImpl : EmailMessageRepository
    {
        public ApplicationDbContext context;
        public EmailMessageRepositoryImpl(ApplicationDbContext context)
        {
            this.context = context; 
        }

        public List<EmailMessage> getAll()
        {
            return this.context.EmailMessages.ToList();
        }

        public EmailMessage Insert(EmailMessage mail)
        {
            this.context.EmailMessages.Add(mail);
            this.context.SaveChanges();
            return mail;
            
        }
    }
}

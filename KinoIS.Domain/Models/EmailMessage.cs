using System;
using System.Collections.Generic;
using System.Text;

namespace KinoIS.Domain.Models
{
    public class EmailMessage
    {
        public Guid Id { get; set; }
        public string MailTo { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Boolean Status { get; set; }
    }
}

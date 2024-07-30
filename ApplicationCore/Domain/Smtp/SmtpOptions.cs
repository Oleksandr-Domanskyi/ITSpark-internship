using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Domain.Smtp
{
    public class SmtpOptions
    {
        public string? smtpServer { get; set; }
        public string? port { get; set; }
        public string? senderEmail { get; set; }
        public string? appPassword  {get; set;}
    }
}
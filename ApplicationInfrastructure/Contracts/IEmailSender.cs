using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationInfrastructure.Contracts
{
    public interface IEmailSender
    {
        public Task SendEmailAsync(string recipientEmail, string subject, string htmlMessage);
    }
}
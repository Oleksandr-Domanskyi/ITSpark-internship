using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ApplicationCore.Domain.Smtp;
using ApplicationInfrastructure.Contracts;
using Microsoft.Extensions.Options;

namespace ApplicationInfrastructure.Services.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpOptions _smtp;
        public EmailSender(IOptions<SmtpOptions> smtp)
        {
            _smtp = smtp.Value;
        }
        public async Task SendEmailAsync(string recipientEmail, string subject, string htmlMessage)
        {
            using var client = new SmtpClient(_smtp.smtpServer,int.Parse(_smtp.port!))
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_smtp.senderEmail, _smtp.appPassword)
            };

            var message = new MailMessage(_smtp.senderEmail!, recipientEmail, subject, htmlMessage)
            {
                IsBodyHtml = true
            };

            await client.SendMailAsync(message);

        }

    }
}
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TWP_API_Payroll.Services
{
    public interface IMailService
    {

        Task SendEmailAsync(string toEmail, string subject, string content);
        Task SendEmailConfirmationAsync(string toEmail, string subject, string content);
    }

    public class SendGridMailService : IMailService
    {
        private IConfiguration _configuration;

        public SendGridMailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            var apiKey = _configuration["SendGridAPI:Key"];
            var fromEmail = _configuration["SendGridAPI:fromEmail"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmail, "MMC");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);
        }
        public async Task SendEmailConfirmationAsync(string toEmail, string subject, string content)
        {
            var apiKey = _configuration["SendGridAPI:Key"];
            var fromEmail = _configuration["SendGridAPI:fromEmail"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmail, "MMC");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);
        }
    }
}

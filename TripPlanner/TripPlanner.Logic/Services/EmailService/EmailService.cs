using Microsoft.Extensions.Options;
using System.Net.Mail;
using TripPlanner.DatabaseModels.Models.Enums;
using TripPlanner.Logic.Services.EmailService.Smtp;

namespace TripPlanner.Logic.Services.EmailService
{   
    public class EmailService : IEmailService
    {
        private EmailServiceConfiguration _config;
        private ISmtpClient _smtpClient;
        
        public EmailService(ISmtpClient smtpClient, IOptions<EmailServiceConfiguration> config)
        {
            _smtpClient = smtpClient;
            _config = config.Value;
        }

        public async void SendEmail(object status,string email) 
        {
            MailAddress from = new MailAddress(_config.Email);
            MailAddress to = new MailAddress(email);
            MailMessage message = new MailMessage(from, to);
            
            message.IsBodyHtml = true;
            switch (status)
            {
                case BusinessTripRequestStatus.Accepted : message.Body = string.Format(_config.Template, status, email,"Your trip request was aceepted"); break;
                case BusinessTripRequestStatus.Rejected : message.Body = string.Format(_config.Template, status, email,"Your trip request was rejected"); break;
                case BusinessTripRequestStatus.Created  : message.Body = string.Format(_config.Template, status, email,"You created a new trip request"); break;
            }
            await _smtpClient.SendMailAsync(message);
        }
    }
}

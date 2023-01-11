using Microsoft.Extensions.Options;
using System.Net.Mail;
using TripPlanner.Logic.Services.EmailService.Smtp;

namespace TripPlanner.Logic.Services.EmailService
{   

    public class EmailService : IEmailService
    {
        private readonly EmailUser _user;
        private ISmtpClient _smtpClient;
        public EmailService(ISmtpClient smtpClient, IOptions<EmailUser> user)
        {
            _smtpClient = smtpClient;
            _user = user.Value;
        }

        public void SendEmail() 
        {
            MailAddress from = new MailAddress(_user.Email);
            MailAddress to = new MailAddress("paul.crasmareanu@nagarro.com");
            MailMessage message = new MailMessage(from, to);
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString("<html><body>test :) </body</html>");
            message.IsBodyHtml = true;
            message.Body = "<html><body>Test Email </body></html>";

            _smtpClient.SendMailAsync(message);
        }
    }
}

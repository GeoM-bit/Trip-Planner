using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TripPlanner.Logic.Services.EmailService.Smtp;

namespace TripPlanner.Logic.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private ISmtpClient smtpClient;
        public EmailService(ISmtpClient smtpClient)
        {
            this.smtpClient = smtpClient;
        }

        public void SendEmail() {
            MailAddress from = new MailAddress("dragos.boboluta@nagarro.com");
            MailAddress to = new MailAddress("paul.crasmareanu@nagarro.com");
            MailMessage message = new MailMessage(from, to);
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString("<html><body>test :) </body</html>");
            message.IsBodyHtml = true;
            message.Body = "<html><body>Test Email </body></html>";
            smtpClient.SendMailAsync(message);
        }
    }
}

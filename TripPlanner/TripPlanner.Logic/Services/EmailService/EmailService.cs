using System;
using System.Collections.Generic;
using System.Linq;
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
            MailAddress from = new MailAddress("dragos.boboluta@nagarro.com", "dragos", System.Text.Encoding.UTF8);
            MailAddress to = new MailAddress("dragos.boboluta@nagarro.com");
            MailMessage message = new MailMessage(from, to);
            smtpClient.SendMailAsync(message);
        }
    }
}

using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace TripPlanner.Logic.Services.EmailService.Smtp
{
    public class RealSmtpClient : ISmtpClient
    {
        private readonly EmailServiceConfiguration _config;
        public string template;
        public string email;
        public string password;
        public RealSmtpClient(IOptions<EmailServiceConfiguration> config )
        {
            _config = config.Value;
        }
       
        public async Task SendMailAsync(MailMessage message )
        {
            SmtpClient smtp = new SmtpClient(_config.ServerName)
            {
                Port = Convert.ToInt32(_config.Port),
                EnableSsl = _config.EnableSsl,
                Credentials = new NetworkCredential(_config.Email, _config.Password),
                Host = _config.Host,
            };
            try
            {
                await smtp.SendMailAsync(message);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace TripPlanner.Logic.Services.EmailService.Smtp
{
    public class RealSmtpClient : ISmtpClient
    {
        private readonly EmailUser _user;
        private readonly SmtpOptions _config;

        public RealSmtpClient(IOptions<SmtpOptions> config ,IOptions<EmailUser>user)
        {
            _config = config.Value;
            _user = user.Value;
        }
       
        public async Task SendMailAsync(MailMessage message )
        {
            SmtpClient smtp = new SmtpClient(_config.ServerName)
            {
                Port = Convert.ToInt32(_config.Port),
                EnableSsl = _config.EnableSsl,
                Credentials = new NetworkCredential(_user.Email, _user.Password),
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

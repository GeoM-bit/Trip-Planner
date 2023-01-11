using System.Net.Mail;

namespace TripPlanner.Logic.Services.EmailService.Smtp
{
    public interface ISmtpClient
    {
        Task SendMailAsync(MailMessage message);
    }
}

namespace TripPlanner.Logic.Services.EmailService
{
    public interface IEmailService
    {
        void SendEmail(string status, string email);
    }
}

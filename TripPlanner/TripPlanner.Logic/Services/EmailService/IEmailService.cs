namespace TripPlanner.Logic.Services.EmailService
{
    public interface IEmailService
    {
        void SendEmail(object status, string email);
    }
}

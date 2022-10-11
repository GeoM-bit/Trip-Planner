using TripPlanner.DatabaseModels.Models;
using TripPlanner.Logic.DtoModels;

namespace TripPlanner.Logic.Repositories
{
    public interface IAuthenticationRepository
    {
        Task<bool> Register(User user, string password);
        Task<string> Login(LoginDto loginUser);
        Task Logout();
        Task<string> GetRole(string email);
    }
}

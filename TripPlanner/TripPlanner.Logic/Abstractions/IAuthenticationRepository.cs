using TripPlanner.DatabaseModels.Models;
using TripPlanner.Logic.DtoModels;

namespace TripPlanner.Logic.Abstractions
{
    public interface IAuthenticationRepository
    {
        Task<bool> Register(User user, string password);
        Task<TokenDto> Login(LoginDto loginUser);
        Task Logout();
        Task<string> GetRole(string email);
    }
}

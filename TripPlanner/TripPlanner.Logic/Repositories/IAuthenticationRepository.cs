using TripPlanner.DatabaseModels.Models;
using TripPlanner.Logic.DtoModels;

namespace TripPlanner.Logic.Repositories
{
    public interface IAuthenticationRepository
    {
        Task<bool> Register(User user, string password);
        Task<bool> Login(User user, string password);

    }
}

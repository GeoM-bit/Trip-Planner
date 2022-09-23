using Microsoft.AspNetCore.Identity;
using TripPlanner.DatabaseModels.Models;

namespace TripPlanner.Logic.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthenticationRepository(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> Login(User user, string password)
        {
            var registeredUser = await _userManager.FindByEmailAsync(user.Email);
            if (registeredUser == null)
            {
                return false;
            }
            var loginResult = await _signInManager.PasswordSignInAsync(registeredUser.UserName, password, false, true);

            if (!loginResult.Succeeded)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> Register(User user, string password)
        {
            var isUserRegistered = (await _userManager.CreateAsync(user, password)).Succeeded;
            var isUserAssignedRole = (await _userManager.AddToRoleAsync(user, "User")).Succeeded;
            return isUserRegistered && isUserAssignedRole;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}

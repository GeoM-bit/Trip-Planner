using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TripPlanner.Context;
using TripPlanner.DatabaseModels.Models;
using TripPlanner.Logic.Common;
using TripPlanner.Logic.Common.Enums;
using TripPlanner.Logic.DtoModels;
using TripPlanner.Logic.Exceptions;
using TripPlanner.Logic.Services;

namespace TripPlanner.Logic.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly TripPlannerContext _context;
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;

        public AuthenticationRepository(UserManager<User> userManager, SignInManager<User> signInManager, TripPlannerContext context, IConfiguration config, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _config = config;
            _tokenService = tokenService;
        }

        public async Task<string> Login(LoginDto loginUser)
        {
            var registeredUser = await _userManager.FindByEmailAsync(loginUser.Email);
            if (registeredUser == null)
            {
                return null;
            }
            var loginResult = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

            if (loginResult.Succeeded)
            {
                string role = GetRole(loginUser.Email).Result;
                string generatedToken = _tokenService.BuildToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), loginUser, role);
                if (generatedToken != null)
                {
                    TokenDto tokenDto = new() { Token = generatedToken };
                    return tokenDto.Token;
                }
                else
                    return null;
            }
            else
                return null;
        }

        public async Task<bool> Register(User user, string password)
        {
            var isUserRegistered = (await _userManager.CreateAsync(user, password)).Succeeded;
            var isUserAssignedRole = (await _userManager.AddToRoleAsync(user, Roles.User.ToString())).Succeeded;
            return isUserRegistered && isUserAssignedRole;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string> GetRole(string email)
        {
            string result = "";
            var userId = await _context.Users.Where(x=>x.Email==email).Select(x => x.Id).FirstOrDefaultAsync();
            if (userId != Guid.NewGuid())
            {
                var userRoleId = await _context.UserRoles.Where(x=>x.UserId==userId).Select(x=>x.RoleId).FirstOrDefaultAsync();
                if (userRoleId != Guid.NewGuid())
                {
                    if (userRoleId == Constants.UserRoleId)
                        result = Roles.User.ToString();
                    else if (userRoleId == Constants.BtoRoleId)
                        result = Roles.BTO.ToString();
                }
                else
                    throw new EntityNotFoundException($"The UserRole for {email} was not found!");
            }
            else
                throw new EntityNotFoundException($"The user with the email {email} was not found!");

            return result;
        }
    }
}

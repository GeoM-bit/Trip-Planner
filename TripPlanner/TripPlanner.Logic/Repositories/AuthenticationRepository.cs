using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TripPlanner.Context;
using TripPlanner.DatabaseModels.Models;
using TripPlanner.Logic.Abstractions;
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

        public async Task<TokenDto> Login(LoginDto loginUser)
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
                string generatedToken = _tokenService.BuildToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), loginUser.Email, role);
                string generatedRefreshToken = _tokenService.GenerateRefreshToken();
                if (generatedToken != null && generatedRefreshToken!=null)
                {
                    TokenDto tokenDto = new() { Token = generatedToken, RefreshToken = generatedRefreshToken};
                    return tokenDto;
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

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            return principal;

        }

        public async Task<TokenDto> Refresh(TokenDto tokenModel)
        {
            string? accessToken = tokenModel.Token;

            var principal = GetPrincipalFromExpiredToken(accessToken);

            string username = principal.Identity.Name;
            string role = GetRole(username).Result;

            var newAccessToken = 
                _tokenService.BuildToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), username, role);
            string generatedRefreshToken = _tokenService.GenerateRefreshToken();

            var token = new TokenDto { Token = newAccessToken, RefreshToken = generatedRefreshToken };

            return token;
        }

       
    }
}

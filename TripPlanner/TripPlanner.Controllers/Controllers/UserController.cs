using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TripPlanner.DatabaseModels.Models;
using TripPlanner.Logic.DtoModels;
using TripPlanner.Logic.Repositories;
using TripPlanner.Logic.Services;

namespace TripPlanner.Controllers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        private readonly IAuthenticationRepository _repository;
        private readonly IMapper _mapper;
        private string generatedToken = null;

        public UserController(IConfiguration config, ITokenService tokenService, IAuthenticationRepository repository, IMapper mapper)
        {
            _config = config;
            _tokenService = tokenService;
            _repository = repository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<bool> Register(RegisterUserDto user)
        {
            var registerUser = _mapper.Map<User>(user);
            return await _repository.Register(registerUser, user.Password);
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<bool> Login(LoginDto loginUser)
        {
            var user = _mapper.Map<User>(loginUser);
            var validUser = await _repository.Login(user, loginUser.Password);
            if (validUser)
            {
                generatedToken = _tokenService.BuildToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), loginUser);
                if (generatedToken != null)
                {
                    HttpContext.Session.SetString("Token", generatedToken);
                    Response.Headers.Add("Authorization", "Bearer " + generatedToken);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }

        [Route("logout")]
        [Authorize(Roles = "User, BTO")]
        [HttpPost]
        public async Task Logout()
        {
            await _repository.Logout();
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripPlanner.DatabaseModels.Models;
using TripPlanner.Logic.DtoModels;
using TripPlanner.Logic.Repositories;

namespace TripPlanner.Controllers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthenticationRepository _repository;
        private readonly IMapper _mapper;

        public UserController(IAuthenticationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<bool> Register(RegisterUserDto registerUser)
        {
            var user = _mapper.Map<User>(registerUser);
            return await _repository.Register(user, registerUser.Password);
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<string> Login(LoginDto loginUser)
        {
            return await _repository.Login(loginUser);
           
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

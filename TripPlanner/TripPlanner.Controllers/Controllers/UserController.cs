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
        public async Task<bool> Register(RegisterUserDto user)
        {
            var registerUser = _mapper.Map<User>(user);
            return await _repository.Register(registerUser);
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<bool> Login(LoginDto loginUser)
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

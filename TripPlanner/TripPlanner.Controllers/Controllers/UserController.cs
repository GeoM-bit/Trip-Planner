﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripPlanner.ApiModels.ApiModels;
using TripPlanner.DatabaseModels.Models;
using TripPlanner.Logic.Abstractions;
using TripPlanner.Logic.DtoModels;

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
        public async Task<bool> Register(RegisterApiModel registerApiModel)
        {
            var registerUser = _mapper.Map<RegisterUserDto>(registerApiModel);
            var user = _mapper.Map<User>(registerUser);

            return await _repository.Register(user, registerUser.Password);
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<TokenDto> Login(LoginApiModel loginApiModel)
        {
            var loginUser = _mapper.Map<LoginDto>(loginApiModel);

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

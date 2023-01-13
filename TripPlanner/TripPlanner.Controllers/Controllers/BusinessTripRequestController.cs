using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TripPlanner.ApiModels.ApiModels;
using TripPlanner.DatabaseModels.Models;
using TripPlanner.Logic.Abstractions;
using TripPlanner.Logic.DtoModels;
using TripPlanner.Logic.Services.EmailService;
using TripPlanner.Logic.Services.EmailService.Smtp;

namespace TripPlanner.Controllers.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessTripRequestController : ControllerBase
    {
        private readonly IBusinessTripRequestRepository _repository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        
        public BusinessTripRequestController(IBusinessTripRequestRepository repository, IMapper mapper , IEmailService emailService)
        {
            _repository = repository;
            _mapper = mapper;
            _emailService = emailService;
            
        }

        [HttpPost]
        public async Task<bool> Post(RegisterBusinessTripApiModel registerBusinessTripApiModel)
        {
            var registerBusinessTripDto = _mapper.Map<RegisterBusinessTripDto>(registerBusinessTripApiModel);
            var businessTripRequest = _mapper.Map<BusinessTripRequest>(registerBusinessTripDto);

            var result = await _repository.CreateTrip(businessTripRequest);
            if (result == false)
            {
                return false;
            }
            else
            {
                _emailService.SendEmail("created", registerBusinessTripApiModel.Email);

                return result;

            }
        }
    }
}

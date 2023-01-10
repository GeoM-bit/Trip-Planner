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
       
        [HttpGet]
        public async Task<IEnumerable<BtoBusinessTripDto>> Get()
        {
            _emailService.SendEmail();
            var trips = await _repository.GetAllTrips();
            var tripsDto = _mapper.Map<List<BtoBusinessTripDto>>(trips);

            return tripsDto;
        }

        [HttpPost]
        public async Task<bool> Post(RegisterBusinessTripApiModel registerBusinessTripApiModel)
        {
            var registerBusinessTripDto = _mapper.Map<RegisterBusinessTripDto>(registerBusinessTripApiModel);
            var businessTripRequest = _mapper.Map<BusinessTripRequest>(registerBusinessTripDto);

            return await _repository.CreateTrip(businessTripRequest);
        }

        [HttpPut("{id}")]
        public async Task<bool> Put(Guid id, BusinessTripRequest trip)
        {
            return await _repository.UpdateTrip(id, trip);
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(Guid id)
        {
            return await _repository.DeleteTrip(id);
        }
    }
}

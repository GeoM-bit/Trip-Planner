using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripPlanner.ApiModels.ApiModels;
using TripPlanner.DatabaseModels.Models;
using TripPlanner.Logic.Abstractions;
using TripPlanner.Logic.DtoModels;

namespace TripPlanner.Controllers.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessTripRequestController : ControllerBase
    {
        private readonly IBusinessTripRequestRepository _repository;
        private readonly IMapper _mapper;

        public BusinessTripRequestController(IBusinessTripRequestRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
       
        [HttpGet]
        public async Task<IEnumerable<BtoBusinessTripDto>> Get()
        {
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

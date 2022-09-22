using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripPlanner.DatabaseModels.Models;
using TripPlanner.Logic.Common;
using TripPlanner.Logic.DtoModels;
using TripPlanner.Logic.Repositories;

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

        [Authorize(Roles = "User")]
        [Route("TripsForUser")]
        [HttpGet]
        public async Task<IEnumerable<UserBusinessTripDto>> GetTripsForUser([FromQuery]string email, [FromQuery]SearchCriteria searchCriteria)
        {
            var trips = await _repository.GetAllTripsForUserByCriteria(email, searchCriteria);
            var tripsDto = _mapper.Map<List<UserBusinessTripDto>>(trips);

            return tripsDto;
        }

        [Authorize(Roles = "BTO")]
        [Route("TripsForBto")]
        [HttpGet]
        public async Task<IEnumerable<BtoBusinessTripDto>> GetTripsForBto([FromQuery] SearchCriteria searchCriteria)
        {
            var trips = await _repository.GetPendingRequestsByCriteria(searchCriteria);
            var tripsDto = _mapper.Map<List<BtoBusinessTripDto>>(trips);

            return tripsDto;
        }

        [HttpPost]
        public async Task<bool> Post(BusinessTripRequest trip)
        {
            return await _repository.CreateTrip(trip);
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

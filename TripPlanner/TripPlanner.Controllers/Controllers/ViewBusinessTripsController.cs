using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripPlanner.ApiModels.ApiModels;
using TripPlanner.Logic.Abstractions;
using TripPlanner.Logic.Common;

namespace TripPlanner.Controllers.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ViewBusinessTripsController : ControllerBase
    {
        private readonly IBusinessTripRequestRepository _repository;
        private readonly IMapper _mapper;

        public ViewBusinessTripsController(IBusinessTripRequestRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [Route("ViewTrips")]
        [HttpPost]
        public async Task<IEnumerable<IBusinessTrip>> GetTrips([FromBody] SearchCriteriaApiModel? searchCriteria)
        {
            var email = User.Identity.Name;
            var criteria = _mapper.Map<SearchCriteria>(searchCriteria);
            var trips = await _repository.GetTripsByCriteria(criteria, email);
           
            return trips;
        }
    }
}

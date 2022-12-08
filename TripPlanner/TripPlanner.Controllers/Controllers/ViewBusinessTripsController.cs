using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripPlanner.ApiModels.ApiModels;
using TripPlanner.Logic.Abstractions;
using TripPlanner.Logic.Common;
using TripPlanner.Logic.DtoModels;

namespace TripPlanner.Controllers.Controllers
{
    [Authorize]
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

        [Route("UpdateTrip")]
        [HttpPut]
        public async Task<bool> UpdateTrip(UpdateStatusApiModel updateStatusApiModel)
        {
            var updateStatusModel = _mapper.Map<UpdateStatusModel>(updateStatusApiModel);
            var result = await _repository.UpdateStatus(updateStatusModel);
            
            return result;
        }
    }
}

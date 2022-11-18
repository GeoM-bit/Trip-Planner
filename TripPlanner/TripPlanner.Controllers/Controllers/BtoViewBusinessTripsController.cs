using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripPlanner.ApiModels.ApiModels;
using TripPlanner.Logic.Common;
using TripPlanner.Logic.DtoModels;
using TripPlanner.Logic.Repositories;

namespace TripPlanner.Controllers.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BtoViewBusinessTripsController : ControllerBase
    {
        private readonly IBusinessTripRequestRepository _repository;
        private readonly IMapper _mapper;

        public BtoViewBusinessTripsController(IBusinessTripRequestRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [Authorize(Roles = "BTO")]
        [Route("TripsForBto")]
        [HttpGet]
        public async Task<IEnumerable<BtoBusinessTripDto>> GetTripsForBto([FromQuery] SearchCriteriaApiModel searchCriteriaApiModel)
        {
            var searchCriteria = _mapper.Map<SearchCriteria>(searchCriteriaApiModel);
            var trips = await _repository.GetPendingRequestsByCriteria(searchCriteria);
            var tripsDto = _mapper.Map<List<BtoBusinessTripDto>>(trips);

            return tripsDto;
        }
    }
}

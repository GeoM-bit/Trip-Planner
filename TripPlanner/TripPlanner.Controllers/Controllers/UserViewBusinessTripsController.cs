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
    public class UserViewBusinessTripsController : ControllerBase
    {
        private readonly IBusinessTripRequestRepository _repository;
        private readonly IMapper _mapper;

        public UserViewBusinessTripsController(IBusinessTripRequestRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [Authorize(Roles = "User")]
        [Route("TripsForUser")]
        [HttpPost]
        public async Task<IEnumerable<UserBusinessTripDto>> GetTripsForUser([FromBody] UserTripsApiModel userTripsApiModel)
        {
            var getTripsForUser = _mapper.Map<GetTripsForUser>(userTripsApiModel);
            var trips = await _repository.GetAllTripsForUserByCriteria(getTripsForUser);
            var tripsDto = _mapper.Map<List<UserBusinessTripDto>>(trips);

            return tripsDto;
        }
    }
}

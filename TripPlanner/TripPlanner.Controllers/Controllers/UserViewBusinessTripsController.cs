using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]
        public async Task<IEnumerable<UserBusinessTripDto>> GetTripsForUser([FromQuery] string email, [FromQuery] SearchCriteria searchCriteria)
        {
            var trips = await _repository.GetAllTripsForUserByCriteria(email, searchCriteria);
            var tripsDto = _mapper.Map<List<UserBusinessTripDto>>(trips);

            return tripsDto;
        }


    }
}

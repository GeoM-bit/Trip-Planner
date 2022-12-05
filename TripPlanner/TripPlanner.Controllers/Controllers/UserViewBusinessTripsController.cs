using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripPlanner.Logic.Abstractions;


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

    }
}

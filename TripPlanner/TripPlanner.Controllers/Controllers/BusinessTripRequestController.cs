using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripPlanner.ApiModels.ApiModels;
using TripPlanner.DatabaseModels.Models;
using TripPlanner.Logic.Abstractions;
using TripPlanner.Logic.DtoModels;

namespace TripPlanner.Controllers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessTripRequestController : ControllerBase
    {
        private readonly IBusinessTripRequestRepository _repository;
        private readonly IMapper _mapper;
        
        public BusinessTripRequestController(IBusinessTripRequestRepository repository, IMapper mapper )
        {
            _repository = repository;
            _mapper = mapper;
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> Post(RegisterBusinessTripApiModel registerBusinessTripApiModel)
        {
            try
            {
                var registerBusinessTripDto = _mapper.Map<RegisterBusinessTripDto>(registerBusinessTripApiModel);
                var businessTripRequest = _mapper.Map<BusinessTripRequest>(registerBusinessTripDto);

                var result = await _repository.CreateTrip(businessTripRequest);

                if (result)
                {
                    return Ok(true);
                }
                else
                {
                    return BadRequest("Invalid data");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
    }
}

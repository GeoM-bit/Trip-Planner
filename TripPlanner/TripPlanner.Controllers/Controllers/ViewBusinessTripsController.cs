using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripPlanner.ApiModels.ApiModels;
using TripPlanner.DatabaseModels.Models.Enums;
using TripPlanner.Logic.Abstractions;
using TripPlanner.Logic.Common;
using TripPlanner.Logic.DtoModels;
using TripPlanner.Logic.Exceptions;
using TripPlanner.Logic.Services.EmailService;

namespace TripPlanner.Controllers.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ViewBusinessTripsController : ControllerBase
    {
        private readonly IBusinessTripRequestRepository _repository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public ViewBusinessTripsController(IBusinessTripRequestRepository repository, IMapper mapper, IEmailService emailService)
        {
            _repository = repository;
            _mapper = mapper;
            _emailService = emailService;
        }

        [Route("ViewTrips")]
        [HttpPost]
        public async Task<IEnumerable<IBusinessTrip>> GetTrips([FromBody] SearchCriteriaApiModel? searchCriteria)
        {
            var email = User.Identity.Name ?? throw new IdentityUserNameNotFoundException("No username was found!");
            var criteria = _mapper.Map<SearchCriteria>(searchCriteria);
            var trips = await _repository.GetTripsByCriteria(criteria, email);

            return trips;
        }

        [Route("UpdateTripStatus/{id}")]
        [HttpPut]
        public async Task<bool> UpdateTripStatus(Guid id, [FromQuery] UpdateStatusApiModel updateStatusApiModel)
        {
            var email = User.Identity.Name ?? throw new IdentityUserNameNotFoundException("No username was found!");
            var updateStatusModel = _mapper.Map<UpdateStatusModel>(updateStatusApiModel);
            var result = await _repository.UpdateStatus(id, updateStatusModel, email);
            if (result == true)
            {
                switch (updateStatusModel.Status)
                {
                    case RequestStatus.Accepted: _emailService.SendEmail("accepted", email); break;
                    case RequestStatus.Rejected: _emailService.SendEmail("rejected", email); break;
                }
                return result;
            }
            else
            {
                return false;
            }
        }
    }
}

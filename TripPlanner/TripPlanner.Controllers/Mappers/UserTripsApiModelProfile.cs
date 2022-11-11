using AutoMapper;
using TripPlanner.ApiModels.ApiModels;
using TripPlanner.Logic.Common;

namespace TripPlanner.Controllers.Mappers
{
    public class UserTripsApiModelProfile : Profile
    {
        public UserTripsApiModelProfile()
        {
            CreateMap<UserTripsApiModel, GetTripsForUser>();
        }
    }
}

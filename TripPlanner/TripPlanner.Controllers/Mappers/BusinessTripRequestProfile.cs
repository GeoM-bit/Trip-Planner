using AutoMapper;
using TripPlanner.DatabaseModels.Models;
using TripPlanner.Logic.DtoModels;

namespace TripPlanner.Controllers.Mappers
{
    public class BusinessTripRequestProfile : Profile
    {
        public BusinessTripRequestProfile()
        {
            CreateMap<BusinessTripRequest, UserBusinessTripDto>();
            CreateMap<BusinessTripRequest, BtoBusinessTripDto>();
        }
    }
}

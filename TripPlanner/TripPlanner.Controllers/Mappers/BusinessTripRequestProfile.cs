using AutoMapper;
using TripPlanner.ApiModels.ApiModels;
using TripPlanner.DatabaseModels.Models;
using TripPlanner.Logic.Common;
using TripPlanner.Logic.DtoModels;

namespace TripPlanner.Controllers.Mappers
{
    public class BusinessTripRequestProfile : Profile
    {
        public BusinessTripRequestProfile()
        {
            CreateMap<BusinessTripRequest, UserBusinessTripDto>();
            CreateMap<RegisterBusinessTripApiModel, RegisterBusinessTripDto>();
            CreateMap<RegisterBusinessTripDto, BusinessTripRequest>();
            CreateMap<BusinessTripRequest, UserBusinessTrip>();
        }
    }
}

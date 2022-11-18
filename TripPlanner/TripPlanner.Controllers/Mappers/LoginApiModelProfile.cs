using AutoMapper;
using TripPlanner.ApiModels.ApiModels;
using TripPlanner.Logic.DtoModels;

namespace TripPlanner.Controllers.Mappers
{
    public class LoginApiModelProfile : Profile
    {
        public LoginApiModelProfile()
        {
            CreateMap<LoginApiModel, LoginDto>();
        }
    }
}

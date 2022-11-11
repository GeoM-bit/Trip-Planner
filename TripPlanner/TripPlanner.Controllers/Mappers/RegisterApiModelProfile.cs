using AutoMapper;
using TripPlanner.ApiModels.ApiModels;
using TripPlanner.Logic.DtoModels;

namespace TripPlanner.Controllers.Mappers
{
    public class RegisterApiModelProfile : Profile
    {
        public RegisterApiModelProfile()
        {
            CreateMap<RegisterApiModel, RegisterUserDto>();
        }
    }
}

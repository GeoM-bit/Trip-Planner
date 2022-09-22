using AutoMapper;
using TripPlanner.DatabaseModels.Models;
using TripPlanner.Logic.DtoModels;

namespace TripPlanner.Controllers.Mappers
{
   public class UserProfile : Profile
   {
        public UserProfile()
        {
            CreateMap<RegisterUserDto, User>();
            CreateMap<LoginDto, User>();

        }
    }
}

using AutoMapper;
using TripPlanner.DatabaseModels.Models;
using TripPlanner.Logic.DtoModels;

namespace TripPlanner.Controllers.Mappers
{
   public class UserProfile : Profile
   {
        public UserProfile()
        {
            CreateMap<RegisterUserDto, User>()
                .ForMember(dest=>dest.UserName, opt=>opt.MapFrom(src=>src.Email));
            CreateMap<LoginDto, User>();

        }
    }
}

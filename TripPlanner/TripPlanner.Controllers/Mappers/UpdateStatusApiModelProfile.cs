using AutoMapper;
using TripPlanner.ApiModels.ApiModels;
using TripPlanner.Logic.DtoModels;

namespace TripPlanner.Controllers.Mappers
{
    public class UpdateStatusApiModelProfile : Profile
    {
        public UpdateStatusApiModelProfile()
        {
            CreateMap<UpdateStatusApiModel, UpdateStatusModel>();
        }
    }
}

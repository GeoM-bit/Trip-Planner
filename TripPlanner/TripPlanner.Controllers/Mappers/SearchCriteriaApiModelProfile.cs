using AutoMapper;
using TripPlanner.ApiModels.ApiModels;
using TripPlanner.Logic.Common;

namespace TripPlanner.Controllers.Mappers
{
    public class SearchCriteriaApiModelProfile : Profile
    {
        public SearchCriteriaApiModelProfile()
        {
            CreateMap<SearchCriteriaApiModel, SearchCriteria>();
        }
    }
}

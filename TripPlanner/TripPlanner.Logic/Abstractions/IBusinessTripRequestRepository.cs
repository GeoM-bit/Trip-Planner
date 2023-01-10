using TripPlanner.DatabaseModels.Models;
using TripPlanner.DatabaseModels.Models.Enums;
using TripPlanner.Logic.Common;
using TripPlanner.Logic.DtoModels;

namespace TripPlanner.Logic.Abstractions
{
    public interface IBusinessTripRequestRepository
    {
        Task<bool> CreateTrip(BusinessTripRequest trip);
        Task<IEnumerable<IBusinessTrip>> GetTripsByCriteria(SearchCriteria searchCriteria, string email);
        Task<bool> UpdateStatus(Guid id, UpdateStatusModel updateStatusModel, string email);
    }
}

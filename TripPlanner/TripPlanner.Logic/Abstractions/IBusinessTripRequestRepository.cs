using TripPlanner.DatabaseModels.Models;
using TripPlanner.Logic.Common;
using TripPlanner.Logic.Common.Enums;

namespace TripPlanner.Logic.Abstractions
{
    public interface IBusinessTripRequestRepository
    {
        Task<bool> CreateTrip(BusinessTripRequest trip);
        Task<IEnumerable<BusinessTripRequest>> GetAllTrips();
        Task<bool> UpdateTrip(Guid id, BusinessTripRequest trip);
        Task<bool> DeleteTrip(Guid id);
        Task<IEnumerable<IBusinessTrip>> GetTripsByCriteria(SearchCriteria searchCriteria, string email);

    }
}

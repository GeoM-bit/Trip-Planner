using TripPlanner.DatabaseModels.Models;
using TripPlanner.Logic.Common;

namespace TripPlanner.Logic.Repositories
{
    public interface IBusinessTripRequestRepository
    {
        Task<bool> CreateTrip(BusinessTripRequest trip);
        Task<IEnumerable<BusinessTripRequest>> GetAllTrips();
        Task<bool> UpdateTrip(Guid id, BusinessTripRequest trip);
        Task<bool> DeleteTrip(Guid id);
        Task<IEnumerable<BusinessTripRequest>> GetAllTripsForUserByCriteria(GetTripsForUser getTripsForUser);
        Task<IEnumerable<BtoTripsModel>> GetPendingRequestsByCriteria(SearchCriteria searchCriteria);
    }
}

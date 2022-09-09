using TripPlanner.DatabaseModels.Models;

namespace TripPlanner.Logic.Repositories
{
    public interface IBusinessTripRequestRepository
    {
        Task<bool> CreateTrip(BusinessTripRequest trip);
        Task<IEnumerable<BusinessTripRequest>> GetAllTrips();
        Task<bool> UpdateTrip(Guid id, BusinessTripRequest trip);
        Task<bool> DeleteTrip(Guid id);
    }
}

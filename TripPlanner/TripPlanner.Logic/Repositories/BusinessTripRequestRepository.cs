using Microsoft.EntityFrameworkCore;
using TripPlanner.Context;
using TripPlanner.DatabaseModels.Models;
using TripPlanner.Logic.Exceptions;

namespace TripPlanner.Logic.Repositories
{
    public class BusinessTripRequestRepository : IBusinessTripRequestRepository
    {
        private readonly TripPlannerContext _context;

        public BusinessTripRequestRepository(TripPlannerContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateTrip(BusinessTripRequest trip)
        {
           await _context.BusinessTripRequests.AddAsync(trip);
           var result = await _context.SaveChangesAsync();

           if (result > 0)
                return true;
            return false;
        }

        public async Task<IEnumerable<BusinessTripRequest>> GetAllTrips()
        {
            return await _context.BusinessTripRequests.ToListAsync();
        }

        public async Task<bool> UpdateTrip(Guid id, BusinessTripRequest trip)
        {
            var tripFromDb = await _context.BusinessTripRequests.FirstOrDefaultAsync(x => x.Id == id);
            if (tripFromDb == null)
                throw new EntityNotFoundException("The Business Trip to be updated does not exist!");

            tripFromDb.Area = trip.Area;
            tripFromDb.ProjectNumber = trip.ProjectNumber;
            tripFromDb.TaskNumber = trip.TaskNumber;
            tripFromDb.PmName = trip.PmName;
            tripFromDb.Accomodation = trip.Accomodation;
            tripFromDb.TaskName = trip.TaskName;
            tripFromDb.AdditionalInfo = trip.AdditionalInfo;
            tripFromDb.Card = trip.Card;
            tripFromDb.Client = trip.Client;
            tripFromDb.ClientLocation = trip.ClientLocation;
            tripFromDb.StartDate = trip.StartDate;
            tripFromDb.EndDate = tripFromDb.EndDate;
            _context.BusinessTripRequests.Update(tripFromDb);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
                return true;
            return false;
        }

        public async Task<bool> DeleteTrip(Guid id)
        {
            var tripFromDb = await _context.BusinessTripRequests.FirstOrDefaultAsync(x=>x.Id==id);
            if(tripFromDb == null)
                throw new EntityNotFoundException("The Business Trip to be deleted does not exist!");

            _context.BusinessTripRequests.Remove(tripFromDb);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
                return true;
            return false;
        }     
    }
}

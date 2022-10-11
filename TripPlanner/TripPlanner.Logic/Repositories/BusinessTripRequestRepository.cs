using Microsoft.EntityFrameworkCore;
using TripPlanner.Context;
using TripPlanner.DatabaseModels.Models;
using TripPlanner.DatabaseModels.Models.Enums;
using TripPlanner.Logic.Common;
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
            tripFromDb.Accommodation = trip.Accommodation;
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

        public async Task<IEnumerable<BusinessTripRequest>> GetAllTripsForUserByCriteria(string email, SearchCriteria searchCriteria)
        {
            var model = await _context.BusinessTripRequests.Where(e => e.Email.Equals(email)).ToListAsync();
            model = FilterBusinessTripsByCriteria(model, searchCriteria).Result;
            return model;
        }

        public async Task<IEnumerable<BusinessTripRequest>> GetPendingRequestsByCriteria(SearchCriteria searchCriteria)
        {
            var model = await _context.BusinessTripRequests.
                Where(b => b.Status == RequestStatus.Pending).ToListAsync();
            model = FilterBusinessTripsByCriteria(model, searchCriteria).Result;
            return model;
        }

        private Task<List<BusinessTripRequest>> FilterBusinessTripsByCriteria(IEnumerable<BusinessTripRequest> businessTrips, SearchCriteria searchCriterias)
        {
            IQueryable<BusinessTripRequest> result = businessTrips.AsQueryable<BusinessTripRequest>();

            if (searchCriterias == null)
            {
                return Task.FromResult(result.Where(bt => bt.Status.Equals(RequestStatus.Pending)).ToList());
            }
            result = result.Where(bt => bt.Status.Equals(searchCriterias.Status));
            if (!string.IsNullOrEmpty(searchCriterias.Location))
            {
                result = result
                                .Where(bt => bt.ClientLocation.Equals(searchCriterias.Location));
            }

            if (!string.IsNullOrEmpty(searchCriterias.Accomodation))
            {
                result = result
                                .Where(bt => bt.Accommodation.Equals(searchCriterias.Accomodation));
            }

            if (!string.IsNullOrEmpty(searchCriterias.Client))
            {
                result = result
                                .Where(bt => bt.Client.Equals(searchCriterias.Client));
            }

            if (searchCriterias.StartDate != null && searchCriterias.EndDate != null)
            {
                result = result
                                .Where(bt => bt.StartDate >= searchCriterias.StartDate && bt.EndDate <= searchCriterias.EndDate);
            }

            return Task.FromResult(result.ToList());
        }

    }
}

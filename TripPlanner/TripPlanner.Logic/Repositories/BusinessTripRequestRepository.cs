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

        public async Task<IEnumerable<BusinessTripRequest>> GetAllTripsForUserByCriteria(GetTripsForUser getTripsForUser)
        {
            var model = _context.BusinessTripRequests.Where(e => e.Email.Equals(getTripsForUser.Email));
            var result = FilterUserBusinessTripsByCriteria(model, getTripsForUser.SearchCriteria).Result;
          
            return result;
        }

        public async Task<IEnumerable<BtoTripsModel>> GetPendingRequestsByCriteria(SearchCriteria searchCriteria)
        {
            var model = _context.BusinessTripRequests.
                    Where(b => b.Status == RequestStatus.Pending)
                    .Join(
                    _context.Users,
                    user => user.Email,
                    request => request.Email,
                    (request, user) => new
                    {
                        request.Id,
                        user.FirstName,
                        user.LastName,
                        user.Email,
                        request.ProjectName,
                        request.PmName,
                        request.Area,
                        request.StartDate,
                        request.EndDate,
                        request.Accommodation,
                        request.ClientLocation,
                        request.Status,
                        request.Client
                    }).Select(bt => new BtoTripsModel()
                    {
                        Id = bt.Id,
                        FirstName = bt.FirstName,
                        LastName = bt.LastName,
                        PMName = bt.PmName,
                        Email = bt.Email,
                        Area = (AreaType)bt.Area,
                        ProjectName = bt.ProjectName,
                        ClientLocation = bt.ClientLocation,
                        StartDate = bt.StartDate,
                        EndDate = bt.EndDate,
                        Client = bt.Client,
                        Accommodation = bt.Accommodation,
                    });
            var result = FilterBtoBusinessTripsByCriteria(model, searchCriteria).Result;

            return result;
        }

        private async Task<List<BusinessTripRequest>> FilterUserBusinessTripsByCriteria(IQueryable<BusinessTripRequest> businessTrips, SearchCriteria searchCriteria)
        {
            if(searchCriteria==null)
            {
                return await businessTrips.ToListAsync();
            }

            if(searchCriteria.Status!=null)
            {
                businessTrips=businessTrips.Where(b => b.Status == searchCriteria.Status);
            }

            if (searchCriteria.Client != null)
            {
                businessTrips = businessTrips.Where(b => b.Client == searchCriteria.Client);
            }

            if (searchCriteria.Accommodation != null)
            {
                businessTrips = businessTrips.Where(b => b.Accommodation == searchCriteria.Accommodation);
            }

            if (searchCriteria.ClientLocation != null)
            {
                businessTrips = businessTrips.Where(b => b.ClientLocation == searchCriteria.ClientLocation);
            }

            if (searchCriteria.StartDate != null && searchCriteria.EndDate!= null)
            {
                businessTrips = businessTrips.Where(bt => bt.StartDate >= searchCriteria.StartDate && bt.EndDate <= searchCriteria.EndDate);
            }

            return await businessTrips.ToListAsync();
        }

        private async Task<List<BtoTripsModel>> FilterBtoBusinessTripsByCriteria(IQueryable<BtoTripsModel> businessTrips, SearchCriteria searchCriteria)
        {
            if (searchCriteria == null)
            {
                return await businessTrips.ToListAsync();
            }

            if (searchCriteria.Client != null)
            {
                businessTrips = businessTrips.Where(b => b.Client == searchCriteria.Client);
            }

            if (searchCriteria.Accommodation != null)
            {
                businessTrips = businessTrips.Where(b => b.Accommodation == searchCriteria.Accommodation);
            }

            if (searchCriteria.ClientLocation != null)
            {
                businessTrips = businessTrips.Where(b => b.ClientLocation == searchCriteria.ClientLocation);
            }

            if (searchCriteria.StartDate != null && searchCriteria.EndDate != null)
            {
                businessTrips = businessTrips.Where(bt => bt.StartDate >= searchCriteria.StartDate && bt.EndDate <= searchCriteria.EndDate);
            }

            return await businessTrips.ToListAsync();
        }

    }
}

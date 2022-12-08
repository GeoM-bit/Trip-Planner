using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TripPlanner.Context;
using TripPlanner.DatabaseModels.Models;
using TripPlanner.DatabaseModels.Models.Enums;
using TripPlanner.Logic.Abstractions;
using TripPlanner.Logic.Common;
using TripPlanner.Logic.Common.Enums;
using TripPlanner.Logic.DtoModels;
using TripPlanner.Logic.Exceptions;

namespace TripPlanner.Logic.Repositories
{
    public class BusinessTripRequestRepository : IBusinessTripRequestRepository
    {
        private readonly TripPlannerContext _context;
        private readonly IMapper _mapper;

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

        public async Task<bool> UpdateStatus(UpdateStatusModel updateStatusModel)
        {
            var currentBusinessTripRequest = await _context.BusinessTripRequests.FirstOrDefaultAsync(user => user.Id.Equals(updateStatusModel.Id));
            currentBusinessTripRequest.Status = (updateStatusModel.Status);
            _context.BusinessTripRequests.Update(currentBusinessTripRequest);
            return await _context.SaveChangesAsync() == 1;
        }

        public async Task<IEnumerable<IBusinessTrip>> GetTripsByCriteria(SearchCriteria searchCriteria, string email)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(x=>x.Email==email);
            var role = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == dbUser.Id);

            if(role.RoleId==Constants.UserRoleId)
            {
                var model = _context.BusinessTripRequests
                    .Where(e => e.Email.Equals(email))
                    .Select(bt=>new UserBusinessTrip()
                    {
                        Id = bt.Id,
                        ProjectName = bt.ProjectName,
                        ClientLocation = bt.ClientLocation,
                        StartDate = bt.StartDate,
                        EndDate = bt.EndDate,
                        Client = bt.Client,
                        Status = bt.Status,
                        Accommodation = bt.Accommodation
                    })
                    ;
                var result = FilterUserBusinessTripsByCriteria(model, searchCriteria).Result;

                return result;
            }

            if(role.RoleId==Constants.BtoRoleId)
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
                        request.Client
                    }).Select(bt => new BtoBusinessTrip()
                    {
                        Id = bt.Id,
                        FirstName = bt.FirstName,
                        LastName = bt.LastName,
                        PMName = bt.PmName,
                        Email = bt.Email,
                        Area = bt.Area,
                        ProjectName = bt.ProjectName,
                        ClientLocation = bt.ClientLocation,
                        StartDate = bt.StartDate,
                        EndDate = bt.EndDate,
                        Client = bt.Client,
                        Accommodation = bt.Accommodation
                    });
                var result = FilterBtoBusinessTripsByCriteria(model, searchCriteria).Result;

                return result;
            }

            return null;
        }

        private async Task<List<UserBusinessTrip>> FilterUserBusinessTripsByCriteria(IQueryable<UserBusinessTrip> businessTrips, SearchCriteria searchCriteria)
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

        private async Task<List<BtoBusinessTrip>> FilterBtoBusinessTripsByCriteria(IQueryable<BtoBusinessTrip> businessTrips, SearchCriteria searchCriteria)
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

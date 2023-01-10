using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TripPlanner.Context;
using TripPlanner.DatabaseModels.Models;
using TripPlanner.DatabaseModels.Models.Enums;
using TripPlanner.Logic.Abstractions;
using TripPlanner.Logic.Common;
using TripPlanner.Logic.DtoModels;
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

        public async Task<bool> UpdateStatus(Guid id, UpdateStatusModel updateStatusModel, string email)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == email) ?? throw new EntityNotFoundException($"The user with the email: {email} does not exist!");
            var role = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == dbUser.Id) ?? throw new EntityNotFoundException($"The user with the id:{dbUser.Id} does not have a role!");

            if (role.RoleId == Constants.UserRoleId)
            {
                if (updateStatusModel.Status != RequestStatus.Cancelled && updateStatusModel.Status != RequestStatus.Accepted)
                    throw new InvalidStatusChangeRequestException($"The user is not allowed to update to {updateStatusModel.Status} status!");
            }

            if (role.RoleId == Constants.BtoRoleId)
            {
                if (updateStatusModel.Status != RequestStatus.Accepted && updateStatusModel.Status != RequestStatus.Rejected)
                    throw new InvalidStatusChangeRequestException($"The BTO is not allowed to update to {updateStatusModel.Status} status!");
            }

            var currentBusinessTripRequest = await _context.BusinessTripRequests.FirstOrDefaultAsync(user => user.Id == id);
            if (currentBusinessTripRequest == null)
                throw new EntityNotFoundException($"The business trip with id {id} was not found");

            currentBusinessTripRequest.Status = (updateStatusModel.Status);
            _context.BusinessTripRequests.Update(currentBusinessTripRequest);

            return await _context.SaveChangesAsync() == 1;
        }

        public async Task<IEnumerable<IBusinessTrip>> GetTripsByCriteria(SearchCriteria searchCriteria, string email)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(x=>x.Email==email) ?? throw new EntityNotFoundException($"The user with the email: {email} does not exist!");
            var role = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == dbUser.Id) ?? throw new EntityNotFoundException($"The user with the id:{dbUser.Id} does not have a role!"); ;

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

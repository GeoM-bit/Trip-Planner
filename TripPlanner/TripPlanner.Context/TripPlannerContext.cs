using Microsoft.EntityFrameworkCore;
using TripPlanner.DatabaseModels.Models;

namespace TripPlanner.Context
{
    public class TripPlannerContext : DbContext
    {
        public TripPlannerContext (DbContextOptions<TripPlannerContext> options) : base (options)
        { }

        public DbSet<BusinessTripRequest> BusinessTripRequests { get; set; }
    }
}
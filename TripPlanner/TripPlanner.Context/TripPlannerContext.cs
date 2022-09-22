using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TripPlanner.DatabaseModels.Models;

namespace TripPlanner.Context
{
    public class TripPlannerContext : IdentityDbContext<User,Role,Guid>
    {
        public TripPlannerContext (DbContextOptions<TripPlannerContext> options) : base (options)
        { }

        public DbSet<BusinessTripRequest> BusinessTripRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Role>().HasData(
                new Role() { Id = new Guid("e41581ea-fc9b-4317-8945-199c92473e7a"), Name = "BTO", ConcurrencyStamp = "1", NormalizedName = "Business Trip Operator" },
                new Role() { Id = new Guid("e3d694f0-d921-43b9-8a5c-f2b4c8e28e11"), Name = "User", ConcurrencyStamp = "2", NormalizedName = "User" }
                );
        }
    }
}
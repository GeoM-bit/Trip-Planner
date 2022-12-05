using TripPlanner.DatabaseModels.Models.Enums;
using TripPlanner.Logic.Abstractions;

namespace TripPlanner.Logic.DtoModels
{
    public class BtoBusinessTrip : IBusinessTrip
    {
        public Guid Id { get; set; }
        public string ClientLocation { get; set; }
        public string Accommodation { get; set; }
        public string ProjectName { get; set; }
        public string Client { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PMName { get; set; }
        public AreaType Area { get; set; }
    }
}

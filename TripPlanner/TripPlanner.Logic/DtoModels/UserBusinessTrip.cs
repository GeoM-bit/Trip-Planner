using TripPlanner.DatabaseModels.Models.Enums;
using TripPlanner.Logic.Abstractions;

namespace TripPlanner.Logic.DtoModels
{
    public class UserBusinessTrip : IBusinessTrip
    {
        public Guid Id { get; set; }
        public string ClientLocation { get; set; }
        public string Accommodation { get; set; }
        public string ProjectName { get; set; }
        public string Client { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public RequestStatus Status { get; set; }

    }
}

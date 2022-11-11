namespace TripPlanner.Logic.DtoModels
{
    public class BusinessTripBaseDto
    {
        public Guid Id { get; set; }

        public string ClientLocation { get; set; }

        public string Accommodation { get; set; }

        public string ProjectName { get; set; }

        public string Client { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }
    }
}

namespace TripPlanner.Logic.DtoModels
{
    public class BaseDto
    {
        public Guid Id { get; set; }

        public string ClientLocation { get; set; }

        public string Accomodation { get; set; }

        public string ProjectName { get; set; }

        public string ClientName { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }
    }
}

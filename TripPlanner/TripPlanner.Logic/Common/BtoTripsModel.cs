using TripPlanner.DatabaseModels.Models.Enums;

namespace TripPlanner.Logic.Common
{
    public class BtoTripsModel
    {
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string ProjectName { get; set; }
		public string PMName { get; set; }
		public string Client { get; set; }
		public string ClientLocation { get; set; }
		public AreaType Area { get; set; }
		public DateTimeOffset StartDate { get; set; }
		public DateTimeOffset EndDate { get; set; }
		public string Accommodation { get; set; }
	}
}

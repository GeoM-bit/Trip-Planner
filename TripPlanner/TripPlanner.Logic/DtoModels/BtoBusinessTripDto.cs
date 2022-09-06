using TripPlanner.DatabaseModels.Models.Enums;

namespace TripPlanner.Logic.DtoModels
{
    public class BtoBusinessTripDto : BaseDto
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public string PMName { get; set; }

		public AreaType Area { get; set; }
	}
}

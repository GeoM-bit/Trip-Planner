using TripPlanner.DatabaseModels.Models.Enums;

namespace TripPlanner.Logic.DtoModels
{
    public class UserBusinessTripDto : BaseDto 
	{
		public RequestStatus Status { get; set; }
	}
}

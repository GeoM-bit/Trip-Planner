using TripPlanner.DatabaseModels.Models.Enums;

namespace TripPlanner.Logic.DtoModels
{
    public class UserBusinessTripDto : BusinessTripBaseDto 
	{
		public RequestStatus Status { get; set; }
	}
}

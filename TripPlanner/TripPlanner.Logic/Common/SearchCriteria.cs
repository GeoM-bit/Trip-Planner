using TripPlanner.DatabaseModels.Models.Enums;

namespace TripPlanner.Logic.Common
{
	public class SearchCriteria
	{
		public string? ClientLocation { get; set; }
		public string? Accommodation { get; set; }
		public string? Client { get; set; }
		public RequestStatus? Status { get; set; }
		public DateTimeOffset? StartDate { get; set; }
		public DateTimeOffset? EndDate { get; set; }

		public SearchCriteria(string clientLocation, string accommodation, string client, RequestStatus? status, DateTimeOffset? startDate, DateTimeOffset? endDate)
		{
			ClientLocation = clientLocation;
			Accommodation = accommodation;
			Client = client;
			Status = status;
			StartDate = startDate;
			EndDate = endDate;
		}
	}
}

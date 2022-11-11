using TripPlanner.DatabaseModels.Models.Enums;

namespace TripPlanner.Logic.DtoModels
{
	public class RegisterBusinessTripDto
	{
		public string Email { get; set; }
		public AreaType Area { get; set; }
		public string PmName { get; set; }
		public string Client { get; set; }
		public string ProjectName { get; set; }
		public string ProjectNumber { get; set; }
		public string TaskName { get; set; }
		public string TaskNumber { get; set; }
		public string ClientLocation { get; set; }
		public string LeavingFrom { get; set; }
		public bool Phone { get; set; }
		public bool Card { get; set; }
		public string MeanOfTransport { get; set; }
		public string Accommodation { get; set; }
		public string AdditionalInfo { get; set; }
		public DateTimeOffset StartDate { get; set; }
		public DateTimeOffset EndDate { get; set; }
	}
}
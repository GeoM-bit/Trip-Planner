using System.ComponentModel.DataAnnotations;
using TripPlanner.DatabaseModels.Models.Enums;

namespace TripPlanner.DatabaseModels.Models
{
	public class BusinessTripRequest
	{
		public Guid Id { get; set; }

		[Required, MaxLength(80)]
		public string Email { get; set; }

		[Required]
		public AreaType Area { get; set; }

		[Required, MaxLength(50)]
		public string PmName { get; set; }

		[Required, MaxLength(50)]
		public string Client { get; set; }

		[Required, MaxLength(50)]
		public string ProjectName { get; set; }

		[Required, MaxLength(7)]
		public string ProjectNumber { get; set; }

		[MaxLength(50)]
		public string TaskName { get; set; }

		[MinLength(4), MaxLength(7)]
		public string TaskNumber { get; set; }

		[Required, MaxLength(50)]
		public string ClientLocation { get; set; }

		[Required, MaxLength(50)]
		public string LeavingFrom { get; set; }

		public bool Phone { get; set; }

		public bool Card { get; set; }

		[Required, MaxLength(50)]
		public string MeanOfTransport { get; set; }

		[Required, MaxLength(50)]
		public string Accommodation { get; set; }

		[MaxLength(250)]
		public string? AdditionalInfo { get; set; }

		[Required]
		public DateTimeOffset StartDate { get; set; }

		[Required]
		public DateTimeOffset EndDate { get; set; }

		[Required]
		public RequestStatus Status { get; set; }
	}
}

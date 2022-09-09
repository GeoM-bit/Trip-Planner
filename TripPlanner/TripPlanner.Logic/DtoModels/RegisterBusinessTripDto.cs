using System.ComponentModel.DataAnnotations;
using TripPlanner.DatabaseModels.Models.Enums;

namespace TripPlanner.Logic.DtoModels
{
	public class RegisterBusinessTripDto : IValidatableObject
	{
		public AreaType Area { get; set; }

		[Required(ErrorMessage = "PM name is required.")]
		[RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "PM name should be a valid name.")]
		[MaxLength(50, ErrorMessage = "Maximum length of this field is 50 characters.")]
		public string PmName { get; set; }

		[Required(ErrorMessage = "Client name is required.")]
		[RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Client name should be a valid name.")]
		[MaxLength(50, ErrorMessage = "Maximum length of this field is 50 characters.")]
		public string Client { get; set; }

		[Required(ErrorMessage = "Project name is required.")]
		[RegularExpression(@"^[A-Za-z0-9 ]+$", ErrorMessage = "Project name should be a valid name.")]
		[MaxLength(50, ErrorMessage = "Maximum length of this field is 50 characters.")]
		public string ProjectName { get; set; }

		[Required(ErrorMessage = "Project number is required.")]
		[RegularExpression("^[0-9]+$", ErrorMessage = "Project number should contain only digits.")]
		[MaxLength(7, ErrorMessage = "Maximum length of this field is 7 digits.")]
		[MinLength(4, ErrorMessage = "Minimum length of this field is 4 digits.")]
		public string ProjectNumber { get; set; }

		[RegularExpression(@"^[A-Za-z ]*$", ErrorMessage = "Task name should contain only letters.")]
		[MaxLength(50, ErrorMessage = "Maximum length of this field is 50 characters.")]
		public string TaskName { get; set; }

		[Required(ErrorMessage = "Task number is required.")]
		[RegularExpression("^[0-9]+$", ErrorMessage = "Task number should contain only digits.")]
		[MinLength(4, ErrorMessage = "Task number should be at least 4 digits long.")]
		[MaxLength(7, ErrorMessage = "Maximum length of this field is of 7 digits.")]
		public string TaskNumber { get; set; }

		[Required(ErrorMessage = "Client location is required.")]
		[RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Client location should contain only letters.")]
		[MaxLength(50, ErrorMessage = "Maximum length of this field is 50 characters.")]
		public string ClientLocation { get; set; }

		[Required(ErrorMessage = "Departure location is required.")]
		[RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Leaving location should contain only letters.")]
		[MaxLength(50, ErrorMessage = "Maximum length of this field is 50 characters.")]
		public string LeavingFrom { get; set; }

		public bool Phone { get; set; }

		public bool Card { get; set; }

		[Required(ErrorMessage = "Mean of transporatation is required.")]
		[RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "All means of transport should contain only letters.")]
		[MaxLength(50, ErrorMessage = "Maximum length of this field is 50 characters.")]
		public string MeanOfTransport { get; set; }

		[Required(ErrorMessage = "Accomodation place is required.")]
		[RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Accomodation place should contain only letters.")]
		[MaxLength(50, ErrorMessage = "Maximum length of this field is 50 characters.")]
		public string Accomodation { get; set; }

		[MaxLength(250, ErrorMessage = "Maximum length of this field is 250 characters.")]
		public string AdditionalInfo { get; set; }

		public DateTimeOffset StartDate { get; set; }

		public DateTimeOffset EndDate { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (StartDate < DateTime.Today)
			{
				yield return new ValidationResult("Start date can't be in the past", new[] { nameof(StartDate) });
			}

			if (StartDate.Date > EndDate.Date)
			{
				yield return new ValidationResult("Start date has to be before end date.", new[] { nameof(StartDate) });
			}

			if (StartDate.Date == EndDate.Date)
			{
				yield return new ValidationResult("Start date shouldn't be equal with end date", new[] { nameof(StartDate) });
			}

			if (StartDate == DateTime.Today)
			{
				yield return new ValidationResult("Start date can't be the current one", new[] { nameof(StartDate) });
			}
		}
	}
}
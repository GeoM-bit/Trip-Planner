using System.ComponentModel.DataAnnotations;

namespace TripPlanner.Logic.DtoModels
{
    public class RegisterUserDto : IValidatableObject
	{

		[Required(ErrorMessage = "First name is required.")]
		[RegularExpression("^[A-Za-z]+$", ErrorMessage = "First name can contain only letters.")]
		[MaxLength(50, ErrorMessage = "Maximum length of this field is 50 characters.")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Last name is required.")]
		[RegularExpression("^[A-Za-z]+$", ErrorMessage = "Last name can contain only letters.")]
		[MaxLength(50, ErrorMessage = "Maximum length of this field is 50 characters.")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Email address is required.")]
		[RegularExpression("^[a-zA-Z]+\\.[a-zA-Z]+@nagarro.com$", ErrorMessage = "Incorrect email (required format: <firstName>.<lastName>@nagarro.com).")]
		[MaxLength(80, ErrorMessage = "Maximum length of this field is 80 characters.")]
		public string Email { get; set; }

		public string UserName { get; set; }

		[Required(ErrorMessage = "Password is required.")]
		[DataType(DataType.Password)]
		[MinLength(10, ErrorMessage = "Password must have at least 10 characters.")]
		[MaxLength(50, ErrorMessage = "Maximum length of this field is 50 characters.")]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Required(ErrorMessage = "A password confirmation is required.")]
		[DataType(DataType.Password)]
		[MinLength(10, ErrorMessage = "Password must have at least 10 characters.")]
		[MaxLength(50, ErrorMessage = "Maximum length of this field is 50 characters.")]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmationPassword { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var validationResult = new List<string>();

			if (!Password.Any(char.IsUpper)) validationResult.Add("At least one uppercase.\n");
			if (!Password.Any(char.IsLower)) validationResult.Add("At least one lowercase.\n");
			if (!Password.Any(char.IsDigit)) validationResult.Add("At least one digit.\n");
			if (Password.Any(char.IsWhiteSpace)) validationResult.Add("No whitespaces allowed.\n");
			if (!Password.Any(c => !char.IsLetterOrDigit(c))) validationResult.Add("At least one symbol.\n");

			if (validationResult.Count != 0)
			{
				string errorMessages = "The password must contain: \n";

				foreach (var errorMessage in validationResult)
				{
					errorMessages += errorMessage;
				}

				yield return new ValidationResult(errorMessages);
			}
		}
	}
}

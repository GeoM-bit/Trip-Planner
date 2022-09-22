using System.ComponentModel.DataAnnotations;

namespace TripPlanner.Logic.DtoModels
{
    public class LoginDto
	{
		[Required]
		[EmailAddress]
		[MaxLength(80, ErrorMessage = "Maximum length of this field is 80 characters.")]
		public string? Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[MaxLength(50, ErrorMessage = "Maximum length of this field is 50 characters.")]
		public string? Password { get; set; }

		public string Role { get; set; }
	}
}

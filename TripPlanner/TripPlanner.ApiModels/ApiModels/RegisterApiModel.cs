using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TripPlanner.ApiModels.ApiModels
{
    public class RegisterApiModel : IValidatableObject
    {
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "FirstName can only contain letters.")]
        public string FirstName { get; set; }
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "LastName can only contain letters.")]
        public string LastName { get; set; }
        [EmailAddress]
		public string Email { get; set; }
		public string Password { get; set; }
		public string ConfirmationPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return ApiValidations.ApiValidations.ValidatePassword(Password);

            if (ConfirmationPassword!=null)
            {
                if(ConfirmationPassword.CompareTo(Password)!=0)
                {
                    yield return new ValidationResult(errorMessage: "Paaswords do not match.", memberNames: new[] { "ConfirmationPassword" });
                }
            }
            else
            {
                yield return new ValidationResult(errorMessage: "Required ConfirmationPassword.", memberNames: new[] { "ConfirmationPassword" });
            }
        }
    }
}


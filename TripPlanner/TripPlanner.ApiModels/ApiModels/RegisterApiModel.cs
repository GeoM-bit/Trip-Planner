using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TripPlanner.ApiModels.ApiModels
{
    public class RegisterApiModel : IValidatableObject
    {
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string ConfirmationPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return ApiValidations.ApiValidations.ValidateOnlyLetters(FirstName, nameof(FirstName));
            yield return ApiValidations.ApiValidations.ValidateRequired(FirstName, nameof(FirstName));

            yield return ApiValidations.ApiValidations.ValidateOnlyLetters(LastName, nameof(LastName));
            yield return ApiValidations.ApiValidations.ValidateRequired(LastName, nameof(LastName));

            yield return ApiValidations.ApiValidations.ValidateEmail(Email);

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


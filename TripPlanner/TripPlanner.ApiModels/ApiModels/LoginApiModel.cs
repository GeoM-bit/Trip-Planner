using System.ComponentModel.DataAnnotations;

namespace TripPlanner.ApiModels.ApiModels
{
    public class LoginApiModel : IValidatableObject
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return ApiValidations.ApiValidations.ValidatePassword(Password);
        }
    }
}

using System.ComponentModel.DataAnnotations;
using TripPlanner.Logic.Common;

namespace TripPlanner.ApiModels.ApiModels
{
    public class UserTripsApiModel: IValidatableObject
	{
        public string Email { get; set; }
		public SearchCriteria? searchCriteria { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
            yield return ApiValidations.ApiValidations.ValidateEmail(Email);

            if (searchCriteria != null) {

                foreach (ValidationResult result in ApiValidations.ApiValidations.ValidateSearchCriteria(searchCriteria))
                {
                    yield return result;
                }
            }
        }
	}
}

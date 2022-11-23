using System.ComponentModel.DataAnnotations;
using TripPlanner.DatabaseModels.Models.Enums;
using TripPlanner.Logic.Common;

namespace TripPlanner.ApiModels.ApiModels
{
    public class SearchCriteriaApiModel : IValidatableObject
    {
		public string? ClientLocation { get; set; }
		public string? Accommodation { get; set; }
		public string? Client { get; set; }
		public RequestStatus? Status { get; set; }
		public DateTimeOffset? StartDate { get; set; }
		public DateTimeOffset? EndDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (ValidationResult result in ApiValidations.ApiValidations.ValidateSearchCriteria(new SearchCriteria(ClientLocation, Accommodation, Client, Status, StartDate, EndDate)))
            {
                yield return result;
            }
        }
    }
}

using System.ComponentModel.DataAnnotations;
using TripPlanner.DatabaseModels.Models.Enums;

namespace TripPlanner.ApiModels.ApiModels
{
    public class UpdateStatusApiModel : IValidatableObject
    {
        public RequestStatus Status { get; set; }
        public Guid Id { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Status != null)
            {
                if (!Enum.IsDefined(typeof(RequestStatus), Status))
                {
                    yield return new ValidationResult(errorMessage: "Invalid Status.", memberNames: new[] { "Status" });
                }
            }
            else
            {
                yield return new ValidationResult(errorMessage: "Required Status.", memberNames: new[] { "Status" });
            }

            if (Id == null)
            {
                yield return new ValidationResult(errorMessage: "Required Id.", memberNames: new[] { "Id" });
            }
        }
    }
}

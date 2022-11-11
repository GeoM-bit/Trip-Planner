using System.ComponentModel.DataAnnotations;
using TripPlanner.DatabaseModels.Models.Enums;

namespace TripPlanner.ApiModels.ApiModels
{
    public class RegisterBusinessTripApiModel : IValidatableObject
    {
        public string Email { get; set; }
        public AreaType? Area { get; set; }
        public string PmName { get; set; }
        public string Client { get; set; }
        public string ProjectName { get; set; }
        public string ProjectNumber { get; set; }
        public string TaskName { get; set; }
        public string TaskNumber { get; set; }
        public string ClientLocation { get; set; }
        public string LeavingFrom { get; set; }
        public bool? Phone { get; set; }
        public bool? Card { get; set; }
        public string MeanOfTransport { get; set; }
        public string Accommodation { get; set; }
        public string? AdditionalInfo { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return ApiValidations.ApiValidations.ValidateEmail(Email);

            yield return ApiValidations.ApiValidations.ValidateOnlyLetters(PmName, nameof(PmName));
            yield return ApiValidations.ApiValidations.ValidateLength(PmName, nameof(PmName));
            yield return ApiValidations.ApiValidations.ValidateRequired(PmName, nameof(PmName));

            yield return ApiValidations.ApiValidations.ValidateOnlyLetters(Client, nameof(Client));
            yield return ApiValidations.ApiValidations.ValidateLength(Client, nameof(Client));
            yield return ApiValidations.ApiValidations.ValidateRequired(Client, nameof(Client));

            foreach (ValidationResult result in ApiValidations.ApiValidations.ValidateLettersDigits(ProjectName, nameof(ProjectName)))
            {
                yield return result;
            }
            yield return ApiValidations.ApiValidations.ValidateRequired(ProjectName, nameof(ProjectName));

            foreach (ValidationResult result in ApiValidations.ApiValidations.ValidateOnlyDigitsLength(ProjectNumber, nameof(ProjectNumber)))
            {
                yield return result;
            }
            yield return ApiValidations.ApiValidations.ValidateRequired(ProjectNumber, nameof(ProjectNumber));

            yield return ApiValidations.ApiValidations.ValidateOnlyLetters(TaskName, nameof(TaskName));
            yield return ApiValidations.ApiValidations.ValidateLength(TaskName, nameof(TaskName));
            yield return ApiValidations.ApiValidations.ValidateRequired(TaskName, nameof(TaskName));

            foreach (ValidationResult result in ApiValidations.ApiValidations.ValidateOnlyDigitsLength(TaskNumber, nameof(TaskNumber)))
            {
                yield return result;
            }
            yield return ApiValidations.ApiValidations.ValidateRequired(TaskNumber, nameof(TaskNumber));

            yield return ApiValidations.ApiValidations.ValidateOnlyLetters(ClientLocation, nameof(ClientLocation));
            yield return ApiValidations.ApiValidations.ValidateLength(ClientLocation, nameof(ClientLocation));
            yield return ApiValidations.ApiValidations.ValidateRequired(ClientLocation, nameof(ClientLocation));

            yield return ApiValidations.ApiValidations.ValidateOnlyLetters(LeavingFrom, nameof(LeavingFrom));
            yield return ApiValidations.ApiValidations.ValidateLength(LeavingFrom, nameof(LeavingFrom));
            yield return ApiValidations.ApiValidations.ValidateRequired(LeavingFrom, nameof(LeavingFrom));

            yield return ApiValidations.ApiValidations.ValidateOnlyLetters(MeanOfTransport, nameof(MeanOfTransport));
            yield return ApiValidations.ApiValidations.ValidateLength(MeanOfTransport, nameof(MeanOfTransport));
            yield return ApiValidations.ApiValidations.ValidateRequired(MeanOfTransport, nameof(MeanOfTransport));

            yield return ApiValidations.ApiValidations.ValidateOnlyLetters(Accommodation, nameof(Accommodation));
            yield return ApiValidations.ApiValidations.ValidateLength(Accommodation, nameof(Accommodation));
            yield return ApiValidations.ApiValidations.ValidateRequired(Accommodation, nameof(Accommodation));

            foreach (ValidationResult result in ApiValidations.ApiValidations.ValidateDates(StartDate, EndDate))
            {
                yield return result;
            }

            if (AdditionalInfo != null)
            {
                if (AdditionalInfo.Length>250)
                {
                    yield return new ValidationResult(errorMessage: "Maximum length of AdditionalInfo field is 250 characters.", memberNames: new[] { nameof(AdditionalInfo) });
                }
            }

            if (Area != null)
            {
                if (!Enum.IsDefined(typeof(AreaType), Area))
                {
                    yield return new ValidationResult(errorMessage: "Invalid Area.", memberNames: new[] { "Area" });
                }
            }
            else
            {
                yield return new ValidationResult(errorMessage: "Required Area.", memberNames: new[] { "Area" });
            }
        }
    }
}

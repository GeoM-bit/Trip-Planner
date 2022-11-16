using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using TripPlanner.DatabaseModels.Models.Enums;
using TripPlanner.Logic.Common;

namespace TripPlanner.ApiModels.ApiValidations
{
    public class ApiValidations 
    {
        public static ValidationResult ValidateEmail(string email)
        {
            if (email != null)
            {
                if (!Regex.Match(email, "^[a-zA-Z]+\\.[a-zA-Z]+@nagarro.com$").Success)
                {
                    return new ValidationResult(errorMessage: "Invalid email.", memberNames: new[] { "Email" });
                }

                return ValidationResult.Success;
            }

            return new ValidationResult(errorMessage: "Email required.", memberNames: new[] { "Email" });
        }

        public static ValidationResult ValidatePassword(string password)
        {
            if (password != null)
            {
                if (!Regex.Match(password, "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#=+';:_,.?!@$%^&*-]).{10,}$").Success)
                {
                  return new ValidationResult(errorMessage: "Invalid password.", memberNames: new[] { "Password" });
                }

                return ValidationResult.Success;
            }

            return new ValidationResult(errorMessage: "Password required.", memberNames: new[] { "Password" });
        }

        public static IEnumerable<ValidationResult> ValidateDates(DateTimeOffset? startDate, DateTimeOffset? endDate)
        {
            
            if (startDate == null && endDate!=null)
            {
                yield return new ValidationResult(errorMessage: "Missing StartDate.", memberNames: new[] { "StartDate" });
            }

            if (endDate == null && startDate!=null)
            {
                yield return new ValidationResult(errorMessage: "Missing EndDate.", memberNames: new[] { "EndDate" });
            }

            if (endDate <= startDate)
            {
                yield return new ValidationResult(errorMessage: "End Date must be greater than Start Date.", memberNames: new[] { "EndDate" });
            }
        }

        public static ValidationResult ValidatePastDate(DateTimeOffset? startDate)
        {
            if (startDate != null)
            {
                if (DateTime.Parse(startDate.ToString().Split(" ")[0]) <= DateTime.Today)
                {
                    return new ValidationResult("Start date cannot be in the past or today.", new[] { "StartDate" });
                }
            }

            return ValidationResult.Success;
        }

        public static ValidationResult ValidateRequired(string fieldValue, string fieldName)
        {
            if(fieldValue==null)
            {
                return new ValidationResult($"Field {fieldName} is required.");
            }

            return ValidationResult.Success;
        }

        public static ValidationResult ValidateDateRequired(DateTimeOffset fieldValue, string fieldName)
        {
            if (fieldValue == null)
            {
                return new ValidationResult($"Field {fieldName} is required.");
            }

            return ValidationResult.Success;
        }

        public static ValidationResult ValidateOnlyLetters(string fieldValue, string fieldName)
        {
            if (fieldValue != null)
            {
                if (!Regex.Match(fieldValue, "^[A-Za-z ]+$").Success)
                {
                    return new ValidationResult(errorMessage: $"Field {fieldName} can only contain letters.", memberNames: new[] { fieldName });
                }

                return ValidationResult.Success;
            }

            return ValidationResult.Success;
        }

        public static ValidationResult ValidateLength(string fieldValue, string fieldName)
        {
            if (fieldValue.Length > 50)
            {
                return new ValidationResult(errorMessage: $"Maximum length of {fieldName} field is 50 characters.", memberNames: new[] { fieldName });
            }

            return ValidationResult.Success;         
        }

        public static IEnumerable<ValidationResult> ValidateLettersDigits(string fieldValue, string fieldName)
        {
            if (fieldValue != null)
            {
                if (!Regex.Match(fieldValue, "^[A-Za-z0-9 ]+$").Success)
                {
                    yield return new ValidationResult(errorMessage: $"Field {fieldName} can only contain letters and digits.", memberNames: new[] { fieldName });
                }
            }
        }

        public static IEnumerable<ValidationResult> ValidateOnlyDigitsLength(string fieldValue, string fieldName)
        {
            if (fieldValue != null)
            {
                if (!Regex.Match(fieldValue, "^[0-9]+$").Success)
                {
                    yield return new ValidationResult(errorMessage: $"Field {fieldName} can only contain digits.", memberNames: new[] { fieldName });
                }

                if (fieldValue.Length > 7 || fieldValue.Length < 4)
                {
                    yield return new ValidationResult(errorMessage: $"The length of {fieldName} field is between 4 and 7 digits.", memberNames: new[] { fieldName });
                }
            }
        }

        public static IEnumerable<ValidationResult> ValidateSearchCriteria(SearchCriteria searchCriteria)
        {
            yield return ValidateOnlyLetters(searchCriteria.ClientLocation, nameof(searchCriteria.ClientLocation));
            yield return ValidateOnlyLetters(searchCriteria.Accommodation, nameof(searchCriteria.Accommodation));
            yield return ValidateOnlyLetters(searchCriteria.Client, nameof(searchCriteria.Client));

            if (searchCriteria.Status != null)
            {
                if (!Enum.IsDefined(typeof(RequestStatus), searchCriteria.Status))
                {
                    yield return new ValidationResult(errorMessage: "Invalid Status.", memberNames: new[] { "Status" });
                }
            }

            foreach (ValidationResult result in ValidateDates(searchCriteria.StartDate, searchCriteria.EndDate))
            {
                yield return result;
            }
        }
    }
}

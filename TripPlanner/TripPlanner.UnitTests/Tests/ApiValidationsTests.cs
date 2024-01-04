using System.ComponentModel.DataAnnotations;
using TripPlanner.ApiModels.ApiModels;
using TripPlanner.ApiModels.ApiValidations;
using TripPlanner.DatabaseModels.Models.Enums;
using TripPlanner.Logic.Common;

namespace TripPlanner.UnitTests.Tests
{
    [TestFixture]
    public class ApiValidationsTests
    {
        [TestCase("valid.email@yahoo.com")]
        [TestCase("another.email@yahoo.com")]
        public void ValidateEmail_ValidEmail_ReturnsSuccess(string email)
        {
            // Act
            var validationResult = ApiValidations.ValidateEmail(email);

            // Assert
            Assert.AreEqual(ValidationResult.Success, validationResult);
        }

        [TestCase("invalid.email@gmail.com")]
        [TestCase("another.email@gmail.com")]
        [TestCase("invalid.email@yahoo.org")]
        public void ValidateEmail_InvalidEmail_ReturnsError(string email)
        {
            // Act
            var validationResult = ApiValidations.ValidateEmail(email);

            // Assert
            Assert.AreEqual("Invalid email.", validationResult.ErrorMessage);
            CollectionAssert.Contains(validationResult.MemberNames, "Email");
        }

        [TestCase("ValidPassword123#")]
        [TestCase("StrongPass2022!")]
        public void ValidatePassword_ValidPassword_ReturnsSuccess(string password)
        {
            // Act
            var validationResult = ApiValidations.ValidatePassword(password);

            // Assert
            Assert.AreEqual(ValidationResult.Success, validationResult);
        }

        [TestCase("weakpassword")]
        [TestCase("ShortPass1")]
        [TestCase("noSpecialCharacter123")]
        public void ValidatePassword_InvalidPassword_ReturnsError(string password)
        {
            // Act
            var validationResult = ApiValidations.ValidatePassword(password);

            // Assert
            Assert.AreEqual("Invalid password.", validationResult.ErrorMessage);
            CollectionAssert.Contains(validationResult.MemberNames, "Password");
        }

        [TestCase(null, "2023-01-01T12:00:00+00:00", "Missing StartDate.")]
        [TestCase("2023-01-01T12:00:00+00:00", null, "Missing EndDate.")]
        [TestCase("2023-01-01T12:00:00+00:00", "2022-12-29T12:00:00+00:00", "End Date must be greater than Start Date.")]

        public void ValidateDates_Validations(string? startDate, string? endDate, string? expectedResult)
        {
            // Arrange
            DateTimeOffset? startDateTime = null;
            DateTimeOffset? endDateTime = null;

            if (startDate != null)
                startDateTime = DateTimeOffset.Parse(startDate);

            if (endDate != null)
                endDateTime = DateTimeOffset.Parse(endDate);


            //Act
            var validationResults = ApiValidations.ValidateDates(startDateTime, endDateTime);

            // Assert
            foreach (var validationResult in validationResults)
            {
                Assert.IsTrue(expectedResult == validationResult.ErrorMessage);
            }
        }

        [TestCase("John", nameof(RegisterBusinessTripApiModel.PmName))]
        [TestCase("Mary Ann", nameof(RegisterBusinessTripApiModel.Client))]
        [TestCase("ProjectName", nameof(RegisterBusinessTripApiModel.ProjectName))]
        [TestCase("ProjectNumber", nameof(RegisterBusinessTripApiModel.ProjectNumber))]
        [TestCase("TaskName", nameof(RegisterBusinessTripApiModel.TaskName))]
        [TestCase("TaskNumber", nameof(RegisterBusinessTripApiModel.TaskNumber))]
        [TestCase("Location", nameof(RegisterBusinessTripApiModel.ClientLocation))]
        [TestCase("Airport", nameof(RegisterBusinessTripApiModel.LeavingFrom))]
        [TestCase("Car", nameof(RegisterBusinessTripApiModel.MeanOfTransport))]
        [TestCase("Hotel", nameof(RegisterBusinessTripApiModel.Accommodation))]
        public void ValidateOnlyLetters_ValidInput_ReturnsSuccess(string fieldValue, string fieldName)
        {
            // Act
            var validationResult = ApiValidations.ValidateOnlyLetters(fieldValue, fieldName);

            // Assert
            Assert.AreEqual(ValidationResult.Success, validationResult);
        }

        [TestCase("1234", nameof(RegisterBusinessTripApiModel.PmName))]
        [TestCase("Client123", nameof(RegisterBusinessTripApiModel.Client))]
        [TestCase("Project 123", nameof(RegisterBusinessTripApiModel.ProjectName))]
        [TestCase("12345678", nameof(RegisterBusinessTripApiModel.ProjectNumber))]
        [TestCase("Task 123", nameof(RegisterBusinessTripApiModel.TaskName))]
        [TestCase("56789A", nameof(RegisterBusinessTripApiModel.TaskNumber))]
        [TestCase("Location 123", nameof(RegisterBusinessTripApiModel.ClientLocation))]
        [TestCase("Airport123", nameof(RegisterBusinessTripApiModel.LeavingFrom))]
        [TestCase("Car123", nameof(RegisterBusinessTripApiModel.MeanOfTransport))]
        [TestCase("Hotel 123", nameof(RegisterBusinessTripApiModel.Accommodation))]
        public void ValidateOnlyLetters_InvalidInput_ReturnsError(string fieldValue, string fieldName)
        {
            // Act
            var validationResult = ApiValidations.ValidateOnlyLetters(fieldValue, fieldName);

            // Assert
            Assert.AreEqual($"Field {fieldName} can only contain letters.", validationResult.ErrorMessage);
            CollectionAssert.Contains(validationResult.MemberNames, fieldName);
        }

        [Test]
        public void ValidateRequired_NonNullField_ReturnsSuccess()
        {
            // Arrange
            var fieldValue = "Test";

            // Act
            var result = ApiValidations.ValidateRequired(fieldValue, "FieldName");

            // Assert
            Assert.AreEqual(ValidationResult.Success, result);
        }

        [Test]
        public void ValidateRequired_NullField_ReturnsError()
        {
            // Arrange
            string fieldValue = null;

            // Act
            var result = ApiValidations.ValidateRequired(fieldValue, "FieldName");

            // Assert
            Assert.IsInstanceOf<ValidationResult>(result);
            Assert.AreEqual("Field FieldName is required.", result.ErrorMessage);
        }

        [Test]
        public void ValidateDateRequired_NonNullDate_ReturnsSuccess()
        {
            // Arrange
            var fieldValue = DateTimeOffset.Now;

            // Act
            var result = ApiValidations.ValidateDateRequired(fieldValue, "DateFieldName");

            // Assert
            Assert.AreEqual(ValidationResult.Success, result);
        }

        [Test]
        public void ValidateDateRequired_NullDate_ReturnsError()
        {
            // Arrange
            DateTimeOffset? fieldValue = null;

            // Act
            var result = ApiValidations.ValidateDateRequired(fieldValue, "DateFieldName");

            // Assert
            Assert.IsInstanceOf<ValidationResult>(result);
            Assert.AreEqual("Field DateFieldName is required.", result.ErrorMessage);
        }

        [Test]
        public void ValidateLength_ValidLength_ReturnsSuccess()
        {
            // Arrange
            var validLength = "12345678901234567890123456789012345678901234567890";

            // Act
            var result = ApiValidations.ValidateLength(validLength, "FieldName");

            // Assert
            Assert.AreEqual(ValidationResult.Success, result);
        }

        [Test]
        public void ValidateLength_ExceedingMaxLength_ReturnsError()
        {
            // Arrange
            var exceedingLength = "123456789012345678901234567890123456789012345678901";

            // Act
            var result = ApiValidations.ValidateLength(exceedingLength, "FieldName");

            // Assert
            Assert.IsInstanceOf<ValidationResult>(result);
            Assert.AreEqual("Maximum length of FieldName field is 50 characters.", result.ErrorMessage);
        }

        [Test]
        public void ValidateOnlyDigitsLength_InvalidCharacters_ReturnsError()
        {
            // Arrange
            var invalidCharacters = "12A34";

            // Act
            var result = ApiValidations.ValidateOnlyDigitsLength(invalidCharacters, "FieldName");

            // Assert
            Assert.IsInstanceOf<IEnumerable<ValidationResult>>(result);
            CollectionAssert.Contains(result.Select(x => x.ErrorMessage).ToList(), "Field FieldName can only contain digits.");
        }
        [Test]
        public void ValidateSearchCriteria_ValidCriteria_ReturnsSuccess()
        {
            // Arrange
            var validCriteria = new SearchCriteria
            (
                "ValidLocation",
                "ValidAccommodation",
                "ValidClient",
                RequestStatus.Accepted,
                DateTimeOffset.UtcNow.AddDays(1),
                DateTimeOffset.UtcNow.AddDays(5)
            );

            // Act
            var result = ApiValidations.ValidateSearchCriteria(validCriteria);

            // Assert
            CollectionAssert.Contains(result, ValidationResult.Success);
        }

        [Test]
        public void ValidateSearchCriteria_InvalidStatus_ReturnsError()
        {
            // Arrange
            var invalidStatusCriteria = new SearchCriteria
            (
                "clientLocation",
                "accomodation",
                "client",
                (RequestStatus)10,
                DateTimeOffset.UtcNow.AddDays(1),
                DateTimeOffset.UtcNow.AddDays(5)
            );

            // Act
            var result = ApiValidations.ValidateSearchCriteria(invalidStatusCriteria);

            // Assert
            Assert.IsInstanceOf<IEnumerable<ValidationResult>>(result);
            CollectionAssert.Contains(result.Where(x => x?.ErrorMessage != null).Select(x => x.ErrorMessage).ToList(), "Invalid Status.");
        }

        [Test]
        public void ValidateSearchCriteria_InvalidDateRange_ReturnsErrors()
        {
            // Arrange
            var invalidDateRangeCriteria = new SearchCriteria
           (
                "clientLocation",
                "accomodation",
                "client",
                0,
                DateTimeOffset.UtcNow.AddDays(5),
                DateTimeOffset.UtcNow.AddDays(1)
            );

            // Act
            var result = ApiValidations.ValidateSearchCriteria(invalidDateRangeCriteria);

            // Assert
            Assert.IsInstanceOf<IEnumerable<ValidationResult>>(result);
            CollectionAssert.Contains(result.Where(x => x?.ErrorMessage != null).Select(x => x.ErrorMessage).ToList(), "End Date must be greater than Start Date.");
        }
    }
}

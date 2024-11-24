using SmartStay.Validation;
using SmartStay.Validation.Validators;
using Xunit;

namespace SmartStay.Validation.Tests.Validators
{
public class NameValidatorTests
{
    [Fact]
    public void ValidateName_ValidName_ReturnsName()
    {
        // Arrange
        string validName = "Enrique Rodrigues";

        // Act
        var result = NameValidator.ValidateName(validName);

        // Assert
        Assert.Equal(validName, result);
    }

    [Fact]
    public void ValidateName_InvalidName_ThrowsValidationException()
    {
        // Arrange
        string invalidName = ""; // Empty name is invalid

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => NameValidator.ValidateName(invalidName));
        Assert.Equal(ValidationErrorCode.InvalidName, exception.ErrorCode);
    }

    [Fact]
    public void ValidateName_TooLongName_ThrowsValidationException()
    {
        // Arrange
        string tooLongName = new string('a', 51); // Exceeds max length

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => NameValidator.ValidateName(tooLongName));
        Assert.Equal(ValidationErrorCode.InvalidName, exception.ErrorCode);
    }

    [Fact]
    public void ValidateAccommodationName_ValidName_ReturnsName()
    {
        // Arrange
        string validAccommodationName = "Cozy Apartment";

        // Act
        var result = NameValidator.ValidateAccommodationName(validAccommodationName);

        // Assert
        Assert.Equal(validAccommodationName, result);
    }

    [Fact]
    public void ValidateAccommodationName_TooLongName_ThrowsValidationException()
    {
        // Arrange
        string tooLongAccommodationName = new string('b', 101); // Exceeds max length

        // Act & Assert
        var exception =
            Assert.Throws<ValidationException>(() => NameValidator.ValidateAccommodationName(tooLongAccommodationName));
        Assert.Equal(ValidationErrorCode.InvalidAccommodationName, exception.ErrorCode);
    }

    [Fact]
    public void IsValidName_ValidName_ReturnsTrue()
    {
        // Arrange
        string validName = "Alice";

        // Act
        var result = NameValidator.IsValidName(validName);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidName_InvalidName_ReturnsFalse()
    {
        // Arrange
        string invalidName = "";

        // Act
        var result = NameValidator.IsValidName(invalidName);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidAccommodationName_ValidName_ReturnsTrue()
    {
        // Arrange
        string validAccommodationName = "Modern Studio";

        // Act
        var result = NameValidator.IsValidAccommodationName(validAccommodationName);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidAccommodationName_InvalidName_ReturnsFalse()
    {
        // Arrange
        string invalidAccommodationName = null;

        // Act
        var result = NameValidator.IsValidAccommodationName(invalidAccommodationName);

        // Assert
        Assert.False(result);
    }
}
}

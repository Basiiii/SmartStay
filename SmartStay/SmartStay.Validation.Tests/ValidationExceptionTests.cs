namespace SmartStay.Validation.Tests
{
using SmartStay.Validation;
using Xunit;

public class ValidationExceptionTests
{
    [Fact]
    public void Constructor_WithErrorCode_SetsErrorCodeAndMessage()
    {
        // Arrange
        var expectedErrorCode = ValidationErrorCode.InvalidName;
        var expectedErrorMessage = ValidationErrorMessages.GetErrorMessage(expectedErrorCode);

        // Act
        var exception = new ValidationException(expectedErrorCode);

        // Assert
        Assert.Equal(expectedErrorCode, exception.ErrorCode);
        Assert.Equal(expectedErrorMessage, exception.Message);
    }

    [Fact]
    public void Constructor_WithUnknownErrorCode_UsesFallbackMessage()
    {
        // Arrange
        var invalidErrorCode = (ValidationErrorCode)9999; // Undefined error code
        var expectedFallbackMessage = "Unknown validation error.";

        // Act
        var exception = new ValidationException(invalidErrorCode);

        // Assert
        Assert.Equal(invalidErrorCode, exception.ErrorCode);
        Assert.Equal(expectedFallbackMessage, exception.Message);
    }
}
}

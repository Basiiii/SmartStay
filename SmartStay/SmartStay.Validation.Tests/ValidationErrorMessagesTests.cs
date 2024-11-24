namespace SmartStay.Validation.Tests
{
using System.Globalization;
using SmartStay.Validation;
using Xunit;

public class ValidationErrorMessagesTests
{
    [Fact]
    public void GetErrorMessage_ValidErrorCode_ReturnsLocalizedMessage()
    {
        // Arrange
        var errorCode = ValidationErrorCode.InvalidName;
        var expectedMessage = "The provided name is invalid.";

        // Act
        var result = ValidationErrorMessages.GetErrorMessage(errorCode);

        // Assert
        Assert.Equal(expectedMessage, result);
    }

    [Fact]
    public void GetErrorMessage_InvalidErrorCode_ReturnsFallbackMessage()
    {
        // Arrange
        var invalidErrorCode = (ValidationErrorCode)9999;
        var expectedFallbackMessage = "Unknown validation error.";

        // Act
        var result = ValidationErrorMessages.GetErrorMessage(invalidErrorCode);

        // Assert
        Assert.Equal(expectedFallbackMessage, result);
    }

    [Fact]
    public void GetErrorMessage_SupportsLocalization()
    {
        // Arrange
        var errorCode = ValidationErrorCode.InvalidName;
        var expectedMessagePt = "O nome fornecido é inválido.";  // Portuguese translation
        var expectedMessageEn = "The provided name is invalid."; // English translation

        // Act
        CultureInfo.CurrentCulture = new CultureInfo("pt-PT");
        var resultPt = ValidationErrorMessages.GetErrorMessage(errorCode);

        CultureInfo.CurrentCulture = new CultureInfo("en-US");
        var resultEn = ValidationErrorMessages.GetErrorMessage(errorCode);

        // Assert
        Assert.Equal(expectedMessagePt, resultPt);
        Assert.Equal(expectedMessageEn, resultEn);
    }
}
}

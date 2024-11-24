/// <copyright file="ValidationErrorMessagesTests.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the implementation of the <see cref="ValidationErrorMessagesTests"/> class,
/// which provides unit tests for the <see cref="ValidationErrorMessages"/> class to ensure
/// the correct retrieval of localized validation error messages based on error codes and current culture.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>24/11/2024</date>

/// <summary>
/// The <c>SmartStay.Validation.Tests</c> namespace contains unit tests for classes within the
/// <c>SmartStay.Validation</c> namespace, ensuring correctness and reliability of validation functionalities.
/// </summary>
namespace SmartStay.Validation.Tests
{
using System.Globalization;
using SmartStay.Validation;
using Xunit;

/// <summary>
/// Contains unit tests for the <see cref="ValidationErrorMessages"/> class.
/// Ensures that error messages are correctly retrieved based on the provided error codes
/// and that localization functions as expected.
/// </summary>
public class ValidationErrorMessagesTests
{
    /// <summary>
    /// Verifies that providing a valid <see cref="ValidationErrorCode"/> returns the correct localized error message.
    /// </summary>
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

    /// <summary>
    /// Verifies that providing an invalid or undefined error code returns the fallback error message.
    /// </summary>
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

    /// <summary>
    /// Verifies that error messages are correctly localized based on the current culture.
    /// </summary>
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

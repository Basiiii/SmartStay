/// <copyright file="ValidationExceptionTests.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the implementation of the <see cref="ValidationExceptionTests"/> class,
/// which provides unit tests for the <see cref="ValidationException"/> class to ensure that
/// validation exceptions are properly instantiated with the correct error codes and messages.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>24/11/2024</date>

/// <summary>
/// The <c>SmartStay.Validation.Tests</c> namespace contains unit tests for classes within the
/// <c>SmartStay.Validation</c> namespace, ensuring correctness and reliability of validation functionalities.
/// </summary>
namespace SmartStay.Validation.Tests
{
using SmartStay.Validation;
using Xunit;

/// <summary>
/// Contains unit tests for the <see cref="ValidationException"/> class.
/// Ensures that exceptions are created with the expected error codes and messages.
/// </summary>
public class ValidationExceptionTests
{
    /// <summary>
    /// Verifies that the <see cref="ValidationException"/> constructor correctly sets the error code
    /// and retrieves the corresponding error message.
    /// </summary>
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

    /// <summary>
    /// Verifies that the <see cref="ValidationException"/> constructor handles unknown error codes by
    /// using the fallback error message while retaining the provided error code.
    /// </summary>
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

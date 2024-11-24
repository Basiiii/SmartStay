/// <copyright file="EmailValidatorTests.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains unit tests for the <see cref="EmailValidator"/> class,
/// ensuring the correct validation of email addresses in the SmartStay application.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>24/11/2024</date>

/// <summary>
/// The <c>SmartStay.Validation.Tests.Validators</c> namespace contains unit tests for the validation logic
/// of different fields.
/// </summary>
namespace SmartStay.Validation.Tests.Validators
{
using SmartStay.Validation;
using SmartStay.Validation.Validators;
using Xunit;

/// <summary>
/// Contains unit tests for the <see cref="EmailValidator"/> class.
/// Tests the validation logic for email addresses used in the SmartStay application.
/// </summary>
public class EmailValidatorTests
{
    /// <summary>
    /// Tests the <see cref="EmailValidator.ValidateEmail(string)"/> method to ensure that
    /// it returns the email address when the address is valid.
    /// </summary>
    [Fact]
    public void ValidateEmail_ValidEmail_ReturnsEmail()
    {
        // Arrange
        string validEmail = "test@example.com";

        // Act
        var result = EmailValidator.ValidateEmail(validEmail);

        // Assert
        Assert.Equal(validEmail, result);
    }

    /// <summary>
    /// Tests the <see cref="EmailValidator.ValidateEmail(string)"/> method to ensure that
    /// it throws a <see cref="ValidationException"/> when the email address is invalid.
    /// </summary>
    [Fact]
    public void ValidateEmail_InvalidEmail_ThrowsValidationException()
    {
        // Arrange
        string invalidEmail = "invalid-email";

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => EmailValidator.ValidateEmail(invalidEmail));
        Assert.Equal(ValidationErrorCode.InvalidEmail, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the <see cref="EmailValidator.IsValidEmail(string)"/> method to ensure that
    /// it returns true for a valid email address.
    /// </summary>
    [Fact]
    public void IsValidEmail_ValidEmail_ReturnsTrue()
    {
        // Arrange
        string validEmail = "valid@example.com";

        // Act
        var result = EmailValidator.IsValidEmail(validEmail);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Tests the <see cref="EmailValidator.IsValidEmail(string)"/> method to ensure that
    /// it returns false for an email address that is missing the '@' symbol.
    /// </summary>
    [Fact]
    public void IsValidEmail_MissingAtSymbol_ReturnsFalse()
    {
        // Arrange
        string invalidEmail = "missingatsign.com";

        // Act
        var result = EmailValidator.IsValidEmail(invalidEmail);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Tests the <see cref="EmailValidator.IsValidEmail(string)"/> method to ensure that
    /// it returns false for an email address that does not contain a domain extension.
    /// </summary>
    [Fact]
    public void IsValidEmail_MissingDomainExtension_ReturnsFalse()
    {
        // Arrange
        string invalidEmail = "missingdomain@com";

        // Act
        var result = EmailValidator.IsValidEmail(invalidEmail);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Tests the <see cref="EmailValidator.IsValidEmail(string)"/> method to ensure that
    /// it returns false for an empty email address.
    /// </summary>
    [Fact]
    public void IsValidEmail_EmptyEmail_ReturnsFalse()
    {
        // Arrange
        string invalidEmail = string.Empty;

        // Act
        var result = EmailValidator.IsValidEmail(invalidEmail);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Tests the <see cref="EmailValidator.IsValidEmail(string)"/> method to ensure that
    /// it returns false for a null email address.
    /// </summary>
    [Fact]
    public void IsValidEmail_NullEmail_ReturnsFalse()
    {
        // Arrange
        string invalidEmail = null;

        // Act
        var result = EmailValidator.IsValidEmail(invalidEmail);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Tests the <see cref="EmailValidator.IsValidEmail(string)"/> method to ensure that
    /// it returns false for an email address with invalid characters.
    /// </summary>
    [Fact]
    public void IsValidEmail_InvalidCharacters_ReturnsFalse()
    {
        // Arrange
        string invalidEmail = "invalid@ex$ample.com"; // Invalid character '$'

        // Act
        var result = EmailValidator.IsValidEmail(invalidEmail);

        // Assert
        Assert.False(result);
    }
}
}

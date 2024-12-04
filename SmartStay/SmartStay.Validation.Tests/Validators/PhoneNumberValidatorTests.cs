/// <copyright file="PhoneNumberValidatorTests.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains unit tests for the <see cref="PhoneNumberValidator"/> class,
/// ensuring the correct validation of phone numbers within the SmartStay application.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>24/11/2024</date>

/// <summary>
/// The <c>SmartStay.Validation.Tests.Validators</c> namespace contains unit tests for the validation logic
/// of different fields, including phone numbers in the SmartStay application.
/// </summary>
namespace SmartStay.Validation.Tests.Validators
{
using SmartStay.Validation;
using SmartStay.Validation.Validators;
using Xunit;

/// <summary>
/// Contains unit tests for the <see cref="PhoneNumberValidator"/> class.
/// Tests the validation logic for phone numbers in the SmartStay application.
/// </summary>
public class PhoneNumberValidatorTests
{
    /// <summary>
    /// Tests the <see cref="PhoneNumberValidator.ValidatePhoneNumber(string)"/> method to ensure that
    /// it returns the phone number when the phone number is valid.
    /// </summary>
    [Fact]
    public void ValidatePhoneNumber_ValidPhoneNumber_ReturnsPhoneNumber()
    {
        // Arrange
        string validPhoneNumber = "+351777888999";

        // Act
        var result = PhoneNumberValidator.ValidatePhoneNumber(validPhoneNumber);

        // Assert
        Assert.Equal(validPhoneNumber, result);
    }

    /// <summary>
    /// Tests the <see cref="PhoneNumberValidator.ValidatePhoneNumber(string)"/> method to ensure that
    /// it throws a <see cref="ValidationException"/> when the phone number is invalid.
    /// </summary>
    [Fact]
    public void ValidatePhoneNumber_InvalidPhoneNumber_ThrowsValidationException()
    {
        // Arrange
        string invalidPhoneNumber = "1234567890"; // Invalid due to missing international code

        // Act & Assert
        var exception =
            Assert.Throws<ValidationException>(() => PhoneNumberValidator.ValidatePhoneNumber(invalidPhoneNumber));
        Assert.Equal(ValidationErrorCode.InvalidPhoneNumber, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the <see cref="PhoneNumberValidator.IsValidPhoneNumber(string)"/> method to ensure that
    /// it returns true for valid phone numbers.
    /// </summary>
    [Fact]
    public void IsValidPhoneNumber_ValidPhoneNumber_ReturnsTrue()
    {
        // Arrange
        string validPhoneNumber = "+1234567890";

        // Act
        var result = PhoneNumberValidator.IsValidPhoneNumber(validPhoneNumber);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Tests the <see cref="PhoneNumberValidator.IsValidPhoneNumber(string)"/> method to ensure that
    /// it returns false for invalid phone numbers.
    /// </summary>
    [Fact]
    public void IsValidPhoneNumber_InvalidPhoneNumber_ReturnsFalse()
    {
        // Arrange
        string invalidPhoneNumber = "1234567890"; // Invalid due to missing international code

        // Act
        var result = PhoneNumberValidator.IsValidPhoneNumber(invalidPhoneNumber);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Tests the <see cref="PhoneNumberValidator.ValidatePhoneNumber(string)"/> method to ensure that
    /// it throws a <see cref="ValidationException"/> when the phone number is empty.
    /// </summary>
    [Fact]
    public void ValidatePhoneNumber_EmptyPhoneNumber_ThrowsValidationException()
    {
        // Arrange
        string emptyPhoneNumber = "";

        // Act & Assert
        var exception =
            Assert.Throws<ValidationException>(() => PhoneNumberValidator.ValidatePhoneNumber(emptyPhoneNumber));
        Assert.Equal(ValidationErrorCode.InvalidPhoneNumber, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the <see cref="PhoneNumberValidator.ValidatePhoneNumber(string)"/> method to ensure that
    /// it throws a <see cref="ValidationException"/> when the phone number is null.
    /// </summary>
    [Fact]
    public void ValidatePhoneNumber_NullPhoneNumber_ThrowsValidationException()
    {
        // Arrange
        string nullPhoneNumber = null;

        // Act & Assert
        var exception =
            Assert.Throws<ValidationException>(() => PhoneNumberValidator.ValidatePhoneNumber(nullPhoneNumber));
        Assert.Equal(ValidationErrorCode.InvalidPhoneNumber, exception.ErrorCode);
    }
}
}

/// <copyright file="OwnerValidatorTests.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains unit tests for the <see cref="OwnerValidator"/> class,
/// ensuring the correct validation of owner-related data in the SmartStay application.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>03/12/2024</date>

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
/// Contains unit tests for the <see cref="OwnerValidator"/> class.
/// Tests the validation logic for owner-related data used in the SmartStay application.
/// </summary>
public class OwnerValidatorTests
{
    /// <summary>
    /// Tests the <see cref="OwnerValidator.ValidateOwnerId(int)"/> method to ensure that
    /// it returns the ID when the ID is valid.
    /// </summary>
    [Fact]
    public void ValidateOwnerId_ValidId_ReturnsId()
    {
        // Arrange
        int validId = 123;

        // Act
        var result = OwnerValidator.ValidateOwnerId(validId);

        // Assert
        Assert.Equal(validId, result);
    }

    /// <summary>
    /// Tests the <see cref="OwnerValidator.ValidateOwnerId(int)"/> method to ensure that
    /// it throws a <see cref="ValidationException"/> when the ID is invalid.
    /// </summary>
    [Fact]
    public void ValidateOwnerId_InvalidId_ThrowsValidationException()
    {
        // Arrange
        int invalidId = 0;

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => OwnerValidator.ValidateOwnerId(invalidId));
        Assert.Equal(ValidationErrorCode.InvalidId, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the <see cref="OwnerValidator.ValidateOwnerName(string)"/> method to ensure that
    /// it returns the name when the name is valid.
    /// </summary>
    [Fact]
    public void ValidateOwnerName_ValidName_ReturnsName()
    {
        // Arrange
        string validName = "John Doe";

        // Act
        var result = OwnerValidator.ValidateOwnerName(validName);

        // Assert
        Assert.Equal(validName, result);
    }

    /// <summary>
    /// Tests the <see cref="OwnerValidator.ValidateOwnerName(string)"/> method to ensure that
    /// it throws a <see cref="ValidationException"/> when the name is invalid.
    /// </summary>
    [Fact]
    public void ValidateOwnerName_InvalidName_ThrowsValidationException()
    {
        // Arrange
        string invalidName = "J";

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => OwnerValidator.ValidateOwnerName(invalidName));
        Assert.Equal(ValidationErrorCode.InvalidName, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the <see cref="OwnerValidator.ValidateOwnerEmail(string)"/> method to ensure that
    /// it returns the email when the email is valid.
    /// </summary>
    [Fact]
    public void ValidateOwnerEmail_ValidEmail_ReturnsEmail()
    {
        // Arrange
        string validEmail = "owner@example.com";

        // Act
        var result = OwnerValidator.ValidateOwnerEmail(validEmail);

        // Assert
        Assert.Equal(validEmail, result);
    }

    /// <summary>
    /// Tests the <see cref="OwnerValidator.ValidateOwnerEmail(string)"/> method to ensure that
    /// it throws a <see cref="ValidationException"/> when the email is invalid.
    /// </summary>
    [Fact]
    public void ValidateOwnerEmail_InvalidEmail_ThrowsValidationException()
    {
        // Arrange
        string invalidEmail = "invalid-email";

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => OwnerValidator.ValidateOwnerEmail(invalidEmail));
        Assert.Equal(ValidationErrorCode.InvalidEmail, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the <see cref="OwnerValidator.ValidateOwnerPhoneNumber(string)"/> method to ensure that
    /// it returns the phone number when the number is valid.
    /// </summary>
    [Fact]
    public void ValidateOwnerPhoneNumber_ValidPhoneNumber_ReturnsPhoneNumber()
    {
        // Arrange
        string validPhoneNumber = "1234567890";

        // Act
        var result = OwnerValidator.ValidateOwnerPhoneNumber(validPhoneNumber);

        // Assert
        Assert.Equal(validPhoneNumber, result);
    }

    /// <summary>
    /// Tests the <see cref="OwnerValidator.ValidateOwnerPhoneNumber(string)"/> method to ensure that
    /// it throws a <see cref="ValidationException"/> when the phone number is invalid.
    /// </summary>
    [Fact]
    public void ValidateOwnerPhoneNumber_InvalidPhoneNumber_ThrowsValidationException()
    {
        // Arrange
        string invalidPhoneNumber = "123";

        // Act & Assert
        var exception =
            Assert.Throws<ValidationException>(() => OwnerValidator.ValidateOwnerPhoneNumber(invalidPhoneNumber));
        Assert.Equal(ValidationErrorCode.InvalidPhoneNumber, exception.ErrorCode);
    }
}
}

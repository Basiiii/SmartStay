/// <copyright file="NameValidatorTests.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains unit tests for the <see cref="NameValidator"/> class,
/// ensuring that name validation logic for both general names and accommodation names
/// works correctly under various scenarios.
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
/// Contains unit tests for the <see cref="NameValidator"/> class.
/// Validates both general names and accommodation names, checking correct behavior
/// when the names are valid or invalid.
/// </summary>
public class NameValidatorTests
{
    /// <summary>
    /// Tests the <see cref="NameValidator.ValidateName(string)"/> method to ensure that it returns the
    /// name as-is when the name is valid.
    /// </summary>
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

    /// <summary>
    /// Tests the <see cref="NameValidator.ValidateName(string)"/> method to ensure that it throws a
    /// <see cref="ValidationException"/> when the name is invalid (empty).
    /// </summary>
    [Fact]
    public void ValidateName_InvalidName_ThrowsValidationException()
    {
        // Arrange
        string invalidName = ""; // Empty name is invalid

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => NameValidator.ValidateName(invalidName));
        Assert.Equal(ValidationErrorCode.InvalidName, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the <see cref="NameValidator.ValidateName(string)"/> method to ensure that it throws a
    /// <see cref="ValidationException"/> when the name is too long (greater than 50 characters).
    /// </summary>
    [Fact]
    public void ValidateName_TooLongName_ThrowsValidationException()
    {
        // Arrange
        string tooLongName = new string('a', 51); // Exceeds max length

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => NameValidator.ValidateName(tooLongName));
        Assert.Equal(ValidationErrorCode.InvalidName, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the <see cref="NameValidator.ValidateAccommodationName(string)"/> method to ensure that it
    /// returns the accommodation name as-is when it is valid.
    /// </summary>
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

    /// <summary>
    /// Tests the <see cref="NameValidator.ValidateAccommodationName(string)"/> method to ensure that it throws
    /// a <see cref="ValidationException"/> when the accommodation name is too long (greater than 100 characters).
    /// </summary>
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

    /// <summary>
    /// Tests the <see cref="NameValidator.IsValidName(string)"/> method to ensure it returns true when the name
    /// is valid.
    /// </summary>
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

    /// <summary>
    /// Tests the <see cref="NameValidator.IsValidName(string)"/> method to ensure it returns false when the name
    /// is invalid (empty).
    /// </summary>
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

    /// <summary>
    /// Tests the <see cref="NameValidator.IsValidAccommodationName(string)"/> method to ensure it returns true when
    /// the accommodation name is valid.
    /// </summary>
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

    /// <summary>
    /// Tests the <see cref="NameValidator.IsValidAccommodationName(string)"/> method to ensure it returns false when
    /// the accommodation name is invalid (null).
    /// </summary>
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

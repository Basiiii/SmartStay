/// <copyright file="AccommodationValidatorTests.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains unit tests for the <see cref="AccommodationValidator"/> class,
/// ensuring the correct validation of accommodation-related data in the SmartStay application.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>24/11/2024</date>

/// <summary>
/// The <c>SmartStay.Validation.Tests.Validators</c> namespace contains unit tests for the validation logic
/// of different fields, specifically focusing on the <see cref="AccommodationValidator"/> class for validating
/// accommodation types and IDs.
/// </summary>
namespace SmartStay.Validation.Tests.Validators
{
using SmartStay.Validation;
using SmartStay.Validation.Validators;
using SmartStay.Common.Enums;
using Xunit;

/// <summary>
/// Contains unit tests for the <see cref="AccommodationValidator"/> class.
/// Tests the validation logic for accommodation types and IDs.
/// </summary>
public class AccommodationValidatorTests
{
    /// <summary>
    /// Tests the <see cref="AccommodationValidator.ValidateAccommodationType(AccommodationType)"/> method to ensure
    /// that it returns the accommodation type when the type is valid.
    /// </summary>
    [Fact]
    public void ValidateAccommodationType_ValidAccommodationType_ReturnsAccommodationType()
    {
        // Arrange
        var validAccommodationType = AccommodationType.Apartment;

        // Act
        var result = AccommodationValidator.ValidateAccommodationType(validAccommodationType);

        // Assert
        Assert.Equal(validAccommodationType, result);
    }

    /// <summary>
    /// Tests the <see cref="AccommodationValidator.ValidateAccommodationType(AccommodationType)"/> method to ensure
    /// that it throws a <see cref="ValidationException"/> when the accommodation type is invalid.
    /// </summary>
    [Fact]
    public void ValidateAccommodationType_InvalidAccommodationType_ThrowsValidationException()
    {
        // Arrange
        var invalidAccommodationType = (AccommodationType)9999; // Invalid enum value

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(
            () => AccommodationValidator.ValidateAccommodationType(invalidAccommodationType));
        Assert.Equal(ValidationErrorCode.InvalidAccommodationType, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the <see cref="AccommodationValidator.IsValidAccommodationType(AccommodationType)"/> method to ensure it
    /// returns true when the accommodation type is valid.
    /// </summary>
    [Fact]
    public void IsValidAccommodationType_ValidAccommodationType_ReturnsTrue()
    {
        // Arrange
        var validAccommodationType = AccommodationType.House;

        // Act
        var result = AccommodationValidator.IsValidAccommodationType(validAccommodationType);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Tests the <see cref="AccommodationValidator.IsValidAccommodationType(AccommodationType)"/> method to ensure it
    /// returns false when the accommodation type is invalid.
    /// </summary>
    [Fact]
    public void IsValidAccommodationType_InvalidAccommodationType_ReturnsFalse()
    {
        // Arrange
        var invalidAccommodationType = (AccommodationType)9999; // Invalid enum value

        // Act
        var result = AccommodationValidator.IsValidAccommodationType(invalidAccommodationType);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Tests the <see cref="AccommodationValidator.ValidateAccommodationId(int)"/> method to ensure that it returns the
    /// accommodation ID when the ID is valid.
    /// </summary>
    [Fact]
    public void ValidateAccommodationId_ValidAccommodationId_ReturnsAccommodationId()
    {
        // Arrange
        var validAccommodationId = 1;

        // Act
        var result = AccommodationValidator.ValidateAccommodationId(validAccommodationId);

        // Assert
        Assert.Equal(validAccommodationId, result);
    }

    /// <summary>
    /// Tests the <see cref="AccommodationValidator.ValidateAccommodationId(int)"/> method to ensure that it throws a
    /// <see cref="ValidationException"/> when the accommodation ID is invalid (non-positive).
    /// </summary>
    [Fact]
    public void ValidateAccommodationId_InvalidAccommodationId_ThrowsValidationException()
    {
        // Arrange
        var invalidAccommodationId = 0; // Invalid ID

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(
            () => AccommodationValidator.ValidateAccommodationId(invalidAccommodationId));
        Assert.Equal(ValidationErrorCode.InvalidId, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the <see cref="AccommodationValidator.IsValidAccommodationId(int)"/> method to ensure it returns true when
    /// the accommodation ID is valid (positive).
    /// </summary>
    [Fact]
    public void IsValidAccommodationId_ValidAccommodationId_ReturnsTrue()
    {
        // Arrange
        var validAccommodationId = 10;

        // Act
        var result = AccommodationValidator.IsValidAccommodationId(validAccommodationId);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Tests the <see cref="AccommodationValidator.IsValidAccommodationId(int)"/> method to ensure it returns false
    /// when the accommodation ID is invalid (non-positive).
    /// </summary>
    [Fact]
    public void IsValidAccommodationId_InvalidAccommodationId_ReturnsFalse()
    {
        // Arrange
        var invalidAccommodationId = -1; // Invalid ID

        // Act
        var result = AccommodationValidator.IsValidAccommodationId(invalidAccommodationId);

        // Assert
        Assert.False(result);
    }
}
}

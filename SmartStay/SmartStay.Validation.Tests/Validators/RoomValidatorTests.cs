/// <copyright file="RoomValidatorTests.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains unit tests for the <see cref="RoomValidator"/> class,
/// ensuring correct validation of room-related data within the SmartStay application.
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
using SmartStay.Common.Enums;
using Xunit;

/// <summary>
/// Contains unit tests for the <see cref="RoomValidator"/> class.
/// Tests the validation logic for room-related data used in the SmartStay application.
/// </summary>
public class RoomValidatorTests
{
    /// <summary>
    /// Tests the <see cref="RoomValidator.ValidateRoomType(RoomType)"/> method to ensure that
    /// it returns the room type when it is valid.
    /// </summary>
    [Fact]
    public void ValidateRoomType_ValidType_ReturnsRoomType()
    {
        // Arrange
        RoomType validRoomType = RoomType.Family;

        // Act
        var result = RoomValidator.ValidateRoomType(validRoomType);

        // Assert
        Assert.Equal(validRoomType, result);
    }

    /// <summary>
    /// Tests the <see cref="RoomValidator.ValidateRoomType(RoomType)"/> method to ensure that
    /// it throws a <see cref="ValidationException"/> when the room type is invalid.
    /// </summary>
    [Fact]
    public void ValidateRoomType_InvalidType_ThrowsValidationException()
    {
        // Arrange
        RoomType invalidRoomType = (RoomType)(-1); // An invalid enum value

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => RoomValidator.ValidateRoomType(invalidRoomType));
        Assert.Equal(ValidationErrorCode.InvalidRoomType, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the <see cref="RoomValidator.ValidateAvailability(bool)"/> method to ensure that
    /// it returns the availability status when it is valid.
    /// </summary>
    [Fact]
    public void ValidateAvailability_ValidStatus_ReturnsAvailability()
    {
        // Arrange
        bool isAvailable = true;

        // Act
        var result = RoomValidator.ValidateAvailability(isAvailable);

        // Assert
        Assert.Equal(isAvailable, result);
    }

    /// <summary>
    /// Tests the <see cref="RoomValidator.ValidateRoomId(int)"/> method to ensure that
    /// it returns the room ID when it is valid.
    /// </summary>
    [Fact]
    public void ValidateRoomId_ValidId_ReturnsRoomId()
    {
        // Arrange
        int validRoomId = 101;

        // Act
        var result = RoomValidator.ValidateRoomId(validRoomId);

        // Assert
        Assert.Equal(validRoomId, result);
    }

    /// <summary>
    /// Tests the <see cref="RoomValidator.ValidateRoomId(int)"/> method to ensure that
    /// it throws a <see cref="ValidationException"/> when the room ID is invalid.
    /// </summary>
    [Fact]
    public void ValidateRoomId_InvalidId_ThrowsValidationException()
    {
        // Arrange
        int invalidRoomId = -1;

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => RoomValidator.ValidateRoomId(invalidRoomId));
        Assert.Equal(ValidationErrorCode.InvalidId, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the <see cref="RoomValidator.IsValidRoomType(RoomType)"/> method to ensure that
    /// it returns true for a valid room type.
    /// </summary>
    [Fact]
    public void IsValidRoomType_ValidType_ReturnsTrue()
    {
        // Arrange
        RoomType validRoomType = RoomType.Deluxe;

        // Act
        var result = RoomValidator.IsValidRoomType(validRoomType);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Tests the <see cref="RoomValidator.IsValidRoomType(RoomType)"/> method to ensure that
    /// it returns false for an invalid room type.
    /// </summary>
    [Fact]
    public void IsValidRoomType_InvalidType_ReturnsFalse()
    {
        // Arrange
        RoomType invalidRoomType = (RoomType)(999); // An undefined enum value

        // Act
        var result = RoomValidator.IsValidRoomType(invalidRoomType);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Tests the <see cref="RoomValidator.IsValidAvailability(bool)"/> method to ensure that
    /// it always returns true (logic is tautological).
    /// </summary>
    [Fact]
    public void IsValidAvailability_AnyStatus_ReturnsTrue()
    {
        // Arrange
        bool anyStatus = true;

        // Act
        var result = RoomValidator.IsValidAvailability(anyStatus);

        // Assert
        Assert.True(result);

        // Repeat with false
        result = RoomValidator.IsValidAvailability(false);
        Assert.True(result);
    }
}
}

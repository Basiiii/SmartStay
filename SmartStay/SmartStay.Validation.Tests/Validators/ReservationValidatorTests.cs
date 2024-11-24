/// <copyright file="ReservationValidatorTests.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains unit tests for the <see cref="ReservationValidator"/> class,
/// ensuring the correct validation of reservation-related data within the SmartStay application.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>24/11/2024</date>

/// <summary>
/// The <c>SmartStay.Validation.Tests.Validators</c> namespace contains unit tests for the validation logic
/// of different fields, including reservation data in the SmartStay application.
/// </summary>
namespace SmartStay.Validation.Tests.Validators
{
using SmartStay.Validation;
using SmartStay.Validation.Validators;
using SmartStay.Common.Enums;
using Xunit;

/// <summary>
/// Contains unit tests for the <see cref="ReservationValidator"/> class.
/// Tests the validation logic for reservation-related data in the SmartStay application.
/// </summary>
public class ReservationValidatorTests
{
    /// <summary>
    /// Tests the <see cref="ReservationValidator.ValidateReservationStatus(ReservationStatus)"/> method
    /// to ensure it returns the reservation status when the status is valid.
    /// </summary>
    [Fact]
    public void ValidateReservationStatus_ValidReservationStatus_ReturnsReservationStatus()
    {
        // Arrange
        var validStatus = ReservationStatus.Confirmed;

        // Act
        var result = ReservationValidator.ValidateReservationStatus(validStatus);

        // Assert
        Assert.Equal(validStatus, result);
    }

    /// <summary>
    /// Tests the <see cref="ReservationValidator.ValidateReservationStatus(ReservationStatus)"/> method
    /// to ensure it throws a <see cref="ValidationException"/> when the status is invalid.
    /// </summary>
    [Fact]
    public void ValidateReservationStatus_InvalidReservationStatus_ThrowsValidationException()
    {
        // Arrange
        var invalidStatus = (ReservationStatus)999; // Invalid status not defined in the enum

        // Act & Assert
        var exception =
            Assert.Throws<ValidationException>(() => ReservationValidator.ValidateReservationStatus(invalidStatus));
        Assert.Equal(ValidationErrorCode.InvalidReservationStatus, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the <see cref="ReservationValidator.IsValidReservationStatus(ReservationStatus)"/> method
    /// to ensure it returns true for valid reservation statuses.
    /// </summary>
    [Fact]
    public void IsValidReservationStatus_ValidReservationStatus_ReturnsTrue()
    {
        // Arrange
        var validStatus = ReservationStatus.Confirmed;

        // Act
        var result = ReservationValidator.IsValidReservationStatus(validStatus);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Tests the <see cref="ReservationValidator.IsValidReservationStatus(ReservationStatus)"/> method
    /// to ensure it returns false for invalid reservation statuses.
    /// </summary>
    [Fact]
    public void IsValidReservationStatus_InvalidReservationStatus_ReturnsFalse()
    {
        // Arrange
        var invalidStatus = (ReservationStatus)999; // Invalid status

        // Act
        var result = ReservationValidator.IsValidReservationStatus(invalidStatus);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Tests the <see cref="ReservationValidator.ValidateReservationId(int)"/> method to ensure it returns
    /// the reservation ID when the ID is valid.
    /// </summary>
    [Fact]
    public void ValidateReservationId_ValidReservationId_ReturnsReservationId()
    {
        // Arrange
        int validId = 123;

        // Act
        var result = ReservationValidator.ValidateReservationId(validId);

        // Assert
        Assert.Equal(validId, result);
    }

    /// <summary>
    /// Tests the <see cref="ReservationValidator.ValidateReservationId(int)"/> method to ensure it throws
    /// a <see cref="ValidationException"/> when the reservation ID is invalid.
    /// </summary>
    [Fact]
    public void ValidateReservationId_InvalidReservationId_ThrowsValidationException()
    {
        // Arrange
        int invalidId = 0; // Invalid ID (less than or equal to 0)

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => ReservationValidator.ValidateReservationId(invalidId));
        Assert.Equal(ValidationErrorCode.InvalidId, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the <see cref="ReservationValidator.IsValidReservationId(int)"/> method to ensure it returns true
    /// for valid reservation IDs.
    /// </summary>
    [Fact]
    public void IsValidReservationId_ValidReservationId_ReturnsTrue()
    {
        // Arrange
        int validId = 123;

        // Act
        var result = ReservationValidator.IsValidReservationId(validId);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Tests the <see cref="ReservationValidator.IsValidReservationId(int)"/> method to ensure it returns false
    /// for invalid reservation IDs.
    /// </summary>
    [Fact]
    public void IsValidReservationId_InvalidReservationId_ReturnsFalse()
    {
        // Arrange
        int invalidId = 0; // Invalid ID

        // Act
        var result = ReservationValidator.IsValidReservationId(invalidId);

        // Assert
        Assert.False(result);
    }
}
}

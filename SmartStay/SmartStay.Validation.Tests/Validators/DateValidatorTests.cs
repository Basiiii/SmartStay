/// <copyright file="DateValidatorTests.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains unit tests for the <see cref="DateValidator"/> class,
/// ensuring the correct validation of dates in the SmartStay application, such as check-in
/// and check-out dates.
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
using System;
using Xunit;

/// <summary>
/// Contains unit tests for the <see cref="DateValidator"/> class.
/// Tests the validation logic for dates such as check-in and check-out dates.
/// </summary>
public class DateValidatorTests
{
    /// <summary>
    /// Tests the <see cref="DateValidator.ValidateCheckInDate(DateTime)"/> method to ensure that
    /// it returns the check-in date when the date is valid (today or in the future).
    /// </summary>
    [Fact]
    public void ValidateCheckInDate_ValidDate_ReturnsCheckInDate()
    {
        // Arrange
        DateTime validCheckInDate = DateTime.Today;

        // Act
        var result = DateValidator.ValidateCheckInDate(validCheckInDate);

        // Assert
        Assert.Equal(validCheckInDate, result);
    }

    /// <summary>
    /// Tests the <see cref="DateValidator.ValidateCheckInDate(DateTime)"/> method to ensure that
    /// it throws a <see cref="ValidationException"/> when the check-in date is in the past.
    /// </summary>
    [Fact]
    public void ValidateCheckInDate_InvalidDate_ThrowsValidationException()
    {
        // Arrange
        DateTime invalidCheckInDate = DateTime.Today.AddDays(-1); // Past date

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => DateValidator.ValidateCheckInDate(invalidCheckInDate));
        Assert.Equal(ValidationErrorCode.InvalidDate, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the <see cref="DateValidator.ValidateCheckOutDate(DateTime, DateTime)"/> method to ensure that
    /// it returns the check-out date when the date range is valid (check-out after check-in).
    /// </summary>
    [Fact]
    public void ValidateCheckOutDate_ValidDateRange_ReturnsCheckOutDate()
    {
        // Arrange
        DateTime validCheckInDate = DateTime.Today;
        DateTime validCheckOutDate = DateTime.Today.AddDays(1); // Check-out after check-in

        // Act
        var result = DateValidator.ValidateCheckOutDate(validCheckOutDate, validCheckInDate);

        // Assert
        Assert.Equal(validCheckOutDate, result);
    }

    /// <summary>
    /// Tests the <see cref="DateValidator.ValidateCheckOutDate(DateTime, DateTime)"/> method to ensure that
    /// it throws a <see cref="ValidationException"/> when the check-out date is before the check-in date.
    /// </summary>
    [Fact]
    public void ValidateCheckOutDate_InvalidDateRange_ThrowsValidationException()
    {
        // Arrange
        DateTime invalidCheckInDate = DateTime.Today;
        DateTime invalidCheckOutDate = DateTime.Today.AddDays(-1); // Check-out before check-in

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(
            () => DateValidator.ValidateCheckOutDate(invalidCheckOutDate, invalidCheckInDate));
        Assert.Equal(ValidationErrorCode.InvalidDateRange, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the <see cref="DateValidator.IsValidFutureDate(DateTime)"/> method to ensure it returns
    /// true when the date is today or in the future.
    /// </summary>
    [Fact]
    public void IsValidFutureDate_ValidDate_ReturnsTrue()
    {
        // Arrange
        DateTime validDate = DateTime.Today;

        // Act
        var result = DateValidator.IsValidFutureDate(validDate);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Tests the <see cref="DateValidator.IsValidFutureDate(DateTime)"/> method to ensure it returns
    /// false when the date is in the past.
    /// </summary>
    [Fact]
    public void IsValidFutureDate_InvalidDate_ReturnsFalse()
    {
        // Arrange
        DateTime invalidDate = DateTime.Today.AddDays(-1); // Past date

        // Act
        var result = DateValidator.IsValidFutureDate(invalidDate);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Tests the <see cref="DateValidator.IsValidDateRange(DateTime, DateTime)"/> method to ensure it returns
    /// true when the check-in date is earlier than the check-out date.
    /// </summary>
    [Fact]
    public void IsValidDateRange_ValidDateRange_ReturnsTrue()
    {
        // Arrange
        DateTime checkInDate = DateTime.Today;
        DateTime checkOutDate = DateTime.Today.AddDays(2);

        // Act
        var result = DateValidator.IsValidDateRange(checkInDate, checkOutDate);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Tests the <see cref="DateValidator.IsValidDateRange(DateTime, DateTime)"/> method to ensure it returns
    /// false when the check-in date is later than the check-out date.
    /// </summary>
    [Fact]
    public void IsValidDateRange_InvalidDateRange_ReturnsFalse()
    {
        // Arrange
        DateTime checkInDate = DateTime.Today.AddDays(2);
        DateTime checkOutDate = DateTime.Today;

        // Act
        var result = DateValidator.IsValidDateRange(checkInDate, checkOutDate);

        // Assert
        Assert.False(result);
    }
}
}

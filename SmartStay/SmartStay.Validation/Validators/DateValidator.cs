/// <copyright file="DateValidator.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the implementation of the <see cref="DateValidator"/> class,
/// which provides methods for validating dates used in the SmartStay application, such as check-in
/// and check-out dates.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>19/11/2024</date>

/// <summary>
/// The <c>SmartStay.Validation.Validators</c> namespace contains classes and methods for validating
/// various types of input data in the SmartStay application. These validations enforce data integrity
/// and compliance with application-specific requirements.
/// </summary>
namespace SmartStay.Validation.Validators
{
/// <summary>
/// Defines the <see cref="DateValidator"/> class, which provides functionality for validating
/// dates related to reservations, ensuring they adhere to application-specific rules.
/// </summary>
public static class DateValidator
{
    /// <summary>
    /// Validates the check-in date, throwing an exception if invalid.
    /// </summary>
    /// <param name="checkInDate">The check-in date to validate.</param>
    /// <returns>The validated check-in date if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the check-in date is invalid.</exception>
    public static DateTime ValidateCheckInDate(DateTime checkInDate)
    {
        if (!IsValidFutureDate(checkInDate))
        {
            throw new ValidationException(ValidationErrorCode.InvalidDate);
        }
        return checkInDate;
    }

    /// <summary>
    /// Validates the check-out date, throwing an exception if invalid.
    /// </summary>
    /// <param name="checkOutDate">The check-out date to validate.</param>
    /// <param name="checkInDate">The check-in date to compare.</param>
    /// <returns>The validated check-out date if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the check-out date is invalid.</exception>
    public static DateTime ValidateCheckOutDate(DateTime checkOutDate, DateTime checkInDate)
    {
        if (!IsValidDateRange(checkInDate, checkOutDate))
        {
            throw new ValidationException(ValidationErrorCode.InvalidDateRange);
        }
        return checkOutDate;
    }

    /// <summary>
    /// Checks if the date is a valid future or present date.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns><c>true</c> if the date is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidFutureDate(DateTime date)
    {
        return date >= DateTime.Today;
    }

    /// <summary>
    /// Validates that the check-in date is before the check-out date.
    /// </summary>
    /// <param name="checkInDate">The check-in date.</param>
    /// <param name="checkOutDate">The check-out date.</param>
    /// <returns><c>true</c> if the check-in date is earlier than the check-out date; otherwise, <c>false</c>.</returns>
    public static bool IsValidDateRange(DateTime checkInDate, DateTime checkOutDate)
    {
        return checkInDate < checkOutDate;
    }
}
}

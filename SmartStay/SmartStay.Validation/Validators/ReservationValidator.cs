/// <copyright file="ReservationValidator.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains validation logic for reservation-related data, including methods to validate
/// reservation statuses for compliance with defined business rules.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>19/11/2024</date>
using SmartStay.Common.Enums;

/// <summary>
/// The <c>SmartStay.Validation.Validators</c> namespace provides classes and methods
/// dedicated to validating various aspects of the SmartStay application. These validations
/// ensure that input data adheres to business requirements and standards.
/// </summary>
namespace SmartStay.Validation.Validators
{
/// <summary>
/// Provides validation methods for reservation-related data.
/// </summary>
public static class ReservationValidator
{
    /// <summary>
    /// Validates the reservation status, throwing an exception if it is invalid.
    /// </summary>
    /// <param name="status">The reservation status to validate.</param>
    /// <returns>The validated reservation status if it is valid.</returns>
    /// <exception cref="ValidationException">
    /// Thrown when the reservation status is not defined in the <see cref="ReservationStatus"/> enum.
    /// </exception>
    public static ReservationStatus ValidateReservationStatus(ReservationStatus status)
    {
        if (!IsValidReservationStatus(status))
        {
            throw new ValidationException(ValidationErrorCode.InvalidReservationStatus);
        }
        return status;
    }

    /// <summary>
    /// Determines whether the specified reservation status is valid by checking if it is defined
    /// in the <see cref="ReservationStatus"/> enum.
    /// </summary>
    /// <param name="status">The reservation status to check.</param>
    /// <returns>
    /// <c>true</c> if the reservation status is defined in the <see cref="ReservationStatus"/> enum;
    /// otherwise, <c>false</c>.
    /// </returns>
    public static bool IsValidReservationStatus(ReservationStatus status)
    {
        return Enum.IsDefined(typeof(ReservationStatus), status);
    }

    /// <summary>
    /// Validates a reservation ID, throwing an exception if invalid.
    /// </summary>
    /// <param name="id">The reservation ID to validate.</param>
    /// <returns>The validated reservation ID if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the reservation ID is invalid.</exception>
    public static int ValidateReservationId(int id)
    {
        if (!IsValidReservationId(id))
        {
            throw new ValidationException(ValidationErrorCode.InvalidId);
        }
        return id;
    }

    /// <summary>
    /// Checks if a reservation ID is valid.
    /// </summary>
    /// <param name="id">The reservation ID to check.</param>
    /// <returns><c>true</c> if the reservation ID is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidReservationId(int id) => id > 0;
}
}

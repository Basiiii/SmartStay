/// <copyright file="UpdateReservationResult.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="UpdateReservationResult"/> enumeration used in the SmartStay
/// application, representing the different results of a reservation update attempt.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>29/11/2024</date>

/// <summary>
/// This namespace contains enumerations used within the SmartStay application.
/// </summary>
namespace SmartStay.Common.Enums
{
/// <summary>
/// Enumeration representing the results of the reservation update process.
/// This enum is used to indicate the outcome of the update operation for a reservation.
/// </summary>
public enum UpdateReservationResult
{
    /// <summary>
    /// Indicates that the reservation was successfully updated.
    /// </summary>
    Success,

    /// <summary>
    /// Indicates that the reservation with the specified ID could not be found.
    /// </summary>
    ReservationNotFound,

    /// <summary>
    /// Indicates that the accommodation with the specified ID could not be found.
    /// </summary>
    AccommodationNotFound,

    /// <summary>
    /// Indicates that the room associated with the reservation could not be found.
    /// </summary>
    RoomNotFound,

    /// <summary>
    /// Indicates that the room found was null.
    /// </summary>
    RoomIsNull,

    /// <summary>
    /// Indicates that the new dates for the reservation are unavailable.
    /// </summary>
    DatesUnavailable,

    /// <summary>
    /// Indicates that the given dates are not valid.
    /// </summary>
    InvalidDates,

    /// <summary>
    /// Indicates that an unknown error occurred during the reservation update process.
    /// </summary>
    Error
}
}

/// <copyright file="CancellationResult.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="CancellationResult"/> enumeration used in the SmartStay
/// application, representing the different results of a reservation cancellation attempt.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>29/11/2024</date>

/// <summary>
/// This namespace contains enumerations used within the SmartStay application.
/// </summary>
namespace SmartStay.Common.Enums
{
/// <summary>
/// Enumeration representing the possible outcomes of a reservation cancellation attempt.
/// </summary>
public enum CancellationResult
{
    /// <summary>
    /// Indicates that the reservation cancellation was successful.
    /// </summary>
    Success,

    /// <summary>
    /// Indicates that the reservation could not be cancelled because the reservation with the specified ID could not be
    /// found.
    /// </summary>
    ReservationNotFound,

    /// <summary>
    /// Indicates that the reservation could not be cancelled because the associated accommodation could not be found.
    /// </summary>
    AccommodationNotFound,

    /// <summary>
    /// Indicates that the reservation could not be cancelled because the associated room could not be found.
    /// </summary>
    RoomNotFound,

    /// <summary>
    /// Indicates an unspecified error occurred during the cancellation process.
    /// </summary>
    Error
}
}

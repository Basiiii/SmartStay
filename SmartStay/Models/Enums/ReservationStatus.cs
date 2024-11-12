/// <copyright file="ReservationStatus.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the definition of the ReservationStatus enumeration used in the SmartStay application,
/// representing the different statuses a reservation can have.
/// </file>
/// <summary>
/// The <see cref="ReservationStatus"/> enumeration defines the various reservation status types available
/// in the SmartStay application.
/// </summary>
/// <author>Enrique Rodrigues</author>
/// <date>07/10/2024</date>

namespace SmartStay.Models.Enums
{
/// <summary>
/// Enumeration representing the current status of a reservation.
/// </summary>
public enum ReservationStatus
{
    /// <summary>
    /// Reservation has been made but the client has not yet checked in.
    /// </summary>
    Pending,

    /// <summary>
    /// Client has checked in to the accommodation.
    /// </summary>
    CheckedIn,

    /// <summary>
    /// Client has checked out from the accommodation.
    /// </summary>
    CheckedOut,

    /// <summary>
    /// Reservation was cancelled before the client checked in.
    /// </summary>
    Cancelled,

    /// <summary>
    /// Client did not show up for the reservation.
    /// </summary>
    NoShow,

    /// <summary>
    /// Reservation has been confirmed, but the client has not yet checked in.
    /// </summary>
    Confirmed,

    /// <summary>
    /// Reservation was declined or denied due to some issue (e.g., payment failure, overbooked, etc.).
    /// </summary>
    Declined
}
}

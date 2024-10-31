/// <copyright file="ReservationStatus.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the definition of the ReservationStatus enumeration used in the SmartStay application.
/// </file>
/// <summary>
/// The <see cref="ReservationStatus"/> enumeration defines the various reservation status types available
/// in the SmartStay application.
/// </summary>
/// <author>Enrique Rodrigues</author>
/// <date>07/10/2024</date>

namespace SmartStay
{
/// <summary>
/// Enumerator representing the current status of a reservation.
/// </summary>
public enum ReservationStatus
{
    Pending,   // Reservation made but not yet checked in
    CheckedIn, // Client has checked in
    CheckedOut // Client has checked out
}
}

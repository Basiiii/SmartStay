/// <copyright file="UpdateReservationRequest.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file defines the <see cref="UpdateReservationRequest"/> class, a data transfer object (DTO) used to
/// encapsulate reservation update details. It ensures a standardized format for incoming reservation update data.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>12/13/2024</date>

/// <summary>
/// The <c>SmartStay.API.DTOs</c> namespace contains data transfer objects (DTOs) used to facilitate
/// communication between the client and server. These objects encapsulate the data required for various
/// API operations, ensuring consistency and simplicity in data handling.
/// </summary>
namespace SmartStay.Api.DTOs.Reservation
{
/// <summary>
/// Represents the data required to update an existing reservation.
/// </summary>
public class UpdateReservationRequest
{
    /// <summary>
    /// Gets or sets the new check-in date for the reservation (optional).
    /// </summary>
    public DateTime? NewCheckIn { get; set; }

    /// <summary>
    /// Gets or sets the new check-out date for the reservation (optional).
    /// </summary>
    public DateTime? NewCheckOut { get; set; }
}
}

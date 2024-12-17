/// <copyright file="CreateReservationRequest.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file defines the <see cref="CreateReservationRequest"/> class, a data transfer object (DTO) used to
/// encapsulate reservation creation details. It ensures a standardized format for incoming reservation creation data.
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
/// Represents the data required to create a new reservation.
/// </summary>
public class CreateReservationRequest
{
    /// <summary>
    /// Gets or sets the ID of the client making the reservation.
    /// </summary>
    public int ClientId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the accommodation associated with the reservation.
    /// </summary>
    public int AccommodationId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the room being reserved.
    /// </summary>
    public int RoomId { get; set; }

    /// <summary>
    /// Gets or sets the check-in date of the reservation.
    /// </summary>
    public DateTime CheckIn { get; set; }

    /// <summary>
    /// Gets or sets the check-out date of the reservation.
    /// </summary>
    public DateTime CheckOut { get; set; }
}
}

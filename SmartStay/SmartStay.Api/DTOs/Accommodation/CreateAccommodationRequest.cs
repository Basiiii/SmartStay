/// <copyright file="CreateAccommodationRequest.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file defines the <see cref="CreateAccommodationRequest"/> class, a data transfer object (DTO) used to
/// encapsulate the details required for creating a new accommodation. It ensures a standardized format for incoming
/// accommodation data.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>05/12/2024</date>

/// <summary>
/// The <c>SmartStay.API.DTOs.Accommodation</c> namespace contains data transfer objects (DTOs) used to facilitate
/// communication between the client and server for accommodation-related operations. These objects ensure consistent
/// data exchange for creating, updating, and managing accommodations.
/// </summary>
namespace SmartStay.API.DTOs.Accommodation
{
using SmartStay.Common.Enums;

/// <summary>
/// Represents the data required to create a new accommodation.
/// </summary>
public class CreateAccommodationRequest
{
    /// <summary>
    /// Gets or sets the ID of the owner of the accommodation.
    /// </summary>
    /// <remarks>
    /// The <see cref="OwnerId"/> must correspond to a valid owner in the system.
    /// </remarks>
    public required int OwnerId { get; set; }

    /// <summary>
    /// Gets or sets the type of the accommodation.
    /// </summary>
    /// <remarks>
    /// The <see cref="Type"/> must be a valid enum value of <see cref="AccommodationType"/>.
    /// Examples: Hotel, Apartment, Villa.
    /// </remarks>
    public required AccommodationType Type { get; set; }

    /// <summary>
    /// Gets or sets the name of the accommodation.
    /// </summary>
    /// <remarks>
    /// The <see cref="Name"/> must be a non-empty string that meets name validation criteria.
    /// </remarks>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the address of the accommodation.
    /// </summary>
    /// <remarks>
    /// The <see cref="Address"/> must include sufficient details such as street, city, and country.
    /// </remarks>
    public required string Address { get; set; }
}
}

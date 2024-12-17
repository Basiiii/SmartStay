/// <copyright file="UpdateAccommodationRequest.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file defines the <see cref="UpdateAccommodationRequest"/> class, a data transfer object (DTO) used to
/// encapsulate the details required to update an existing accommodation. It ensures a standardized format for updating
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
/// Represents the data required to update an existing accommodation.
/// </summary>
public class UpdateAccommodationRequest
{
    /// <summary>
    /// Gets or sets the new type of the accommodation (optional).
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="AccommodationType.None"/> if no new type is provided.
    /// Only valid <see cref="AccommodationType"/> values are accepted.
    /// </remarks>
    public AccommodationType Type { get; set; } = AccommodationType.None;

    /// <summary>
    /// Gets or sets the new name of the accommodation (optional).
    /// </summary>
    /// <remarks>
    /// If provided, the <see cref="Name"/> must meet name validation criteria.
    /// </remarks>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the new address of the accommodation (optional).
    /// </summary>
    /// <remarks>
    /// If provided, the <see cref="Address"/> must include sufficient details such as street, city, and country.
    /// </remarks>
    public string? Address { get; set; }
}
}

/// <copyright file="CreateOwnerRequest.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file defines the <see cref="UpdateOwnerRequest"/> class, a data transfer object (DTO) used to
/// encapsulate owner update details. It ensures a standardized format for incoming owner data.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>05/12/2024</date>

/// <summary>
/// The <c>SmartStay.API.DTOs</c> namespace contains data transfer objects (DTOs) used to facilitate
/// communication between the client and server. These objects encapsulate the data required for various
/// API operations, ensuring consistency and simplicity in data handling.
/// </summary>
namespace SmartStay.Api.DTOs.Owner
{
/// <summary>
/// Represents the data required to update an existing owner.
/// </summary>
public class UpdateOwnerRequest
{
    /// <summary>
    /// Gets or sets the first name of the owner (optional).
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the owner (optional).
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Gets or sets the email address of the owner (optional).
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the owner (optional).
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the residential address of the owner (optional).
    /// </summary>
    public string? Address { get; set; }
}
}

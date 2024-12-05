/// <copyright file="CreateClientRequest.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file defines the <see cref="CreateCompleteClientRequest"/> class, a data transfer object (DTO) used to
/// encapsulate client creation request details. It ensures a standardized format for incoming client data.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>05/12/2024</date>

/// <summary>
/// The <c>SmartStay.API.DTOs</c> namespace contains data transfer objects (DTOs) used to facilitate
/// communication between the client and server. These objects encapsulate the data required for various
/// API operations, ensuring consistency and simplicity in data handling.
/// </summary>
namespace SmartStay.Api.DTOs.Client
{
/// <summary>
/// Represents the data required to create a new complete client.
/// </summary>
public class CreateCompleteClientRequest
{
    /// <summary>
    /// Gets or sets the first name of the client.
    /// </summary>
    public required string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the client.
    /// </summary>
    public required string LastName { get; set; }

    /// <summary>
    /// Gets or sets the email address of the client.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the client.
    /// </summary>
    public required string PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the address of the client.
    /// </summary>
    public required string Address { get; set; }
}
}

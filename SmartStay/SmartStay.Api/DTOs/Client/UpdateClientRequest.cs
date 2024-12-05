/// <copyright file="CreateClientRequest.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file defines the <see cref="UpdateClientRequest"/> class, a data transfer object (DTO) used to
/// encapsulate client update details. It ensures a standardized format for incoming client data.
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
using SmartStay.Common.Enums;

/// <summary>
/// Represents the data required to update an existing client.
/// </summary>
public class UpdateClientRequest
{
    /// <summary>
    /// Gets or sets the first name of the client (optional).
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the client (optional).
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Gets or sets the email address of the client (optional).
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the client (optional).
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the residential address of the client (optional).
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets the preferred payment method of the client (optional).
    /// </summary>
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Unchanged;
}
}

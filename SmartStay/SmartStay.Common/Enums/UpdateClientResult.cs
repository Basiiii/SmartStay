/// <copyright file="UpdateClientResult.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="UpdateClientResult"/> enumeration used in the SmartStay
/// application, representing the different results of a client update attempt.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>29/11/2024</date>

/// <summary>
/// This namespace contains enumerations used within the SmartStay application.
/// </summary>
namespace SmartStay.Common.Enums
{
/// <summary>
/// Enumeration representing the results of the client update process.
/// This enum is used to indicate the outcome of the update operation for a client.
/// </summary>
public enum UpdateClientResult
{
    /// <summary>
    /// Indicates that the client was successfully updated.
    /// </summary>
    Success,

    /// <summary>
    /// Indicates that the client with the specified ID could not be found.
    /// </summary>
    ClientNotFound,

    /// <summary>
    /// Indicates that the provided first name is invalid.
    /// </summary>
    InvalidFirstName,

    /// <summary>
    /// Indicates that the provided last name is invalid.
    /// </summary>
    InvalidLastName,

    /// <summary>
    /// Indicates that the provided email address is invalid.
    /// </summary>
    InvalidEmail,

    /// <summary>
    /// Indicates that the provided phone number is invalid.
    /// </summary>
    InvalidPhoneNumber,

    /// <summary>
    /// Indicates that the provided address is invalid.
    /// </summary>
    InvalidAddress,

    /// <summary>
    /// Indicates that the provided payment method is invalid.
    /// </summary>
    InvalidPaymentMethod,

    /// <summary>
    /// Indicates that an unknown error occurred during the update process.
    /// </summary>
    Error
}
}

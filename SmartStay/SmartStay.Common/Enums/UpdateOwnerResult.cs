/// <copyright file="UpdateOwnerResult.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="UpdateOwnerResult"/> enumeration used in the SmartStay
/// application, representing the different results of an owner update attempt.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>29/11/2024</date>

/// <summary>
/// This namespace contains enumerations used within the SmartStay application.
/// </summary>
namespace SmartStay.Common.Enums
{
/// <summary>
/// Enum representing the result of an owner update operation.
/// </summary>
public enum UpdateOwnerResult
{
    /// <summary>
    /// The operation was successful.
    /// </summary>
    Success,

    /// <summary>
    /// The owner with the specified ID was not found.
    /// </summary>
    OwnerNotFound,

    /// <summary>
    /// The first name provided is invalid.
    /// </summary>
    InvalidFirstName,

    /// <summary>
    /// The last name provided is invalid.
    /// </summary>
    InvalidLastName,

    /// <summary>
    /// The email provided is invalid.
    /// </summary>
    InvalidEmail,

    /// <summary>
    /// The phone number provided is invalid.
    /// </summary>
    InvalidPhoneNumber,

    /// <summary>
    /// The address provided is invalid.
    /// </summary>
    InvalidAddress
}

}

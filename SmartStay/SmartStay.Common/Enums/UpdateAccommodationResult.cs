/// <copyright file="UpdateAccommodationResult.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="UpdateAccommodationResult"/> enumeration used in the SmartStay
/// application, representing the different results of an accommodation update attempt.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>29/11/2024</date>

/// <summary>
/// This namespace contains enumerations used within the SmartStay application.
/// </summary>
namespace SmartStay.Common.Enums
{
/// <summary>
/// Enumeration representing the results of the accommodation update process.
/// This enum is used to indicate the outcome of the update operation for an accommodation.
/// </summary>
public enum UpdateAccommodationResult
{
    /// <summary>
    /// Indicates that the accommodation was successfully updated.
    /// </summary>
    Success,

    /// <summary>
    /// Indicates that the accommodation with the specified ID could not be found.
    /// </summary>
    AccommodationNotFound,

    /// <summary>
    /// Indicates that the provided accommodation type is invalid.
    /// </summary>
    InvalidType,

    /// <summary>
    /// Indicates that the provided accommodation name is invalid.
    /// </summary>
    InvalidName,

    /// <summary>
    /// Indicates that the provided accommodation address is invalid.
    /// </summary>
    InvalidAddress,

    /// <summary>
    /// Indicates that an unknown error occurred during the accommodation update process.
    /// </summary>
    Error
}
}

/// <copyright file="RemoveAccommodationResult.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="RemoveAccommodationResult"/> enumeration used in the SmartStay
/// application, representing the different results of an accommodation removal attempt.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>29/11/2024</date>

/// <summary>
/// This namespace contains enumerations used within the SmartStay application.
/// </summary>
namespace SmartStay.Common.Enums
{
/// <summary>
/// Enumeration representing the results of the accommodation removal process.
/// This enum is used to indicate the outcome of the removal operation for an accommodation.
/// </summary>
public enum RemoveAccommodationResult
{
    /// <summary>
    /// Indicates that the accommodation was successfully removed.
    /// </summary>
    Success,

    /// <summary>
    /// Indicates that the accommodation with the specified ID could not be found.
    /// </summary>
    AccommodationNotFound,

    /// <summary>
    /// Indicates that the owner associated with the accommodation could not be found.
    /// </summary>
    OwnerNotFound,

    /// <summary>
    /// Indicates that the accommodation could not be removed from the system.
    /// </summary>
    AccommodationRemovalFailed,

    /// <summary>
    /// Indicates that the accommodation could not be disassociated from the owner.
    /// </summary>
    AccommodationDisassociationFailed,

    /// <summary>
    /// Indicates that an unknown error occurred during the removal process.
    /// </summary>
    Error
}
}

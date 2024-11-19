/// <copyright file="AccommodationValidator.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the implementation of the <see cref="AccommodationValidator"/> class,
/// which provides methods for validating accommodation-related data in the SmartStay application.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>19/11/2024</date>
/// <summary>
/// The <c>SmartStay.Validation.Validators</c> namespace contains classes and methods for validating
/// various types of input data in the SmartStay application. These validations enforce data integrity
/// and compliance with application-specific requirements.
/// </summary>
using SmartStay.Common.Enums;

namespace SmartStay.Validation.Validators
{
/// <summary>
/// Defines the <see cref="AccommodationValidator"/> class, which provides functionality for validating
/// accommodation types in the SmartStay application.
/// </summary>
public static class AccommodationValidator
{
    /// <summary>
    /// Validates the accommodation type, throwing an exception if invalid.
    /// </summary>
    /// <param name="accommodationType">The accommodation type to validate.</param>
    /// <returns>The validated accommodation type if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the accommodation type is invalid.</exception>
    public static AccommodationType ValidateAccommodationType(AccommodationType accommodationType)
    {
        if (!IsValidAccommodationType(accommodationType))
        {
            throw new ValidationException(ValidationErrorCode.InvalidAccommodationType);
        }
        return accommodationType;
    }

    /// <summary>
    /// Checks if the accommodation type is valid by verifying it is defined in the <see cref="AccommodationType"/>
    /// enum.
    /// </summary>
    /// <param name="accommodationType">The accommodation type to check.</param>
    /// <returns><c>true</c> if the accommodation type is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidAccommodationType(AccommodationType accommodationType)
    {
        return Enum.IsDefined(typeof(AccommodationType), accommodationType);
    }

    /// <summary>
    /// Validates an accommodation ID, throwing an exception if invalid.
    /// </summary>
    /// <param name="id">The accommodation ID to validate.</param>
    /// <returns>The validated accommodation ID if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the accommodation ID is invalid.</exception>
    public static int ValidateAccommodationId(int id)
    {
        if (!IsValidAccommodationId(id))
        {
            throw new ValidationException(ValidationErrorCode.InvalidId);
        }
        return id;
    }

    /// <summary>
    /// Checks if an accommodation ID is valid.
    /// </summary>
    /// <param name="id">The accommodation ID to check.</param>
    /// <returns><c>true</c> if the accommodation ID is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidAccommodationId(int id) => id > 0;
}
}

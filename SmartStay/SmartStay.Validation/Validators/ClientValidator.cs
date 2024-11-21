/// <copyright file="ClientValidator.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the implementation of the <see cref="ClientValidator"/> class,
/// which provides validation methods for client-related data in the SmartStay application.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>19/11/2024</date>

/// <summary>
/// The <c>SmartStay.Validation.Validators</c> namespace contains classes and methods for validating
/// various types of input data in the SmartStay application. These validations enforce data integrity
/// and compliance with application-specific requirements.
/// </summary>
namespace SmartStay.Validation.Validators
{
/// <summary>
/// Defines the <see cref="ClientValidator"/> class, which provides functionality for validating
/// client-related data in the SmartStay application.
/// </summary>
public static class ClientValidator
{
    /// <summary>
    /// Validates a client ID, throwing an exception if invalid.
    /// </summary>
    /// <param name="id">The client ID to validate.</param>
    /// <returns>The validated client ID if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the client ID is invalid.</exception>
    public static int ValidateClientId(int id)
    {
        if (!IsValidClientId(id))
        {
            throw new ValidationException(ValidationErrorCode.InvalidId);
        }
        return id;
    }

    /// <summary>
    /// Checks if a client ID is valid.
    /// </summary>
    /// <param name="id">The client ID to check.</param>
    /// <returns><c>true</c> if the client ID is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidClientId(int id) => id > 0;
}
}

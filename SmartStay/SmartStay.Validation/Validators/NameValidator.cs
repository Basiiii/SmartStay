/// <copyright file="NameValidator.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the implementation of the <see cref="NameValidator"/> class,
/// which provides methods for validating names, including user names and accommodation names,
/// ensuring they meet the application's requirements.
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
  /// Defines the <see cref="NameValidator"/> class, which provides functionality for validating
  /// various types of names within the SmartStay application.
  /// </summary>
  public static class NameValidator
{
    /// <summary>
    /// Validates a name, throwing an exception if invalid.
    /// </summary>
    /// <param name="name">The name to validate.</param>
    /// <returns>The validated name if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the name is invalid.</exception>
    public static string ValidateName(string name)
    {
        if (!IsValidName(name))
        {
            throw new ValidationException(ValidationErrorCode.InvalidName);
        }
        return name;
    }

    /// <summary>
    /// Checks if a name is valid.
    /// </summary>
    /// <param name="name">The name to check.</param>
    /// <returns><c>true</c> if the name is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidName(string name) => !string.IsNullOrWhiteSpace(name) && name.Length <= 50;

    /// <summary>
    /// Validates an accommodation name, throwing an exception if invalid.
    /// </summary>
    /// <param name="name">The accommodation name to validate.</param>
    /// <returns>The validated name if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the name is invalid.</exception>
    public static string ValidateAccommodationName(string name)
    {
        if (!IsValidAccommodationName(name))
        {
            throw new ValidationException(ValidationErrorCode.InvalidAccommodationName);
        }
        return name;
    }

    /// <summary>
    /// Checks if an accommodation name is valid.
    /// </summary>
    /// <param name="name">The name to check.</param>
    /// <returns><c>true</c> if the name is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidAccommodationName(string name) => !string.IsNullOrWhiteSpace(name) && name.Length <= 100;
}
}

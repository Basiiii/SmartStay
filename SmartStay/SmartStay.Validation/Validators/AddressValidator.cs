/// <copyright file="AddressValidator.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the implementation of the <see cref="AddressValidator"/> class,
/// which provides methods for validating address-related data in the SmartStay application.
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
  /// Defines the <see cref="AddressValidator"/> class, which provides functionality for validating
  /// addresses used in the SmartStay application.
  /// </summary>
  public static class AddressValidator
{
    /// <summary>
    /// Validates an address, throwing an exception if invalid.
    /// </summary>
    /// <param name="address">The address to validate.</param>
    /// <returns>The validated address if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the address is invalid.</exception>
    public static string ValidateAddress(string address)
    {
        if (!IsValidAddress(address))
        {
            throw new ValidationException(ValidationErrorCode.InvalidAddress);
        }
        return address;
    }

    /// <summary>
    /// Checks if an address is valid.
    /// </summary>
    /// <param name="address">The address to check.</param>
    /// <returns><c>true</c> if the address is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidAddress(string address) => !string.IsNullOrWhiteSpace(address);
}
}

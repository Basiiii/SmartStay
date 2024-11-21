/// <copyright file="EmailValidator.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the implementation of the <see cref="EmailValidator"/> class,
/// which provides methods for validating email addresses to ensure they adhere to standard formatting.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>19/11/2024</date>
using System.Text.RegularExpressions;

/// <summary>
/// The <c>SmartStay.Validation.Validators</c> namespace contains classes and methods for validating
/// various types of input data in the SmartStay application. These validations enforce data integrity
/// and compliance with application-specific requirements.
/// </summary>
namespace SmartStay.Validation.Validators
{
/// <summary>
/// Defines the <see cref="EmailValidator"/> class, which provides functionality for validating
/// email addresses within the SmartStay application.
/// </summary>
public static class EmailValidator
{
    // TODO: add a way to check if an email is already being used

    /// <summary>
    /// Regular expression pattern for validating email addresses.
    /// </summary>
    static readonly string EmailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

    /// <summary>
    /// Validates an email address, throwing an exception if invalid.
    /// </summary>
    /// <param name="email">The email address to validate.</param>
    /// <returns>The validated email address if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the email address is invalid.</exception>
    public static string ValidateEmail(string email)
    {
        if (!IsValidEmail(email))
        {
            throw new ValidationException(ValidationErrorCode.InvalidEmail);
        }
        return email;
    }

    /// <summary>
    /// Checks if an email address is valid based on the defined <see cref="EmailPattern"/>.
    /// </summary>
    /// <param name="email">The email address to check.</param>
    /// <returns><c>true</c> if the email address is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidEmail(string email) => !string.IsNullOrWhiteSpace(email) && Regex.IsMatch(email,
                                                                                                        EmailPattern);
}
}

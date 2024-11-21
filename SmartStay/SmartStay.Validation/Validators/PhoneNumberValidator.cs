/// <copyright file="PhoneNumberValidator.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the implementation of the <see cref="PhoneNumberValidator"/> class,
/// which provides methods for validating phone numbers within the SmartStay application.
/// The validation ensures compliance with specific formats and business rules.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>19/11/2024</date>
using System.Text.RegularExpressions;

/// <summary>
/// The <c>SmartStay.Validation.Validators</c> namespace provides classes and methods
/// dedicated to validating various aspects of the SmartStay application. These validations
/// ensure that input data adheres to business requirements and standards.
/// </summary>
namespace SmartStay.Validation.Validators
{
/// <summary>
/// The <c>SmartStay.Validation.Validators</c> namespace contains classes and methods for validating
/// various types of input data in the SmartStay application. These validations enforce data integrity
/// and compliance with application-specific requirements.
/// </summary>
public static class PhoneNumberValidator
{
    /// <summary>
    /// Regular expression pattern for validating phone numbers. Follows the European format
    /// with an international dialing code prefix and a valid number length.
    /// </summary>
    private static readonly string PhoneNumberPattern = @"^\+(\d{1,3})\d{7,15}$";

    /// <summary>
    /// Validates a phone number, throwing an exception if it does not match the expected format.
    /// </summary>
    /// <param name="phoneNumber">The phone number to validate.</param>
    /// <returns>The validated phone number if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the phone number is invalid.</exception>
    public static string ValidatePhoneNumber(string phoneNumber)
    {
        if (!IsValidPhoneNumber(phoneNumber))
        {
            throw new ValidationException(ValidationErrorCode.InvalidPhoneNumber);
        }
        return phoneNumber;
    }

    /// <summary>
    /// Determines if a phone number is valid by checking against the defined pattern and ensuring
    /// it is not null or empty.
    /// </summary>
    /// <param name="phoneNumber">The phone number to check.</param>
    /// <returns><c>true</c> if the phone number is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidPhoneNumber(string phoneNumber) =>
        !string.IsNullOrWhiteSpace(phoneNumber) && Regex.IsMatch(phoneNumber, PhoneNumberPattern);
}
}

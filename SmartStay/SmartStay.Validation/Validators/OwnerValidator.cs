/// <copyright file="OwnerValidator.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the implementation of the <see cref="OwnerValidator"/> class,
/// which provides validation methods for owner-related data in the SmartStay application.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>27/11/2024</date>

/// <summary>
/// The <c>SmartStay.Validation.Validators</c> namespace contains classes and methods for validating
/// various types of input data in the SmartStay application. These validations enforce data integrity
/// and compliance with application-specific requirements.
/// </summary>
namespace SmartStay.Validation.Validators
{
/// <summary>
/// Defines the <see cref="OwnerValidator"/> class, which provides functionality for validating
/// owner-related data in the SmartStay application.
/// </summary>
public static class OwnerValidator
{
    /// <summary>
    /// Validates an owner ID, throwing an exception if invalid.
    /// </summary>
    /// <param name="id">The owner ID to validate.</param>
    /// <returns>The validated owner ID if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the owner ID is invalid.</exception>
    public static int ValidateOwnerId(int id)
    {
        if (!IsValidOwnerId(id))
        {
            throw new ValidationException(ValidationErrorCode.InvalidId);
        }
        return id;
    }

    /// <summary>
    /// Checks if an owner ID is valid.
    /// </summary>
    /// <param name="id">The owner ID to check.</param>
    /// <returns><c>true</c> if the owner ID is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidOwnerId(int id) => id > 0;

    /// <summary>
    /// Validates the owner's name, throwing an exception if invalid.
    /// </summary>
    /// <param name="name">The owner's name to validate.</param>
    /// <returns>The validated name if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the name is invalid.</exception>
    public static string ValidateOwnerName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
        {
            throw new ValidationException(ValidationErrorCode.InvalidName);
        }
        return name;
    }

    /// <summary>
    /// Validates the owner's email address, throwing an exception if invalid.
    /// </summary>
    /// <param name="email">The owner's email to validate.</param>
    /// <returns>The validated email if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the email is invalid.</exception>
    public static string ValidateOwnerEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@') || !email.Contains('.'))
        {
            throw new ValidationException(ValidationErrorCode.InvalidEmail);
        }
        return email;
    }

    /// <summary>
    /// Validates the owner's phone number, throwing an exception if invalid.
    /// </summary>
    /// <param name="phoneNumber">The owner's phone number to validate.</param>
    /// <returns>The validated phone number if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the phone number is invalid.</exception>
    public static string ValidateOwnerPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length < 10)
        {
            throw new ValidationException(ValidationErrorCode.InvalidPhoneNumber);
        }
        return phoneNumber;
    }
}
}

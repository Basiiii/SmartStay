/// <copyright file="Validator.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the definition of validation methods used in the SmartStay application.
/// These methods validate various input parameters such as IDs, names, emails, phone numbers,
/// addresses, and payment methods to ensure data integrity.
/// </file>
/// <summary>
/// The <see cref="Validator"/> class provides static methods for validating input data
/// related to the SmartStay application. It includes methods for checking the validity of
/// IDs, names, emails, phone numbers, addresses, and payment methods. These validation
/// functions help maintain data integrity and ensure that input values conform to expected
/// formats and constraints.
/// </summary>
/// <author>Enrique Rodrigues</author>
/// <date>07/10/2024</date>
using System.Text.RegularExpressions;

namespace SmartStay
{
/// <summary>
/// Provides a set of static methods for validating input data in the SmartStay application.
/// </summary>
public static class Validator
{
    private static readonly string EmailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

    /// <summary>
    /// Checks if the provided ID is valid.
    /// </summary>
    /// <param name="id">The ID to validate.</param>
    /// <returns>true if the ID is valid, otherwise false.</returns>
    public static bool IsValidId(int id)
    {
        return id >= 0;
    }

    /// <summary>
    /// Validates the provided ID and throws an exception if it is not valid.
    /// </summary>
    /// <param name="id">The ID to validate.</param>
    /// <param name="parameterName">The name of the parameter for error reporting.</param>
    /// <exception cref="ArgumentException">Thrown when the ID is less than 0.</exception>
    public static void ValidateId(int id, string parameterName)
    {
        if (!IsValidId(id))
        {
            throw new ArgumentException("Id must be greater than zero.", parameterName);
        }
    }

    /// <summary>
    /// Checks if the provided name is valid.
    /// </summary>
    /// <param name="name">The name to validate.</param>
    /// <returns>true if the name is not null, whitespace, and is 50 characters or fewer; otherwise, false.</returns>
    public static bool IsValidName(string name)
    {
        return !string.IsNullOrWhiteSpace(name) && name.Length <= 50;
    }

    /// <summary>
    /// Validates the provided name and throws an exception if it is not valid.
    /// </summary>
    /// <param name="name">The name to validate.</param>
    /// <param name="parameterName">The name of the parameter for error reporting.</param>
    /// <exception cref="ArgumentException">Thrown when the name is null, whitespace, or exceeds 50
    /// characters.</exception>
    public static void ValidateName(string name, string parameterName)
    {
        if (!IsValidName(name))
        {
            throw new ArgumentException($"{parameterName} cannot be empty and must be 50 characters or fewer.",
                                        parameterName);
        }
    }

    /// <summary>
    /// Checks if the provided email address is valid.
    /// </summary>
    /// <param name="email">The email address to validate.</param>
    /// <returns>true if the email is not null, whitespace, and matches the email pattern; otherwise, false.</returns>
    public static bool IsValidEmail(string email)
    {
        return !string.IsNullOrWhiteSpace(email) && Regex.IsMatch(email, EmailPattern);
    }

    /// <summary>
    /// Validates the provided email address and throws an exception if it is not valid.
    /// </summary>
    /// <param name="email">The email address to validate.</param>
    /// <param name="parameterName">The name of the parameter for error reporting.</param>
    /// <exception cref="ArgumentException">Thrown when the email is null, whitespace, or invalid.</exception>
    public static void ValidateEmail(string email, string parameterName)
    {
        if (!IsValidEmail(email))
        {
            throw new ArgumentException("Email must be valid and cannot be empty.", parameterName);
        }
    }

    /// <summary>
    /// Checks if the provided phone number is valid.
    /// </summary>
    /// <param name="phoneNumber">The phone number to validate.</param>
    /// <returns>true if the phone number is not null or whitespace; otherwise, false.</returns>
    public static bool IsValidPhoneNumber(string phoneNumber)
    {
        return !string.IsNullOrWhiteSpace(phoneNumber);
    }

    /// <summary>
    /// Validates the provided phone number and throws an exception if it is not valid.
    /// </summary>
    /// <param name="phoneNumber">The phone number to validate.</param>
    /// <param name="parameterName">The name of the parameter for error reporting.</param>
    /// <exception cref="ArgumentException">Thrown when the phone number is null or whitespace.</exception>
    public static void ValidatePhoneNumber(string phoneNumber, string parameterName)
    {
        if (!IsValidPhoneNumber(phoneNumber))
        {
            throw new ArgumentException("Phone number cannot be empty.", parameterName);
        }
    }

    /// <summary>
    /// Checks if the provided address is valid.
    /// </summary>
    /// <param name="address">The address to validate.</param>
    /// <returns>true if the address is not null or whitespace; otherwise, false.</returns>
    public static bool IsValidAddress(string address)
    {
        return !string.IsNullOrWhiteSpace(address);
    }

    /// <summary>
    /// Validates the provided address and throws an exception if it is not valid.
    /// </summary>
    /// <param name="address">The address to validate.</param>
    /// <param name="parameterName">The name of the parameter for error reporting.</param>
    /// <exception cref="ArgumentException">Thrown when the address is null or whitespace.</exception>
    public static void ValidateAddress(string address, string parameterName)
    {
        if (!IsValidAddress(address))
        {
            throw new ArgumentException("Address cannot be empty.", parameterName);
        }
    }

    /// <summary>
    /// Checks if the provided payment method is valid.
    /// </summary>
    /// <param name="preferredPaymentMethod">The payment method to validate.</param>
    /// <returns>true if the payment method is defined in the PaymentMethod enum; otherwise, false.</returns>
    public static bool IsValidPaymentMethod(PaymentMethod preferredPaymentMethod)
    {
        return Enum.IsDefined(typeof(PaymentMethod), preferredPaymentMethod);
    }

    /// <summary>
    /// Validates the provided payment method and throws an exception if it is not valid.
    /// </summary>
    /// <param name="preferredPaymentMethod">The payment method to validate.</param>
    /// <param name="parameterName">The name of the parameter for error reporting.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the payment method is not defined in the PaymentMethod
    /// enum.</exception>
    public static void ValidatePaymentMethod(PaymentMethod preferredPaymentMethod, string parameterName)
    {
        if (!IsValidPaymentMethod(preferredPaymentMethod))
        {
            throw new ArgumentOutOfRangeException(parameterName, "Invalid payment method specified.");
        }
    }
}
}

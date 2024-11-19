/// <copyright file="ValidationException.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file defines the <see cref="ValidationException"/> class, which is a custom exception used to represent
/// validation errors in the SmartStay application. This exception includes an error code and a localized error message
/// based on the validation failure.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>19/11/2024</date>

/// <summary>
/// The <c>SmartStay.Validation</c> namespace contains classes and methods for validating data and enforcing business
/// rules within the SmartStay application. These validations help ensure data integrity and compliance with application
/// requirements.
/// </summary>
namespace SmartStay.Validation
{
/// <summary>
/// Represents an exception that is thrown when a validation error occurs.
/// This exception contains an error code that corresponds to a specific validation failure.
/// The error message is retrieved from the resource files based on the error code and the current culture.
/// </summary>
public class ValidationException : Exception
{
    /// <summary>
    /// Gets the error code that corresponds to the specific validation failure.
    /// </summary>
    public ValidationErrorCode ErrorCode { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationException"/> class with the provided error code.
    /// The error message is automatically fetched based on the error code and the current culture.
    /// </summary>
    /// <param name="errorCode">The error code from the <see cref="ValidationErrorCode"/> enum that indicates the type
    /// of validation error.</param>
    public ValidationException(ValidationErrorCode errorCode) : base(ValidationErrorMessages.GetErrorMessage(errorCode))
    {
        ErrorCode = errorCode;
    }
}
}

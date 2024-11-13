/// <copyright file="ValidationException.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the <see cref="ValidationException"/> class, which represents a custom exception
/// used for handling validation errors within the SmartStay application. The exception includes a
/// specific error code and a descriptive message for easy identification of validation issues.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>09/11/2024</date>

/// <summary>
/// The <c>SmartStay.Validation</c> namespace contains classes and methods for validating data and enforcing business
/// rules within the SmartStay application. These validations help ensure data integrity and compliance with application
/// requirements.
/// </summary>
namespace SmartStay.Validation
{
/// <summary>
/// Initializes a new instance of the <see cref="ValidationException"/> class with a specified
/// validation error code. The error message is derived from <see cref="ValidationErrorMessages.GetErrorMessage"/>
/// based on the provided error code.
/// </summary>
/// <param name="errorCode">The <see cref="ValidationErrorCode"/> indicating the type of validation error.</param>
public class ValidationException
(ValidationErrorCode errorCode) : Exception(ValidationErrorMessages.GetErrorMessage(errorCode))
{
    /// <summary>
    /// Gets the specific <see cref="ValidationErrorCode"/> associated with this validation exception,
    /// providing context for the validation failure.
    /// </summary>
    public ValidationErrorCode ErrorCode { get; } = errorCode;
}
}

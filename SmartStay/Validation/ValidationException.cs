/// <copyright file="ValidationException.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the <see cref="ValidationException"/> class, which represents a custom exception
/// used for handling validation errors within the SmartStay application. The exception includes a
/// specific error code and a descriptive message for easy identification of validation issues.
/// </file>
/// <summary>
/// Defines the <see cref="ValidationException"/> class, a custom exception for validation errors,
/// encapsulating both an error code and a descriptive message based on that code.
/// </summary>
/// <author>Enrique Rodrigues</author>
/// <date>09/11/2024</date>
/// <remarks>
/// This custom exception is thrown whenever validation fails for a particular field, with the
/// associated <see cref="ValidationErrorCode"/> helping identify the nature of the issue.
/// </remarks>

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

/// <copyright file="ValidationErrorMessages.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the <see cref="ValidationErrorMessages"/> static class, which provides
/// human-readable error messages for validation error codes used in the SmartStay application.
/// </file>
/// <summary>
/// Provides a centralized repository of error messages associated with specific validation error codes
/// for use throughout the SmartStay application.
/// </summary>
/// <author>Enrique Rodrigues</author>
/// <date>09/11/2024</date>
/// <remarks>
/// The <see cref="ValidationErrorMessages"/> class simplifies error handling by linking each
/// <see cref="ValidationErrorCode"/> with a detailed, user-friendly error message. These messages
/// are returned to the user or logged during validation failures, ensuring that error reporting
/// is consistent and meaningful.
/// </remarks>

namespace SmartStay
{
/// <summary>
/// Provides error messages corresponding to each <see cref="ValidationErrorCode"/> value
/// used in client data validation.
/// </summary>
public static class ValidationErrorMessages
{
    /// <summary>
    /// A dictionary mapping each <see cref="ValidationErrorCode"/> to a descriptive error message.
    /// </summary>
    private static readonly Dictionary<ValidationErrorCode, string> ErrorMessages = new() {
        { ValidationErrorCode.InvalidName, "The name must be a non-empty string and not exceed 50 characters." },
        { ValidationErrorCode.InvalidEmail, "The email address is invalid or does not match the required format." },
        { ValidationErrorCode.InvalidPhoneNumber, "The phone number is invalid or empty." },
        { ValidationErrorCode.InvalidAddress, "The address is invalid or empty." },
        { ValidationErrorCode.InvalidPaymentMethod, "The selected payment method is not valid." },
        { ValidationErrorCode.InvalidAccommodationType, "The accommodation type is invalid or not recognized." },
        { ValidationErrorCode.InvalidId, "The provided ID is invalid or does not exist." },
        { ValidationErrorCode.InvalidDateRange,
          "The date range is invalid. Ensure the check-out date is later than the check-in date." },
        { ValidationErrorCode.InvalidDate, "The date provided is invalid. It must be a valid date in the future." },
        { ValidationErrorCode.InvalidTotalCost, "The total cost is invalid. It cannot be negative." },
        { ValidationErrorCode.InvalidPaymentValue,
          "The payment value is invalid. It cannot be negative or greater than the total cost." },
        { ValidationErrorCode.InvalidReservationStatus, "The reservation status is invalid or unrecognized." },
        { ValidationErrorCode.InvalidAccommodationName, "The accommodation name is invalid or empty." },
        { ValidationErrorCode.InvalidPrice, "The price is invalid. It must be a positive value." }
    };

    /// <summary>
    /// Retrieves the error message associated with a specified <see cref="ValidationErrorCode"/>.
    /// </summary>
    /// <param name="errorCode">The validation error code representing a specific validation failure.</param>
    /// <returns>
    /// A descriptive error message explaining the validation failure, or a default message
    /// if the error code is unrecognized.
    /// </returns>
    public static string GetErrorMessage(ValidationErrorCode errorCode)
    {
        return ErrorMessages.TryGetValue(errorCode, out var message) ? message : "Unknown validation error.";
    }
}
}

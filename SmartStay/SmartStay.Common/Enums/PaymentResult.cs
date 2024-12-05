/// <copyright file="PaymentResult.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="PaymentResult"/> enumeration used in the SmartStay
/// application, representing the possible outcomes of a payment attempt.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>01/12/2024</date>

/// <summary>
/// This namespace contains enumerations used within the SmartStay application.
/// </summary>
namespace SmartStay.Common.Enums
{
/// <summary>
/// Enumeration representing the possible outcomes of a payment attempt.
/// </summary>
public enum PaymentResult
{
    /// <summary>
    /// Indicates that the payment was successful.
    /// </summary>
    Success,

    /// <summary>
    /// Indicates that the payment amount provided was invalid (e.g., less than or equal to zero).
    /// </summary>
    InvalidAmount,

    /// <summary>
    /// Indicates that the reservation is already fully paid.
    /// </summary>
    AlreadyFullyPaid,

    /// <summary>
    /// Indicates that the payment amount exceeds the total cost of the reservation.
    /// </summary>
    AmountExceedsTotal,

    /// <summary>
    /// Indicates that the provided payment method is invalid.
    /// </summary>
    InvalidPaymentMethod,

    /// <summary>
    /// Indicates an unspecified error occurred during the payment process.
    /// </summary>
    Error
}
}

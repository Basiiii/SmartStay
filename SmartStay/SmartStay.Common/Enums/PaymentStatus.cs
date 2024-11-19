/// <copyright file="PaymentStatus.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="PaymentStatus"/> enumeration used in the SmartStay application
/// representing various payment status.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>07/10/2024</date>

/// <summary>
/// This namespace contains enumerations used within the SmartStay application.
/// </summary>
namespace SmartStay.Common.Enums
{
/// <summary>
/// Enumerator representing payment status.
/// </summary>
public enum PaymentStatus
{
    /// <summary>
    /// Payment has not been made yet.
    /// </summary>
    Unpaid,

    /// <summary>
    /// Payment has been initiated but not yet completed (e.g., pending in processing).
    /// </summary>
    Pending,

    /// <summary>
    /// Payment has been completed successfully.
    /// </summary>
    Completed,

    /// <summary>
    /// Payment was partially completed; more payments are expected.
    /// </summary>
    PartiallyPaid,

    /// <summary>
    /// Payment was rejected, usually by the payment processor.
    /// </summary>
    Rejected,

    /// <summary>
    /// Payment was refunded to the client.
    /// </summary>
    Refunded,

    /// <summary>
    /// Payment has been cancelled, typically by the client or system.
    /// </summary>
    Cancelled
}
}

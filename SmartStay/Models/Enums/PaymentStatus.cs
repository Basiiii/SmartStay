/// <copyright file="PaymentStatus.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the definition of the PaymentStatus enumeration used in the SmartStay application.
/// </file>
/// <summary>
/// The <see cref="PaymentStatus"/> enumeration defines the various payment status in the SmartStay application.
/// </summary>
/// <author>Enrique Rodrigues</author>
/// <date>07/10/2024</date>

namespace SmartStay.Models.Enums
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

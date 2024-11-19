/// <copyright file="PaymentMethod.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="PaymentMethod"/> enumeration used in the SmartStay application,
/// representing different payment methods available for bookings and transactions.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>07/10/2024</date>

/// <summary>
/// This namespace contains enumerations related to accommodation types used within the SmartStay application.
/// </summary>
namespace Core.Models.Enums
{
/// <summary>
/// Enumeration representing the possible payment methods available for transactions.
/// </summary>
public enum PaymentMethod
{
    /// <summary>
    /// No specific payment method selected; used as a default or placeholder value.
    /// </summary>
    None,

    /// <summary>
    /// Payment method through PayPal, allowing secure online payments.
    /// </summary>
    PayPal,

    /// <summary>
    /// Payment method using MultiBanco, a popular Portuguese banking payment system.
    /// </summary>
    MultiBanco,

    /// <summary>
    /// Payment method via bank transfer, where funds are transferred directly between bank accounts.
    /// </summary>
    BankTransfer
}
}

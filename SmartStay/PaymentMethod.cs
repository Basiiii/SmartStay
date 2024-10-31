/// <copyright file="PaymentMethod.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the definition of the PaymentMethod enumeration used in the SmartStay application.
/// </file>
/// <summary>
/// The <see cref="PaymentMethod"/> enumeration defines the various payment methods available
/// in the SmartStay application.
/// </summary>
/// <author>Enrique Rodrigues</author>
/// <date>07/10/2024</date>

namespace SmartStay
{
/// <summary>
/// Enumerator representing possible payment methods.
/// </summary>
public enum PaymentMethod
{
    None,        // No payment method
    PayPal,      // Payment through PayPal
    MultiBanco,  // Payment via MultiBanco
    BankTransfer // Bank transfer payment
}
}

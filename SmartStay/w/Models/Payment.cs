/// <copyright file="Payment.cs">
/// Copyright (c) 2024 All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="Payment"/> class, which represents a payment record
/// in the SmartStay application. The class includes details such as payment amount, date, method, and status,
/// along with validation logic to ensure data integrity.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>11/11/2024</date>
using System;
using System.Text.Json;
using System.Threading;
using Core.Models.Enums;
using Core.Validation;

/// <summary>
/// The <c>SmartStay.Models</c> namespace contains the primary data models used within the SmartStay application.
/// These models represent core entities and structures essential for managing application data.
/// </summary>
namespace Core.Models
{
/// <summary>
/// Represents a payment made in the SmartStay system, with details such as amount, date, method, and status.
/// </summary>
public class Payment
{
    static int _lastPaymentId = 0;                                                       // Last assigned payment ID
    static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true }; // JSON Serializer options

    readonly int _id;               // ID of the payment
    readonly int _reservationId;    // ID of the reservation being paid
    readonly decimal _amount;       // Amount of the payment
    readonly DateTime _date;        // Date the payment was made
    readonly PaymentMethod _method; // Payment Method used
    PaymentStatus _status;          // Status of the payment

    /// <summary>
    /// Initializes a new instance of the <see cref="Payment"/> class with specified details.
    /// </summary>
    /// <param name="amount">The amount for the payment.</param>
    /// <param name="paymentDate">The date when the payment was made.</param>
    /// <param name="paymentMethod">The method used for the payment.</param>
    /// <param name="paymentStatus">The status of the payment.</param>
    /// <exception cref="ValidationException">
    /// Thrown when the provided amount, payment method, or payment status is invalid.
    /// </exception>
    public Payment(int reservationId, decimal amount, DateTime paymentDate, PaymentMethod paymentMethod,
                   PaymentStatus paymentStatus)
    {
        if (!Validator.IsValidPaymentAmount(amount))
            throw new ValidationException(ValidationErrorCode.InvalidPaymentValue);
        if (!Validator.IsValidPaymentMethod(paymentMethod))
            throw new ValidationException(ValidationErrorCode.InvalidPaymentMethod);
        if (!Validator.IsValidPaymentStatus(paymentStatus))
            throw new ValidationException(ValidationErrorCode.InvalidPaymentStatus);

        _id = GeneratePaymentId();
        _reservationId = reservationId;
        _amount = amount;
        _date = paymentDate;
        _method = paymentMethod;
        _status = paymentStatus;
    }

    /// <summary>
    /// Public getter for the payment Id.
    /// </summary>
    public int Id => _id;

    /// <summary>
    /// Public getter for the reservation Id being paid.
    /// </summary>
    public int ReservationId => _reservationId;

    /// <summary>
    /// Public getter for the Amount.
    /// </summary>
    public decimal Amount => _amount;

    /// <summary>
    /// Gets the date the payment was made.
    /// </summary>
    public DateTime Date => _date;

    /// <summary>
    /// Gets the method used for the payment (e.g., Credit Card, Bank Transfer).
    /// </summary>
    public PaymentMethod Method => _method;

    /// <summary>
    /// Gets or sets the status of the payment.
    /// When setting, validates the new status using <see cref="Validator.ValidatePaymentStatus"/>.
    /// </summary>
    /// <exception cref="ValidationException">
    /// Thrown when the provided status is invalid.
    /// </exception>
    public PaymentStatus Status
    {

        get => _status;
        set => _status = Validator.ValidatePaymentStatus(value);
    }

    /// <summary>
    /// Generates a unique payment ID in a thread-safe manner using Interlocked.Increment.
    /// </summary>
    /// <returns>A unique payment ID.</returns>
    /// <exception cref="InvalidOperationException">Thrown when max limit of int is reached.</exception>
    private static int GeneratePaymentId()
    {
        // Check if the current value exceeds the max limit of int (2,147,483,647)
        if (_lastPaymentId >= int.MaxValue)
        {
            throw new InvalidOperationException("Client ID limit exceeded.");
        }

        return Interlocked.Increment(ref _lastPaymentId);
    }

    /// <summary>
    /// Overridden ToString method to provide payment information in a readable JSON format.
    /// </summary>
    /// <returns>A JSON string representation of the payment object.</returns>
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, _jsonOptions);
    }
}
}

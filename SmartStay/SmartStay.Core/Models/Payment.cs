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
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using ProtoBuf;
using SmartStay.Common.Enums;
using SmartStay.Validation;
using SmartStay.Validation.Validators;

/// <summary>
/// The <c>SmartStay.Core.Models</c> namespace contains the primary data models used within the SmartStay application.
/// These models represent core entities and structures essential for managing application data.
/// </summary>
namespace SmartStay.Core.Models
{
/// <summary>
/// Represents a payment made in the SmartStay system, with details such as amount, date, method, and status.
/// </summary>
[ProtoContract]
public class Payment
{
    /// <summary>
    /// The last assigned payment ID, used for tracking the most recent payment ID.
    /// </summary>
    static int _lastPaymentId = 0;

    /// <summary>
    /// JSON serializer options used for serializing the payment data.
    /// </summary>
    static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions() { WriteIndented = true };

    /// <summary>
    /// The unique ID of the payment. This ID is used to identify the payment.
    /// </summary>
    [ProtoMember(1)]
    readonly int _id; // ID of the payment

    /// <summary>
    /// The unique ID of the reservation for which the payment was made.
    /// </summary>
    [ProtoMember(2)]
    readonly int _reservationId; // ID of the reservation being paid

    /// <summary>
    /// The amount paid in this payment transaction.
    /// </summary>
    [ProtoMember(3)]
    readonly decimal _amount; // Amount of the payment

    /// <summary>
    /// The date the payment was made.
    /// </summary>
    [ProtoMember(4)]
    readonly DateTime _date; // Date the payment was made

    /// <summary>
    /// The payment method used for the transaction (e.g., PayPal, Bank Transfer).
    /// </summary>
    [ProtoMember(5)]
    readonly PaymentMethod _method; // Payment Method used

    /// <summary>
    /// The current status of the payment (e.g., Pending, Completed, Failed).
    /// </summary>
    [ProtoMember(6)]
    PaymentStatus _status; // Status of the payment

    /// <summary>
    /// Initializes a new instance of the <see cref="Payment"/> class.
    /// <para>This constructor is required for Protobuf-net serialization/deserialization.</para>
    /// <para>It should **not** be used directly in normal application code. Instead, use the constructor with
    /// parameters for creating instances of <see cref="Payment"/>.</para>
    /// </summary>
#pragma warning disable CS8618
    public Payment()
#pragma warning restore CS8618
    {
        // This constructor is intentionally empty and only needed for Protobuf-net deserialization.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Payment"/> class with specified details.
    /// </summary>
    /// <param name="amount">The amount for the payment.</param>
    /// <param name="paymentDate">The date when the payment was made.</param>
    /// <param name="paymentMethod">The method used for the payment.</param>
    /// <param name="paymentStatus">The status of the payment.</param>
    /// <exception cref="ValidationException">
    /// Thrown when the provided reservation id, amount, payment method, or payment status is invalid.
    /// </exception>
    public Payment(int reservationId, decimal amount, DateTime paymentDate, PaymentMethod paymentMethod,
                   PaymentStatus paymentStatus)
    {
        ReservationValidator.ValidateReservationId(reservationId);
        PaymentValidator.ValidatePayment(amount);
        PaymentValidator.ValidatePaymentMethod(paymentMethod);
        PaymentValidator.ValidatePaymentStatus(paymentStatus);

        _id = GeneratePaymentId();
        _reservationId = reservationId;
        _amount = amount;
        _date = paymentDate;
        _method = paymentMethod;
        _status = paymentStatus;
    }

    /// <summary>
    /// Public getter and setter for the last assigned ID.
    /// </summary>
    public static int LastAssignedId
    {
        get => _lastPaymentId;
        set => _lastPaymentId = value;
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
        set => _status = PaymentValidator.ValidatePaymentStatus(value);
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
            throw new InvalidOperationException("Payment ID limit exceeded.");
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

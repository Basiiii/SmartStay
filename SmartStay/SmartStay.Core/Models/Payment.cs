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
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
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
    /// JSON serializer options used for formatting and serializing client data in JSON.
    /// <para>
    /// - <see cref="WriteIndented"/> is enabled to improve readability with indented formatting.
    /// - <see cref="Encoder"/> is set to <see cref="JavaScriptEncoder.UnsafeRelaxedJsonEscaping"/> to allow unsafe
    /// characters
    ///   (such as `<`, `>`, and other special characters) to be included without escaping.
    /// - <see cref="Converters"/> contains a <see cref="JsonStringEnumConverter"/> to serialize enum values as their
    /// string names
    ///   instead of integer values.
    /// </para>
    /// </summary>
    static readonly JsonSerializerOptions _jsonOptions =
        new() { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                Converters = { new JsonStringEnumConverter() } };

    /// <summary>
    /// The unique ID of the payment. This ID is used to identify the payment.
    /// </summary>
    [ProtoMember(1)]
    readonly int _id; // ID of the payment

    /// <summary>
    /// The unique ID of the reservation for which the payment was made.
    /// </summary>
    [ProtoMember(2)]
    int _reservationId; // ID of the reservation being paid

    /// <summary>
    /// The amount paid in this payment transaction.
    /// </summary>
    [ProtoMember(3)]
    decimal _amount; // Amount of the payment

    /// <summary>
    /// The date the payment was made.
    /// </summary>
    [ProtoMember(4)]
    DateTime _date; // Date the payment was made

    /// <summary>
    /// The payment method used for the transaction (e.g., PayPal, Bank Transfer).
    /// </summary>
    [ProtoMember(5)]
    PaymentMethod _method; // Payment Method used

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
    /// Constructor to initialize a new <see cref="Payment"/> with all details, including a manually specified ID,
    /// reservation ID, amount, date, payment method, and payment status. <b>This constructor should be avoided in
    /// normal cases</b> as it allows manual assignment of the payment ID, which can lead to conflicts and issues with
    /// ID uniqueness. The system is designed to automatically handle unique ID assignment, and other constructors
    /// should be used for creating payment objects to ensure proper handling of IDs. <br/>This constructor is marked
    /// with <see cref="[JsonConstructor]"/> so it will be used for JSON deserialization purposes, but it should not be
    /// used when creating new payment objects manually.
    /// </summary>
    /// <param name="id">The manually specified ID of the payment. This should not be used under normal circumstances as
    /// the system handles ID assignment automatically.</param>
    /// <param name="reservationId">The ID of the reservation associated with the payment.</param>
    /// <param name="amount">The amount paid for the reservation.</param>
    /// <param name="date">The date when the payment was made.</param>
    /// <param name="method">The payment method used for the transaction.</param>
    /// <param name="status">The status of the payment (e.g., Pending, Completed, Failed).</param>
    [JsonConstructor]
    public Payment(int id, int reservationId, decimal amount, DateTime date, PaymentMethod method, PaymentStatus status)
    {
        _id = id;
        UpdateLastPaymentId(id);
        _reservationId = reservationId;
        _amount = amount;
        _date = date;
        _method = method;
        _status = status;
    }

    /// <summary>
    /// Public getter and setter for the last assigned ID.
    /// </summary>
    public static int LastAssignedId
    {
        get => _lastPaymentId;
        set {
            if (_lastPaymentId < value)
                _lastPaymentId = value;
        }
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
    /// Gets the method used for the payment (e.g., PayPal, Bank Transfer).
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
    /// Method to ensure _lastPaymentId is up-to-date after deserialization or manual assignment.
    /// </summary>
    /// <param name="id">Manually given ID.</param>
    private static void UpdateLastPaymentId(int id)
    {
        if (id > _lastPaymentId)
        {
            _lastPaymentId = id; // Update the last assigned owner ID if the new ID is larger
        }
    }

    /// <summary>
    /// Creates a deep copy of the current <see cref="Payment"/> instance.
    /// </summary>
    /// <returns>A new <see cref="Payment"/> instance with identical data to the current instance.</returns>
    public Payment Clone()
    {
        // Create a new instance of Payment and deep copy the fields
        return new Payment(_id,            // Immutable, directly copy
                           _reservationId, // Value type, directly copy
                           _amount,        // Value type, directly copy
                           _date,          // Value type (DateTime), directly copy
                           _method,        // Enum type, directly copy
                           _status         // Enum type, directly copy
        );
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

﻿/// <copyright file="Reservation.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the <see cref="Reservation"/> class, which stores information about client reservations,
/// including accommodation details, check-in and check-out dates, and payment status. This class manages
/// reservation data effectively while ensuring data integrity through input validation.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>09/11/2024</date>
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
/// Defines the <see cref="Reservation"/> class, which encapsulates reservation details such as client ID,
/// accommodation type, dates, and payment information. This class ensures data consistency by validating
/// input parameters upon creation or when modifying specific properties.
/// </summary>
[ProtoContract]
public class Reservation
{
    /// <summary>
    /// The last assigned reservation ID, used for tracking the most recent reservation ID.
    /// </summary>
    static int _lastReservationId = 0;

    /// <summary>
    /// JSON serializer options used for serializing the reservation data.
    /// </summary>
    static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions() { WriteIndented = true };

    /// <summary>
    /// The unique ID of the reservation. This ID is used to identify the reservation.
    /// </summary>
    [ProtoMember(1)]
    readonly int _reservationId; // ID of the reservation

    /// <summary>
    /// The unique ID of the client who made the reservation.
    /// </summary>
    [ProtoMember(2)]
    readonly int _clientId; // ID of the client making the reservation

    /// <summary>
    /// The unique ID of the accommodation being reserved.
    /// </summary>
    [ProtoMember(3)]
    readonly int _accommodationId; // ID of the accommodation

    /// <summary>
    /// The unique ID of the room being reserved.
    /// </summary>
    [ProtoMember(4)]
    readonly int _roomId; // ID of the room

    /// <summary>
    /// The type of accommodation for the reservation (e.g., Room, Suite, etc.).
    /// </summary>
    [ProtoMember(5)]
    AccommodationType _accommodationType; // Type of accommodation (e.g., Room, Suite, etc.)

    /// <summary>
    /// The check-in date for the reservation.
    /// </summary>
    [ProtoMember(6)]
    DateTime _checkInDate; // Check-in date for the reservation

    /// <summary>
    /// The check-out date for the reservation.
    /// </summary>
    [ProtoMember(7)]
    DateTime _checkOutDate; // Check-out date for the reservation

    /// <summary>
    /// The current status of the reservation (e.g., Pending, Confirmed, Canceled, etc.).
    /// The default status is set to <see cref="ReservationStatus.Pending"/>.
    /// </summary>
    [ProtoMember(8)]
    ReservationStatus _status = ReservationStatus.Pending; // Current reservation status

    /// <summary>
    /// The total cost of the reservation.
    /// </summary>
    [ProtoMember(9)]
    decimal _totalCost; // Total cost of the reservation

    /// <summary>
    /// The amount already paid towards the reservation. Default value is 0.
    /// </summary>
    [ProtoMember(10)]
    decimal _amountPaid = 0; // Amount paid towards the reservation

    /// <summary>
    /// A list of payments made for the reservation.
    /// </summary>
    [ProtoMember(11)]
    readonly List<Payment> _payments = new(); // List of payments made for the reservation

    /// <summary>
    /// Initializes a new instance of the <see cref="Reservation"/> class.
    /// <para>This constructor is required for Protobuf-net serialization/deserialization.</para>
    /// <para>It should **not** be used directly in normal application code. Instead, use the constructor with
    /// parameters for creating instances of <see cref="Reservation"/>.</para>
    /// </summary>
#pragma warning disable CS8618
    public Reservation()
#pragma warning restore CS8618
    {
        // This constructor is intentionally empty and only needed for Protobuf-net deserialization.
    }

    /// <summary>
    /// Constructor to initialize a new reservation with essential details.
    /// Validates the input parameters.
    /// </summary>
    /// <param name="clientId">The ID of the client.</param>
    /// <param name="accommodationId">The ID of the accommodation.</param>
    /// <param name="roomId">The ID of the room.</param>
    /// <param name="accommodationType">The type of accommodation.</param>
    /// <param name="checkInDate">The check-in date.</param>
    /// <param name="checkOutDate">The check-out date.</param>
    /// <param name="totalCost">The total cost of the reservation.</param>
    /// <exception cref="ValidationException">Thrown when any of the input parameters are invalid.
    ///     Each validation has a specific error code:
    ///     <br/><b>InvalidId:</b> if the client, accommodation or room ID is invalid.
    ///     <br/><b>InvalidTotalCost:</b> if total cost is invalid.
    ///     <br/><b>InvalidDateRange:</b> if the check-in date is later than the check-out date.
    /// </exception>
    public Reservation(int clientId, int accommodationId, int roomId, AccommodationType accommodationType,
                       DateTime checkInDate, DateTime checkOutDate, decimal totalCost)
    {
        ClientValidator.ValidateClientId(clientId);
        AccommodationValidator.ValidateAccommodationId(accommodationId);
        RoomValidator.ValidateRoomId(roomId);
        PaymentValidator.ValidateTotalCost(totalCost);
        if (!DateValidator.IsValidDateRange(checkInDate, checkOutDate))
            throw new ValidationException(ValidationErrorCode.InvalidDateRange);

        _reservationId = GenerateReservationId();
        _clientId = clientId;
        _accommodationId = accommodationId;
        _roomId = roomId;
        _accommodationType = accommodationType;
        _checkInDate = checkInDate;
        _checkOutDate = checkOutDate;
        _totalCost = totalCost;
    }

    /// <summary>
    /// Public getter and setter for the last assigned ID.
    /// </summary>
    public static int LastAssignedId
    {
        get => _lastReservationId;
        set => _lastReservationId = value;
    }

    /// <summary>
    /// Gets the Reservation ID.
    /// </summary>
    public int Id => _reservationId;

    /// <summary>
    /// Gets the Client ID associated with the reservation.
    /// </summary>
    public int ClientId => _clientId;

    /// <summary>
    /// Gets the Accommodation ID associated with the reservation.
    /// </summary>
    public int AccommodationId => _accommodationId;

    /// <summary>
    /// Gets the room ID associated with the reservation.
    /// </summary>
    public int RoomId => _roomId;

    /// <summary>
    /// Gets or sets the Accommodation Type.
    /// </summary>
    public AccommodationType AccommodationType
    {
        get => _accommodationType;
        set => _accommodationType = AccommodationValidator.ValidateAccommodationType(value);
    }

    /// <summary>
    /// Gets or sets the Check-In Date.
    /// </summary>
    public DateTime CheckInDate
    {
        get => _checkInDate;
        set => _checkInDate = DateValidator.ValidateCheckInDate(value);
    }

    /// <summary>
    /// Gets or sets the Check-Out Date.
    /// </summary>
    public DateTime CheckOutDate
    {
        get => _checkOutDate;
        set => _checkOutDate = DateValidator.ValidateCheckOutDate(value, _checkInDate);
    }

    /// <summary>
    /// Gets or sets the Total Cost.
    /// </summary>
    public decimal TotalCost
    {
        get => _totalCost;
        set => _totalCost = PaymentValidator.ValidateTotalCost(value);
    }

    /// <summary>
    /// Gets or sets the Reservation Status.
    /// </summary>
    public ReservationStatus Status
    {
        get => _status;
        set => _status = ReservationValidator.ValidateReservationStatus(value);
    }

    /// <summary>
    /// Gets or sets the Amount Paid towards the reservation.
    /// </summary>
    public decimal AmountPaid
    {
        get => _amountPaid;
        set => _amountPaid = PaymentValidator.ValidatePayment(value);
    }

    /// <summary>
    /// Gets the list of payments made towards the reservation.
    /// </summary>
    public IReadOnlyList<Payment> Payments => _payments.AsReadOnly();

    /// <summary>
    /// Marks the reservation as checked in and updates the status to CheckedIn.
    /// </summary>
    /// <returns>
    /// True if the reservation status was successfully updated to CheckedIn;
    /// false if the current status is not Pending.
    /// </returns>
    /// <remarks>
    /// This method will not modify the reservation status if it is not in Pending state.
    /// Ensure the status is appropriately validated before calling this method if strict workflows are required.
    /// </remarks>
    public bool CheckIn()
    {
        if (_status != ReservationStatus.Pending)
        {
            return false;
        }
        _status = ReservationStatus.CheckedIn;
        return true;
    }

    /// <summary>
    /// Marks the reservation as checked out and updates the status to CheckedOut.
    /// </summary>
    /// <returns>
    /// True if the reservation status was successfully updated to CheckedOut;
    /// false if the current status is not CheckedIn.
    /// </returns>
    /// <remarks>
    /// This method will not modify the reservation status if it is not in CheckedIn state.
    /// Ensure the status is appropriately validated before calling this method if strict workflows are required.
    /// </remarks>
    public bool CheckOut()
    {
        if (_status != ReservationStatus.CheckedIn)
        {
            return false;
        }
        _status = ReservationStatus.CheckedOut;
        return true;
    }

    /// <summary>
    /// Makes a payment towards the reservation and adds a new Payment object to the payment list.
    /// </summary>
    /// <param name="paymentAmount">The amount of the payment. Must be greater than zero.</param>
    /// <param name="paymentMethod">The payment method used for the payment.</param>
    /// <returns>
    /// A <see cref="PaymentResult"/> indicating the result of the payment attempt.
    /// </returns>
    /// <remarks>
    /// This method validates the payment amount and ensures the reservation is not overpaid or already fully paid.
    /// The payment method is also validated using the <see cref="PaymentValidator.ValidatePaymentMethod"/> method.
    /// </remarks>
    public PaymentResult MakePayment(decimal paymentAmount, PaymentMethod paymentMethod)
    {
        if (paymentAmount <= 0)
            return PaymentResult.InvalidAmount;
        if (IsFullyPaid())
            return PaymentResult.AlreadyFullyPaid;
        if (paymentAmount > _totalCost - _amountPaid)
            return PaymentResult.AmountExceedsTotal;
        if (!PaymentValidator.IsValidPaymentMethod(paymentMethod))
            return PaymentResult.InvalidPaymentMethod;

        // Create a new Payment instance and add it to the list
        var payment = new Payment(_reservationId, paymentAmount, DateTime.Now, paymentMethod, PaymentStatus.Completed);
        _payments.Add(payment);

        // Update the amount paid
        _amountPaid += paymentAmount;

        return PaymentResult.Success;
    }

    /// <summary>
    /// Checks if the reservation is fully paid.
    /// </summary>
    /// <returns>True if the amount paid equals the total cost, otherwise false.</returns>
    public bool IsFullyPaid()
    {
        return _amountPaid >= _totalCost;
    }

    /// <summary>
    /// Generates a unique reservation ID in a thread-safe manner using Interlocked.Increment.
    /// </summary>
    /// <returns>A unique reservation ID.</returns>
    /// <exception cref="InvalidOperationException">Thrown when max limit of int is reached.</exception>
    private static int GenerateReservationId()
    {
        if (_lastReservationId >= int.MaxValue)
        {
            throw new InvalidOperationException("Reservation ID limit exceeded.");
        }
        return Interlocked.Increment(ref _lastReservationId);
    }

    /// <summary>
    /// Overridden ToString method to provide reservation information in a readable JSON format.
    /// </summary>
    /// <returns>A JSON string representation of the reservation object.</returns>
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, _jsonOptions);
    }
}
}

using System.Net;
using System.Text.Json;

/// <copyright file="Reservation.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the definition of the Reservation class used in the SmartStay application.
/// </file>
/// <summary>
/// Represents the <see cref="Reservation"/> class, which stores information about client reservations,
/// including accommodation details, check-in and check-out dates, and payment status. This class manages
/// reservation data effectively while ensuring data integrity through input validation.
/// </summary>
/// <author>Enrique Rodrigues</author>
/// <date>09/11/2024</date>
namespace SmartStay
{
/// <summary>
/// Defines the <see cref="Reservation"/> class, which encapsulates reservation details such as client ID,
/// accommodation type, dates, and payment information. This class ensures data consistency by validating
/// input parameters upon creation or when modifying specific properties.
/// </summary>
internal class Reservation
{
    static int _lastReservationId = 0;                                                   // Last assigned reservation ID
    static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true }; // JSON Serializer options

    readonly int _reservationId;                           // ID of the reservation
    readonly int _clientId;                                // ID of the client making the reservation
    readonly int _accommodationId;                         // ID of the accommodation
    AccommodationType _accommodationType;                  // Type of accommodation (e.g., Room, Suite, etc.)
    DateTime _checkInDate;                                 // Check-in date for the reservation
    DateTime _checkOutDate;                                // Check-out date for the reservation
    ReservationStatus _status = ReservationStatus.Pending; // Current reservation status
    decimal _totalCost;                                    // Total cost of the reservation
    int _amountPaid = 0;                                   // Amount paid towards the reservation
    PaymentMethod _paymentMethodUsed = PaymentMethod.None; // Payment method used

    /// <summary>
    /// Constructor to initialize a new reservation with essential details.
    /// Validates the input parameters.
    /// </summary>
    /// <param name="clientId">The ID of the client.</param>
    /// <param name="accommodationId">The ID of the accommodation.</param>
    /// <param name="accommodationType">The type of accommodation.</param>
    /// <param name="checkInDate">The check-in date.</param>
    /// <param name="checkOutDate">The check-out date.</param>
    /// <param name="totalCost">The total cost of the reservation.</param>
    /// <exception cref="ValidationException">Thrown when any of the input parameters are invalid.</exception>
    public Reservation(int clientId, int accommodationId, AccommodationType accommodationType, DateTime checkInDate,
                       DateTime checkOutDate, decimal totalCost)
    {
        if (clientId <= 0)
            throw new ValidationException(ValidationErrorCode.InvalidId);
        if (accommodationId <= 0)
            throw new ValidationException(ValidationErrorCode.InvalidId);
        if (!Validator.IsValidDateRange(checkInDate, checkOutDate))
            throw new ValidationException(ValidationErrorCode.InvalidDateRange);
        if (totalCost < 0)
            throw new ValidationException(ValidationErrorCode.InvalidTotalCost);

        _reservationId = GenerateReservationId();
        _clientId = clientId;
        _accommodationId = accommodationId;
        _accommodationType = accommodationType;
        _checkInDate = checkInDate;
        _checkOutDate = checkOutDate;
        _totalCost = totalCost;
    }

    /// <summary>
    /// Constructor to initialize a new reservation with essential details and payment information.
    /// Validates the input parameters.
    /// </summary>
    /// <param name="clientId">The ID of the client.</param>
    /// <param name="accommodationId">The ID of the accommodation.</param>
    /// <param name="accommodationType">The type of accommodation.</param>
    /// <param name="checkInDate">The check-in date.</param>
    /// <param name="checkOutDate">The check-out date.</param>
    /// <param name="totalCost">The total cost of the reservation.</param>
    /// <param name="amountPaid">The amount paid towards the reservation.</param>
    /// <param name="paymentMethod">The payment method used.</param>
    /// <exception cref="ValidationException">Thrown when any of the input parameters are invalid.</exception>
    public Reservation(int clientId, int accommodationId, AccommodationType accommodationType, DateTime checkInDate,
                       DateTime checkOutDate, int totalCost, int amountPaid, PaymentMethod paymentMethod)
        : this(clientId, accommodationId, accommodationType, checkInDate, checkOutDate, totalCost)
    {
        if (amountPaid < 0 || amountPaid > totalCost)
            throw new ValidationException(ValidationErrorCode.InvalidPaymentValue);
        if (!Validator.IsValidPaymentMethod(paymentMethod))
            throw new ValidationException(ValidationErrorCode.InvalidPaymentMethod);

        _amountPaid = amountPaid;
        _paymentMethodUsed = paymentMethod;
    }

    /// <summary>
    /// Gets the Reservation ID.
    /// </summary>
    public int ReservationId => _reservationId;

    /// <summary>
    /// Gets the Client ID associated with the reservation.
    /// </summary>
    public int ClientId => _clientId;

    /// <summary>
    /// Gets the Accommodation ID associated with the reservation.
    /// </summary>
    public int AccommodationId => _accommodationId;

    /// <summary>
    /// Gets or sets the Accommodation Type.
    /// </summary>
    public AccommodationType AccommodationType
    {
        get => _accommodationType;
        set => _accommodationType = Validator.ValidateAccommodationType(value);
    }

    /// <summary>
    /// Gets or sets the Check-In Date.
    /// </summary>
    public DateTime CheckInDate
    {
        get => _checkInDate;
        set => _checkInDate = Validator.ValidateCheckInDate(value);
    }

    /// <summary>
    /// Gets or sets the Check-Out Date.
    /// </summary>
    public DateTime CheckOutDate
    {
        get => _checkOutDate;
        set => _checkOutDate = Validator.ValidateCheckOutDate(value, _checkInDate);
    }

    /// <summary>
    /// Gets or sets the Total Cost.
    /// </summary>
    public decimal TotalCost
    {
        get => _totalCost;
        set => _totalCost = Validator.ValidateTotalCost(value);
    }

    /// <summary>
    /// Gets or sets the Reservation Status.
    /// </summary>
    public ReservationStatus Status
    {
        get => _status;
        set => _status = Validator.ValidateReservationStatus(value);
    }

    /// <summary>
    /// Gets or sets the Amount Paid towards the reservation.
    /// </summary>
    public int AmountPaid
    {
        get => _amountPaid;
        set => _amountPaid = Validator.ValidatePayment(value);
    }

    /// <summary>
    /// Gets or sets the Payment Method used in the reservation.
    /// </summary>
    public PaymentMethod PaymentMethodUsed
    {
        get => _paymentMethodUsed;
        set => _paymentMethodUsed = Validator.ValidatePaymentMethod(value);
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
    /// Marks the reservation as checked in and updates the status to CheckedIn.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the reservation status is not Pending.</exception>
    public void CheckIn()
    {
        if (_status != ReservationStatus.Pending)
        {
            throw new InvalidOperationException("Reservation must be in Pending status to check in.");
        }
        _status = ReservationStatus.CheckedIn;
    }

    /// <summary>
    /// Marks the reservation as checked out and updates the status to CheckedOut.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the reservation status is not CheckedIn.</exception>
    public void CheckOut()
    {
        if (_status != ReservationStatus.CheckedIn)
        {
            throw new InvalidOperationException("Reservation must be in CheckedIn status to check out.");
        }
        _status = ReservationStatus.CheckedOut;
    }

    /// <summary>
    /// Makes a payment towards the reservation and updates the amount paid.
    /// </summary>
    /// <param name="paymentAmount">The amount to be paid.</param>
    /// <param name="paymentMethod">The payment method used.</param>
    /// <exception cref="InvalidOperationException">Thrown if the payment amount is less than or equal to zero, if
    /// the reservation is already fully paid, or if the payment amount is more than total required.</exception>
    /// <exception cref="ValidationException">Thrown if the payment method is invalid.</exception>
    public void MakePayment(int paymentAmount, PaymentMethod paymentMethod)
    {
        if (paymentAmount <= 0)
        {
            throw new InvalidOperationException("Payment amount must be greater than zero.");
        }

        if (IsFullyPaid())
        {
            throw new InvalidOperationException("Reservation is already fully paid.");
        }

        if (paymentAmount > (_totalCost - _amountPaid))
        {
            throw new InvalidOperationException("Payment is more than total required.");
        }

        if (!Validator.IsValidPaymentMethod(paymentMethod))
        {
            throw new ValidationException(ValidationErrorCode.InvalidPaymentMethod);
        }

        _amountPaid += paymentAmount;
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
    /// Overridden ToString method to provide reservation information in a readable JSON format.
    /// </summary>
    /// <returns>A JSON string representation of the reservation object.</returns>
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, _jsonOptions);
    }
}
}

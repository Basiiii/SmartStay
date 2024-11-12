/// <copyright file="Validator.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the definition of validation methods used in the SmartStay application.
/// These methods validate various input parameters such as IDs, names, emails, phone numbers,
/// addresses, and payment methods to ensure data integrity.
/// </file>
/// <summary>
/// The <see cref="Validator"/> class provides static methods for validating input data
/// related to the SmartStay application. It includes methods for checking the validity of
/// names, emails, phone numbers, addresses, and payment methods. These validation
/// functions help maintain data integrity and ensure that input values conform to expected
/// formats and constraints.
/// </summary>
/// <author>Enrique Rodrigues</author>
/// <date>07/10/2024</date>
using System.Text.RegularExpressions;
using SmartStay.Models.Enums;

namespace SmartStay.Validation
{
/// <summary>
/// Provides a set of static methods for validating input data in the SmartStay application,
/// including names, emails, phone numbers, addresses, and payment methods.
/// </summary>
public static class Validator
{
    /// <summary>
    /// Regular expression pattern for validating email addresses.
    /// </summary>
    private static readonly string EmailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

    /// <summary>
    /// Validates a name, throwing an exception if invalid.
    /// </summary>
    /// <param name="name">The name to validate.</param>
    /// <returns>The validated name if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the name is invalid.</exception>
    public static string ValidateName(string name)
    {
        if (!IsValidName(name))
        {
            throw new ValidationException(ValidationErrorCode.InvalidName);
        }
        return name;
    }

    /// <summary>
    /// Validates an accommodation name, throwing an exception if invalid.
    /// </summary>
    /// <param name="name">The accommodation name to validate.</param>
    /// <returns>The validated name if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the name is invalid.</exception>
    public static string ValidateAccommodationName(string name)
    {
        if (!IsValidName(name))
        {
            throw new ValidationException(ValidationErrorCode.InvalidAccommodationName);
        }
        return name;
    }

    /// <summary>
    /// Validates an email address, throwing an exception if invalid.
    /// </summary>
    /// <param name="email">The email address to validate.</param>
    /// <returns>The validated email address if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the email address is invalid.</exception>
    public static string ValidateEmail(string email)
    {
        if (!IsValidEmail(email))
        {
            throw new ValidationException(ValidationErrorCode.InvalidEmail);
        }
        return email;
    }

    /// <summary>
    /// Validates a phone number, throwing an exception if invalid.
    /// </summary>
    /// <param name="phoneNumber">The phone number to validate.</param>
    /// <returns>The validated phone number if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the phone number is invalid.</exception>
    public static string ValidatePhoneNumber(string phoneNumber)
    {
        if (!IsValidPhoneNumber(phoneNumber))
        {
            throw new ValidationException(ValidationErrorCode.InvalidPhoneNumber);
        }
        return phoneNumber;
    }

    /// <summary>
    /// Validates an address, throwing an exception if invalid.
    /// </summary>
    /// <param name="address">The address to validate.</param>
    /// <returns>The validated address if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the address is invalid.</exception>
    public static string ValidateAddress(string address)
    {
        if (!IsValidAddress(address))
        {
            throw new ValidationException(ValidationErrorCode.InvalidAddress);
        }
        return address;
    }

    /// <summary>
    /// Validates a price, throwing an exception if invalid.
    /// </summary>
    /// <param name="price">The price to validate.</param>
    /// <returns>The validated price if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the price is invalid.</exception>
    public static decimal ValidatePrice(decimal price)
    {
        if (!IsValidPrice(price))
        {
            throw new ValidationException(ValidationErrorCode.InvalidPrice);
        }
        return price;
    }

    /// <summary>
    /// Validates a payment amount, throwing an exception if invalid.
    /// </summary>
    /// <param name="amount">The payment amount to validate.</param>
    /// <returns>The validated payment amount if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the payment amount is invalid.</exception>
    public static decimal ValidatePaymentAmount(decimal amount)
    {
        if (!IsValidPaymentAmount(amount))
        {
            throw new ValidationException(ValidationErrorCode.InvalidPaymentValue);
        }
        return amount;
    }

    /// <summary>
    /// Validates a payment status, throwing an exception if invalid.
    /// </summary>
    /// <param name="status">The payment status to validate.</param>
    /// <returns>The validated payment status if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the payment status is invalid.</exception>
    public static PaymentStatus ValidatePaymentStatus(PaymentStatus status)
    {
        if (!IsValidPaymentStatus(status))
        {
            throw new ValidationException(ValidationErrorCode.InvalidPaymentStatus);
        }
        return status;
    }

    /// <summary>
    /// Validates a payment method, throwing an exception if invalid.
    /// </summary>
    /// <param name="paymentMethod">The payment method to validate.</param>
    /// <returns>The validated payment method if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the payment method is invalid.</exception>
    public static PaymentMethod ValidatePaymentMethod(PaymentMethod paymentMethod)
    {
        if (!IsValidPaymentMethod(paymentMethod))
        {
            throw new ValidationException(ValidationErrorCode.InvalidPaymentMethod);
        }
        return paymentMethod;
    }

    /// <summary>
    /// Validates the accommodation type, throwing an exception if invalid.
    /// </summary>
    /// <param name="accommodationType">The accommodation type to validate.</param>
    /// <returns>The validated accommodation type if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the accommodation type is invalid.</exception>
    public static AccommodationType ValidateAccommodationType(AccommodationType accommodationType)
    {
        if (!IsValidAccommodationType(accommodationType))
        {
            throw new ValidationException(ValidationErrorCode.InvalidAccommodationType);
        }
        return accommodationType;
    }

    /// <summary>
    /// Validates the check-in date, throwing an exception if invalid.
    /// </summary>
    /// <param name="checkInDate">The check-in date to validate.</param>
    /// <returns>The validated check-in date if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the check-in date is invalid.</exception>
    public static DateTime ValidateCheckInDate(DateTime checkInDate)
    {
        if (!IsValidFutureDate(checkInDate))
        {
            throw new ValidationException(ValidationErrorCode.InvalidDate);
        }
        return checkInDate;
    }

    /// <summary>
    /// Validates the check-out date, throwing an exception if invalid.
    /// </summary>
    /// <param name="checkOutDate">The check-out date to validate.</param>
    /// <param name="checkInDate">The check-in date to compare.</param>
    /// <returns>The validated check-out date if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the check-out date is invalid.</exception>
    public static DateTime ValidateCheckOutDate(DateTime checkOutDate, DateTime checkInDate)
    {
        if (!IsValidDateRange(checkInDate, checkOutDate))
        {
            throw new ValidationException(ValidationErrorCode.InvalidDateRange);
        }
        return checkOutDate;
    }

    /// <summary>
    /// Validates the total cost, throwing an exception if invalid.
    /// </summary>
    /// <param name="totalCost">The total cost to validate.</param>
    /// <returns>The validated total cost if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the total cost is invalid.</exception>
    public static decimal ValidateTotalCost(decimal totalCost)
    {
        if (totalCost < 0)
        {
            throw new ValidationException(ValidationErrorCode.InvalidTotalCost);
        }
        return totalCost;
    }

    /// <summary>
    /// Validates a new payment value, throwing an exception if invalid.
    /// </summary>
    /// <param name="paymentValue">The payment value to validate.</param>
    /// <returns>The validated payment value if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the payment value is invalid.</exception>
    public static decimal ValidatePayment(decimal paymentValue)
    {
        if (paymentValue < 0)
        {
            throw new ValidationException(ValidationErrorCode.InvalidPaymentValue);
        }
        return paymentValue;
    }

    /// <summary>
    /// Validates the reservation status, throwing an exception if invalid.
    /// </summary>
    /// <param name="status">The reservation status to validate.</param>
    /// <returns>The validated reservation status if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the reservation status is invalid.</exception>
    public static ReservationStatus ValidateReservationStatus(ReservationStatus status)
    {
        if (!IsValidReservationStatus(status))
        {
            throw new ValidationException(ValidationErrorCode.InvalidReservationStatus);
        }
        return status;
    }

    /// <summary>
    /// Checks if a name is valid.
    /// </summary>
    /// <param name="name">The name to check.</param>
    /// <returns><c>true</c> if the name is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidName(string name) => !string.IsNullOrWhiteSpace(name) && name.Length <= 50;

    /// <summary>
    /// Checks if an accommodation name is valid.
    /// </summary>
    /// <param name="name">The name to check.</param>
    /// <returns><c>true</c> if the name is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidAccommodationName(string name) => !string.IsNullOrWhiteSpace(name) && name.Length <= 100;

    /// <summary>
    /// Checks if an email address is valid based on the defined <see cref="EmailPattern"/>.
    /// </summary>
    /// <param name="email">The email address to check.</param>
    /// <returns><c>true</c> if the email address is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidEmail(string email) => !string.IsNullOrWhiteSpace(email) && Regex.IsMatch(email,
                                                                                                        EmailPattern);

    /// <summary>
    /// Checks if a phone number is valid.
    /// </summary>
    /// <param name="phoneNumber">The phone number to check.</param>
    /// <returns><c>true</c> if the phone number is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidPhoneNumber(string phoneNumber) => !string.IsNullOrWhiteSpace(phoneNumber);

    /// <summary>
    /// Checks if a price is valid.
    /// </summary>
    /// <param name="price">The price to check.</param>
    /// <returns><c>true</c> if the price is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidPrice(decimal price) => price > 0;

    /// <summary>
    /// Checks if a payment amount is valid.
    /// </summary>
    /// <param name="amount">The payment amount to check.</param>
    /// <returns><c>true</c> if the payment amount is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidPaymentAmount(decimal amount) => amount > 0;

    /// <summary>
    /// Checks if an address is valid.
    /// </summary>
    /// <param name="address">The address to check.</param>
    /// <returns><c>true</c> if the address is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidAddress(string address) => !string.IsNullOrWhiteSpace(address);

    /// <summary>
    /// Checks if a payment method is valid by confirming it is a defined enum value.
    /// </summary>
    /// <param name="paymentMethod">The payment method to check.</param>
    /// <returns><c>true</c> if the payment method is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidPaymentMethod(PaymentMethod paymentMethod) => Enum.IsDefined(typeof(PaymentMethod),
                                                                                           paymentMethod);

    /// <summary>
    /// Checks if a payment status is valid by confirming it is a defined enum value.
    /// </summary>
    /// <param name="paymentStatus">The payment status to check.</param>
    /// <returns><c>true</c> if the payment status is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidPaymentStatus(PaymentStatus paymentStatus) => Enum.IsDefined(typeof(PaymentStatus),
                                                                                           paymentStatus);

    /// <summary>
    /// Checks if the accommodation type is valid by verifying it is defined in the <see cref="AccommodationType"/>
    /// enum.
    /// </summary>
    /// <param name="accommodationType">The accommodation type to check.</param>
    /// <returns><c>true</c> if the accommodation type is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidAccommodationType(AccommodationType accommodationType)
    {
        return Enum.IsDefined(typeof(AccommodationType), accommodationType);
    }

    /// <summary>
    /// Validates that the check-in date is before the check-out date.
    /// </summary>
    /// <param name="checkInDate">The check-in date.</param>
    /// <param name="checkOutDate">The check-out date.</param>
    /// <returns><c>true</c> if the check-in date is earlier than the check-out date; otherwise, <c>false</c>.</returns>
    public static bool IsValidDateRange(DateTime checkInDate, DateTime checkOutDate)
    {
        return checkInDate < checkOutDate;
    }

    /// <summary>
    /// Checks if the date is a valid future or present date.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns><c>true</c> if the date is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidFutureDate(DateTime date)
    {
        return date >= DateTime.Today;
    }

    /// <summary>
    /// Checks if the reservation status is valid by verifying it is defined in the <see cref="ReservationStatus"/>
    /// enum.
    /// </summary>
    /// <param name="status">The reservation status to check.</param>
    /// <returns><c>true</c> if the reservation status is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidReservationStatus(ReservationStatus status)
    {
        return Enum.IsDefined(typeof(ReservationStatus), status);
    }
}
}

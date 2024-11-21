/// <copyright file="PaymentValidator.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the implementation of the <see cref="PaymentValidator"/> class,
/// which provides methods for validating various aspects of payments such as price,
/// total cost, payment status, and payment methods within the SmartStay application.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>19/11/2024</date>
using SmartStay.Common.Enums;

/// <summary>
/// The <c>SmartStay.Validation.Validators</c> namespace provides classes and methods
/// dedicated to validating various aspects of the SmartStay application. These validations
/// ensure that input data adheres to business requirements and standards.
/// </summary>
namespace SmartStay.Validation.Validators
{
  /// <summary>
  /// The <c>SmartStay.Validation.Validators</c> namespace contains classes and methods for validating 
  /// various types of input data in the SmartStay application. These validations enforce data integrity 
  /// and compliance with application-specific requirements.
  /// </summary>
  public static class PaymentValidator
{

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
    /// Checks if a price is valid.
    /// </summary>
    /// <param name="price">The price to check.</param>
    /// <returns><c>true</c> if the price is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidPrice(decimal price) => price > 0;

    /// <summary>
    /// Validates the total cost, throwing an exception if invalid.
    /// </summary>
    /// <param name="totalCost">The total cost to validate.</param>
    /// <returns>The validated total cost if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the total cost is invalid.</exception>
    public static decimal ValidateTotalCost(decimal totalCost)
    {
        if (!IsValidTotalCost(totalCost))
        {
            throw new ValidationException(ValidationErrorCode.InvalidTotalCost);
        }
        return totalCost;
    }

    /// <summary>
    /// Checks if a total cost is valid.
    /// </summary>
    /// <param name="totalCost">The total cost to check.</param>
    /// <returns><c>true</c> if the total cost is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidTotalCost(decimal totalCost) => totalCost > 0;

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
    /// Checks if a payment amount is valid.
    /// </summary>
    /// <param name="amount">The payment amount to check.</param>
    /// <returns><c>true</c> if the payment amount is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidPaymentAmount(decimal amount) => amount > 0;

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
    /// Checks if a payment status is valid by confirming it is a defined enum value.
    /// </summary>
    /// <param name="paymentStatus">The payment status to check.</param>
    /// <returns><c>true</c> if the payment status is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidPaymentStatus(PaymentStatus paymentStatus) => Enum.IsDefined(typeof(PaymentStatus),
                                                                                           paymentStatus);

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
    /// Checks if a payment method is valid by confirming it is a defined enum value.
    /// </summary>
    /// <param name="paymentMethod">The payment method to check.</param>
    /// <returns><c>true</c> if the payment method is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidPaymentMethod(PaymentMethod paymentMethod) => Enum.IsDefined(typeof(PaymentMethod),
                                                                                           paymentMethod);

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
}
}

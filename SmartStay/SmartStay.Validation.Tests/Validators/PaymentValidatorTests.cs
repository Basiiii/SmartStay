/// <copyright file="PaymentValidatorTests.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains unit tests for the <see cref="PaymentValidator"/> class,
/// ensuring the correct validation of various aspects of payments within the SmartStay application.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>24/11/2024</date>

/// <summary>
/// The <c>SmartStay.Validation.Tests.Validators</c> namespace contains unit tests for the validation logic
/// of different fields.
/// </summary>
namespace SmartStay.Validation.Tests.Validators
{
using SmartStay.Validation;
using SmartStay.Validation.Validators;
using SmartStay.Common.Enums;
using Xunit;

/// <summary>
/// Contains unit tests for the <see cref="PaymentValidator"/> class.
/// Tests the validation logic for payment-related data in the SmartStay application.
/// </summary>
public class PaymentValidatorTests
{
    /// <summary>
    /// Tests the <see cref="PaymentValidator.ValidatePrice(decimal)"/> method to ensure that
    /// it returns the price when the price is valid.
    /// </summary>
    [Fact]
    public void ValidatePrice_ValidPrice_ReturnsPrice()
    {
        // Arrange
        decimal validPrice = 100.00m;

        // Act
        var result = PaymentValidator.ValidatePrice(validPrice);

        // Assert
        Assert.Equal(validPrice, result);
    }

    /// <summary>
    /// Tests the <see cref="PaymentValidator.ValidatePrice(decimal)"/> method to ensure that
    /// it throws a <see cref="ValidationException"/> when the price is invalid.
    /// </summary>
    [Fact]
    public void ValidatePrice_InvalidPrice_ThrowsValidationException()
    {
        // Arrange
        decimal invalidPrice = -100.00m;

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => PaymentValidator.ValidatePrice(invalidPrice));
        Assert.Equal(ValidationErrorCode.InvalidPrice, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the <see cref="PaymentValidator.ValidateTotalCost(decimal)"/> method to ensure that
    /// it returns the total cost when the total cost is valid.
    /// </summary>
    [Fact]
    public void ValidateTotalCost_ValidTotalCost_ReturnsTotalCost()
    {
        // Arrange
        decimal validTotalCost = 200.00m;

        // Act
        var result = PaymentValidator.ValidateTotalCost(validTotalCost);

        // Assert
        Assert.Equal(validTotalCost, result);
    }

    /// <summary>
    /// Tests the <see cref="PaymentValidator.ValidateTotalCost(decimal)"/> method to ensure that
    /// it throws a <see cref="ValidationException"/> when the total cost is invalid.
    /// </summary>
    [Fact]
    public void ValidateTotalCost_InvalidTotalCost_ThrowsValidationException()
    {
        // Arrange
        decimal invalidTotalCost = -200.00m;

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => PaymentValidator.ValidateTotalCost(invalidTotalCost));
        Assert.Equal(ValidationErrorCode.InvalidTotalCost, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the <see cref="PaymentValidator.ValidatePaymentAmount(decimal)"/> method to ensure that
    /// it returns the payment amount when the amount is valid.
    /// </summary>
    [Fact]
    public void ValidatePaymentAmount_ValidAmount_ReturnsAmount()
    {
        // Arrange
        decimal validAmount = 50.00m;

        // Act
        var result = PaymentValidator.ValidatePaymentAmount(validAmount);

        // Assert
        Assert.Equal(validAmount, result);
    }

    /// <summary>
    /// Tests the <see cref="PaymentValidator.ValidatePaymentAmount(decimal)"/> method to ensure that
    /// it throws a <see cref="ValidationException"/> when the payment amount is invalid.
    /// </summary>
    [Fact]
    public void ValidatePaymentAmount_InvalidAmount_ThrowsValidationException()
    {
        // Arrange
        decimal invalidAmount = -50.00m;

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => PaymentValidator.ValidatePaymentAmount(invalidAmount));
        Assert.Equal(ValidationErrorCode.InvalidPaymentValue, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the <see cref="PaymentValidator.ValidatePaymentStatus(PaymentStatus)"/> method to ensure that
    /// it returns the payment status when the status is valid.
    /// </summary>
    [Fact]
    public void ValidatePaymentStatus_ValidStatus_ReturnsStatus()
    {
        // Arrange
        PaymentStatus validStatus = PaymentStatus.Completed;

        // Act
        var result = PaymentValidator.ValidatePaymentStatus(validStatus);

        // Assert
        Assert.Equal(validStatus, result);
    }

    /// <summary>
    /// Tests the <see cref="PaymentValidator.ValidatePaymentStatus(PaymentStatus)"/> method to ensure that
    /// it throws a <see cref="ValidationException"/> when the payment status is invalid.
    /// </summary>
    [Fact]
    public void ValidatePaymentStatus_InvalidStatus_ThrowsValidationException()
    {
        // Arrange
        PaymentStatus invalidStatus = (PaymentStatus)999; // Invalid enum value

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => PaymentValidator.ValidatePaymentStatus(invalidStatus));
        Assert.Equal(ValidationErrorCode.InvalidPaymentStatus, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the <see cref="PaymentValidator.ValidatePaymentMethod(PaymentMethod)"/> method to ensure that
    /// it returns the payment method when the method is valid.
    /// </summary>
    [Fact]
    public void ValidatePaymentMethod_ValidMethod_ReturnsMethod()
    {
        // Arrange
        PaymentMethod validMethod = PaymentMethod.BankTransfer;

        // Act
        var result = PaymentValidator.ValidatePaymentMethod(validMethod);

        // Assert
        Assert.Equal(validMethod, result);
    }

    /// <summary>
    /// Tests the <see cref="PaymentValidator.ValidatePaymentMethod(PaymentMethod)"/> method to ensure that
    /// it throws a <see cref="ValidationException"/> when the payment method is invalid.
    /// </summary>
    [Fact]
    public void ValidatePaymentMethod_InvalidMethod_ThrowsValidationException()
    {
        // Arrange
        PaymentMethod invalidMethod = (PaymentMethod)999; // Invalid enum value

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => PaymentValidator.ValidatePaymentMethod(invalidMethod));
        Assert.Equal(ValidationErrorCode.InvalidPaymentMethod, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the <see cref="PaymentValidator.ValidatePayment(decimal)"/> method to ensure that
    /// it returns the payment value when the payment value is valid.
    /// </summary>
    [Fact]
    public void ValidatePayment_ValidPayment_ReturnsPayment()
    {
        // Arrange
        decimal validPayment = 150.00m;

        // Act
        var result = PaymentValidator.ValidatePayment(validPayment);

        // Assert
        Assert.Equal(validPayment, result);
    }

    /// <summary>
    /// Tests the <see cref="PaymentValidator.ValidatePayment(decimal)"/> method to ensure that
    /// it throws a <see cref="ValidationException"/> when the payment value is invalid (negative).
    /// </summary>
    [Fact]
    public void ValidatePayment_InvalidPayment_ThrowsValidationException()
    {
        // Arrange
        decimal invalidPayment = -150.00m;

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => PaymentValidator.ValidatePayment(invalidPayment));
        Assert.Equal(ValidationErrorCode.InvalidPaymentValue, exception.ErrorCode);
    }
}
}

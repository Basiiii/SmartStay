/// <copyright file="PaymentTests.cs">
/// Copyright (c) 2024 All Rights Reserved.
/// </copyright>
/// <file>
/// Contains unit tests for the <see cref="Payment"/> class to verify its functionality
/// and adherence to business rules.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>03/12/2024</date>

/// <summary>
/// The <c>SmartStay.Core.Tests.Models</c> namespace contains unit tests for the models used in the SmartStay
/// application.
/// </summary>
namespace SmartStay.Core.Tests.Models
{
using System;
using SmartStay.Common.Enums;
using SmartStay.Core.Models;
using SmartStay.Validation;
using Xunit;

/// <summary>
/// Unit tests for the <see cref="Payment"/> class.
/// </summary>
public class PaymentTests
{
    /// <summary>
    /// Verifies that a valid payment is created successfully.
    /// </summary>
    [Fact]
    public void Payment_ValidData_CreatesPayment()
    {
        // Arrange
        int reservationId = 101;
        decimal amount = 250.50m;
        DateTime date = DateTime.UtcNow;
        PaymentMethod method = PaymentMethod.PayPal;
        PaymentStatus status = PaymentStatus.Completed;

        // Act
        var payment = new Payment(reservationId, amount, date, method, status);

        // Assert
        Assert.Equal(reservationId, payment.ReservationId);
        Assert.Equal(amount, payment.Amount);
        Assert.Equal(date, payment.Date);
        Assert.Equal(method, payment.Method);
        Assert.Equal(status, payment.Status);
        Assert.True(payment.Id > 0);
    }

    /// <summary>
    /// Verifies that attempting to create a payment with an invalid reservation ID throws a <see
    /// cref="ValidationException"/>.
    /// </summary>
    [Fact]
    public void Payment_InvalidReservationId_ThrowsValidationException()
    {
        // Arrange
        int invalidReservationId = -1;
        decimal amount = 100.00m;
        DateTime date = DateTime.UtcNow;
        PaymentMethod method = PaymentMethod.BankTransfer;
        PaymentStatus status = PaymentStatus.Pending;

        // Act & Assert
        var exception =
            Assert.Throws<ValidationException>(() => new Payment(invalidReservationId, amount, date, method, status));
        Assert.Equal(ValidationErrorCode.InvalidId, exception.ErrorCode);
    }

    /// <summary>
    /// Verifies that attempting to create a payment with an invalid amount throws a <see cref="ValidationException"/>.
    /// </summary>
    [Fact]
    public void Payment_InvalidAmount_ThrowsValidationException()
    {
        // Arrange
        int reservationId = 101;
        decimal invalidAmount = -50.00m;
        DateTime date = DateTime.UtcNow;
        PaymentMethod method = PaymentMethod.PayPal;
        PaymentStatus status = PaymentStatus.Completed;

        // Act & Assert
        var exception =
            Assert.Throws<ValidationException>(() => new Payment(reservationId, invalidAmount, date, method, status));
        Assert.Equal(ValidationErrorCode.InvalidPaymentValue, exception.ErrorCode);
    }

    /// <summary>
    /// Verifies that the payment status can be updated when valid.
    /// </summary>
    [Fact]
    public void Payment_UpdateValidStatus_UpdatesStatus()
    {
        // Arrange
        int reservationId = 101;
        decimal amount = 150.00m;
        DateTime date = DateTime.UtcNow;
        PaymentMethod method = PaymentMethod.BankTransfer;
        PaymentStatus initialStatus = PaymentStatus.Pending;
        PaymentStatus updatedStatus = PaymentStatus.Completed;

        var payment = new Payment(reservationId, amount, date, method, initialStatus);

        // Act
        payment.Status = updatedStatus;

        // Assert
        Assert.Equal(updatedStatus, payment.Status);
    }

    /// <summary>
    /// Verifies that attempting to update the payment status with an invalid value throws a <see
    /// cref="ValidationException"/>.
    /// </summary>
    [Fact]
    public void Payment_UpdateInvalidStatus_ThrowsValidationException()
    {
        // Arrange
        int reservationId = 101;
        decimal amount = 200.00m;
        DateTime date = DateTime.UtcNow;
        PaymentMethod method = PaymentMethod.MultiBanco;
        PaymentStatus initialStatus = PaymentStatus.Pending;
        var payment = new Payment(reservationId, amount, date, method, initialStatus);

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => payment.Status = (PaymentStatus)(-1));
        Assert.Equal(ValidationErrorCode.InvalidPaymentStatus, exception.ErrorCode);
    }

    /// <summary>
    /// Verifies that the payment ID is generated uniquely for each payment.
    /// </summary>
    [Fact]
    public void Payment_UniqueIds_AssignsIncrementalIds()
    {
        // Arrange
        int reservationId = 101;
        decimal amount = 100.00m;
        DateTime date = DateTime.UtcNow;
        PaymentMethod method = PaymentMethod.MultiBanco;
        PaymentStatus status = PaymentStatus.Completed;

        // Act
        var payment1 = new Payment(reservationId, amount, date, method, status);
        var payment2 = new Payment(reservationId, amount, date, method, status);

        // Assert
        Assert.NotEqual(payment1.Id, payment2.Id);
        Assert.True(payment2.Id > payment1.Id);
    }

    /// <summary>
    /// Tests the <see cref="Payment.ToString()"/> method to ensure it returns a valid JSON string
    /// representation of the client.
    /// </summary>
    [Fact]
    public void Payment_ToString_ReturnsJson()
    {
        // Arrange
        int reservationId = 101;
        decimal amount = 300.00m;
        DateTime date = DateTime.UtcNow;
        PaymentMethod method = PaymentMethod.PayPal;
        PaymentStatus status = PaymentStatus.Completed;

        var payment = new Payment(reservationId, amount, date, method, status);

        // Act
        var jsonString = payment.ToString();

        // Assert
        Assert.Contains($"\"Id\": {payment.Id}", jsonString);
        Assert.Contains($"\"ReservationId\": {reservationId}", jsonString);
        Assert.Contains($"\"Amount\": {amount}", jsonString);
        Assert.Contains($"\"Method\": \"{method}\"", jsonString);
        Assert.Contains($"\"Status\": \"{status}\"", jsonString);
    }
}
}

/// <copyright file="ClientTests.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains unit tests for the <see cref="Client"/> class, which represents clients
/// in the SmartStay application, including their properties, methods, and validation logic.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>03/12/2024</date>

/// <summary>
/// The <c>SmartStay.Core.Tests.Models</c> namespace contains unit tests for the models used in the SmartStay
/// application.
/// </summary>
namespace SmartStay.Core.Tests.Models
{
using Xunit;
using SmartStay.Core.Models;
using SmartStay.Common.Enums;
using SmartStay.Validation;

/// <summary>
/// Contains unit tests for the <see cref="Client"/> class.
/// Tests include validation, property assignments, and string representation.
/// </summary>
public class ClientTests
{
    /// <summary>
    /// Tests the constructor of the <see cref="Client"/> class to ensure that it properly initializes
    /// a client with the provided first name, last name, and email.
    /// </summary>
    /// <exception cref="ValidationException">Thrown if any validation fails.</exception>
    [Fact]
    public void Constructor_ValidParameters_InitializesClient()
    {
        // Arrange
        var firstName = "Enrique";
        var lastName = "Rodrigues";
        var email = "Enrique.Rodrigues@example.com";

        // Act
        var client = new Client(firstName, lastName, email);

        // Assert
        Assert.Equal(firstName, client.FirstName);
        Assert.Equal(lastName, client.LastName);
        Assert.Equal(email, client.Email);
        Assert.Equal(PaymentMethod.None, client.PreferredPaymentMethod); // Default value
        Assert.NotEqual(0, client.Id);                                   // ID should be auto-assigned
    }

    /// <summary>
    /// Tests the constructor of the <see cref="Client"/> class to ensure that it throws a
    /// <see cref="ValidationException"/> if an invalid name is provided.
    /// </summary>
    [Fact]
    public void Constructor_InvalidName_ThrowsValidationException()
    {
        // Arrange
        var invalidName = "";
        var validEmail = "enrique.rodrigues@example.com";

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => new Client(invalidName, "Rodrigues", validEmail));

        Assert.Equal("The provided name is invalid.", exception.Message);
    }

    /// <summary>
    /// Tests the <see cref="Client.FirstName"/> property to ensure that it correctly sets and retrieves
    /// the first name. Validates that invalid values throw a <see cref="ValidationException"/>.
    /// </summary>
    [Fact]
    public void FirstName_SetAndGet_ValidatesValue()
    {
        // Arrange
        var client = new Client("Enrique", "Rodrigues", "enrique.rodrigues@example.com");

        // Act
        client.FirstName = "Jane"; // Valid name

        // Assert
        Assert.Equal("Jane", client.FirstName);

        // Act & Assert for invalid value
        Assert.Throws<ValidationException>(() => client.FirstName = "");
    }

    /// <summary>
    /// Tests the <see cref="Client.Email"/> property to ensure that it correctly sets and retrieves
    /// the email. Validates that invalid email formats throw a <see cref="ValidationException"/>.
    /// </summary>
    [Fact]
    public void Email_SetAndGet_ValidatesValue()
    {
        // Arrange
        var client = new Client("Enrique", "Rodrigues", "enrique.rodrigues@example.com");

        // Act
        client.Email = "jane.doe@example.com"; // Valid email

        // Assert
        Assert.Equal("jane.doe@example.com", client.Email);

        // Act & Assert for invalid value
        Assert.Throws<ValidationException>(() => client.Email = "invalid-email");
    }

    /// <summary>
    /// Tests the <see cref="Client.ToString()"/> method to ensure it returns a valid JSON string
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
        Assert.Contains("\"Id\":", jsonString);
        Assert.Contains($"\"ReservationId\": {reservationId}", jsonString);
        Assert.Contains($"\"Amount\": {amount}", jsonString);
        Assert.Contains($"\"Method\": \"{method}\"", jsonString);
        Assert.Contains($"\"Status\": \"{status}\"", jsonString);
    }

    /// <summary>
    /// Tests the <see cref="Client.PreferredPaymentMethod"/> property to ensure it correctly sets
    /// and retrieves the preferred payment method.
    /// </summary>
    [Fact]
    public void PreferredPaymentMethod_SetAndGet_CorrectlyAssignsValue()
    {
        // Arrange
        var client = new Client("Enrique", "Rodrigues", "enrique.rodrigues@example.com");

        // Act
        client.PreferredPaymentMethod = PaymentMethod.MultiBanco;

        // Assert
        Assert.Equal(PaymentMethod.MultiBanco, client.PreferredPaymentMethod);
    }

    /// <summary>
    /// Tests that the <see cref="Client.Id"/> is a unique and non-zero value assigned during initialization.
    /// </summary>
    [Fact]
    public void Id_AutoGenerated_IsUniqueAndNonZero()
    {
        // Arrange
        var client1 = new Client("John", "Doe", "john.doe@example.com");
        var client2 = new Client("Jane", "Smith", "jane.smith@example.com");

        // Act
        var id1 = client1.Id;
        var id2 = client2.Id;

        // Assert
        Assert.NotEqual(0, id1);
        Assert.NotEqual(0, id2);
        Assert.NotEqual(id1, id2); // Ensure unique IDs
    }
}
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartStay.Models;
using SmartStay.Models.Enums;
using SmartStay.Services;
using SmartStay.Validation;
using System;

namespace SmartStay.Tests
{
[TestClass]
public class ClientTests
{
    /// <summary>
    /// Tests the constructor of the Client class when valid data is provided.
    /// Ensures the client is created successfully with the given information.
    /// </summary>
    [TestMethod]
    public void Client_ValidData_CreatesClient()
    {
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var client = new Client(firstName, lastName, email);

        Assert.IsNotNull(client);
        Assert.AreEqual(firstName, client.FirstName);
        Assert.AreEqual(lastName, client.LastName);
        Assert.AreEqual(email, client.Email);
    }

    /// <summary>
    /// Tests the constructor of the Client class when all optional parameters are provided.
    /// Ensures the client is created successfully with full details.
    /// </summary>
    [TestMethod]
    public void Client_FullData_CreatesClient()
    {
        var firstName = "Jane";
        var lastName = "Smith";
        var email = "jane.smith@example.com";
        var phoneNumber = "+351999999999";
        var address = "123 Elm St, Springfield";
        var preferredPaymentMethod = PaymentMethod.BankTransfer;

        var client = new Client(firstName, lastName, email, phoneNumber, address, preferredPaymentMethod);

        Assert.IsNotNull(client);
        Assert.AreEqual(firstName, client.FirstName);
        Assert.AreEqual(lastName, client.LastName);
        Assert.AreEqual(email, client.Email);
        Assert.AreEqual(phoneNumber, client.PhoneNumber);
        Assert.AreEqual(address, client.Address);
        Assert.AreEqual(preferredPaymentMethod, client.PreferredPaymentMethod);
    }

    /// <summary>
    /// Tests the constructor of the Client class when invalid email is provided.
    /// Ensures a ValidationException is thrown.
    /// </summary>
    [TestMethod]
    public void Client_InvalidEmail_ThrowsValidationException()
    {
        var firstName = "John";
        var lastName = "Doe";
        var invalidEmail = "invalid-email";

        var exception =
            Assert.ThrowsException<ValidationException>(() => new Client(firstName, lastName, invalidEmail));
        Assert.AreEqual(ValidationErrorCode.InvalidEmail, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the constructor of the Client class when invalid phone number is provided.
    /// Ensures a ValidationException is thrown.
    /// </summary>
    [TestMethod]
    public void Client_InvalidPhoneNumber_ThrowsValidationException()
    {
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var invalidPhoneNumber = "invalid-phone";

        var exception = Assert.ThrowsException<ValidationException>(
            () => new Client(firstName, lastName, email, invalidPhoneNumber, "123 Elm St"));
        Assert.AreEqual(ValidationErrorCode.InvalidPhoneNumber, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the constructor of the Client class when invalid address is provided.
    /// Ensures a ValidationException is thrown.
    /// </summary>
    [TestMethod]
    public void Client_InvalidAddress_ThrowsValidationException()
    {
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var phoneNumber = "+351999999999";
        var invalidAddress = "";

        var exception = Assert.ThrowsException<ValidationException>(
            () => new Client(firstName, lastName, email, phoneNumber, invalidAddress));
        Assert.AreEqual(ValidationErrorCode.InvalidAddress, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the constructor of the Client class when an invalid payment method is provided.
    /// Ensures a ValidationException is thrown.
    /// </summary>
    [TestMethod]
    public void Client_InvalidPaymentMethod_ThrowsValidationException()
    {
        var firstName = "Jane";
        var lastName = "Smith";
        var email = "jane.smith@example.com";
        var phoneNumber = "+351999999999";
        var address = "123 Elm St, Springfield";
        var invalidPaymentMethod = (PaymentMethod)999; // Invalid payment method

        var exception = Assert.ThrowsException<ValidationException>(
            () => new Client(firstName, lastName, email, phoneNumber, address, invalidPaymentMethod));
        Assert.AreEqual(ValidationErrorCode.InvalidPaymentMethod, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the property setter and getter for FirstName.
    /// Ensures that a valid name can be set and retrieved correctly.
    /// </summary>
    [TestMethod]
    public void Client_SetValidFirstName_UpdatesFirstName()
    {
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var client = new Client(firstName, lastName, email);

        var newFirstName = "Johnny";
        client.FirstName = newFirstName;

        Assert.AreEqual(newFirstName, client.FirstName);
    }

    /// <summary>
    /// Tests the property setter and getter for LastName.
    /// Ensures that a valid last name can be set and retrieved correctly.
    /// </summary>
    [TestMethod]
    public void Client_SetValidLastName_UpdatesLastName()
    {
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var client = new Client(firstName, lastName, email);

        var newLastName = "Smith";
        client.LastName = newLastName;

        Assert.AreEqual(newLastName, client.LastName);
    }

    /// <summary>
    /// Tests the property setter and getter for Email.
    /// Ensures that a valid email can be set and retrieved correctly.
    /// </summary>
    [TestMethod]
    public void Client_SetValidEmail_UpdatesEmail()
    {
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var client = new Client(firstName, lastName, email);

        var newEmail = "johnny.doe@example.com";
        client.Email = newEmail;

        Assert.AreEqual(newEmail, client.Email);
    }

    /// <summary>
    /// Tests the property setter and getter for PhoneNumber.
    /// Ensures that a valid phone number can be set and retrieved correctly.
    /// </summary>
    [TestMethod]
    public void Client_SetValidPhoneNumber_UpdatesPhoneNumber()
    {
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var phoneNumber = "+351999999999";
        var client = new Client(firstName, lastName, email, phoneNumber, "123 Elm St");

        var newPhoneNumber = "+351888888888";
        client.PhoneNumber = newPhoneNumber;

        Assert.AreEqual(newPhoneNumber, client.PhoneNumber);
    }

    /// <summary>
    /// Tests the property setter and getter for Address.
    /// Ensures that a valid address can be set and retrieved correctly.
    /// </summary>
    [TestMethod]
    public void Client_SetValidAddress_UpdatesAddress()
    {
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var phoneNumber = "+351999999999";
        var address = "123 Elm St, Springfield";
        var client = new Client(firstName, lastName, email, phoneNumber, address);

        var newAddress = "456 Oak St, Springfield";
        client.Address = newAddress;

        Assert.AreEqual(newAddress, client.Address);
    }

    /// <summary>
    /// Tests the property setter and getter for PreferredPaymentMethod.
    /// Ensures that a valid payment method can be set and retrieved correctly.
    /// </summary>
    [TestMethod]
    public void Client_SetValidPaymentMethod_UpdatesPaymentMethod()
    {
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var phoneNumber = "+351999999999";
        var address = "123 Elm St, Springfield";
        var preferredPaymentMethod = PaymentMethod.PayPal;

        var client = new Client(firstName, lastName, email, phoneNumber, address, preferredPaymentMethod);

        var newPaymentMethod = PaymentMethod.BankTransfer;
        client.PreferredPaymentMethod = newPaymentMethod;

        Assert.AreEqual(newPaymentMethod, client.PreferredPaymentMethod);
    }

    /// <summary>
    /// Tests the client ID generation to ensure it increments properly for multiple clients.
    /// Ensures that each client has a unique ID.
    /// </summary>
    [TestMethod]
    public void Client_GenerateUniqueClientId_CreatesUniqueIds()
    {
        var firstClient = new Client("John", "Doe", "john.doe@example.com");
        var secondClient = new Client("Jane", "Smith", "jane.smith@example.com");

        Assert.AreNotEqual(firstClient.Id, secondClient.Id);
    }

    /// <summary>
    /// Tests the ToString method of the Client class.
    /// Ensures the client object is serialized to a JSON string with proper formatting.
    /// </summary>
    [TestMethod]
    public void Client_ToString_ReturnsValidJson()
    {
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var client = new Client(firstName, lastName, email);

        var json = client.ToString();

        Assert.IsTrue(json.Contains("\"FirstName\": \"John\""));
        Assert.IsTrue(json.Contains("\"LastName\": \"Doe\""));
        Assert.IsTrue(json.Contains("\"Email\": \"john.doe@example.com\""));
    }
}
}

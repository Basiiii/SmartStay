using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartStay.Models;
using SmartStay.Services;
using SmartStay.Validation;
using System;

namespace SmartStay.Tests
{
[TestClass]
public class ClientTests
{
    /// <summary>
    /// Test the creation of a valid Client object.
    /// </summary>
    [TestMethod]
    public void Client_Creation_WithValidData_ShouldPass()
    {
        // Arrange
        string firstName = "John";
        string lastName = "Doe";
        string email = "john.doe@example.com";

        // Act
        var client = new Client(firstName, lastName, email);

        // Assert
        Assert.IsNotNull(client); // Ensure client is created
        Assert.AreEqual(firstName, client.FirstName);
        Assert.AreEqual(lastName, client.LastName);
        Assert.AreEqual(email, client.Email);
    }

    /// <summary>
    /// Test creation of Client with invalid email (should throw ValidationException).
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void Client_Creation_WithInvalidEmail_ShouldThrowException()
    {
        // Arrange
        string firstName = "John";
        string lastName = "Doe";
        string invalidEmail = "invalid-email";

        // Act
        var client = new Client(firstName, lastName, invalidEmail);
    }

    /// <summary>
    /// Test creation of Client with invalid phone number (should throw ValidationException).
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void Client_Creation_WithInvalidPhoneNumber_ShouldThrowException()
    {
        // Arrange
        string firstName = "Jane";
        string lastName = "Smith";
        string email = "jane.smith@example.com";
        string invalidPhoneNumber = "123"; // Invalid phone number

        // Act
        var client = new Client(firstName, lastName, email, invalidPhoneNumber, "Foo Address");
    }
}
}

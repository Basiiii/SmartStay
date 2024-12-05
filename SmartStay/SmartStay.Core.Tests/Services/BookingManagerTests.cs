/// <copyright file="BookingManagerTests.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains unit tests for the <see cref="BookingManager"/> class, specifically testing the file creation
/// functionality for saving data to files in the SmartStay system.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>04/12/2024</date>

/// <summary>
/// The <c>SmartStay.Core.Tests.Services</c> namespace contains unit tests for the services used in the SmartStay
/// application. These tests verify the correct behavior of the <see cref="BookingManager"/> class and its methods.
/// </summary>
namespace SmartStay.Core.Tests.Services
{
using SmartStay.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Xunit;
using SmartStay.Common.Exceptions;
using SmartStay.Core.Models;
using SmartStay.Common.Enums;

/// <summary>
/// Contains unit tests for the <see cref="BookingManager"/> class.
/// Tests the <see cref="BookingManager.SaveAll"/> method to ensure that it creates the necessary files when saving
/// repositories.
/// </summary>
public class BookingManagerTests
{
    /// <summary>
    /// Tests the <see cref="BookingManager.SaveAll"/> method to ensure that it creates the necessary files
    /// in the specified directory when saving repositories.
    /// </summary>
    [Fact]
    public void SaveAll_CreatesFiles_WhenCalled()
    {
        // Arrange: Set up a temporary folder path for testing
        string dataFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(dataFolder); // Create the temp directory

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager with the logger
        var bookingManager = new BookingManager(logger);

        // Act: Call SaveAll to create files in the temp directory
        bookingManager.SaveAll(dataFolder);

        // Assert: Verify that the expected files were created
        Assert.True(File.Exists(Path.Combine(dataFolder, "clients.dat")), "Clients file was not created.");
        Assert.True(File.Exists(Path.Combine(dataFolder, "accommodations.dat")),
                    "Accommodations file was not created.");
        Assert.True(File.Exists(Path.Combine(dataFolder, "reservations.dat")), "Reservations file was not created.");
        Assert.True(File.Exists(Path.Combine(dataFolder, "owners.dat")), "Owners file was not created.");

        // Clean up: Delete the temporary directory and its contents after the test
        Directory.Delete(dataFolder, true); // Remove the temp folder and files
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.CreateBasicClient"/> method to ensure it creates a client
    /// with valid input data.
    /// </summary>
    [Fact]
    public void CreateBasicClient_CreatesClient_WhenValidInput()
    {
        // Arrange: Create input data for client creation
        string firstName = "John";
        string lastName = "Doe";
        string email = "john.doe@example.com";

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager with the logger
        var bookingManager = new BookingManager(logger);

        // Act: Call CreateBasicClient to create a new client
        var client = bookingManager.CreateBasicClient(firstName, lastName, email);

        // Assert: Verify that the client was created with the expected values
        Assert.NotNull(client);
        Assert.Equal(firstName, client.FirstName);
        Assert.Equal(lastName, client.LastName);
        Assert.Equal(email, client.Email);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.CreateBasicClient"/> method to ensure it throws a
    /// <see cref="ClientCreationException"/> when invalid input data is provided.
    /// </summary>
    [Fact]
    public void CreateBasicClient_ThrowsClientCreationException_WhenValidationFails()
    {
        // Arrange: Create invalid input data (e.g., invalid email)
        string firstName = "John";
        string lastName = "Doe";
        string email = "invalid-email"; // Invalid email

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager with the logger
        var bookingManager = new BookingManager(logger);

        // Act & Assert: Verify that creating a client with invalid input throws an exception
        var exception =
            Assert.Throws<ClientCreationException>(() => bookingManager.CreateBasicClient(firstName, lastName, email));
        Assert.Contains("An error occurred while creating the client", exception.Message);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.CreateCompleteClient"/> method to ensure it creates a client
    /// with all information (first name, last name, email, phone number, and address) when valid input data is
    /// provided.
    /// </summary>
    [Fact]
    public void CreateCompleteClient_CreatesClient_WhenValidInput()
    {
        // Arrange: Create input data for complete client creation
        string firstName = "Jane";
        string lastName = "Doe";
        string email = "jane.doe@example.com";
        string phoneNumber = "+351222333444";
        string address = "123 Main St";

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager with the logger
        var bookingManager = new BookingManager(logger);

        // Act: Call CreateCompleteClient to create a new client
        var client = bookingManager.CreateCompleteClient(firstName, lastName, email, phoneNumber, address);

        // Assert: Verify that the client was created with the expected values
        Assert.NotNull(client);
        Assert.Equal(firstName, client.FirstName);
        Assert.Equal(lastName, client.LastName);
        Assert.Equal(email, client.Email);
        Assert.Equal(phoneNumber, client.PhoneNumber);
        Assert.Equal(address, client.Address);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.CreateCompleteClient"/> method to ensure it throws a
    /// <see cref="ClientCreationException"/> when invalid input data is provided.
    /// </summary>
    [Fact]
    public void CreateCompleteClient_ThrowsClientCreationException_WhenValidationFails()
    {
        // Arrange: Create invalid input data (e.g., invalid email)
        string firstName = "Jane";
        string lastName = "Doe";
        string email = "invalid-email"; // Invalid email
        string phoneNumber = "+351222333444";
        string address = "123 Main St";

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager with the logger
        var bookingManager = new BookingManager(logger);

        // Act & Assert: Verify that creating a client with invalid input throws an exception
        var exception = Assert.Throws<ClientCreationException>(
            () => bookingManager.CreateCompleteClient(firstName, lastName, email, phoneNumber, address));
        Assert.Contains("An error occurred while creating the client", exception.Message);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.FindClientById"/> method to ensure it correctly finds a client by their ID.
    /// </summary>
    [Fact]
    public void FindClientById_ReturnsClient_WhenClientExists()
    {
        // Arrange: Create a client and add them to the system
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager and add the client to the system
        var bookingManager = new BookingManager(logger);
        var client = new Client(firstName, lastName, email);
        bookingManager.Clients.Add(client);

        // Act: Call FindClientById to find the client by ID
        var foundClient = bookingManager.FindClientById(client.Id);

        // Assert: Verify that the correct client is returned
        Assert.NotNull(foundClient);
        Assert.Equal(client.Id, foundClient.Id);
        Assert.Equal(firstName, foundClient.FirstName);
        Assert.Equal(lastName, foundClient.LastName);
        Assert.Equal(email, foundClient.Email);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.FindClientById"/> method to ensure it throws an exception
    /// when no client with the specified ID is found.
    /// </summary>
    [Fact]
    public void FindClientById_ThrowsArgumentException_WhenClientNotFound()
    {
        // Arrange: Use a non-existent client ID
        var nonExistentClientId = 999;

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager
        var bookingManager = new BookingManager(logger);

        // Act & Assert: Verify that an exception is thrown when no client is found with the given ID
        var exception = Assert.Throws<ArgumentException>(() => bookingManager.FindClientById(nonExistentClientId));
        Assert.Contains($"Client with ID {nonExistentClientId} not found.", exception.Message);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.UpdateClient"/> method to ensure that it updates the client details
    /// correctly when valid data is provided.
    /// </summary>
    [Fact]
    public void UpdateClient_UpdatesClient_WhenValidData()
    {
        // Arrange: Create a client and add them to the system
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var phoneNumber = "+351222333444";
        var address = "123 Main St";
        var paymentMethod = PaymentMethod.BankTransfer;

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager and add the client to the system
        var bookingManager = new BookingManager(logger);
        var client = new Client(firstName, lastName, email);
        bookingManager.Clients.Add(client); // Assuming AddClient is a method to add clients to the system

        // Act: Call UpdateClient to update the client information
        var result =
            bookingManager.UpdateClient(client.Id, firstName, lastName, email, phoneNumber, address, paymentMethod);

        // Assert: Verify that the update was successful and the client details were updated
        Assert.Equal(UpdateClientResult.Success, result);
        var updatedClient = bookingManager.FindClientById(client.Id);
        Assert.Equal(firstName, updatedClient.FirstName);
        Assert.Equal(lastName, updatedClient.LastName);
        Assert.Equal(email, updatedClient.Email);
        Assert.Equal(phoneNumber, updatedClient.PhoneNumber);
        Assert.Equal(address, updatedClient.Address);
        Assert.Equal(paymentMethod, updatedClient.PreferredPaymentMethod);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.UpdateClient"/> method to ensure that it returns
    /// <see cref="UpdateClientResult.ClientNotFound"/> when the client does not exist.
    /// </summary>
    [Fact]
    public void UpdateClient_ReturnsClientNotFound_WhenClientDoesNotExist()
    {
        // Arrange: Use a non-existent client ID
        var nonExistentClientId = 999;
        var firstName = "Jane";
        var lastName = "Doe";
        var email = "jane.doe@example.com";

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager
        var bookingManager = new BookingManager(logger);

        // Act: Call UpdateClient with a non-existent client ID
        var result = bookingManager.UpdateClient(nonExistentClientId, firstName, lastName, email);

        // Assert: Verify that the result is ClientNotFound
        Assert.Equal(UpdateClientResult.ClientNotFound, result);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.UpdateClient"/> method to ensure it returns the correct validation error
    /// when the first name is invalid.
    /// </summary>
    [Fact]
    public void UpdateClient_ReturnsInvalidFirstName_WhenFirstNameIsInvalid()
    {
        // Arrange: Create a client and add them to the system
        var invalidFirstName = ""; // Invalid first name (empty string)
        var lastName = "Doe";
        var email = "john.doe@example.com";

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager and add the client to the system
        var bookingManager = new BookingManager(logger);
        var client = new Client("John", lastName, email);
        bookingManager.Clients.Add(client); // Assuming AddClient is a method to add clients to the system

        // Act: Call UpdateClient with an invalid first name
        var result = bookingManager.UpdateClient(client.Id, invalidFirstName, lastName, email);

        // Assert: Verify that the result is InvalidFirstName
        Assert.Equal(UpdateClientResult.InvalidFirstName, result);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.UpdateClient"/> method to ensure it returns the correct validation error
    /// when the last name is invalid.
    /// </summary>
    [Fact]
    public void UpdateClient_ReturnsInvalidLastName_WhenLastNameIsInvalid()
    {
        // Arrange: Create a client and add them to the system
        var firstName = "John";
        var invalidLastName = ""; // Invalid last name (empty string)
        var email = "john.doe@example.com";

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager and add the client to the system
        var bookingManager = new BookingManager(logger);
        var client = new Client(firstName, "Doe", email);
        bookingManager.Clients.Add(client); // Assuming AddClient is a method to add clients to the system

        // Act: Call UpdateClient with an invalid last name
        var result = bookingManager.UpdateClient(client.Id, firstName, invalidLastName, email);

        // Assert: Verify that the result is InvalidLastName
        Assert.Equal(UpdateClientResult.InvalidLastName, result);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.UpdateClient"/> method to ensure it returns the correct validation error
    /// when the email is invalid.
    /// </summary>
    [Fact]
    public void UpdateClient_ReturnsInvalidEmail_WhenEmailIsInvalid()
    {
        // Arrange: Create a client and add them to the system
        var firstName = "John";
        var lastName = "Doe";
        var invalidEmail = "invalid-email"; // Invalid email format

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager and add the client to the system
        var bookingManager = new BookingManager(logger);
        var client = new Client(firstName, lastName, "john.doe@example.com");
        bookingManager.Clients.Add(client); // Assuming AddClient is a method to add clients to the system

        // Act: Call UpdateClient with an invalid email
        UpdateClientResult result = bookingManager.UpdateClient(client.Id, firstName, lastName, invalidEmail);

        // Assert: Verify that the result is InvalidEmail
        Assert.Equal(UpdateClientResult.InvalidEmail, result);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.RemoveClient"/> method to ensure that it removes a client from the system
    /// when the client exists.
    /// </summary>
    [Fact]
    public void RemoveClient_RemovesClient_WhenClientExists()
    {
        // Arrange: Create a client and add them to the system
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var client = new Client(firstName, lastName, email);

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager and add the client to the system
        var bookingManager = new BookingManager(logger);
        bookingManager.Clients.Add(client); // Add the client to the system

        // Act: Call RemoveClient to remove the client
        var result = bookingManager.RemoveClient(client.Id);

        // Assert: Verify that the result is true and the client has been removed
        Assert.True(result);
        var removedClient = bookingManager.Clients.FindClientById(client.Id);
        Assert.Null(removedClient); // Ensure the client no longer exists in the system
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.RemoveClient"/> method to ensure that it returns false
    /// when attempting to remove a client that does not exist.
    /// </summary>
    [Fact]
    public void RemoveClient_ReturnsFalse_WhenClientDoesNotExist()
    {
        // Arrange: Use a non-existent client ID
        var nonExistentClientId = 999;

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager
        var bookingManager = new BookingManager(logger);

        // Act: Call RemoveClient with a non-existent client ID
        var result = bookingManager.RemoveClient(nonExistentClientId);

        // Assert: Verify that the result is false
        Assert.False(result);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.CreateBasicOwner"/> method to ensure that it creates a new owner
    /// with basic information when valid data is provided.
    /// </summary>
    [Fact]
    public void CreateBasicOwner_CreatesOwner_WhenValidDataIsProvided()
    {
        // Arrange: Create an owner with valid data
        var firstName = "Alice";
        var lastName = "Smith";
        var email = "alice.smith@example.com";

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager
        var bookingManager = new BookingManager(logger);

        // Act: Call CreateBasicOwner to create the owner
        var owner = bookingManager.CreateBasicOwner(firstName, lastName, email);

        // Assert: Verify that the owner is created and added to the system
        Assert.NotNull(owner);
        Assert.Equal(firstName, owner.FirstName);
        Assert.Equal(lastName, owner.LastName);
        Assert.Equal(email, owner.Email);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.CreateBasicOwner"/> method to ensure that it throws an exception
    /// when invalid email is provided.
    /// </summary>
    [Fact]
    public void CreateBasicOwner_ThrowsException_WhenEmailIsInvalid()
    {
        // Arrange: Create an owner with invalid email
        var firstName = "Alice";
        var lastName = "Smith";
        var invalidEmail = "invalid-email"; // Invalid email format

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager
        var bookingManager = new BookingManager(logger);

        // Act & Assert: Verify that an exception is thrown for invalid email
        var exception = Assert.Throws<OwnerCreationException>(
            () => bookingManager.CreateBasicOwner(firstName, lastName, invalidEmail));
        Assert.Contains("An error occurred while creating the owner due to invalid input", exception.Message);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.CreateCompleteOwner"/> method to ensure that it creates a new owner
    /// with full details when valid data is provided.
    /// </summary>
    [Fact]
    public void CreateCompleteOwner_CreatesOwner_WhenValidDataIsProvided()
    {
        // Arrange: Create an owner with full valid data
        var firstName = "Bob";
        var lastName = "Johnson";
        var email = "bob.johnson@example.com";
        var phoneNumber = "+351222333444"; // Correct phone number format
        var address = "123 Main St, Lisbon";

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager
        var bookingManager = new BookingManager(logger);

        // Act: Call CreateCompleteOwner to create the owner
        var owner = bookingManager.CreateCompleteOwner(firstName, lastName, email, phoneNumber, address);

        // Assert: Verify that the owner is created and added to the system
        Assert.NotNull(owner);
        Assert.Equal(firstName, owner.FirstName);
        Assert.Equal(lastName, owner.LastName);
        Assert.Equal(email, owner.Email);
        Assert.Equal(phoneNumber, owner.PhoneNumber);
        Assert.Equal(address, owner.Address);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.CreateCompleteOwner"/> method to ensure that it throws an exception
    /// when invalid phone number is provided.
    /// </summary>
    [Fact]
    public void CreateCompleteOwner_ThrowsException_WhenPhoneNumberIsInvalid()
    {
        // Arrange: Create an owner with invalid phone number
        var firstName = "Bob";
        var lastName = "Johnson";
        var email = "bob.johnson@example.com";
        var invalidPhoneNumber = "invalid-phone"; // Invalid phone number format
        var address = "123 Main St, Lisbon";

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager
        var bookingManager = new BookingManager(logger);

        // Act & Assert: Verify that an exception is thrown for invalid phone number
        var exception = Assert.Throws<OwnerCreationException>(
            () => bookingManager.CreateCompleteOwner(firstName, lastName, email, invalidPhoneNumber, address));
        Assert.Contains("An error occurred while creating the owner due to invalid input", exception.Message);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.FindOwnerById"/> method to ensure that it finds an owner when the ID is
    /// valid.
    /// </summary>
    [Fact]
    public void FindOwnerById_FindsOwner_WhenOwnerExists()
    {
        // Arrange: Create an owner and add them to the system
        var firstName = "Charlie";
        var lastName = "Brown";
        var email = "charlie.brown@example.com";
        var owner = new Owner(firstName, lastName, email);

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager and add the owner to the system
        var bookingManager = new BookingManager(logger);
        bookingManager.Owners.Add(owner);

        // Act: Call FindOwnerById to find the owner
        var foundOwner = bookingManager.FindOwnerById(owner.Id);

        // Assert: Verify that the correct owner is returned
        Assert.NotNull(foundOwner);
        Assert.Equal(owner.Id, foundOwner.Id);
        Assert.Equal(firstName, foundOwner.FirstName);
        Assert.Equal(lastName, foundOwner.LastName);
        Assert.Equal(email, foundOwner.Email);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.FindOwnerById"/> method to ensure that it throws an exception
    /// when the owner does not exist.
    /// </summary>
    [Fact]
    public void FindOwnerById_ThrowsException_WhenOwnerDoesNotExist()
    {
        // Arrange: Use a non-existent owner ID
        var nonExistentOwnerId = 999;

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager
        var bookingManager = new BookingManager(logger);

        // Act & Assert: Verify that an exception is thrown when the owner is not found
        var exception = Assert.Throws<ArgumentException>(() => bookingManager.FindOwnerById(nonExistentOwnerId));
        Assert.Contains("Owner with ID", exception.Message);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.UpdateOwner"/> method to ensure that it updates an owner
    /// when all input data is valid.
    /// </summary>
    [Fact]
    public void UpdateOwner_UpdatesOwner_WhenValidDataIsProvided()
    {
        // Arrange: Create an owner and add them to the system
        var firstName = "David";
        var lastName = "Clark";
        var email = "david.clark@example.com";
        var owner = new Owner(firstName, lastName, email);

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager and add the owner to the system
        var bookingManager = new BookingManager(logger);
        bookingManager.Owners.Add(owner);

        // Act: Call UpdateOwner to update the owner's details
        var updatedFirstName = "Davidson";
        var updatedLastName = "Clarkson";
        var updatedEmail = "david.clarkson@example.com";
        var result = bookingManager.UpdateOwner(owner.Id, updatedFirstName, updatedLastName, updatedEmail);

        // Assert: Verify that the update was successful
        Assert.Equal(UpdateOwnerResult.Success, result);
        var updatedOwner = bookingManager.FindOwnerById(owner.Id);
        Assert.Equal(updatedFirstName, updatedOwner.FirstName);
        Assert.Equal(updatedLastName, updatedOwner.LastName);
        Assert.Equal(updatedEmail, updatedOwner.Email);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.UpdateOwner"/> method to ensure that it returns
    /// OwnerNotFound when trying to update a non-existent owner.
    /// </summary>
    [Fact]
    public void UpdateOwner_ReturnsOwnerNotFound_WhenOwnerDoesNotExist()
    {
        // Arrange: Use a non-existent owner ID
        var nonExistentOwnerId = 999;

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager
        var bookingManager = new BookingManager(logger);

        // Act: Try to update the non-existent owner
        var result =
            bookingManager.UpdateOwner(nonExistentOwnerId, "NewFirstName", "NewLastName", "new.email@example.com");

        // Assert: Verify that the result is OwnerNotFound
        Assert.Equal(UpdateOwnerResult.OwnerNotFound, result);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.UpdateOwner"/> method to ensure that it returns
    /// InvalidFirstName when an invalid first name is provided.
    /// </summary>
    [Fact]
    public void UpdateOwner_ReturnsInvalidFirstName_WhenFirstNameIsInvalid()
    {
        // Arrange: Create an owner and add them to the system
        var firstName = "Emily";
        var lastName = "Taylor";
        var email = "emily.taylor@example.com";
        var owner = new Owner(firstName, lastName, email);

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager and add the owner to the system
        var bookingManager = new BookingManager(logger);
        bookingManager.Owners.Add(owner);

        // Act: Try to update the owner with an invalid first name (empty string)
        var result = bookingManager.UpdateOwner(owner.Id, "", lastName, email);

        // Assert: Verify that the result is InvalidFirstName
        Assert.Equal(UpdateOwnerResult.InvalidFirstName, result);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.UpdateOwner"/> method to ensure that it returns
    /// InvalidEmail when an invalid email is provided.
    /// </summary>
    [Fact]
    public void UpdateOwner_ReturnsInvalidEmail_WhenEmailIsInvalid()
    {
        // Arrange: Create an owner and add them to the system
        var firstName = "Emily";
        var lastName = "Taylor";
        var email = "emily.taylor@example.com";
        var owner = new Owner(firstName, lastName, email);

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager and add the owner to the system
        var bookingManager = new BookingManager(logger);
        bookingManager.Owners.Add(owner);

        // Act: Try to update the owner with an invalid email
        var result = bookingManager.UpdateOwner(owner.Id, firstName, lastName, "invalid-email");

        // Assert: Verify that the result is InvalidEmail
        Assert.Equal(UpdateOwnerResult.InvalidEmail, result);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.RemoveOwner"/> method to ensure it removes an owner when the ID is valid.
    /// </summary>
    [Fact]
    public void RemoveOwner_RemovesOwner_WhenOwnerExists()
    {
        // Arrange: Create an owner and add them to the system
        var firstName = "James";
        var lastName = "Smith";
        var email = "james.smith@example.com";
        var owner = new Owner(firstName, lastName, email);

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager and add the owner to the system
        var bookingManager = new BookingManager(logger);
        bookingManager.Owners.Add(owner);

        // Act: Call RemoveOwner to remove the owner
        var result = bookingManager.RemoveOwner(owner.Id);

        // Assert: Verify that the result is true and the owner has been removed
        Assert.True(result);
        var removedOwner = bookingManager.Owners.FindOwnerById(owner.Id);
        Assert.Null(removedOwner); // Ensure the owner no longer exists in the system
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.RemoveOwner"/> method to ensure it returns false
    /// when trying to remove an owner that does not exist.
    /// </summary>
    [Fact]
    public void RemoveOwner_ReturnsFalse_WhenOwnerDoesNotExist()
    {
        // Arrange: Use a non-existent owner ID
        var nonExistentOwnerId = 999;

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager
        var bookingManager = new BookingManager(logger);

        // Act: Try to remove the non-existent owner
        var result = bookingManager.RemoveOwner(nonExistentOwnerId);

        // Assert: Verify that the result is false
        Assert.False(result);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.CreateAccommodation"/> method to ensure it successfully creates an
    /// accommodation when the owner exists and all conditions are met.
    /// </summary>
    [Fact]
    public void CreateAccommodation_CreatesAccommodation_WhenOwnerExists()
    {
        // Arrange: Create an owner and add them to the system
        var firstName = "Alice";
        var lastName = "Johnson";
        var email = "alice.johnson@example.com";
        var owner = new Owner(firstName, lastName, email);

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager and add the owner to the system
        var bookingManager = new BookingManager(logger);
        bookingManager.Owners.Add(owner);

        // Define accommodation details
        var accommodationName = "Ocean View Villa";
        var accommodationType = AccommodationType.Villa;
        var accommodationAddress = "123 Beachfront Ave, Seaside, CA";

        // Act: Call CreateAccommodation to create an accommodation
        var accommodation =
            bookingManager.CreateAccommodation(owner.Id, accommodationType, accommodationName, accommodationAddress);

        // Assert: Verify the accommodation is created successfully
        Assert.NotNull(accommodation);
        Assert.Equal(accommodationName, accommodation.Name);
        Assert.Equal(accommodationAddress, accommodation.Address);
        Assert.Equal(owner.Id, accommodation.OwnerId);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.CreateAccommodation"/> method to ensure it throws an exception
    /// when attempting to create accommodation for a non-existent owner.
    /// </summary>
    [Fact]
    public void CreateAccommodation_ThrowsEntityNotFoundException_WhenOwnerDoesNotExist()
    {
        // Arrange: Use a non-existent owner ID
        var nonExistentOwnerId = 999;
        var accommodationName = "Mountain Retreat";
        var accommodationType = AccommodationType.Cabin;
        var accommodationAddress = "456 Mountain Road, Summit, CA";

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager
        var bookingManager = new BookingManager(logger);

        // Act & Assert: Ensure that calling CreateAccommodation throws EntityNotFoundException
        var exception = Assert.Throws<AccommodationCreationException>(
            () => bookingManager.CreateAccommodation(nonExistentOwnerId, accommodationType, accommodationName,
                                                     accommodationAddress));

        Assert.Equal("An error occurred while creating the accommodation due to missing owner.", exception.Message);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.UpdateAccommodation"/> method to ensure that it successfully updates
    /// accommodation details when valid information is provided.
    /// </summary>
    [Fact]
    public void UpdateAccommodation_UpdatesAccommodation_WhenValidDataProvided()
    {
        // Arrange: Create an accommodation and add it to the system
        var ownerId = 1;
        var accommodationType = AccommodationType.Apartment;
        var accommodationName = "Lake View Apartment";
        var accommodationAddress = "123 Lake St, Lakeside, CA";
        var accommodation = new Accommodation(ownerId, accommodationType, accommodationName, accommodationAddress);

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager and add the accommodation to the system
        var bookingManager = new BookingManager(logger);
        bookingManager.Accommodations.Add(accommodation);

        // Define new details for updating
        var newAccommodationType = AccommodationType.Cabin;
        var newAccommodationName = "Mountain Retreat";
        var newAccommodationAddress = "456 Mountain Rd, Summit, CO";

        // Act: Call UpdateAccommodation to update the accommodation details
        var result = bookingManager.UpdateAccommodation(accommodation.Id, newAccommodationType, newAccommodationName,
                                                        newAccommodationAddress);

        // Assert: Verify that the result is Success
        Assert.Equal(UpdateAccommodationResult.Success, result);

        // Assert: Verify that the accommodation details are updated
        var updatedAccommodation = bookingManager.Accommodations.FindAccommodationById(accommodation.Id);
        Assert.NotNull(updatedAccommodation);
        Assert.Equal(newAccommodationType, updatedAccommodation.Type);
        Assert.Equal(newAccommodationName, updatedAccommodation.Name);
        Assert.Equal(newAccommodationAddress, updatedAccommodation.Address);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.UpdateAccommodation"/> method to ensure it returns AccommodationNotFound
    /// when attempting to update an accommodation that does not exist.
    /// </summary>
    [Fact]
    public void UpdateAccommodation_ReturnsAccommodationNotFound_WhenAccommodationDoesNotExist()
    {
        // Arrange: Use a non-existent accommodation ID
        var nonExistentAccommodationId = 999;

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager
        var bookingManager = new BookingManager(logger);

        // Act: Call UpdateAccommodation with a non-existent accommodation ID
        var result = bookingManager.UpdateAccommodation(nonExistentAccommodationId);

        // Assert: Verify that the result is AccommodationNotFound
        Assert.Equal(UpdateAccommodationResult.AccommodationNotFound, result);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.UpdateAccommodation"/> method to ensure it returns InvalidName
    /// when an invalid accommodation name is provided.
    /// </summary>
    [Fact]
    public void UpdateAccommodation_ReturnsInvalidName_WhenInvalidNameProvided()
    {
        // Arrange: Create an accommodation and add it to the system
        var ownerId = 1;
        var accommodationType = AccommodationType.Apartment;
        var accommodationName = "Lake View Apartment";
        var accommodationAddress = "123 Lake St, Lakeside, CA";
        var accommodation = new Accommodation(ownerId, accommodationType, accommodationName, accommodationAddress);

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager and add the accommodation to the system
        var bookingManager = new BookingManager(logger);
        bookingManager.Accommodations.Add(accommodation);

        // Define an invalid name (e.g., an empty string)
        var invalidAccommodationName = "";

        // Act: Call UpdateAccommodation with an invalid accommodation name
        var result = bookingManager.UpdateAccommodation(accommodation.Id, accommodationType, invalidAccommodationName);

        // Assert: Verify that the result is InvalidName
        Assert.Equal(UpdateAccommodationResult.InvalidName, result);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.UpdateAccommodation"/> method to ensure it returns InvalidAddress
    /// when an invalid accommodation address is provided.
    /// </summary>
    [Fact]
    public void UpdateAccommodation_ReturnsInvalidAddress_WhenInvalidAddressProvided()
    {
        // Arrange: Create an accommodation and add it to the system
        var ownerId = 1;
        var accommodationType = AccommodationType.Apartment;
        var accommodationName = "Lake View Apartment";
        var accommodationAddress = "123 Lake St, Lakeside, CA";
        var accommodation = new Accommodation(ownerId, accommodationType, accommodationName, accommodationAddress);

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager and add the accommodation to the system
        var bookingManager = new BookingManager(logger);
        bookingManager.Accommodations.Add(accommodation);

        // Define an invalid address (e.g., an empty string)
        var invalidAccommodationAddress = "";

        // Act: Call UpdateAccommodation with an invalid accommodation address
        var result = bookingManager.UpdateAccommodation(accommodation.Id, accommodationType, accommodationName,
                                                        invalidAccommodationAddress);

        // Assert: Verify that the result is InvalidAddress
        Assert.Equal(UpdateAccommodationResult.InvalidAddress, result);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.RemoveAccommodation"/> method to ensure that it successfully removes an
    /// accommodation and disassociates it from the owner when valid data is provided.
    /// </summary>
    [Fact]
    public void RemoveAccommodation_SuccessfullyRemovesAccommodation_WhenAccommodationAndOwnerExist()
    {
        // Arrange: Create an accommodation and an owner, then add them to the system
        var owner = new Owner("John", "Doe", "john.doe@email.com");

        var accommodationType = AccommodationType.Apartment;
        var accommodationName = "Lake View Apartment";
        var accommodationAddress = "123 Lake St, Lakeside, CA";
        var accommodation = new Accommodation(owner.Id, accommodationType, accommodationName, accommodationAddress);

        owner.AddAccommodation(accommodation);

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager and add the accommodation and owner to the system
        var bookingManager = new BookingManager(logger);
        bookingManager.Accommodations.Add(accommodation);
        bookingManager.Owners.Add(owner);

        // Act: Call RemoveAccommodation to remove the accommodation
        var result = bookingManager.RemoveAccommodation(accommodation.Id);

        // Assert: Verify that the result is Success
        Assert.Equal(RemoveAccommodationResult.Success, result);

        // Assert: Verify that the accommodation is removed from the system
        var removedAccommodation = bookingManager.Accommodations.FindAccommodationById(accommodation.Id);
        Assert.Null(removedAccommodation);

        // Assert: Verify that the accommodation is removed from the owner's list
        var removedOwnerAccommodation = owner.AccommodationsOwned.Find(a => a.Id == accommodation.Id);
        Assert.Null(removedOwnerAccommodation);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.RemoveAccommodation"/> method to ensure it returns AccommodationNotFound
    /// when the accommodation does not exist.
    /// </summary>
    [Fact]
    public void RemoveAccommodation_ReturnsAccommodationNotFound_WhenAccommodationDoesNotExist()
    {
        // Arrange: Create an owner and add them to the system
        var owner = new Owner("John", "Doe", "john.doe@email.com");

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager
        var bookingManager = new BookingManager(logger);
        bookingManager.Owners.Add(owner);

        // Act: Call RemoveAccommodation with a non-existent accommodation ID
        var result = bookingManager.RemoveAccommodation(999);

        // Assert: Verify that the result is AccommodationNotFound
        Assert.Equal(RemoveAccommodationResult.AccommodationNotFound, result);
    }

    /// <summary>
    /// Tests the <see cref="BookingManager.RemoveAccommodation"/> method to ensure it returns
    /// AccommodationNotFound when the accommodation cannot be found in the system.
    /// </summary>
    [Fact]
    public void RemoveAccommodation_ReturnsAccommodationNotFound_WhenAccommodationCannotBeFound()
    {
        // Arrange: Create an accommodation and an owner, then add them to the system
        var owner = new Owner("John", "Doe", "john.doe@email.com");

        var accommodationType = AccommodationType.Apartment;
        var accommodationName = "Lake View Apartment";
        var accommodationAddress = "123 Lake St, Lakeside, CA";
        var accommodation = new Accommodation(owner.Id, accommodationType, accommodationName, accommodationAddress);

        owner.AddAccommodation(accommodation);

        // Create a real logger using LoggerFactory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<BookingManager>();

        // Create an instance of BookingManager and add the accommodation and owner to the system
        var bookingManager = new BookingManager(logger);
        bookingManager.Accommodations.Add(accommodation);
        bookingManager.Owners.Add(owner);

        // Act: Call RemoveAccommodation to remove accommodation from the system
        bookingManager.Accommodations.Remove(accommodation);

        // Act: Call RemoveAccommodation again after the accommodation has been removed from the system
        var result = bookingManager.RemoveAccommodation(accommodation.Id);

        // Assert: Verify that the result is AccommodationRemovalFailed
        Assert.Equal(RemoveAccommodationResult.AccommodationNotFound, result);
    }
}
}

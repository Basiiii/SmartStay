/// <copyright file="ClientsTests.cs">
/// Copyright (c) 2024 All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains unit tests for the <see cref="Clients"/> class, which manages a collection of <see
/// cref="Client"/> objects. These tests verify the functionality of methods such as adding, removing, importing,
/// exporting, and searching clients by their unique ID.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>03/12/2024</date>

/// <summary>
/// The <c>SmartStay.Core.Tests.Repositories</c> namespace contains unit tests for the repository classes that interact
/// with the application data.
/// </summary>
namespace SmartStay.Core.Tests.Repositories
{
using SmartStay.Common.Enums;
using SmartStay.Core.Models;
using SmartStay.Core.Repositories;
using SmartStay.Core.Utilities;
using Xunit;

/// <summary>
/// Contains unit tests for the <see cref="Clients"/> repository class.
/// Tests include adding, removing, importing, exporting clients, and serialization/deserialization processes.
/// </summary>
public class ClientsTests
{
    /// <summary>
    /// Tests the <see cref="Clients.Add(Client)"/> method to ensure that a client is successfully added.
    /// </summary>
    [Fact]
    public void Add_ValidClient_AddsClientSuccessfully()
    {
        // Arrange
        var clientRepo = new Clients();
        var client = new Client("John", "Doe", "johndoe@example.com");

        // Act
        var result = clientRepo.Add(client);

        // Assert
        Assert.True(result);
        Assert.Equal(1, clientRepo.CountClients());
    }

    /// <summary>
    /// Tests the <see cref="Clients.Remove(Client)"/> method to ensure that a client is successfully removed.
    /// </summary>
    [Fact]
    public void Remove_ValidClient_RemovesClientSuccessfully()
    {
        // Arrange
        var clientRepo = new Clients();
        var client = new Client("John", "Doe", "johndoe@example.com");
        clientRepo.Add(client);

        // Act
        var result = clientRepo.Remove(client);

        // Assert
        Assert.True(result);
        Assert.Equal(0, clientRepo.CountClients()); // No clients should remain
    }

    /// <summary>
    /// Tests the <see cref="Clients.Remove(Client)"/> method to ensure that attempting to remove a non-existing client
    /// returns false.
    /// </summary>
    [Fact]
    public void Remove_NonExistingClient_ReturnsFalse()
    {
        // Arrange
        var clientRepo = new Clients();
        var client = new Client("John", "Doe", "johndoe@example.com");

        // Act
        var result = clientRepo.Remove(client);

        // Assert
        Assert.False(result); // Client does not exist, should return false
    }

    /// <summary>
    /// Tests the <see cref="Clients.Import(string)"/> method to ensure that clients are imported correctly,
    /// including all fields such as ID, name, email, phone number, address, and preferred payment method.
    /// </summary>
    [Fact]
    public void Import_ValidData_ImportsClients()
    {
        // Arrange
        var clientRepo = new Clients();

        // Example JSON data representing multiple clients
        var jsonData = @"
        [
            {
                ""Id"": 1,
                ""FirstName"": ""John"",
                ""LastName"": ""Doe"",
                ""Email"": ""johndoe@example.com"",
                ""PhoneNumber"": ""+351222333444"",
                ""Address"": ""123 Main St"",
                ""PreferredPaymentMethod"": 2
            }
        ]";

        // Act
        var result = clientRepo.Import(jsonData);

        // Assert: Verify the import counts
        Assert.Equal(1, result.ImportedCount); // Only 1 client is imported
        Assert.Equal(0, result.ReplacedCount); // No existing clients should be replaced

        // Assert: Verify the number of clients in the repository
        Assert.Equal(1, clientRepo.CountClients());

        // Assert: Verify the details of the imported client
        var importedClient = clientRepo.FindClientById(1);
        Assert.NotNull(importedClient);
        Assert.Equal(1, importedClient.Id);
        Assert.Equal("John", importedClient.FirstName);
        Assert.Equal("Doe", importedClient.LastName);
        Assert.Equal("johndoe@example.com", importedClient.Email);
        Assert.Equal("+351222333444", importedClient.PhoneNumber);
        Assert.Equal("123 Main St", importedClient.Address);
        Assert.Equal(PaymentMethod.PayPal, importedClient.PreferredPaymentMethod);
    }

    /// <summary>
    /// Tests the <see cref="Clients.Export"/> method to ensure that clients can be exported correctly.
    /// </summary>
    [Fact]
    public void Export_ValidData_ExportsClients()
    {
        // Arrange
        var clientRepo = new Clients();

        // Create clients
        var client1 =
            new Client(1, "John", "Doe", "johndoe@example.com", "+351222333444", "123 Main St", PaymentMethod.PayPal);
        var client2 = new Client(2, "Jane", "Smith", "janesmith@example.com", "+351222333444", "456 Oak St",
                                 PaymentMethod.PayPal);

        clientRepo.Add(client1);
        clientRepo.Add(client2);

        // Act
        var jsonData = clientRepo.Export();

        // Assert: Verify that all clients are exported
        Assert.Contains("John", jsonData);
        Assert.Contains("Doe", jsonData);
        Assert.Contains("johndoe@example.com", jsonData);
        Assert.Contains("123 Main St", jsonData);

        Assert.Contains("Jane", jsonData);
        Assert.Contains("Smith", jsonData);
        Assert.Contains("janesmith@example.com", jsonData);
        Assert.Contains("456 Oak St", jsonData);

        // Assert: Verify that all fields in client1 are exported
        Assert.Contains("\"Id\": 1", jsonData);
        Assert.Contains("\"FirstName\": \"John\"", jsonData);
        Assert.Contains("\"LastName\": \"Doe\"", jsonData);
        Assert.Contains("\"Email\": \"johndoe@example.com\"", jsonData);
        Assert.Contains("\"PhoneNumber\": \"\\u002B351222333444\"", jsonData);
        Assert.Contains("\"Address\": \"123 Main St\"", jsonData);
        Assert.Contains("\"PreferredPaymentMethod\": 2", jsonData);

        // Assert: Verify that all fields in client2 are exported
        Assert.Contains("\"Id\": 2", jsonData);
        Assert.Contains("\"FirstName\": \"Jane\"", jsonData);
        Assert.Contains("\"LastName\": \"Smith\"", jsonData);
        Assert.Contains("\"Email\": \"janesmith@example.com\"", jsonData);
        Assert.Contains("\"PhoneNumber\": \"\\u002B351222333444\"", jsonData);
        Assert.Contains("\"Address\": \"456 Oak St\"", jsonData);
        Assert.Contains("\"PreferredPaymentMethod\": 2", jsonData); // Assuming PayPal is serialized as 2
    }

    /// <summary>
    /// Tests the <see cref="Clients.FindClientById(int)"/> method to ensure that it finds a client by their ID.
    /// </summary>
    [Fact]
    public void FindClientById_ExistingId_ReturnsClient()
    {
        // Arrange
        var clientRepo = new Clients();
        var client = new Client("John", "Doe", "johndoe@example.com");
        clientRepo.Add(client);

        // Act
        var foundClient = clientRepo.FindClientById(client.Id);

        // Assert
        Assert.NotNull(foundClient);
        Assert.Equal("John", foundClient.FirstName);
        Assert.Equal("Doe", foundClient.LastName);
    }

    /// <summary>
    /// Tests the <see cref="Clients.FindClientById(int)"/> method to ensure that it returns null when a client with the
    /// specified ID does not exist.
    /// </summary>
    [Fact]
    public void FindClientById_NonExistingId_ReturnsNull()
    {
        // Arrange
        var clientRepo = new Clients();

        // Act
        var foundClient = clientRepo.FindClientById(1); // ID does not exist

        // Assert
        Assert.Null(foundClient);
    }

    /// <summary>
    /// Tests the <see cref="Clients.Save(string)"/> method to ensure that the clients collection can be saved to a
    /// file.
    /// </summary>
    [Fact]
    public void Save_ValidData_SavesToFile()
    {
        // Arrange
        var clientRepo = new Clients();
        var client = new Client("John", "Doe", "johndoe@example.com");
        clientRepo.Add(client);
        var filePath = "clients_test.dat";

        // Act & Assert
        var exception = Record.Exception(() => clientRepo.Save(filePath));
        Assert.Null(exception); // No exceptions should occur during save
    }

    /// <summary>
    /// Tests the <see cref="Clients.Load(string)"/> method to ensure that the clients collection can be loaded from a
    /// file.
    /// </summary>
    [Fact]
    public void Load_ValidFile_LoadsClients()
    {
        // Arrange
        var clientRepo = new Clients();
        var client = new Client("John", "Doe", "johndoe@example.com");
        clientRepo.Add(client);
        var filePath = "clients_test.dat";
        clientRepo.Save(filePath); // Save before loading

        // Act
        var newRepo = new Clients();
        newRepo.Load(filePath);

        // Assert
        Assert.Equal(1, newRepo.CountClients());
    }
}
}

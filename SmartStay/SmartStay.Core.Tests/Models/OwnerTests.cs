/// <copyright file="OwnerTests.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains unit tests for the <see cref="Owner"/> class, which represents
/// accommodation owners in the SmartStay application, including their properties, methods,
/// and validation logic.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>03/12/2024</date>

/// <summary>
/// The <c>SmartStay.Core.Tests.Models</c> namespace contains unit tests for the models used in the SmartStay
/// application. These tests verify the correct behavior of the <see cref="Owner"/> class and its methods.
/// </summary>
namespace SmartStay.Core.Tests.Models
{
using Xunit;
using SmartStay.Core.Models;
using SmartStay.Validation;

/// <summary>
/// Contains unit tests for the <see cref="Owner"/> class.
/// Tests include validation, property assignments, and methods for managing accommodations.
/// </summary>
public class OwnerTests
{
    /// <summary>
    /// Tests the constructor of the <see cref="Owner"/> class with basic details to ensure
    /// proper initialization and validation.
    /// </summary>
    [Fact]
    public void Constructor_BasicDetails_InitializesOwner()
    {
        // Arrange
        var firstName = "Enrique";
        var lastName = "Rodrigues";
        var email = "enrique.rodrigues@example.com";

        // Act
        var owner = new Owner(firstName, lastName, email);

        // Assert
        Assert.Equal(firstName, owner.FirstName);
        Assert.Equal(lastName, owner.LastName);
        Assert.Equal(email, owner.Email);
        Assert.NotEqual(0, owner.Id); // ID should be auto-assigned
    }

    /// <summary>
    /// Tests the constructor of the <see cref="Owner"/> class with additional details
    /// (phone number and address) to ensure proper initialization and validation.
    /// </summary>
    [Fact]
    public void Constructor_AdditionalDetails_InitializesOwner()
    {
        // Arrange
        var firstName = "Bob";
        var lastName = "Smith";
        var email = "bob.smith@example.com";
        var phoneNumber = "+351777888999";
        var address = "123 Main Street";

        // Act
        var owner = new Owner(firstName, lastName, email, phoneNumber, address);

        // Assert
        Assert.Equal(firstName, owner.FirstName);
        Assert.Equal(lastName, owner.LastName);
        Assert.Equal(email, owner.Email);
        Assert.Equal(phoneNumber, owner.PhoneNumber);
        Assert.Equal(address, owner.Address);
    }

    /// <summary>
    /// Tests the <see cref="Owner.FirstName"/> property for proper value validation and assignment.
    /// </summary>
    [Fact]
    public void FirstName_SetAndGet_ValidatesValue()
    {
        // Arrange
        var owner = new Owner("Alice", "Johnson", "alice.johnson@example.com");

        // Act
        owner.FirstName = "Eve"; // Valid name

        // Assert
        Assert.Equal("Eve", owner.FirstName);

        // Act & Assert for invalid value
        Assert.Throws<ValidationException>(() => owner.FirstName = "");
    }

    /// <summary>
    /// Tests the <see cref="Owner.Email"/> property for proper value validation and assignment.
    /// </summary>
    [Fact]
    public void Email_SetAndGet_ValidatesValue()
    {
        // Arrange
        var owner = new Owner("Alice", "Johnson", "alice.johnson@example.com");

        // Act
        owner.Email = "eve.johnson@example.com"; // Valid email

        // Assert
        Assert.Equal("eve.johnson@example.com", owner.Email);

        // Act & Assert for invalid value
        Assert.Throws<ValidationException>(() => owner.Email = "invalid-email");
    }

    /// <summary>
    /// Tests the <see cref="Owner.PhoneNumber"/> property for proper value validation and assignment.
    /// </summary>
    [Fact]
    public void PhoneNumber_SetAndGet_ValidatesValue()
    {
        // Arrange
        var owner = new Owner("Alice", "Johnson", "alice.johnson@example.com");

        // Act
        owner.PhoneNumber = "+9876543210"; // Valid phone number

        // Assert
        Assert.Equal("+9876543210", owner.PhoneNumber);

        // Act & Assert for invalid value
        Assert.Throws<ValidationException>(() => owner.PhoneNumber = "1234");
    }

    /// <summary>
    /// Tests the <see cref="Owner.ToString()"/> method to ensure it returns a valid JSON string
    /// representation of the owner.
    /// </summary>
    [Fact]
    public void Owner_ToString_ReturnsJson()
    {
        // Arrange
        var owner = new Owner("Alice", "Johnson", "alice.johnson@example.com");

        // Act
        var jsonString = owner.ToString();

        // Assert
        Assert.Contains($"\"Id\": {owner.Id}", jsonString);
        Assert.Contains($"\"FirstName\": \"{owner.FirstName}\"", jsonString);
        Assert.Contains($"\"LastName\": \"{owner.LastName}\"", jsonString);
        Assert.Contains($"\"Email\": \"{owner.Email}\"", jsonString);
    }

    /// <summary>
    /// Tests the <see cref="Owner.AddAccommodation"/> method to ensure accommodations can
    /// be added successfully.
    /// </summary>
    [Fact]
    public void AddAccommodation_ValidAccommodation_AddsSuccessfully()
    {
        // Arrange
        var owner = new Owner("Alice", "Johnson", "alice.johnson@example.com");
        var accommodation = new Accommodation { Name = "Hotel Paradise" };

        // Act
        var result = owner.AddAccommodation(accommodation);

        // Assert
        Assert.True(result);
        Assert.Contains(accommodation, owner.AccommodationsOwned);
    }

    /// <summary>
    /// Tests the <see cref="Owner.RemoveAccommodation"/> method to ensure accommodations can
    /// be removed successfully.
    /// </summary>
    [Fact]
    public void RemoveAccommodation_ValidAccommodation_RemovesSuccessfully()
    {
        // Arrange
        var owner = new Owner("Alice", "Johnson", "alice.johnson@example.com");
        var accommodation = new Accommodation { Name = "Hotel Paradise" };
        owner.AddAccommodation(accommodation);

        // Act
        var result = owner.RemoveAccommodation(accommodation);

        // Assert
        Assert.True(result);
        Assert.DoesNotContain(accommodation, owner.AccommodationsOwned);
    }

    /// <summary>
    /// Tests the <see cref="Owner.LastAssignedId"/> property for proper tracking of the last
    /// assigned owner ID.
    /// </summary>
    [Fact]
    public void LastAssignedId_TracksCorrectly()
    {
        // Arrange
        Owner.LastAssignedId = 0; // Reset for testing
        var owner1 = new Owner("Alice", "Johnson", "alice.johnson@example.com");
        var owner2 = new Owner("Bob", "Smith", "bob.smith@example.com");

        // Act
        var lastId = Owner.LastAssignedId;

        // Assert
        Assert.Equal(owner2.Id, lastId);
    }
}
}

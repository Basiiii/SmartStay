/// <copyright file="AccommodationTests.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains unit tests for the <see cref="Accommodation"/> class, which represents accommodations,
/// including their properties, methods, and validation logic in the SmartStay application.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>03/12/2024</date>

/// <summary>
/// The <c>SmartStay.Core.Tests.Models</c> namespace contains unit tests for the models used in the SmartStay
/// application. These tests verify the correct behavior of the <see cref="Accommodation"/> class and its methods.
/// </summary>
namespace SmartStay.Core.Tests.Models
{
using SmartStay.Core.Models;
using SmartStay.Common.Enums;
using SmartStay.Validation;
using System;
using System.Collections.Generic;
using Xunit;

/// <summary>
/// Contains unit tests for the <see cref="Accommodation"/> class.
/// Tests include validation, property assignments, room management, and string representation.
/// </summary>
public class AccommodationTests
{
    /// <summary>
    /// Tests the constructor of the <see cref="Accommodation"/> class to ensure that it properly initializes an
    /// accommodation with the provided owner ID, type, name, and address.
    /// </summary>
    /// <exception cref="ValidationException">Thrown if any validation fails.</exception>
    [Fact]
    public void Constructor_ValidParameters_InitializesAccommodation()
    {
        // Arrange
        var ownerId = 1;
        var type = AccommodationType.Hotel;
        var name = "Sunset Hotel";
        var address = "123 Beach Ave, Ocean City";

        // Act
        var accommodation = new Accommodation(ownerId, type, name, address);

        // Assert
        Assert.Equal(ownerId, accommodation.OwnerId);
        Assert.Equal(type, accommodation.Type);
        Assert.Equal(name, accommodation.Name);
        Assert.Equal(address, accommodation.Address);
        Assert.Empty(accommodation.Rooms); // No rooms by default
    }

    /// <summary>
    /// Tests the constructor of the <see cref="Accommodation"/> class to ensure that it throws a ValidationException
    /// if an invalid name is provided.
    /// </summary>
    [Fact]
    public void Constructor_InvalidName_ThrowsValidationException()
    {
        // Arrange
        var ownerId = 1;
        var type = AccommodationType.Hotel;
        var name = ""; // Invalid name
        var address = "123 Beach Ave, Ocean City";

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => new Accommodation(ownerId, type, name, address));

        Assert.Equal("The provided accommodation name is invalid.", exception.Message);
    }

    /// <summary>
    /// Tests the <see cref="Accommodation.FindRoomById(int)"/> method to ensure it correctly returns the room with the
    /// specified ID.
    /// </summary>
    [Fact]
    public void FindRoomById_RoomExists_ReturnsRoom()
    {
        // Arrange
        var accommodation = new Accommodation(1, AccommodationType.Hotel, "Enrique Hotel", "123 Beach Ave");
        var room = new Room(RoomType.Single, 100.0m);
        accommodation.AddRoom(room);

        // Act
        var foundRoom = accommodation.FindRoomById(room.Id);

        // Assert
        Assert.NotNull(foundRoom);
        Assert.Equal(room.Id, foundRoom.Id);
    }

    /// <summary>
    /// Tests the <see cref="Accommodation.FindRoomById(int)"/> method to ensure it returns null
    /// when a room with the specified ID does not exist.
    /// </summary>
    [Fact]
    public void FindRoomById_RoomDoesNotExist_ReturnsNull()
    {
        // Arrange
        var accommodation = new Accommodation(1, AccommodationType.Hotel, "Sunset Hotel", "123 Beach Ave");

        // Act
        var foundRoom = accommodation.FindRoomById(101); // Room ID does not exist

        // Assert
        Assert.Null(foundRoom);
    }

    /// <summary>
    /// Tests the <see cref="Owner.ToString"/> method to ensure it returns a valid JSON string
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
    /// Tests the <see cref="Accommodation.OwnerId"/> property to ensure it correctly sets and retrieves the owner ID.
    /// </summary>
    [Fact]
    public void OwnerId_SetAndGet_CorrectlySetsOwnerId()
    {
        // Arrange
#pragma warning disable IDE0017 // Simplify object initialization
        var accommodation = new Accommodation(1, AccommodationType.Hotel, "Sunset Hotel", "123 Beach Ave");
#pragma warning restore IDE0017 // Simplify object initialization

        // Act
        accommodation.OwnerId = 2;

        // Assert
        Assert.Equal(2, accommodation.OwnerId);
    }

    /// <summary>
    /// Tests the <see cref="Accommodation.OwnerId"/> property to ensure it throws an exception when an invalid ID is
    /// set.
    /// </summary>
    [Fact]
    public void OwnerId_InvalidValue_ThrowsException()
    {
        // Arrange
        var accommodation = new Accommodation(1, AccommodationType.Hotel, "Sunset Hotel", "123 Beach Ave");

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => accommodation.OwnerId = -1);
        Assert.Equal("The provided ID is invalid.", exception.Message);
    }
}
}

/// <copyright file="OwnersTests.cs">
/// Copyright (c) 2024 All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains unit tests for the <see cref="Owners"/> class, which manages a collection of <see
/// cref="Owner"/> objects. These tests verify the functionality of methods such as adding, removing, importing,
/// exporting, and searching owners by their unique ID.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>03/12/2024</date>

/// <summary>
/// The <c>SmartStay.Core.Tests.Repositories</c> namespace contains unit tests for the repository classes that interact
/// with the application data.
/// </summary>
namespace SmartStay.Core.Tests.Repositories
{
using SmartStay.Core.Models;
using SmartStay.Core.Repositories;
using SmartStay.Core.Utilities;
using Xunit;

/// <summary>
/// Contains unit tests for the <see cref="Owners"/> repository class.
/// Tests include adding, removing, importing, exporting owners, and serialization/deserialization processes.
/// </summary>
public class OwnersTests
{
    /// <summary>
    /// Tests the <see cref="Owners.Add(Owner)"/> method to ensure that an owner is successfully added.
    /// </summary>
    [Fact]
    public void Add_ValidOwner_AddsOwnerSuccessfully()
    {
        // Arrange
        var ownerRepo = new Owners();
        var owner = new Owner("John", "Doe", "johndoe@example.com");

        // Act
        var result = ownerRepo.Add(owner);

        // Assert
        Assert.True(result);
        Assert.Equal(1, ownerRepo.CountOwners());
    }

    /// <summary>
    /// Tests the <see cref="Owners.Remove(Owner)"/> method to ensure that an owner is successfully removed.
    /// </summary>
    [Fact]
    public void Remove_ValidOwner_RemovesOwnerSuccessfully()
    {
        // Arrange
        var ownerRepo = new Owners();
        var owner = new Owner("John", "Doe", "johndoe@example.com");
        ownerRepo.Add(owner);

        // Act
        var result = ownerRepo.Remove(owner);

        // Assert
        Assert.True(result);
        Assert.Equal(0, ownerRepo.CountOwners()); // No owners should remain
    }

    /// <summary>
    /// Tests the <see cref="Owners.Remove(Owner)"/> method to ensure that attempting to remove a non-existing owner
    /// returns false.
    /// </summary>
    [Fact]
    public void Remove_NonExistingOwner_ReturnsFalse()
    {
        // Arrange
        var ownerRepo = new Owners();
        var owner = new Owner("John", "Doe", "johndoe@example.com");

        // Act
        var result = ownerRepo.Remove(owner);

        // Assert
        Assert.False(result); // Owner does not exist, should return false
    }

    /// <summary>
    /// Tests the <see cref="Owners.Import(string)"/> method to ensure that owners are imported correctly,
    /// including all fields such as ID, first name, last name, email, phone number, address, and accommodations owned.
    /// </summary>
    [Fact]
    public void Import_ValidData_ImportsOwners()
    {
        // Arrange
        var ownerRepo = new Owners();

        // Example JSON data representing multiple owners
        var jsonData = @"
        [
            {
                ""Id"": 1,
                ""FirstName"": ""Alice"",
                ""LastName"": ""Johnson"",
                ""Email"": ""alicejohnson@example.com"",
                ""PhoneNumber"": ""+351333444555"",
                ""Address"": ""789 Pine St"",
                ""AccommodationsOwned"": [
                    {
                        ""Id"": 1,
                        ""OwnerId"": 1,
                        ""Type"": 1,
                        ""Name"": ""Grand Hotel"",
                        ""Address"": ""456 Luxury Ave"",
                        ""Rooms"": [
                            {
                                ""Id"": 101,
                                ""Type"": 1,
                                ""PricePerNight"": 250.0,
                                ""ReservationDates"": [
                                    {
                                        ""Start"": ""2024-12-10T14:00:00"",
                                        ""End"": ""2024-12-15T11:00:00""
                                    }
                                ]
                            }
                        ]
                    }
                ]
            }
        ]";

        // Act
        var result = ownerRepo.Import(jsonData);

        // Assert: Verify the import counts
        Assert.Equal(1, result.ImportedCount); // Only 1 owner is imported
        Assert.Equal(0, result.ReplacedCount); // No existing owners should be replaced

        // Assert: Verify the number of owners in the repository
        Assert.Equal(1, ownerRepo.CountOwners());

        // Assert: Verify the details of the imported owner
        var importedOwner = ownerRepo.FindOwnerById(1);
        Assert.NotNull(importedOwner);
        Assert.Equal(1, importedOwner.Id);
        Assert.Equal("Alice", importedOwner.FirstName);
        Assert.Equal("Johnson", importedOwner.LastName);
        Assert.Equal("alicejohnson@example.com", importedOwner.Email);
        Assert.Equal("+351333444555", importedOwner.PhoneNumber);
        Assert.Equal("789 Pine St", importedOwner.Address);

        // Assert: Verify the accommodations owned
        Assert.NotNull(importedOwner.AccommodationsOwned);
        Assert.Single(importedOwner.AccommodationsOwned);
        Assert.Contains(importedOwner.AccommodationsOwned, a => a.Name == "Grand Hotel");
    }

    /// <summary>
    /// Tests the <see cref="Owners.Export"/> method to ensure that owners, accommodations, rooms, and reservation dates
    /// are exported correctly.
    /// </summary>
    [Fact]
    public void Export_ValidData_ExportsOwnersWithAccommodations()
    {
        // Arrange
        var ownerRepo = new Owners();

        // Create accommodations with rooms and reservation dates
        var accommodation1 = new Accommodation(
            id: 1, ownerId: 1, type: Common.Enums.AccommodationType.Hotel, name: "Grand Hotel",
            address: "456 Luxury Ave",
            rooms: new List<Room> { new Room(
                id: 101, type: Common.Enums.RoomType.Double, pricePerNight: 250.0m,
                reservationDates: new SortedSet<DateRange> { new DateRange(
                    start: new DateTime(2024, 12, 10, 14, 0, 0), end: new DateTime(2024, 12, 15, 11, 0, 0)) }) });

        var owner1 = new Owner(id: 1, firstName: "Alice", lastName: "Johnson", email: "alicejohnson@example.com",
                               phoneNumber: "+351333444555", address: "789 Pine St",
                               accommodationsOwned: new List<Accommodation> { accommodation1 });

        ownerRepo.Add(owner1);

        // Act
        var jsonData = ownerRepo.Export();

        // Assert: Verify that all owners are exported
        Assert.Contains("Alice", jsonData);
        Assert.Contains("Johnson", jsonData);
        Assert.Contains("alicejohnson@example.com", jsonData);
        Assert.Contains("789 Pine St", jsonData);

        // Assert: Verify that the accommodation details are exported
        Assert.Contains("\"Id\": 1", jsonData);                       // Accommodation Id
        Assert.Contains("\"OwnerId\": 1", jsonData);                  // OwnerId of the accommodation
        Assert.Contains("\"Type\": 1", jsonData);                     // Accommodation type
        Assert.Contains("\"Name\": \"Grand Hotel\"", jsonData);       // Accommodation name
        Assert.Contains("\"Address\": \"456 Luxury Ave\"", jsonData); // Accommodation address

        // Assert: Verify that rooms inside the accommodation are exported
        Assert.Contains("\"Id\": 101", jsonData);              // Room Id
        Assert.Contains("\"Type\": 1", jsonData);              // Room type
        Assert.Contains("\"PricePerNight\": 250.0", jsonData); // Room price
        Assert.Contains("\"ReservationDates\":", jsonData);    // ReservationDates key

        // Assert: Verify that the reservation dates are exported
        Assert.Contains("\"Start\": \"2024-12-10T14:00:00\"", jsonData); // Reservation start date
        Assert.Contains("\"End\": \"2024-12-15T11:00:00\"", jsonData);   // Reservation end date
    }

    /// <summary>
    /// Tests the <see cref="Owners.FindOwnerById(int)"/> method to ensure that it finds an owner by their ID.
    /// </summary>
    [Fact]
    public void FindOwnerById_ExistingId_ReturnsOwner()
    {
        // Arrange
        var ownerRepo = new Owners();
        var owner = new Owner("John", "Doe", "johndoe@example.com");
        ownerRepo.Add(owner);

        // Act
        var foundOwner = ownerRepo.FindOwnerById(owner.Id);

        // Assert
        Assert.NotNull(foundOwner);
        Assert.Equal("John", foundOwner.FirstName);
        Assert.Equal("Doe", foundOwner.LastName);
    }

    /// <summary>
    /// Tests the <see cref="Owners.FindOwnerById(int)"/> method to ensure that it returns null when an owner with the
    /// specified ID does not exist.
    /// </summary>
    [Fact]
    public void FindOwnerById_NonExistingId_ReturnsNull()
    {
        // Arrange
        var ownerRepo = new Owners();

        // Act
        var foundOwner = ownerRepo.FindOwnerById(1); // ID does not exist

        // Assert
        Assert.Null(foundOwner);
    }

    /// <summary>
    /// Tests the <see cref="Owners.Save(string)"/> method to ensure that the owners collection can be saved to a
    /// file.
    /// </summary>
    [Fact]
    public void Save_ValidData_SavesToFile()
    {
        // Arrange
        var ownerRepo = new Owners();
        var owner = new Owner("John", "Doe", "johndoe@example.com");
        ownerRepo.Add(owner);
        var filePath = "owners_test.dat";

        // Act & Assert
        var exception = Record.Exception(() => ownerRepo.Save(filePath));
        Assert.Null(exception); // No exceptions should occur during save
    }

    /// <summary>
    /// Tests the <see cref="Owners.Load(string)"/> method to ensure that the owners collection can be loaded from a
    /// file.
    /// </summary>
    [Fact]
    public void Load_ValidFile_LoadsOwners()
    {
        // Arrange
        var ownerRepo = new Owners();
        var owner = new Owner("John", "Doe", "johndoe@example.com");
        ownerRepo.Add(owner);
        var filePath = "owners_test.dat";
        ownerRepo.Save(filePath); // Save before loading

        // Act
        var newRepo = new Owners();
        newRepo.Load(filePath);

        // Assert
        Assert.Equal(1, newRepo.CountOwners());
    }
}
}

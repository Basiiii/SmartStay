/// <copyright file="AccommodationsTests.cs">
/// Copyright (c) 2024 All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains unit tests for the <see cref="Accommodations"/> class, which represents a collection of <see
/// cref="Accommodation"/> objects managed within the SmartStay application. These tests verify the functionality of
/// methods such as adding, removing, importing, exporting, and serializing accommodations.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>03/12/2024</date>

/// <summary>
/// The <c>SmartStay.Core.Tests.Repositories</c> namespace contains unit tests for the repository classes that interact
/// with the application data.
/// </summary>
namespace SmartStay.Core.Tests.Repositories
{
using System.Globalization;
using SmartStay.Common.Enums;
using SmartStay.Core.Models;
using SmartStay.Core.Repositories;
using SmartStay.Core.Utilities;
using Xunit;

/// <summary>
/// Contains unit tests for the <see cref="Accommodations"/> repository class.
/// Tests include adding, removing, importing, exporting accommodations, and serialization/deserialization processes.
/// </summary>
public class AccommodationsTests
{
    /// <summary>
    /// Tests the <see cref="Accommodations.Add(Accommodation)"/> method to ensure that an accommodation is successfully
    /// added.
    /// </summary>
    [Fact]
    public void Add_ValidAccommodation_AddsAccommodationSuccessfully()
    {
        // Arrange
        var accommodationRepo = new Accommodations();
        var accommodation = new Accommodation(1, AccommodationType.Hotel, "Grand Hotel", "456 Luxury Ave");

        // Act
        var result = accommodationRepo.Add(accommodation);

        // Assert
        Assert.True(result);
        Assert.Equal(1, accommodationRepo.CountAccommodations());
    }

    /// <summary>
    /// Tests the <see cref="Accommodations.Remove(Accommodation)"/> method to ensure that an accommodation is
    /// successfully removed.
    /// </summary>
    [Fact]
    public void Remove_ValidAccommodation_RemovesAccommodationSuccessfully()
    {
        // Arrange
        var accommodationRepo = new Accommodations();
        var accommodation = new Accommodation(1, AccommodationType.Hotel, "Grand Hotel", "456 Luxury Ave");
        accommodationRepo.Add(accommodation);

        // Act
        var result = accommodationRepo.Remove(accommodation);

        // Assert
        Assert.True(result);
        Assert.Equal(0, accommodationRepo.CountAccommodations()); // No accommodations should remain
    }

    /// <summary>
    /// Tests the <see cref="Accommodations.Remove(Accommodation)"/> method to ensure that attempting to remove a
    /// non-existing accommodation returns false.
    /// </summary>
    [Fact]
    public void Remove_NonExistingAccommodation_ReturnsFalse()
    {
        // Arrange
        var accommodationRepo = new Accommodations();
        var accommodation = new Accommodation(1, AccommodationType.Hotel, "Grand Hotel", "456 Luxury Ave");

        // Act
        var result = accommodationRepo.Remove(accommodation);

        // Assert
        Assert.False(result); // Accommodation does not exist, should return false
    }

    /// <summary>
    /// Tests the <see cref="Accommodations.Import(string)"/> method to ensure that accommodations are imported
    /// correctly, including all fields and room data.
    /// </summary>
    [Fact]
    public void Import_ValidData_ImportsAccommodations()
    {
        // Arrange
        var accommodationRepo = new Accommodations();

        // Example JSON data representing multiple accommodations with rooms
        var jsonData = @"
        [
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
        ]";

        // Act
        var result = accommodationRepo.Import(jsonData);

        // Assert: Verify the import counts
        Assert.Equal(1, result.ImportedCount); // Only 1 accommodation is imported
        Assert.Equal(0, result.ReplacedCount); // No existing accommodations should be replaced

        // Assert: Verify the number of accommodations in the repository
        Assert.Equal(1, accommodationRepo.CountAccommodations());

        // Assert: Verify the details of the imported accommodation
        var importedAccommodation = accommodationRepo.FindAccommodationById(1);
        Assert.NotNull(importedAccommodation);
        Assert.Equal(1, importedAccommodation.Id);
        Assert.Equal(1, importedAccommodation.OwnerId);
        Assert.Equal(AccommodationType.Hotel, importedAccommodation.Type);
        Assert.Equal("Grand Hotel", importedAccommodation.Name);
        Assert.Equal("456 Luxury Ave", importedAccommodation.Address);

        // Assert: Verify that rooms are properly imported
        Assert.Single(importedAccommodation.Rooms); // Only 1 room
        var importedRoom = importedAccommodation.Rooms[0];
        Assert.Equal(101, importedRoom.Id);
        Assert.Equal(RoomType.Single, importedRoom.Type);
        Assert.Equal(250.00m, importedRoom.PricePerNight);

        // Assert: Verify ReservationDates for the room
        Assert.Single(importedRoom.ReservationDates); // Only 1 reservation date for the room
        var reservationDate = importedRoom.ReservationDates.First();
        var format = "yyyy-MM-ddTHH:mm:ss";         // Define the expected format
        var culture = CultureInfo.InvariantCulture; // Use a culture-insensitive format provider
        Assert.Equal(DateTime.ParseExact("2024-12-10T14:00:00", format, culture), reservationDate.Start);
        Assert.Equal(DateTime.ParseExact("2024-12-15T11:00:00", format, culture), reservationDate.End);

        // Verify the total number of rooms
        Assert.Single(importedAccommodation.Rooms);
    }

    /// <summary>
    /// Tests the <see cref="Accommodations.Export"/> method to ensure that accommodations can be exported correctly.
    /// </summary>
    [Fact]
    public void Export_ValidData_ExportsAccommodations()
    {
        // Arrange
        var accommodationRepo = new Accommodations();

        // Create DateRange objects for the rooms' reservation dates
        var format = "yyyy-MM-ddTHH:mm:ss";         // Define the expected format
        var culture = CultureInfo.InvariantCulture; // Use a culture-insensitive format provider

        var reservationDate1 = new DateRange(DateTime.ParseExact("2024-12-10T14:00:00", format, culture),
                                             DateTime.ParseExact("2024-12-15T11:00:00", format, culture));

        var reservationDate2 = new DateRange(DateTime.ParseExact("2024-12-20T14:00:00", format, culture),
                                             DateTime.ParseExact("2024-12-25T11:00:00", format, culture));

        // Create rooms and add reservation dates as SortedSet<DateRange>
        var room1 = new Room(101, RoomType.Single, 250.0m, [reservationDate1]);
        var room2 = new Room(201, RoomType.Double, 350.0m, [reservationDate2]);

        // Create accommodations and add rooms
        var accommodation1 = new Accommodation(1, 1, AccommodationType.Hotel, "Grand Hotel", "456 Luxury Ave", [room1]);
        var accommodation2 =
            new Accommodation(2, 2, AccommodationType.Apartment, "Luxury Suites", "789 Elite St", [room2]);

        accommodationRepo.Add(accommodation1);
        accommodationRepo.Add(accommodation2);

        // Act
        var jsonData = accommodationRepo.Export();

        // Assert: Verify that all accommodations are exported
        Assert.Contains("Grand Hotel", jsonData);
        Assert.Contains("456 Luxury Ave", jsonData);
        Assert.Contains("Luxury Suites", jsonData);
        Assert.Contains("789 Elite St", jsonData);

        // Assert: Verify that all fields in accommodation1 are exported
        Assert.Contains("\"Id\": 1", jsonData);
        Assert.Contains("\"OwnerId\": 1", jsonData);
        Assert.Contains("\"Type\": 1", jsonData); // Ensure the type is properly serialized
        Assert.Contains("\"Name\": \"Grand Hotel\"", jsonData);
        Assert.Contains("\"Address\": \"456 Luxury Ave\"", jsonData);

        // Assert: Verify that rooms and reservation dates for accommodation1 are exported
        Assert.Contains("\"Rooms\":", jsonData);
        Assert.Contains("\"Id\": 101", jsonData);              // Room ID
        Assert.Contains("\"Type\": 1", jsonData);              // Room type (Single)
        Assert.Contains("\"PricePerNight\": 250.0", jsonData); // Room price
        Assert.Contains("\"ReservationDates\":", jsonData);    // Reservation dates array

        // Assert: Verify that the reservation dates are correctly serialized and sorted (for accommodation1)
        Assert.Contains("\"Start\": \"2024-12-10T14:00:00\"", jsonData);
        Assert.Contains("\"End\": \"2024-12-15T11:00:00\"", jsonData);

        // Assert: Verify that accommodation2 data is exported correctly
        Assert.Contains("\"Id\": 2", jsonData);
        Assert.Contains("\"OwnerId\": 2", jsonData);
        Assert.Contains("\"Type\": 2", jsonData); // Ensure the type is properly serialized
        Assert.Contains("\"Name\": \"Luxury Suites\"", jsonData);
        Assert.Contains("\"Address\": \"789 Elite St\"", jsonData);

        // Assert: Verify that rooms and reservation dates for accommodation2 are exported
        Assert.Contains("\"Id\": 201", jsonData);              // Room ID for accommodation2
        Assert.Contains("\"Type\": 2", jsonData);              // Room type (Double)
        Assert.Contains("\"PricePerNight\": 350.0", jsonData); // Room price
        Assert.Contains("\"ReservationDates\":", jsonData);    // Reservation dates array

        // Assert: Verify that the reservation dates for accommodation2 are correctly serialized and sorted
        Assert.Contains("\"Start\": \"2024-12-20T14:00:00\"", jsonData);
        Assert.Contains("\"End\": \"2024-12-25T11:00:00\"", jsonData);

        // Assert: Verify that the JSON data contains the correct number of accommodations
        Assert.Contains("[", jsonData); // Should be an array of accommodations
        Assert.Contains("]", jsonData); // Should be an array of accommodations
    }

    /// <summary>
    /// Tests the <see cref="Accommodations.FindAccommodationById(int)"/> method to ensure that it finds an
    /// accommodation by its ID.
    /// </summary>
    [Fact]
    public void FindAccommodationById_ExistingId_ReturnsAccommodation()
    {
        // Arrange
        var accommodationRepo = new Accommodations();
        var accommodation = new Accommodation(1, AccommodationType.Hotel, "Grand Hotel", "456 Luxury Ave");
        accommodationRepo.Add(accommodation);

        // Act
        var foundAccommodation = accommodationRepo.FindAccommodationById(accommodation.Id);

        // Assert
        Assert.NotNull(foundAccommodation);
        Assert.Equal("Grand Hotel", foundAccommodation.Name);
    }

    /// <summary>
    /// Tests the <see cref="Accommodations.FindAccommodationById(int)"/> method to ensure that it returns null when an
    /// accommodation with the specified ID does not exist.
    /// </summary>
    [Fact]
    public void FindAccommodationById_NonExistingId_ReturnsNull()
    {
        // Arrange
        var accommodationRepo = new Accommodations();

        // Act
        var foundAccommodation = accommodationRepo.FindAccommodationById(1); // ID does not exist

        // Assert
        Assert.Null(foundAccommodation);
    }

    /// <summary>
    /// Tests the <see cref="Accommodations.Save(string)"/> method to ensure that accommodations can be saved to a file.
    /// </summary>
    [Fact]
    public void Save_ValidData_SavesToFile()
    {
        // Arrange
        var accommodationRepo = new Accommodations();
        var accommodation = new Accommodation(1, AccommodationType.Hotel, "Grand Hotel", "456 Luxury Ave");
        accommodationRepo.Add(accommodation);
        var filePath = "accommodations_test.dat";

        // Act & Assert
        var exception = Record.Exception(() => accommodationRepo.Save(filePath));
        Assert.Null(exception); // No exceptions should occur during save
    }

    /// <summary>
    /// Tests the <see cref="Accommodations.Load(string)"/> method to ensure that accommodations can be loaded from a
    /// file.
    /// </summary>
    [Fact]
    public void Load_ValidFile_LoadsAccommodations()
    {
        // Arrange
        var accommodationRepo = new Accommodations();
        var accommodation = new Accommodation(1, AccommodationType.Hotel, "Grand Hotel", "456 Luxury Ave");
        accommodationRepo.Add(accommodation);
        var filePath = "accommodations_test.dat";
        accommodationRepo.Save(filePath); // Save before loading

        // Act
        var newRepo = new Accommodations();
        newRepo.Load(filePath);

        // Assert
        Assert.Equal(1, newRepo.CountAccommodations());
    }
}
}

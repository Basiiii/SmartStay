/// <copyright file="ReservationsTests.cs">
/// Copyright (c) 2024 All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains unit tests for the <see cref="Reservations"/> class, which manages a collection of <see
/// cref="Reservation"/> objects. These tests verify the functionality of methods such as adding, removing, importing,
/// exporting, and searching reservations by their unique ID.
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
using Xunit;
using System;
using SmartStay.Common.Enums;
using SmartStay.Validation;

/// <summary>
/// Contains unit tests for the <see cref="Reservations"/> repository class.
/// Tests include adding, removing, importing, exporting reservations, and serialization/deserialization processes.
/// </summary>
public class ReservationsTests
{
    /// <summary>
    /// Tests the <see cref="Reservations.Add(Reservation)"/> method to ensure that a reservation is successfully added.
    /// </summary>
    [Fact]
    public void Add_ValidReservation_AddsReservationSuccessfully()
    {
        // Arrange
        var reservationsRepo = new Reservations();
        var reservation =
            new Reservation(1, 1, 101, AccommodationType.Hotel, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), 100m);

        // Act
        var result = reservationsRepo.Add(reservation);

        // Assert
        Assert.True(result);
        Assert.Equal(1, reservationsRepo.CountReservations());
    }

    /// <summary>
    /// Tests the <see cref="Reservations.Add(Reservation)"/> method to ensure that attempting to add a null reservation
    /// throws an exception.
    /// </summary>
    [Fact]
    public void Add_NullReservation_ThrowsArgumentNullException()
    {
        // Arrange
        var reservationsRepo = new Reservations();

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => reservationsRepo.Add(null!));
        Assert.Equal("Reservation cannot be null (Parameter 'reservation')", exception.Message);
    }

    /// <summary>
    /// Tests the <see cref="Reservations.Remove(Reservation)"/> method to ensure that a reservation is successfully
    /// removed.
    /// </summary>
    [Fact]
    public void Remove_ValidReservation_RemovesReservationSuccessfully()
    {
        // Arrange
        var reservationsRepo = new Reservations();
        var reservation =
            new Reservation(1, 1, 101, AccommodationType.Hotel, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), 100m);
        reservationsRepo.Add(reservation);

        // Act
        var result = reservationsRepo.Remove(reservation);

        // Assert
        Assert.True(result);
        Assert.Equal(0, reservationsRepo.CountReservations()); // No reservations should remain
    }

    /// <summary>
    /// Tests the <see cref="Reservations.Remove(Reservation)"/> method to ensure that attempting to remove a
    /// non-existing reservation returns false.
    /// </summary>
    [Fact]
    public void Remove_NonExistingReservation_ReturnsFalse()
    {
        // Arrange
        var reservationsRepo = new Reservations();
        var reservation =
            new Reservation(1, 1, 101, AccommodationType.Hotel, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), 100m);

        // Act
        var result = reservationsRepo.Remove(reservation);

        // Assert
        Assert.False(result); // Reservation does not exist, should return false
    }

    /// <summary>
    /// Tests the <see cref="Reservations.Import(string)"/> method to ensure that reservations with payments are
    /// imported correctly.
    /// </summary>
    [Fact]
    public void Import_ValidData_ImportsReservationsWithPayments()
    {
        // Arrange
        var reservationRepo = new Reservations();

        // Example JSON data representing multiple reservations with payments
        var jsonData = @"
        [
            {
                ""Id"": 1,
                ""ClientId"": 101,
                ""AccommodationId"": 202,
                ""RoomId"": 303,
                ""AccommodationType"": 1,
                ""CheckInDate"": ""2024-12-10T14:00:00"",
                ""CheckOutDate"": ""2024-12-15T11:00:00"",
                ""Status"": 1,
                ""TotalCost"": 1250.00,
                ""AmountPaid"": 500.00,
                ""Payments"": [
                    {
                        ""Id"": 1,
                        ""ReservationId"": 1,
                        ""Amount"": 500.00,
                        ""Date"": ""2024-12-01T10:00:00"",
                        ""Method"": 2,
                        ""Status"": 2
                    }
                ]
            }
        ]";

        // Act
        var result = reservationRepo.Import(jsonData);

        // Assert: Verify the import counts
        Assert.Equal(1, result.ImportedCount); // Only 1 reservation is imported
        Assert.Equal(0, result.ReplacedCount); // No existing reservations should be replaced

        // Assert: Verify the number of reservations in the repository
        Assert.Equal(1, reservationRepo.CountReservations());

        // Assert: Verify the details of the imported reservation
        var importedReservation = reservationRepo.FindReservationById(1);
        Assert.NotNull(importedReservation);
        Assert.Equal(1, importedReservation.Id);
        Assert.Equal(101, importedReservation.ClientId);
        Assert.Equal(202, importedReservation.AccommodationId);
        Assert.Equal(303, importedReservation.RoomId);
        Assert.Equal(AccommodationType.Hotel, importedReservation.AccommodationType); // Assuming 1 maps to "Hotel"
        Assert.Equal(new DateTime(2024, 12, 10, 14, 0, 0), importedReservation.CheckInDate);
        Assert.Equal(new DateTime(2024, 12, 15, 11, 0, 0), importedReservation.CheckOutDate);
        Assert.Equal(ReservationStatus.CheckedIn, importedReservation.Status);
        Assert.Equal(1250.00m, importedReservation.TotalCost);
        Assert.Equal(500.00m, importedReservation.AmountPaid);

        // Assert: Verify the payment details
        Assert.NotNull(importedReservation.Payments);
        Assert.Single(importedReservation.Payments);
        var payment = importedReservation.Payments.First();
        Assert.Equal(1, payment.Id);
        Assert.Equal(1, payment.ReservationId);
        Assert.Equal(500.00m, payment.Amount);
        Assert.Equal(new DateTime(2024, 12, 1, 10, 0, 0), payment.Date);
        Assert.Equal(PaymentMethod.PayPal, payment.Method);
        Assert.Equal(PaymentStatus.Completed, payment.Status);
    }

    /// <summary>
    /// Tests the <see cref="Reservations.Export"/> method to ensure that reservations with payments are exported
    /// correctly.
    /// </summary>
    [Fact]
    public void Export_ValidData_ExportsReservationsWithPayments()
    {
        // Arrange
        var reservationRepo = new Reservations();

        // Create reservation with a payment
        var payment1 = new Payment(1, 500.0m, new DateTime(2024, 12, 1, 10, 0, 0), PaymentMethod.BankTransfer,
                                   PaymentStatus.Completed);

        var reservation1 = new Reservation(
            id: 1, clientId: 101, accommodationId: 202, roomId: 303, accommodationType: AccommodationType.Hotel,
            checkInDate: new DateTime(2024, 12, 10, 14, 0, 0), checkOutDate: new DateTime(2024, 12, 15, 11, 0, 0),
            status: ReservationStatus.CheckedIn, totalCost: 1250.0m, amountPaid: 500.0m,
            payments: new List<Payment> { payment1 });

        reservationRepo.Add(reservation1);

        // Act
        var jsonData = reservationRepo.Export();

        // Assert: Verify that all reservation fields are exported
        Assert.Contains("\"Id\": 1", jsonData);
        Assert.Contains("\"ClientId\": 101", jsonData);
        Assert.Contains("\"AccommodationId\": 202", jsonData);
        Assert.Contains("\"RoomId\": 303", jsonData);
        Assert.Contains("\"AccommodationType\": 1", jsonData); // Assuming 1 is "Hotel"
        Assert.Contains("\"CheckInDate\": \"2024-12-10T14:00:00\"", jsonData);
        Assert.Contains("\"CheckOutDate\": \"2024-12-15T11:00:00\"", jsonData);
        Assert.Contains("\"Status\": 2", jsonData); // Assuming 2 is "CheckedIn"
        Assert.Contains("\"TotalCost\": 1250.0", jsonData);
        Assert.Contains("\"AmountPaid\": 500.0", jsonData);

        // Assert: Verify that the payments are exported correctly
        Assert.Contains("\"Payments\":", jsonData);
        Assert.Contains("\"Id\": 1", jsonData);
        Assert.Contains("\"ReservationId\": 1", jsonData);
        Assert.Contains("\"Amount\": 500.0", jsonData);
        Assert.Contains("\"Date\": \"2024-12-01T10:00:00\"", jsonData);
        Assert.Contains("\"Method\": 4", jsonData);
        Assert.Contains("\"Status\": 2", jsonData);
    }

    /// <summary>
    /// Tests the <see cref="Reservations.FindReservationById(int)"/> method to ensure that it finds a reservation by
    /// its ID.
    /// </summary>
    [Fact]
    public void FindReservationById_ExistingId_ReturnsReservation()
    {
        // Arrange
        var reservationsRepo = new Reservations();
        var reservation =
            new Reservation(1, 1, 101, AccommodationType.Hotel, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), 100m);
        reservationsRepo.Add(reservation);

        // Act
        var foundReservation = reservationsRepo.FindReservationById(reservation.Id);

        // Assert
        Assert.NotNull(foundReservation);
        Assert.Equal(1, foundReservation.ClientId);
        Assert.Equal(1, foundReservation.AccommodationId);
    }

    /// <summary>
    /// Tests the <see cref="Reservations.FindReservationById(int)"/> method to ensure that it returns null when a
    /// reservation with the specified ID does not exist.
    /// </summary>
    [Fact]
    public void FindReservationById_NonExistingId_ReturnsNull()
    {
        // Arrange
        var reservationsRepo = new Reservations();

        // Act
        var foundReservation = reservationsRepo.FindReservationById(1); // ID does not exist

        // Assert
        Assert.Null(foundReservation);
    }

    /// <summary>
    /// Tests the <see cref="Reservations.Save(string)"/> method to ensure that the reservations collection can be saved
    /// to a file.
    /// </summary>
    [Fact]
    public void Save_ValidData_SavesToFile()
    {
        // Arrange
        var reservationsRepo = new Reservations();
        var reservation =
            new Reservation(1, 1, 101, AccommodationType.Hotel, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), 100m);
        reservationsRepo.Add(reservation);
        var filePath = "reservations_test.dat";

        // Act & Assert
        var exception = Record.Exception(() => reservationsRepo.Save(filePath));
        Assert.Null(exception); // No exceptions should occur during save
    }

    /// <summary>
    /// Tests the <see cref="Reservations.Load(string)"/> method to ensure that the reservations collection can be
    /// loaded from a file.
    /// </summary>
    [Fact]
    public void Load_ValidFile_LoadsReservations()
    {
        // Arrange
        var reservationsRepo = new Reservations();
        var reservation =
            new Reservation(1, 1, 101, AccommodationType.Hotel, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), 100m);
        reservationsRepo.Add(reservation);
        var filePath = "reservations_test.dat";
        reservationsRepo.Save(filePath); // Save before loading

        // Act
        var newRepo = new Reservations();
        newRepo.Load(filePath);

        // Assert
        Assert.Equal(1, newRepo.CountReservations());
    }

    /// <summary>
    /// Tests the <see cref="Reservations.Add(Reservation)"/> method to ensure that adding a reservation with invalid
    /// total cost throws an exception.
    /// </summary>
    [Fact]
    public void Add_InvalidTotalCost_ThrowsValidationException()
    {
        // Arrange & Act & Assert
        var exception = Assert.Throws<ValidationException>(() => new Reservation(1, 1, 101, AccommodationType.Hotel,
                                                                                 DateTime.Now.AddDays(1),
                                                                                 DateTime.Now.AddDays(2), -100m));
        Assert.Equal(ValidationErrorCode.InvalidTotalCost, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the <see cref="Reservations.Add(Reservation)"/> method to ensure that adding a reservation with invalid
    /// dates throws an exception.
    /// </summary>
    [Fact]
    public void Add_InvalidDateRange_ThrowsValidationException()
    {
        // Arrange & Act & Assert
        var exception = Assert.Throws<ValidationException>(() => new Reservation(1, 1, 101, AccommodationType.Hotel,
                                                                                 DateTime.Now.AddDays(3),
                                                                                 DateTime.Now.AddDays(2), 100m));
        Assert.Equal(ValidationErrorCode.InvalidDateRange, exception.ErrorCode);
    }
}
}

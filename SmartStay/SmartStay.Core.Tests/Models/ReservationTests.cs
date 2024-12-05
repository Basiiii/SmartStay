/// <copyright file="ReservationTests.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains unit tests for the <see cref="Reservation"/> class, which stores reservation data in the
/// SmartStay application, including client IDs, accommodation types, check-in/check-out dates, payment statuses,
/// and more. These tests verify the correct functionality and validation of the class methods.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>03/12/2024</date>

/// <summary>
/// The <c>SmartStay.Core.Tests.Models</c> namespace contains unit tests for the models used in the SmartStay
/// application.
/// </summary>
namespace SmartStay.Core.Tests.Models
{
using SmartStay.Core.Models;
using SmartStay.Common.Enums;
using System;
using Xunit;

/// <summary>
/// Contains unit tests for the <see cref="Reservation"/> class.
/// Tests include validation, property assignments, payment methods, and string representation.
/// </summary>
public class ReservationTests
{
    /// <summary>
    /// Tests the constructor of the <see cref="Reservation"/> class to ensure that it properly initializes a
    /// reservation with valid parameters.
    /// </summary>
    [Fact]
    public void Constructor_ValidParameters_InitializesReservation()
    {
        // Arrange
        var clientId = 1;
        var accommodationId = 2;
        var roomId = 3;
        var accommodationType = AccommodationType.Hotel;
        var checkInDate = new DateTime(2024, 12, 1);
        var checkOutDate = new DateTime(2024, 12, 5);
        var totalCost = 500.0m;

        // Act
        var reservation =
            new Reservation(clientId, accommodationId, roomId, accommodationType, checkInDate, checkOutDate, totalCost);

        // Assert
        Assert.Equal(clientId, reservation.ClientId);
        Assert.Equal(accommodationId, reservation.AccommodationId);
        Assert.Equal(roomId, reservation.RoomId);
        Assert.Equal(accommodationType, reservation.AccommodationType);
        Assert.Equal(checkInDate, reservation.CheckInDate);
        Assert.Equal(checkOutDate, reservation.CheckOutDate);
        Assert.Equal(totalCost, reservation.TotalCost);
        Assert.Equal(ReservationStatus.Pending, reservation.Status); // Default status
    }

    /// <summary>
    /// Tests the <see cref="Reservation.ToString"/> method to ensure it returns a valid JSON representation.
    /// </summary>
    [Fact]
    public void ToString_ReturnsValidJson()
    {
        // Arrange
        var reservation = new Reservation(1, 2, 3, AccommodationType.Hotel, new DateTime(2024, 12, 1),
                                          new DateTime(2024, 12, 5), 500.0m);

        // Act
        var jsonString = reservation.ToString();

        // Assert
        Assert.Contains($"\"ClientId\": {reservation.ClientId}", jsonString);
        Assert.Contains($"\"AccommodationId\": {reservation.AccommodationId}", jsonString);
        Assert.Contains($"\"RoomId\": {reservation.RoomId}", jsonString);
        Assert.Contains($"\"AccommodationType\": \"{reservation.AccommodationType}\"", jsonString);
        Assert.Contains($"\"CheckInDate\": \"{reservation.CheckInDate:yyyy-MM-ddTHH:mm:ss}\"", jsonString);
        Assert.Contains($"\"CheckOutDate\": \"{reservation.CheckOutDate:yyyy-MM-ddTHH:mm:ss}\"", jsonString);
        Assert.Contains($"\"TotalCost\": {reservation.TotalCost}", jsonString);
        Assert.Contains($"\"Status\": \"{reservation.Status}\"", jsonString);
    }

    /// <summary>
    /// Tests the <see cref="Reservation.CheckIn"/> method to ensure that the reservation is correctly marked
    /// as CheckedIn when the status is Pending.
    /// </summary>
    [Fact]
    public void CheckIn_StatusPending_ChangesStatusToCheckedIn()
    {
        // Arrange
        var reservation = new Reservation(1, 2, 3, AccommodationType.Hotel, new DateTime(2024, 12, 1),
                                          new DateTime(2024, 12, 5), 500.0m);

        // Act
        var result = reservation.CheckIn();

        // Assert
        Assert.True(result);
        Assert.Equal(ReservationStatus.CheckedIn, reservation.Status);
    }

    /// <summary>
    /// Tests the <see cref="Reservation.CheckIn"/> method to ensure that the reservation does not change status
    /// if it is not Pending.
    /// </summary>
    [Fact]
    public void CheckIn_StatusNotPending_ReturnsFalse()
    {
        // Arrange
        var reservation = new Reservation(1, 2, 3, AccommodationType.Hotel, new DateTime(2024, 12, 1),
                                          new DateTime(2024, 12, 5), 500.0m);
        reservation.Status = ReservationStatus.CheckedOut;

        // Act
        var result = reservation.CheckIn();

        // Assert
        Assert.False(result);
        Assert.Equal(ReservationStatus.CheckedOut, reservation.Status);
    }

    /// <summary>
    /// Tests the <see cref="Reservation.CheckOut"/> method to ensure that the reservation is correctly marked
    /// as CheckedOut when the status is CheckedIn.
    /// </summary>
    [Fact]
    public void CheckOut_StatusCheckedIn_ChangesStatusToCheckedOut()
    {
        // Arrange
        var reservation = new Reservation(1, 2, 3, AccommodationType.Hotel, new DateTime(2024, 12, 1),
                                          new DateTime(2024, 12, 5), 500.0m);
        reservation.Status = ReservationStatus.CheckedIn;

        // Act
        var result = reservation.CheckOut();

        // Assert
        Assert.True(result);
        Assert.Equal(ReservationStatus.CheckedOut, reservation.Status);
    }

    /// <summary>
    /// Tests the <see cref="Reservation.CheckOut"/> method to ensure that the reservation does not change status
    /// if it is not CheckedIn.
    /// </summary>
    [Fact]
    public void CheckOut_StatusNotCheckedIn_ReturnsFalse()
    {
        // Arrange
        var reservation = new Reservation(1, 2, 3, AccommodationType.Hotel, new DateTime(2024, 12, 1),
                                          new DateTime(2024, 12, 5), 500.0m);
        reservation.Status = ReservationStatus.Pending;

        // Act
        var result = reservation.CheckOut();

        // Assert
        Assert.False(result);
        Assert.Equal(ReservationStatus.Pending, reservation.Status);
    }

    /// <summary>
    /// Tests the <see cref="Reservation.MakePayment"/> method to ensure that it correctly processes payments
    /// and updates the amount paid.
    /// </summary>
    [Fact]
    public void MakePayment_ValidPayment_UpdatesAmountPaid()
    {
        // Arrange
        var reservation = new Reservation(1, 2, 3, AccommodationType.Hotel, new DateTime(2024, 12, 1),
                                          new DateTime(2024, 12, 5), 500.0m);

        // Act
        var paymentResult = reservation.MakePayment(200.0m, PaymentMethod.BankTransfer);

        // Assert
        Assert.Equal(PaymentResult.Success, paymentResult);
        Assert.Equal(200.0m, reservation.AmountPaid);
    }

    /// <summary>
    /// Tests the <see cref="Reservation.IsFullyPaid"/> method to ensure it correctly identifies whether
    /// the reservation has been fully paid.
    /// </summary>
    [Fact]
    public void IsFullyPaid_FullyPaid_ReturnsTrue()
    {
        // Arrange
        var reservation = new Reservation(1, 2, 3, AccommodationType.Hotel, new DateTime(2024, 12, 1),
                                          new DateTime(2024, 12, 5), 500.0m);
        reservation.AmountPaid = 500.0m;

        // Act
        var result = reservation.IsFullyPaid();

        // Assert
        Assert.True(result);
    }
}
}

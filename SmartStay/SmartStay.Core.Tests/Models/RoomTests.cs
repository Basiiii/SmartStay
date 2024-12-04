/// <copyright file="RoomTests.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains unit tests for the <see cref="Room"/> class, which represents an individual room within an
/// accommodation. These tests verify the correct functionality of the class methods, including room initialization,
/// availability checks, reservation management, price calculation, and string representation.
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
/// Contains unit tests for the <see cref="Room"/> class.
/// Tests include validation, property assignments, reservation management, cost calculation, and string representation.
/// </summary>
public class RoomTests
{
    /// <summary>
    /// Tests the constructor of the <see cref="Room"/> class to ensure that it properly initializes a
    /// room with valid parameters.
    /// </summary>
    [Fact]
    public void Constructor_ValidParameters_InitializesRoom()
    {
        // Arrange
        var roomType = RoomType.Single;
        var pricePerNight = 100.0m;

        // Act
        var room = new Room(roomType, pricePerNight);

        // Assert
        Assert.Equal(roomType, room.Type);
        Assert.Equal(pricePerNight, room.PricePerNight);
        Assert.True(room.Id > 0); // ID should be positive and unique
    }

    /// <summary>
    /// Tests the <see cref="Room.ToString"/> method to ensure it returns a valid JSON representation.
    /// </summary>
    [Fact]
    public void ToString_ReturnsValidJson()
    {
        // Arrange
        var room = new Room(RoomType.Single, 100.0m);

        // Act
        var jsonString = room.ToString();

        // Assert
        Assert.Contains($"\"Id\": {room.Id}", jsonString);
        Assert.Contains($"\"Type\": \"{room.Type}\"", jsonString);
        Assert.Contains($"\"PricePerNight\": {room.PricePerNight}", jsonString);
    }

    /// <summary>
    /// Tests the <see cref="Room.IsAvailable"/> method to ensure it correctly checks availability
    /// for a room when the given date range is not overlapping with existing reservations.
    /// </summary>
    [Fact]
    public void IsAvailable_NoOverlap_ReturnsTrue()
    {
        // Arrange
        var room = new Room(RoomType.Single, 100.0m);
        var startDate = new DateTime(2024, 12, 1);
        var endDate = new DateTime(2024, 12, 5);
        room.AddReservation(startDate, endDate); // Add reservation to this room

        // Act
        var isAvailable = room.IsAvailable(new DateTime(2024, 12, 6), new DateTime(2024, 12, 10));

        // Assert
        Assert.True(isAvailable);
    }

    /// <summary>
    /// Tests the <see cref="Room.IsAvailable"/> method to ensure it correctly detects overlap with existing
    /// reservations.
    /// </summary>
    [Fact]
    public void IsAvailable_Overlap_ReturnsFalse()
    {
        // Arrange
        var room = new Room(RoomType.Single, 100.0m);
        var existingStartDate = new DateTime(2024, 12, 1);
        var existingEndDate = new DateTime(2024, 12, 5);
        room.AddReservation(existingStartDate, existingEndDate); // Add reservation to this room

        // Act
        var isAvailable = room.IsAvailable(new DateTime(2024, 12, 4), new DateTime(2024, 12, 6));

        // Assert
        Assert.False(isAvailable);
    }

    /// <summary>
    /// Tests the <see cref="Room.AddReservation"/> method to ensure a reservation can be added successfully.
    /// </summary>
    [Fact]
    public void AddReservation_ValidDates_AddsReservation()
    {
        // Arrange
        var room = new Room(RoomType.Single, 100.0m);
        var startDate = new DateTime(2024, 12, 1);
        var endDate = new DateTime(2024, 12, 5);

        // Act
        var result = room.AddReservation(startDate, endDate);

        // Assert
        Assert.True(result);
        Assert.Single(room.ReservationDates);
    }

    /// <summary>
    /// Tests the <see cref="Room.RemoveReservation"/> method to ensure a reservation can be removed successfully.
    /// </summary>
    [Fact]
    public void RemoveReservation_ExistingReservation_RemovesReservation()
    {
        // Arrange
        var room = new Room(RoomType.Single, 100.0m);
        var startDate = new DateTime(2024, 12, 1);
        var endDate = new DateTime(2024, 12, 5);
        room.AddReservation(startDate, endDate); // Add reservation to this room

        // Act
        var result = room.RemoveReservation(startDate, endDate);

        // Assert
        Assert.True(result);
        Assert.Empty(room.ReservationDates);
    }

    /// <summary>
    /// Tests the <see cref="Room.CalculateTotalCost"/> method to ensure it calculates the correct total cost for a
    /// stay.
    /// </summary>
    [Fact]
    public void CalculateTotalCost_ValidDates_ReturnsCorrectCost()
    {
        // Arrange
        var room = new Room(RoomType.Single, 100.0m);
        var startDate = new DateTime(2024, 12, 1);
        var endDate = new DateTime(2024, 12, 5);

        // Act
        var totalCost = room.CalculateTotalCost(startDate, endDate);

        // Assert
        Assert.Equal(400.0m, totalCost); // 4 nights at 100.0m per night
    }

    /// <summary>
    /// Tests the <see cref="Room.CalculateTotalCost"/> method to ensure it throws an exception when the end date is
    /// before the start date.
    /// </summary>
    [Fact]
    public void CalculateTotalCost_EndDateBeforeStartDate_ThrowsArgumentException()
    {
        // Arrange
        var room = new Room(RoomType.Single, 100.0m);
        var startDate = new DateTime(2024, 12, 5);
        var endDate = new DateTime(2024, 12, 1);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => room.CalculateTotalCost(startDate, endDate));
    }
}
}

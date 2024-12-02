/// <copyright file="Room.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the <see cref="Room"/> class, which represents an individual room within an accommodation.
/// It stores information such as room type, nightly price, and reservation details.
/// This class also provides methods to calculate the total cost for a stay and manage room availability.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>10/11/2024</date>
using System.Text.Json;
using ProtoBuf;
using SmartStay.Common.Enums;
using SmartStay.Core.Utilities;
using SmartStay.Validation;
using SmartStay.Validation.Validators;

/// <summary>
/// The <c>SmartStay.Core.Models</c> namespace contains the primary data models used within the SmartStay application.
/// These models represent core entities and structures essential for managing application data.
/// </summary>
namespace SmartStay.Core.Models
{
/// <summary>
/// Defines the <see cref="Room"/> class, which encapsulates the details of a room within an accommodation,
/// such as its type, price per night, and reservation details. This class provides methods for updating
/// availability and calculating the total cost for a stay.
/// </summary>
[ProtoContract]
public class Room
{
    static int _lastRoomId = 0;                                                          // Last assigned room ID
    static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true }; // JSON Serializer options

    [ProtoMember(1)]
    readonly int _id; // ID of the room

    [ProtoMember(2)]
    RoomType _type; // Type of room (Single, Double, Suite, etc.)

    [ProtoMember(3)]
    decimal _pricePerNight; // Price per night for the room

    [ProtoMember(4)]
    readonly SortedSet<DateRange> _reservationDates = new(); // Sorted set for efficient availability check

    /// <summary>
    /// Initializes a new instance of the <see cref="Room"/> class.
    /// <para>This constructor is required for Protobuf-net serialization/deserialization.</para>
    /// <para>It should **not** be used directly in normal application code. Instead, use the constructor with
    /// parameters for creating instances of <see cref="Room"/>.</para>
    /// </summary>
#pragma warning disable CS8618
    public Room()
#pragma warning restore CS8618
    {
        // This constructor is intentionally empty and only needed for Protobuf-net deserialization.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Room"/> class with the specified details: type and price per night.
    /// </summary>
    /// <param name="type">The type of the room (e.g., Single, Double).</param>
    /// <param name="pricePerNight">The nightly price of the room.</param>
    /// <exception cref="ValidationException">Thrown if the room type is invalid.</exception>
    /// <exception cref="ValidationException">Thrown if the price per night is invalid.</exception>
    public Room(RoomType type, decimal pricePerNight)
    {
        RoomValidator.ValidateRoomType(type);
        PaymentValidator.ValidatePrice(pricePerNight);

        _id = GenerateRoomId();
        _type = type;
        _pricePerNight = pricePerNight;
    }

    /// <summary>
    /// Public getter for the room ID.
    /// </summary>
    public int Id => _id;

    /// <summary>
    /// Public getter and setter for the Type.
    /// </summary>
    public RoomType Type
    {
        get => _type;
        set => _type = RoomValidator.ValidateRoomType(value);
    }

    /// <summary>
    /// Public getter and setter for the PricePerNight.
    /// </summary>
    public decimal PricePerNight
    {
        get => _pricePerNight;
        set => _pricePerNight = PaymentValidator.ValidatePrice(value);
    }

    /// <summary>
    /// Public getter for the ReservationDates DateRange as a readonly collection.
    /// </summary>
    public IReadOnlyCollection<DateRange> ReservationDates
    {
        get => _reservationDates.ToList().AsReadOnly();
    }

    /// <summary>
    /// Checks if a given date range is available for a new reservation, ensuring there are no overlaps with existing
    /// reservations.
    /// </summary>
    /// <param name="startDate">The start date of the new reservation.</param>
    /// <param name="endDate">The end date of the new reservation.</param>
    /// <param name="existingReservationRange">
    /// Optional parameter representing an existing reservation that can be ignored during the availability check,
    /// used for modifying reservations.
    /// </param>
    /// <returns>
    /// Returns <c>true</c> if the accommodation is available during the specified date range; otherwise, returns
    /// <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="endDate"/> is less than or equal to <paramref
    /// name="startDate"/>.</exception> <remarks> This method uses a <see cref="SortedSet{T}"/> to efficiently find
    /// potential conflicting reservations by leveraging the <c>GetViewBetween</c> method, which narrows down the search
    /// space to reservations potentially overlapping with the requested dates. Overlapping reservations are identified
    /// based on whether the requested range intersects with any existing reservation.
    /// </remarks>
    public bool IsAvailable(DateTime startDate, DateTime endDate, DateRange? existingReservationRange = null)
    {
        if (endDate <= startDate)
            throw new ArgumentException("End date must be after the start date.");

        var newReservation = new DateRange(startDate, endDate);

        // Get potential conflicting reservations within the requested range
        var potentialConflicts = _reservationDates.GetViewBetween(
            new DateRange(DateTime.MinValue, startDate), // All reservations that end before the start
            new DateRange(DateTime.MaxValue, endDate)    // All reservations that start after the end
        );

        // Check if there are any overlapping reservations
        foreach (var existingReservation in potentialConflicts)
        {
            // Skip the existing reservation if it's the one we're trying to modify
            if (existingReservation.Equals(existingReservationRange))
            {
                continue; // Skip this reservation as it's the one we're modifying
            }

            // An overlap occurs if the start date is before the end date, and the end date is after the start date
            if ((newReservation.Start < existingReservation.End) && (newReservation.End > existingReservation.Start))
            {
                return false; // There's an overlap, so the accommodation is not available
            }
        }

        return true; // No overlap found, accommodation is available
    }

    /// <summary>
    /// Adds a new reservation to the accommodation.
    /// </summary>
    /// <param name="startDate">The start date of the reservation.</param>
    /// <param name="endDate">The end date of the reservation.</param>
    /// <returns>
    /// Returns <c>true</c> if the reservation was successfully added. If the date range is unavailable (overlap),
    /// returns <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method adds the reservation to a <see cref="SortedSet{T}"/> that maintains ordered reservations by date
    /// range. If the date range overlaps with an existing reservation, the method will return <c>false</c>.
    /// </remarks>
    public bool AddReservation(DateTime startDate, DateTime endDate)
    {
        var newReservation = new DateRange(startDate, endDate);

        // Attempt to add the new reservation
        bool addedSuccessfully = _reservationDates.Add(newReservation);

        // Return the result of the Add operation (true if added, false if it already exists due to overlap)
        return addedSuccessfully;
    }

    /// <summary>
    /// Removes an existing reservation from the accommodation.
    /// </summary>
    /// <param name="startDate">The start date of the reservation to be removed.</param>
    /// <param name="endDate">The end date of the reservation to be removed.</param>
    /// <returns>
    /// Returns <c>true</c> if the reservation was successfully removed; otherwise, returns <c>false</c> if the
    /// specified reservation was not found.
    /// </returns>
    /// <remarks>
    /// This method uses the <see cref="SortedSet{T}.Remove"/> method to delete a specific reservation by matching its
    /// <see cref="DateRange"/>. It ensures efficient removal operations due to the underlying data structure.
    /// </remarks>
    public bool RemoveReservation(DateTime startDate, DateTime endDate)
    {
        // Create the date range object to remove
        var reservationToRemove = new DateRange(startDate, endDate);

        // Remove the reservation from the SortedSet
        bool removed = _reservationDates.Remove(reservationToRemove);

        // Return whether the reservation was successfully removed
        return removed;
    }

    /// <summary>
    /// Calculates the total cost for a given stay duration.
    /// </summary>
    /// <param name="startDate">The start date of the stay.</param>
    /// <param name="endDate">The end date of the stay.</param>
    /// <returns>The total cost for the stay based on the price per night.</returns>
    /// <exception cref="ArgumentException">Thrown when the end date is before the start date.</exception>
    public decimal CalculateTotalCost(DateTime startDate, DateTime endDate)
    {
        if (endDate <= startDate)
        {
            throw new ArgumentException("End date must be after the start date.");
        }

        int nights = (endDate - startDate).Days;
        return nights * _pricePerNight;
    }

    /// <summary>
    /// Generates a unique room ID in a thread-safe manner using Interlocked.Increment.
    /// </summary>
    /// <returns>A unique room ID.</returns>
    private static int GenerateRoomId()
    {
        if (_lastRoomId >= int.MaxValue)
            throw new InvalidOperationException("Room ID limit exceeded.");

        return Interlocked.Increment(ref _lastRoomId);
    }

    /// <summary>
    /// Overridden ToString method to provide room information in a readable JSON format.
    /// </summary>
    /// <returns>A JSON string representation of the room object.</returns>
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, _jsonOptions);
    }
}
}

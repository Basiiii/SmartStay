/// <copyright file="Accommodation.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the <see cref="Accommodation"/> class, which stores information about individual accommodations,
/// including their type, name, address, price per night, and availability status. This class also provides
/// methods to calculate the total cost based on stay duration and manage availability status.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>10/11/2024</date>
using System.Text.Json;
using System.Text.Json.Serialization;
using SmartStay.Common.Enums;
using SmartStay.Core.Repositories;
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
/// Defines the <see cref="Accommodation"/> class, which encapsulates the details of an accommodation,
/// such as its type, name, address, nightly price, and availability status.
/// This class provides methods to update availability and calculate total cost.
/// </summary>
[JsonConverter(typeof(AccommodationConverter))]
public class Accommodation
{
    static int _lastAccommodationId = 0; // Last assigned accommodation ID
    static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true }; // JSON Serializer options

    readonly int _id;        // ID of the accommodation
    AccommodationType _type; // Type of accommodation (Hotel, House, etc.)
    string _name;            // Name of the accommodation
    string _address;         // Address of the accommodation
    decimal _pricePerNight;  // Price per night for the accommodation
    readonly SortedSet<DateRange> _reservationDates =
        new SortedSet<DateRange>(); // Sorted set for efficient availability check

    /// <summary>
    /// Initializes a new instance of the <see cref="Accommodation"/> class with the specified details: type, name,
    /// address, and price per night.
    /// </summary>
    /// <param name="type">The type of the accommodation (e.g., Hotel, House).</param>
    /// <param name="name">The name of the accommodation.</param>
    /// <param name="address">The address of the accommodation.</param>
    /// <param name="pricePerNight">The nightly price of the accommodation.</param>
    /// <exception cref="ValidationException">Thrown if any of the provided parameters fail validation:</exception>
    /// <exception cref="ValidationException">Thrown if the accommodation type is invalid.</exception>
    /// <exception cref="ValidationException">Thrown if the accommodation name is invalid.</exception>
    /// <exception cref="ValidationException">Thrown if the address is invalid.</exception>
    /// <exception cref="ValidationException">Thrown if the price per night is invalid.</exception>
    /// <remarks>
    /// The constructor validates the provided parameters using the <see cref="Validator"/> class before initializing
    /// the properties. If any validation fails, a <see cref="ValidationException"/> is thrown with the appropriate
    /// error code.
    /// </remarks>
    public Accommodation(AccommodationType type, string name, string address, decimal pricePerNight)
    {
        AccommodationValidator.ValidateAccommodationType(type);
        NameValidator.ValidateAccommodationName(name);
        AddressValidator.ValidateAddress(address);
        PaymentValidator.ValidatePrice(pricePerNight);

        _id = GenerateAccommodationId();
        _type = type;
        _name = name;
        _address = address;
        _pricePerNight = pricePerNight;
    }

    /// <summary>
    /// Public getter for the accommodation ID.
    /// </summary>
    public int Id => _id;

    /// <summary>
    /// Public getter and setter for the Type.
    /// </summary>
    public AccommodationType Type
    {
        get => _type;
        set => _type = AccommodationValidator.ValidateAccommodationType(value);
    }

    /// <summary>
    /// Public getter and setter for the Name.
    /// </summary>
    public string Name
    {
        get => _name;
        set => _name = NameValidator.ValidateAccommodationName(value);
    }

    /// <summary>
    /// Public getter and setter for the Address.
    /// </summary>
    public string Address
    {
        get => _address;
        set => _address = AddressValidator.ValidateAddress(value);
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
    /// Public getter for the ReservationDates DateRange as readonly collection.
    /// </summary>
    public IReadOnlyCollection<DateRange> ReservationDates
    {
        get {
            return _reservationDates.ToList().AsReadOnly();
        }
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
    /// Adds a new reservation to the accommodation, with an optional ability to skip the availability check for faster
    /// bulk operations.
    /// </summary>
    /// <param name="startDate">The start date of the reservation.</param>
    /// <param name="endDate">The end date of the reservation.</param>
    /// <param name="skipAvailabilityCheck">
    /// A boolean flag indicating whether to skip the availability check. Set to <c>true</c> during bulk operations or
    /// trusted inputs where availability is pre-validated.
    /// </param>
    /// <returns>
    /// Returns <c>true</c> if the reservation was successfully added. If <paramref name="skipAvailabilityCheck"/> is
    /// <c>false</c> and the date range is unavailable, returns <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method adds the reservation to a <see cref="SortedSet{T}"/> that maintains ordered reservations by date
    /// range. Skipping the availability check can improve performance significantly during bulk operations but should
    /// only be used with pre-validated or trusted data.
    /// </remarks>
    public bool AddReservation(DateTime startDate, DateTime endDate, bool skipAvailabilityCheck = false)
    {
        // If not skipping the availability check, validate the dates
        if (!skipAvailabilityCheck && !IsAvailable(startDate, endDate))
        {
            return false; // If not available, return false
        }

        // Add the new reservation's date range to the SortedSet
        _reservationDates.Add(new DateRange(startDate, endDate));
        return true; // Successfully added the reservation
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
    /// Generates a unique accommodation ID in a thread-safe manner using Interlocked.Increment.
    /// </summary>
    /// <returns>A unique accommodation ID.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the max limit of int is reached.</exception>
    private static int GenerateAccommodationId()
    {
        // Check if the current value exceeds the max limit of int (2,147,483,647)
        if (_lastAccommodationId >= int.MaxValue)
        {
            throw new InvalidOperationException("Accommodation ID limit exceeded.");
        }

        return Interlocked.Increment(ref _lastAccommodationId);
    }

    /// <summary>
    /// Overridden ToString method to provide accommodation information in a readable JSON format.
    /// </summary>
    /// <returns>A JSON string representation of the accommodation object.</returns>
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, _jsonOptions);
    }
}
}

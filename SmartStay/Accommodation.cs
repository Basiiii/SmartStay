/// <copyright file="Accommodation.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the definition of the Accommodation class used in the SmartStay application.
/// </file>
/// <summary>
/// Represents the <see cref="Accommodation"/> class, which stores information about individual accommodations,
/// including their type, name, address, price per night, and availability status. This class also provides
/// methods to calculate the total cost based on stay duration and manage availability status.
/// </summary>
/// <author>Enrique Rodrigues</author>
/// <date>10/11/2024</date>
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SmartStay
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

    readonly int _id;                                                  // ID of the accommodation
    AccommodationType _type;                                           // Type of accommodation (Hotel, House, etc.)
    string _name;                                                      // Name of the accommodation
    string _address;                                                   // Address of the accommodation
    decimal _pricePerNight;                                            // Price per night for the accommodation
    readonly List<(DateTime Start, DateTime End)> _reservedDates = []; // Booked date ranges

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
        if (!Validator.IsValidAccommodationType(type))
            throw new ValidationException(ValidationErrorCode.InvalidAccommodationType);
        if (!Validator.IsValidAccommodationName(name))
            throw new ValidationException(ValidationErrorCode.InvalidAccommodationName);
        if (!Validator.IsValidAddress(address))
            throw new ValidationException(ValidationErrorCode.InvalidAddress);
        if (!Validator.IsValidPrice(pricePerNight))
            throw new ValidationException(ValidationErrorCode.InvalidPrice);

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
        set => _type = Validator.ValidateAccommodationType(value);
    }

    /// <summary>
    /// Public getter and setter for the Name.
    /// </summary>
    public string Name
    {
        get => _name;
        set => _name = Validator.ValidateAccommodationName(value);
    }

    /// <summary>
    /// Public getter and setter for the Address.
    /// </summary>
    public string Address
    {
        get => _address;
        set => _address = Validator.ValidateAddress(value);
    }

    /// <summary>
    /// Public getter and setter for the PricePerNight.
    /// </summary>
    public decimal PricePerNight
    {
        get => _pricePerNight;
        set => _pricePerNight = Validator.ValidatePrice(value);
    }

    /// <summary>
    /// Public getter for a read-only list of reserved dates.
    /// </summary>
    public IReadOnlyList<(DateTime Start, DateTime End)> ReservedDates => _reservedDates.AsReadOnly();

    /// <summary>
    /// Checks if the accommodation is available for the specified date range.
    /// </summary>
    /// <param name="startDate">The start date of the requested booking period.</param>
    /// <param name="endDate">The end date of the requested booking period.</param>
    /// <returns>True if the accommodation is available for the entire date range; otherwise, false.</returns>
    /// <exception cref="ArgumentException">Thrown if the end date is not after the start date.</exception>
    public bool IsAvailable(DateTime startDate, DateTime endDate)
    {
        if (endDate <= startDate)
            throw new ArgumentException("End date must be after the start date.");

        // Perform binary search to find the nearest potential conflict
        int index = _reservedDates.BinarySearch((startDate, endDate), new DateRangeComparer());
        if (index < 0)
            index = ~index; // If not found, BinarySearch returns bitwise complement of the insertion index

        // Check previous and next entries for potential overlaps
        if (index > 0 && _reservedDates[index - 1].End > startDate)
        {
            return false; // Overlaps with previous booking
        }
        if (index < _reservedDates.Count && _reservedDates[index].Start < endDate)
        {
            return false; // Overlaps with next booking
        }

        return true; // No overlap found, accommodation is available
    }

    /// <summary>
    /// Attempts to add a reservation for the specified date range if the accommodation is available.
    /// </summary>
    /// <param name="startDate">The start date of the booking.</param>
    /// <param name="endDate">The end date of the booking.</param>
    /// <returns>True if the reservation was successfully added; otherwise, false.</returns>
    /// <remarks>
    /// This method first checks if the accommodation is available for the given date range using
    /// <see cref="IsAvailable"/>. If available, it inserts the booking in the sorted list at
    /// the correct position to maintain the list's order.
    /// </remarks>
    public bool AddReservation(DateTime startDate, DateTime endDate)
    {
        if (!IsAvailable(startDate, endDate))
        {
            return false; // Not available, booking cannot be added
        }

        // Find the correct position to insert the new booking using binary search
        int index = _reservedDates.BinarySearch((startDate, endDate), new DateRangeComparer());
        if (index < 0)
            index = ~index; // If not found, BinarySearch returns the bitwise complement of the index

        _reservedDates.Insert(index, (startDate, endDate)); // Insert at the correct position
        return true;                                        // Booking added successfully
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

        return System.Threading.Interlocked.Increment(ref _lastAccommodationId);
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

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
using SmartStay.Common.Enums;
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
public class Accommodation
{
    static int _lastAccommodationId = 0; // Last assigned accommodation ID
    static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true }; // JSON Serializer options

    readonly int _id;                        // ID of the accommodation
    int _ownerId;                            // ID of the owner
    AccommodationType _type;                 // Type of accommodation (Hotel, House, etc.)
    string _name;                            // Name of the accommodation
    string _address;                         // Address of the accommodation
    List<Room> _rooms { get; set; } = new(); // List of rooms

    /// <summary>
    /// Initializes a new instance of the <see cref="Accommodation"/> class with the specified details: type, name,
    /// address, and price per night.
    /// </summary>
    /// <param name="ownerId">The ID of the owner of the accommodation.</param>
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
    public Accommodation(int ownerId, AccommodationType type, string name, string address)
    {
        AccommodationValidator.ValidateAccommodationType(type);
        NameValidator.ValidateAccommodationName(name);
        AddressValidator.ValidateAddress(address);

        _id = GenerateAccommodationId();
        _ownerId = ownerId;
        _type = type;
        _name = name;
        _address = address;
    }

    /// <summary>
    /// Public getter for the accommodation ID.
    /// </summary>
    public int Id => _id;

    /// <summary>
    /// Public getter and setter for the Owner ID.
    /// </summary>
    public int OwnerId
    {
        get => _ownerId;
        set => _ownerId = OwnerValidator.ValidateOwnerId(value);
    }

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
    /// Public getter for the list of rooms in the accommodation.
    /// </summary>
    public List<Room> Rooms => _rooms;

    /// <summary>
    /// Gets the total number of rooms in the accommodation.
    /// </summary>
    public int TotalRooms => Rooms.Count;

    /// <summary>
    /// Finds and returns a room from the accommodation by its room ID.
    /// </summary>
    /// <param name="roomId">The ID of the room to find.</param>
    /// <returns>The room with the specified ID, or null if not found.</returns>
    public Room? FindRoomById(int roomId)
    {
        return Rooms.Find(room => room.Id == roomId);
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
        // Create a dictionary for the properties you want to serialize
        var accommodationData = new { Id = _id, Type = _type.ToString(), Name = _name, Address = _address,
                                      Rooms = _rooms.Select(room => new {
                                          room.Id,
                                          room.PricePerNight,
                                          room.Type,
                                      }) };

        // Serialize the dictionary into a JSON string, which will include Rooms as an array
        return JsonSerializer.Serialize(accommodationData, _jsonOptions);
    }
}
}

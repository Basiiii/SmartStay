/// <copyright file="Reservations.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="Reservations"/> class, which manages a collection of <see
/// cref="Reservation"/> objects. The class allows for adding, removing, importing, exporting, and searching
/// reservations by their unique ID.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>11/11/2024</date>
#nullable enable
using System.Runtime.Serialization;
using ProtoBuf;
using SmartStay.Common.Models;
using SmartStay.Core.Models;
using SmartStay.Core.Models.Interfaces;
using SmartStay.Core.Utilities;

/// <summary>
/// The <c>SmartStay.Repositories</c> namespace provides data access layers for retrieving and storing application data.
/// It contains repositories that manage database interactions for various entities within the SmartStay application.
/// </summary>
namespace SmartStay.Core.Repositories
{
/// <summary>
/// Represents a collection of <see cref="Reservation"/> objects, managed in a dictionary for fast lookup by reservation
/// ID.
/// </summary>
[ProtoContract]
public class Reservations : IManageableEntity<Reservation>
{
    /// <summary>
    /// Internal dictionary to store reservations by their unique ID.
    /// </summary>
    readonly Dictionary<int, Reservation> _reservationDictionary = new Dictionary<int, Reservation>();

    /// <summary>
    /// A temporary list used for serialization by Protobuf. This list holds the reservations
    /// that are copied from the dictionary during serialization. Protobuf-Net does not serialize
    /// dictionaries directly, so the dictionary is temporarily copied to this list before serialization.
    /// This list is cleared and rebuilt after deserialization from the binary data.
    /// </summary>
    [ProtoMember(1)] // Serialize the list of accommodations
    List<Reservation> _reservationList = new();

    /// <summary>
    /// Attempts to add a new reservation to the collection.
    /// </summary>
    /// <param name="reservation">The <see cref="Reservation"/> to add to the collection.</param>
    /// <returns>
    /// <c>true</c> if the reservation was successfully added to the collection;
    /// <c>false</c> if a reservation with the same ID already exists in the collection.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="reservation"/> is <c>null</c>.
    /// </exception>
    public bool Add(Reservation reservation)
    {
        if (reservation == null)
        {
            throw new ArgumentNullException(nameof(reservation), "Reservation cannot be null");
        }

        if (_reservationDictionary.ContainsKey(reservation.Id))
        {
            return false; // Reservation already exists
        }

        _reservationDictionary[reservation.Id] = reservation;
        return true; // Reservation added successfully
    }

    /// <summary>
    /// Removes a reservation from the collection.
    /// </summary>
    /// <param name="reservation">The <see cref="Reservation"/> to remove from the collection.</param>
    /// <returns>
    /// <c>true</c> if the reservation was successfully removed from the collection;
    /// <c>false</c> if the reservation was not found.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="reservation"/> is <c>null</c>.</exception>
    public bool Remove(Reservation reservation)
    {
        if (reservation == null)
        {
            throw new ArgumentNullException(nameof(reservation), "Reservation cannot be null");
        }

        // Remove the reservation using its ID
        return _reservationDictionary.Remove(reservation.Id);
    }

    /// <summary>
    /// Imports reservations from a JSON string into the collection, replacing any existing reservations with the same
    /// ID.
    /// </summary>
    /// <param name="data">The JSON string containing the list of reservations.</param>
    /// <returns>
    /// An <see cref="ImportResult"/> summarizing the outcome of the import operation.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if the data is null or empty.</exception>
    /// <exception cref="ArgumentException">Thrown if deserialization of the data fails.</exception>
    public ImportResult Import(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            throw new ArgumentException("Data cannot be null or empty", nameof(data));
        }

        // Deserialize the data into a List<Reservation> instead of a single Reservation
        var reservations = JsonHelper.DeserializeFromJson<Reservation>(data) ??
                           throw new ArgumentException("Deserialized reservation data cannot be null", nameof(data));

        int replacedCount = 0;
        int importedCount = 0;

        foreach (var reservation in reservations)
        {
            if (_reservationDictionary.ContainsKey(reservation.Id))
            {
                replacedCount++;
            }
            else
            {
                importedCount++;
            }
            _reservationDictionary[reservation.Id] = reservation; // Direct insertion for efficiency
        }

        return new ImportResult { ImportedCount = importedCount, ReplacedCount = replacedCount };
    }

    /// <summary>
    /// Exports the current list of reservations to a JSON string.
    /// </summary>
    /// <returns>A JSON string representation of the reservations in the collection.</returns>
    public string Export()
    {
        return JsonHelper.SerializeToJson<Reservation>(_reservationDictionary.Values);
    }

    /// <summary>
    /// Prepares the object for serialization by copying all reservations
    /// from the dictionary to the temporary list. This is necessary because
    /// Protobuf-Net serializes the list and not the dictionary directly.
    /// </summary>
    [ProtoBeforeSerialization]
    private void PrepareForSerialization()
    {
        // Clear the temporary list to ensure no leftover data
        _reservationList.Clear();

        // Add all reservations from the dictionary to the temporary list
        foreach (var reservation in _reservationDictionary.Values)
        {
            _reservationList.Add(reservation);
        }
    }

    /// <summary>
    /// Rebuilds the dictionary from the list of reservations after deserialization.
    /// This is necessary because Protobuf-Net deserializes the list and not the dictionary.
    /// </summary>
    [ProtoAfterDeserialization]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members",
                                                     Justification =
                                                         "IDE Error, this is called automatically by protobuf-net.")]
    private void AfterDeserialization()
    {
        // Clear the dictionary before rebuilding
        _reservationDictionary.Clear();

        // Rebuild the dictionary using the data from the list
        foreach (var reservation in _reservationList)
        {
            _reservationDictionary[reservation.Id] = reservation;
        }

        // Clear the temporary list once the dictionary is rebuilt
        _reservationList.Clear();

        // Set _lastReservationId to the highest ID in the deserialized data
        if (_reservationDictionary.Count > 0)
        {
            // Find the highest ID from the loaded reservations
            Reservation.LastAssignedId = _reservationDictionary.Values.Max(r => r.Id);
        }
        else
        {
            // If no reservations, reset to 0
            Reservation.LastAssignedId = 0;
        }
    }

    /// <summary>
    /// Saves the current state of the reservations collection to a file by serializing
    /// the object into a Protobuf format. If an error occurs during the saving process,
    /// it will be caught and logged.
    /// </summary>
    /// <param name="filePath">The path of the file to save the data.</param>
    /// <exception cref="IOException">Thrown when an I/O error occurs while saving the data.</exception>
    /// <exception cref="SerializationException">Thrown when a serialization error occurs while saving the
    /// data.</exception>
    public void Save(string filePath)
    {
        try
        {
            // Prepare for serialization by copying the dictionary contents to the temporary list
            PrepareForSerialization();

            // Open the file stream for saving the data to the specified file
            using (var fileStream = File.Create(filePath))
            {
                // Serialize the reservations object and write it to the file stream
                Serializer.Serialize(fileStream, this);
            }
        }
        catch (IOException ioEx)
        {
            throw new IOException("An error occurred while saving the reservations data.", ioEx);
        }
        catch (SerializationException serEx)
        {
            throw new SerializationException(
                "An error occurred during serialization while saving the reservations data.", serEx);
        }
    }

    /// <summary>
    /// Loads the collection from a binary file and assigns it to the current instance.
    /// If an error occurs during the loading process, it will be caught and logged.
    /// </summary>
    /// <param name="filePath">The file path to load the collection from.</param>
    /// <exception cref="IOException">Thrown when an I/O error occurs while loading the data.</exception>
    /// <exception cref="SerializationException">Thrown when a deserialization error occurs while loading the
    /// data.</exception>
    public void Load(string filePath)
    {
        try
        {
            // Open the file stream for reading
            using (var fileStream = File.OpenRead(filePath))
            {
                // Deserialize the reservations object from the file
                var reservations = Serializer.Deserialize<Reservations>(fileStream);

                // Clear the current dictionary
                _reservationDictionary.Clear();

                // Reset LastAssignedId to ensure consistency
                Reservation.LastAssignedId = 0;

                // Iterate over the deserialized data and copy it
                foreach (var reservation in reservations._reservationDictionary)
                {
                    _reservationDictionary[reservation.Key] = reservation.Value;

                    // Update LastAssignedId to the maximum ID found so far
                    Reservation.LastAssignedId = Math.Max(Reservation.LastAssignedId, reservation.Value.Id);
                }
            }
        }
        catch (IOException ioEx)
        {
            throw new IOException("An error occurred while loading the reservations data.", ioEx);
        }
        catch (SerializationException serEx)
        {
            throw new SerializationException(
                "An error occurred during deserialization while loading the reservations data.", serEx);
        }
    }

    /// <summary>
    /// Finds a reservation by its unique ID.
    /// </summary>
    /// <param name="reservationId">The unique ID of the reservation to find.</param>
    /// <returns>
    /// Returns the <see cref="Reservation"/> object if found; otherwise, <c>null</c>.
    /// </returns>
    public Reservation? FindReservationById(int reservationId)
    {
      _reservationDictionary.TryGetValue(reservationId, out Reservation? reservation);
      return reservation;
    }

    /// <summary>
    /// Finds all reservations associated with a client by their unique client ID.
    /// </summary>
    /// <param name="clientId">The unique ID of the client whose reservations to find.</param>
    /// <returns>A list of <see cref="Reservation"/> objects for the given client.</returns>
    public IEnumerable<Reservation> FindReservationsByClientId(int clientId)
    {
        return _reservationDictionary.Values.Where(r => r.ClientId == clientId);
    }

    /// <summary>
    /// Finds all reservations associated with an accommodation by its unique accommodation ID.
    /// </summary>
    /// <param name="accommodationId">The unique ID of the accommodation whose reservations to find.</param>
    /// <returns>
    /// A list of <see cref="Reservation"/> objects for the given accommodation. Returns an empty list if no
    /// reservations are found.
    /// </returns>
    public IEnumerable<Reservation> FindReservationsByAccommodationId(int accommodationId)
    {
        return _reservationDictionary.Values.Where(r => r.AccommodationId == accommodationId);
    }

    /// <summary>
    /// Retrieves all reservations for a given accommodation, with check-in dates after the current time.
    /// </summary>
    /// <param name="accommodationId">The accommodation ID to filter by.</param>
    /// <returns>A list of future reservations for the given accommodation.</returns>
    public IEnumerable<Reservation> GetFutureReservations(int accommodationId)
    {
        // Use LINQ to filter the dictionary's values directly without copying to a list.
        return _reservationDictionary.Values
            .Where(reservation =>
                       reservation.AccommodationId == accommodationId && reservation.CheckInDate >= DateTime.Now)
            .ToList();
    }

    /// <summary>
    /// Counts the number of reservations in the collection.
    /// </summary>
    /// <returns>
    /// The number of reservations in the collection.
    /// </returns>
    public int CountReservations()
    {
        return _reservationDictionary.Count;
    }
}
}

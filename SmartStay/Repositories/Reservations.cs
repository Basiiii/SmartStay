/// <copyright file="Reservations.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="Reservations"/> class, which manages a collection of <see
/// cref="Reservation"/> objects. The class allows for adding, removing, importing, exporting, and searching
/// reservations by their unique ID.
/// </file>
/// <summary>
/// Defines the <see cref="Reservations"/> class, which represents a collection of <see cref="Reservation"/> objects.
/// The reservations are stored in an internal dictionary for fast lookup by reservation ID. This class implements the
/// <see cref="IManageableEntity{Reservation}"/> interface, providing a standardized approach for managing reservations
/// within the SmartStay application, including functionalities for adding, removing, importing, and exporting
/// reservations.
/// </summary>
/// <author>Enrique Rodrigues</author>
/// <date>11/11/2024</date>
/// <remarks>
/// This class is designed to facilitate the management of reservation data, with fast access to reservations by their
/// ID. It also provides import and export capabilities to handle serialized data, allowing for easy integration with
/// other systems. The class is intended to be flexible for future changes in data formats (e.g., switching to XML or
/// other serialization formats).
/// </remarks>
using SmartStay.Models;
using SmartStay.Models.Interfaces;
using SmartStay.Utilities;

namespace SmartStay.Repositories
{
/// <summary>
/// Represents a collection of <see cref="Reservation"/> objects, managed in a dictionary for fast lookup by reservation
/// ID.
/// </summary>
public class Reservations : IManageableEntity<Reservation>
{
    /// <summary>
    /// Internal dictionary to store reservations by their unique ID.
    /// </summary>
    readonly Dictionary<int, Reservation> _reservationDictionary = [];

    /// <summary>
    /// Attempts to add a new reservation to the collection.
    /// </summary>
    /// <param name="reservation">The <see cref="Reservation"/> to add to the collection.</param>
    /// <returns>
    /// <c>true</c> if the reservation was successfully added to the collection;
    /// <c>false</c> if a reservation with the same ID already exists in the collection.
    /// </returns>
    public bool Add(Reservation reservation)
    {
        if (reservation == null)
        {
            throw new ArgumentNullException(nameof(reservation), "Reservation cannot be null");
        }

        if (_reservationDictionary.ContainsKey(reservation.ReservationId))
        {
            return false; // Reservation already exists
        }

        _reservationDictionary[reservation.ReservationId] = reservation;
        return true; // Reservation added successfully
    }

    /// <summary>
    /// Removes a reservation from the collection by the reservation object.
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
        return _reservationDictionary.Remove(reservation.ReservationId);
    }

    /// <summary>
    /// Imports reservations from a JSON string into the collection.
    /// </summary>
    /// <param name="data">The JSON string containing the list of reservations.</param>
    /// <exception cref="ArgumentException">Thrown if the data is null or empty.</exception>
    public void Import(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            throw new ArgumentException("Data cannot be null or empty", nameof(data));
        }

        var reservations = JsonHelper.DeserializeFromJson<Reservation>(data) ??
                           throw new ArgumentException("Deserialized reservation data cannot be null", nameof(data));

        foreach (var reservation in reservations)
        {
            _reservationDictionary[reservation.ReservationId] = reservation;
        }
    }

    /// <summary>
    /// Exports the current list of reservations to a JSON string.
    /// </summary>
    /// <returns>A JSON string representation of the reservations in the collection.</returns>
    public string Export()
    {
        return JsonHelper.SerializeToJson(_reservationDictionary.Values);
    }

    /// <summary>
    /// Finds a reservation by its unique ID.
    /// </summary>
    /// <param name="reservationId">The unique ID of the reservation to find.</param>
    /// <returns>Returns the <see cref="Reservation"/> object if found; otherwise, <c>null</c>.</returns>
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
    /// <returns>A list of <see cref="Reservation"/> objects for the given accommodation.</returns>
    public IEnumerable<Reservation> FindReservationsByAccommodationId(int accommodationId)
    {
        return _reservationDictionary.Values.Where(r => r.AccommodationId == accommodationId);
    }

    /// <summary>
    /// Retrieves all the reservations in the collection.
    /// </summary>
    /// <returns>
    /// A read-only collection of <see cref="Reservation"/> objects.
    /// </returns>
    public IReadOnlyCollection<Reservation> GetAllReservations()
    {
        return _reservationDictionary.Values.ToList(); // Returns a copy of the reservation collection.
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

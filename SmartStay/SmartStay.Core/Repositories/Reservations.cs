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
using SmartStay.Core.Models;
using SmartStay.Core.Models.Interfaces;
using SmartStay.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

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
public class Reservations : IManageableEntity<Reservation>
{
    private static readonly Dictionary<int, Reservation> value = new Dictionary<int, Reservation>();

    /// <summary>
    /// Internal dictionary to store reservations by their unique ID.
    /// </summary>
    readonly Dictionary<int, Reservation> _reservationDictionary = value;

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

        if (_reservationDictionary.ContainsKey(reservation.ReservationId))
        {
            return false; // Reservation already exists
        }

        _reservationDictionary[reservation.ReservationId] = reservation;
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
        return _reservationDictionary.Remove(reservation.ReservationId);
    }

    /// <summary>
    /// Imports reservations from a JSON string into the collection, replacing any existing reservations with the same
    /// ID.
    /// </summary>
    /// <param name="data">The JSON string containing the list of reservations.</param>
    /// <exception cref="ArgumentException">Thrown if the data is null or empty.</exception>
    /// <exception cref="ArgumentException">Thrown if deserialization of the data fails.</exception>
    public void Import(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            throw new ArgumentException("Data cannot be null or empty", nameof(data));
        }

        var reservations = JsonHelper.DeserializeFromJson<Reservation>(data) ??
                           throw new ArgumentException("Deserialized reservation data cannot be null", nameof(data));

        foreach (Reservation reservation in reservations)
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
        return JsonHelper.SerializeToJson<Reservation>(_reservationDictionary.Values);
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
    /// Retrieves all the reservations in the collection.
    /// </summary>
    /// <returns>
    /// A read-only collection of <see cref="Reservation"/> objects.
    /// </returns>
    /// <remarks>
    /// Returns a copy of the internal dictionary's values as a list to prevent external modification.
    /// </remarks>
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

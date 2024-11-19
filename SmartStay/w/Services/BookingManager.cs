/// <copyright file="BookingManager.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="BookingManager"/> class,
/// which manages client, reservation, and accommodation operations for the booking system.
/// It provides static methods to add, remove, import, and export clients, reservations, and accommodations.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>11/11/2024</date>
#nullable enable
using System.Collections.Generic;
using System.IO;
using Core.Models;
using Core.Repositories;

/// <summary>
/// The <c>Core.Services</c> namespace contains service classes that implement business logic for the SmartStay
/// application. These services coordinate actions between repositories and models to fulfill application requirements.
/// </summary>
namespace Core.Services
{
/// <summary>
/// Provides a static facade for managing clients, reservations, and accommodations in the booking system.
/// This class centralizes all operations for adding, removing, importing, and exporting data for these entities.
/// It interacts with internal repositories to simplify the main API and ensure a standardized approach.
/// </summary>
/// <remarks>
/// This class offers a unified interface for handling key booking operations and data entities, facilitating
/// integrations with other system components or external applications.
/// </remarks>
public static class BookingManager
{
#region Collections

    /// <summary>
    /// Holds the collection of all clients in the system, stored in the <see cref="Clients"/> repository.
    /// </summary>
    internal static readonly Clients _clients = new();

    /// <summary>
    /// Holds the collection of all reservations in the system, stored in the <see cref="Reservations"/> repository.
    /// </summary>
    internal static readonly Reservations _reservations = new();

    /// <summary>
    /// Holds the collection of all accommodations in the system, stored in the <see cref="Accommodations"/> repository.
    /// </summary>
    internal static readonly Accommodations _accommodations = new();

#endregion

#region Client Operations

    /// <summary>
    /// Adds a new client to the system.
    /// </summary>
    /// <param name="client">The <see cref="Client"/> object to add.</param>
    /// <returns>
    /// <c>true</c> if the client was successfully added; <c>false</c> if a client with the same ID already exists.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="client"/> is <c>null</c>.
    /// </exception>
    public static bool AddClient(Client client) => _clients.Add(client);

    /// <summary>
    /// Removes an existing client from the system.
    /// </summary>
    /// <param name="client">The <see cref="Client"/> object to remove.</param>
    /// <returns>
    /// <c>true</c> if the client was successfully removed; <c>false</c> if the client was not found.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="client"/> is <c>null</c>.
    /// </exception>
    public static bool RemoveClient(Client client) => _clients.Remove(client);

    /// <summary>
    /// Imports a list of clients from a JSON string.
    /// </summary>
    /// <param name="data">A JSON string containing client data to import.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="data"/> is <c>null</c> or an empty string.
    /// </exception>
    public static void ImportClients(string data) => _clients.Import(data);

    /// <summary>
    /// Exports the list of all clients to a specified file as a JSON string.
    /// </summary>
    /// <param name="filePath">The file path where the client data will be saved.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="filePath"/> is <c>null</c> or an empty string.
    /// </exception>
    /// <exception cref="IOException">
    /// Thrown when an I/O error occurs while writing to <paramref name="filePath"/>.
    /// </exception>
    public static void ExportClients(string filePath) => File.WriteAllText(filePath, _clients.Export());

    /// <summary>
    /// Retrieves all clients in the system.
    /// </summary>
    /// <returns>A read-only collection of <see cref="Client"/> objects.</returns>
    public static IReadOnlyCollection<Client> GetAllClients() => _clients.GetAllClients();

    /// <summary>
    /// Finds a client by their unique ID.
    /// </summary>
    /// <param name="id">The unique ID of the client to find.</param>
    /// <returns>
    /// The <see cref="Client"/> object if found; otherwise, <c>null</c>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="id"/> is less than zero.
    /// </exception>
    public static Client? FindClientById(int id) => _clients.FindClientById(id);

    /// <summary>
    /// Counts the total number of clients in the system.
    /// </summary>
    /// <returns>The number of clients.</returns>
    public static int CountClients() => _clients.CountClients();

#endregion

#region Reservation Operations

    /// <summary>
    /// Adds a new reservation to the system.
    /// </summary>
    /// <param name="reservation">The <see cref="Reservation"/> object to add.</param>
    /// <returns>
    /// <c>true</c> if the reservation was successfully added; <c>false</c> if a reservation with the same ID exists.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="reservation"/> is <c>null</c>.
    /// </exception>
    public static bool AddReservation(Reservation reservation) => _reservations.Add(reservation);

    /// <summary>
    /// Removes an existing reservation from the system.
    /// </summary>
    /// <param name="reservation">The <see cref="Reservation"/> object to remove.</param>
    /// <returns>
    /// <c>true</c> if the reservation was removed; <c>false</c> if the reservation was not found.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="reservation"/> is <c>null</c>.
    /// </exception>
    public static bool RemoveReservation(Reservation reservation) => _reservations.Remove(reservation);

    /// <summary>
    /// Imports reservations from a JSON string.
    /// </summary>
    /// <param name="data">A JSON string containing reservation data to import.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="data"/> is <c>null</c> or an empty string.
    /// </exception>
    public static void ImportReservations(string data) => _reservations.Import(data);

    /// <summary>
    /// Exports all reservations to a specified file as a JSON string.
    /// </summary>
    /// <param name="filePath">The file path where reservation data will be saved.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="filePath"/> is <c>null</c> or an empty string.
    /// </exception>
    /// <exception cref="IOException">
    /// Thrown when an I/O error occurs while writing to <paramref name="filePath"/>.
    /// </exception>
    public static void ExportReservations(string filePath) => File.WriteAllText(filePath, _reservations.Export());

    /// <summary>
    /// Retrieves all reservations in the system.
    /// </summary>
    /// <returns>A read-only collection of <see cref="Reservation"/> objects.</returns>
    public static IReadOnlyCollection<Reservation> GetAllReservations() => _reservations.GetAllReservations();

    /// <summary>
    /// Finds a reservation by its unique ID.
    /// </summary>
    /// <param name="id">The unique ID of the reservation to find.</param>
    /// <returns>
    /// The <see cref="Reservation"/> object if found; otherwise, <c>null</c>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="id"/> is less than zero.
    /// </exception>
    public static Reservation? FindReservationById(int id) => _reservations.FindReservationById(id);

    /// <summary>
    /// Finds all reservations associated with a specific client by their ID.
    /// </summary>
    /// <param name="id">The unique ID of the client.</param>
    /// <returns>A collection of <see cref="Reservation"/> objects for the specified client.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="id"/> is less than zero.
    /// </exception>
    public static IEnumerable<Reservation> FindReservationsByClientId(int id) =>
        _reservations.FindReservationsByClientId(id);

    /// <summary>
    /// Finds all reservations associated with a specific accommodation by its ID.
    /// </summary>
    /// <param name="id">The unique ID of the accommodation.</param>
    /// <returns>A collection of <see cref="Reservation"/> objects for the specified accommodation.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="id"/> is less than zero.
    /// </exception>
    public static IEnumerable<Reservation> FindReservationsByAccommodationId(int id) =>
        _reservations.FindReservationsByAccommodationId(id);

    /// <summary>
    /// Counts the total number of reservations in the system.
    /// </summary>
    /// <returns>The number of reservations.</returns>
    public static int CountReservations() => _reservations.CountReservations();

#endregion

#region Accommodation Operations

    /// <summary>
    /// Adds a new accommodation to the system.
    /// </summary>
    /// <param name="accommodation">The <see cref="Accommodation"/> object to add.</param>
    /// <returns>
    /// <c>true</c> if the accommodation was added; <c>false</c> if an accommodation with the same ID exists.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="accommodation"/> is <c>null</c>.
    /// </exception>
    public static bool AddAccommodation(Accommodation accommodation) => _accommodations.Add(accommodation);

    /// <summary>
    /// Removes an existing accommodation from the system.
    /// </summary>
    /// <param name="accommodation">The <see cref="Accommodation"/> object to remove.</param>
    /// <returns>
    /// <c>true</c> if the accommodation was removed; <c>false</c> if the accommodation was not found.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="accommodation"/> is <c>null</c>.
    /// </exception>
    public static bool RemoveAccommodation(Accommodation accommodation) => _accommodations.Remove(accommodation);

    /// <summary>
    /// Imports accommodations from a JSON string.
    /// </summary>
    /// <param name="data">A JSON string containing accommodation data to import.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="data"/> is <c>null</c> or an empty string.
    /// </exception>
    public static void ImportAccommodations(string data) => _accommodations.Import(data);

    /// <summary>
    /// Exports all accommodations to a specified file as a JSON string.
    /// </summary>
    /// <param name="filePath">The file path where accommodation data will be saved.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="filePath"/> is <c>null</c> or an empty string.
    /// </exception>
    /// <exception cref="IOException">
    /// Thrown when an I/O error occurs while writing to <paramref name="filePath"/>.
    /// </exception>
    public static void ExportAccommodations(string filePath) => File.WriteAllText(filePath, _accommodations.Export());

    /// <summary>
    /// Retrieves all accommodations in the system.
    /// </summary>
    /// <returns>A read-only collection of <see cref="Accommodation"/> objects.</returns>
    public static IReadOnlyCollection<Accommodation> GetAllAccommodations() => _accommodations.GetAllAccommodations();

    /// <summary>
    /// Finds an accommodation by its unique ID.
    /// </summary>
    /// <param name="id">The unique ID of the accommodation to find.</param>
    /// <returns>
    /// The <see cref="Accommodation"/> object if found; otherwise, <c>null</c>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="id"/> is less than zero.
    /// </exception>
    public static Accommodation? FindAccommodationById(int id) => _accommodations.FindAccommodationById(id);

    /// <summary>
    /// Counts the total number of accommodations in the system.
    /// </summary>
    /// <returns>The number of accommodations.</returns>
    public static int CountAccommodations() => _accommodations.CountAccommodations();

#endregion
}
}

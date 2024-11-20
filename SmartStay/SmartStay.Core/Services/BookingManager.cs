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
using SmartStay.Core.Models;
using SmartStay.Core.Repositories;
using SmartStay.IO.FileOperations;

/// <summary>
/// The <c>Core.Services</c> namespace contains service classes that implement business logic for the SmartStay
/// application. These services coordinate actions between repositories and models to fulfill application requirements.
/// </summary>
namespace SmartStay.Core.Services
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
    internal static readonly Clients _clients = new Clients();

    /// <summary>
    /// Holds the collection of all reservations in the system, stored in the <see cref="Reservations"/> repository.
    /// </summary>
    internal static readonly Reservations _reservations = new Reservations();

    /// <summary>
    /// Holds the collection of all accommodations in the system, stored in the <see cref="Accommodations"/> repository.
    /// </summary>
    internal static readonly Accommodations _accommodations = new Accommodations();

#endregion

#region Accessors for Repositories

    /// <summary>
    /// Exposes the `Clients` repository as a read-only property.
    /// </summary>
    public static Clients Clients => _clients;

    /// <summary>
    /// Exposes the `Reservations` repository as a read-only property.
    /// </summary>
    public static Reservations Reservations => _reservations;

    /// <summary>
    /// Exposes the `Accommodations` repository as a read-only property.
    /// </summary>
    public static Accommodations Accommodations => _accommodations;

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
    /// Imports clients from a given JSON file.
    /// </summary>
    /// <param name="filePath">A path to a JSON file containing the clients to import.</param>
    public static void ImportClients(string filePath)
    {
        var fileContent = FileHandler.ReadFile(filePath);
        _clients.Import(fileContent);
    }

    /// <summary>
    /// Exports the clients to a specified file as JSON.
    /// </summary>
    /// <param name="filePath">The file path where the client data will be saved.</param>
    public static void ExportClients(string filePath)
    {
        var json = _clients.Export();
        FileHandler.WriteFile(filePath, json);
    }

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
    /// Imports reservations from a given JSON file.
    /// </summary>
    /// <param name="filePath">A path to a JSON file containing the reservations to import.</param>
    public static void ImportReservations(string filePath)
    {
        var fileContent = FileHandler.ReadFile(filePath);
        _reservations.Import(fileContent);
    }

    /// <summary>
    /// Exports the reservations to a specified file as JSON.
    /// </summary>
    /// <param name="filePath">The file path where the reservations data will be saved.</param>
    public static void ExportReservations(string filePath)
    {
        var json = _reservations.Export();
        FileHandler.WriteFile(filePath, json);
    }

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
    /// Imports accommodations from a given JSON file.
    /// </summary>
    /// <param name="filePath">A path to a JSON file containing the accommodations to import.</param>
    public static void ImportAccommodations(string filePath)
    {
        var fileContent = FileHandler.ReadFile(filePath);
        _accommodations.Import(fileContent);
    }

    /// <summary>
    /// Exports the accommodations to a specified file as JSON.
    /// </summary>
    /// <param name="filePath">The file path where the accommodations data will be saved.</param>
    public static void ExportAccommodations(string filePath)
    {
        var json = _accommodations.Export();
        FileHandler.WriteFile(filePath, json);
    }

#endregion
}
}

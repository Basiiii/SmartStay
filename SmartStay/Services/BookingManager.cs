/// <copyright file="BookingManager.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the definition of the BookingManager class,
/// which manages client, reservation, and accommodation operations for the booking system.
/// It provides static methods to add, remove, import, and export clients, reservations, and accommodations.
/// </file>
/// <summary>
/// Defines the <see cref="BookingManager"/> class, responsible for managing various operations related to
/// clients, reservations, and accommodations within the booking system.
/// It provides a centralized interface for interacting with the system's core data structures and operations.
/// </summary>
/// <author>Enrique Rodrigues</author>
/// <date>11/11/2024</date>
/// <remarks>
/// This class serves as the primary interface for interacting with the booking system's key entities.
/// It encapsulates all client, reservation, and accommodation operations in a single class.
/// </remarks>
namespace SmartStay.Services
{
using SmartStay.Models;
using SmartStay.Repositories;

/// <summary>
/// Defines the <see cref="BookingManager" />
/// </summary>
public static class BookingManager
{
    /// <summary>
    /// Defines the <see cref="_clients"/> field, which holds the collection of all clients in the system.
    /// This static readonly field is initialized at the time of declaration and represents the collection
    /// that stores all client information.
    /// </summary>
    static readonly Clients _clients = new();

    /// <summary>
    /// Defines the <see cref="_reservations"/> field, which holds the collection of all reservations in the system.
    /// This static readonly field is initialized at the time of declaration and represents the collection
    /// that stores all reservation records.
    /// </summary>
    static readonly Reservations _reservations = new();

    /// <summary>
    /// Defines the <see cref="_accommodations"/> field, which holds the collection of all accommodations in the system.
    /// This static readonly field is initialized at the time of declaration and represents the collection
    /// that stores all accommodation details.
    /// </summary>
    static readonly Accommodations _accommodations = new();

    /// <summary>
    /// Adds a new client to the collection
    /// </summary>
    /// <param name="client">The <see cref="Client"/> to add to the collection</param>
    /// <returns>Returns <c>true</c> if the client was successfully added; otherwise, <c>false</c></returns>
    public static bool AddClient(Client client)
    {
        try
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client), "Client cannot be null");
            }

            return _clients.Add(client);
        }
        catch (ArgumentNullException ex)
        {
            // TODO: Log the exception for null client
            return false;
        }
        catch (Exception ex)
        {
            // TODO: Log any other unexpected exception
            return false;
        }
    }

    /// <summary>
    /// Removes a client from the collection
    /// </summary>
    /// <param name="client">The <see cref="Client"/> to remove from the collection</param>
    /// <returns>Returns <c>true</c> if the client was successfully removed; otherwise, <c>false</c></returns>
    public static bool RemoveClient(Client client)
    {
        try
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client), "Client cannot be null");
            }

            return _clients.Remove(client);
        }
        catch (ArgumentNullException ex)
        {
            // TODO: Log the exception for null client
            return false;
        }
        catch (Exception ex)
        {
            // TODO: Log any other unexpected exception
            return false;
        }
    }

    /// <summary>
    /// Imports clients from a JSON string
    /// </summary>
    /// <param name="data">The serialized string containing client data</param>
    public static void ImportClients(string data)
    {
        try
        {
            // Check if the data is null or empty
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentException("Data cannot be null or empty", nameof(data));
            }

            // Call the Import method on the _clients object
            _clients.Import(data);
        }
        catch (ArgumentException ex)
        {
            // Argument exception is explicitly thrown if data is invalid
            throw new ArgumentException("Invalid data provided for import", ex);
        }
        catch (Exception ex)
        {
            // General unexpected exception
            throw new InvalidOperationException("An error occurred while importing clients", ex);
        }
    }

    /// <summary>
    /// Exports the current list of clients to a JSON file
    /// </summary>
    /// <param name="filePath">The path where the JSON file should be saved</param>
    public static void ExportClients(string filePath)
    {
        try
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));
            }

            string jsonData = _clients.Export();

            // Check if the directory exists; throw an exception if it does not
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException($"Directory '{directory}' not found.");
            }

            // Write the JSON data to the specified file
            File.WriteAllText(filePath, jsonData);
        }
        catch (ArgumentException ex)
        {
            // TODO: Log the exception
            throw new ArgumentException($"Invalid file path: {ex.Message}", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            // TODO: Log the exception
            throw new UnauthorizedAccessException($"Permission error: {ex.Message}", ex);
        }
        catch (IOException ex)
        {
            // TODO: Log the exception
            throw new IOException($"File operation error: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Adds a new reservation to the collection
    /// </summary>
    /// <param name="reservation">The <see cref="Reservation"/> to add to the collection</param>
    /// <returns>Returns <c>true</c> if the reservation was successfully added; otherwise, <c>false</c></returns>
    public static bool AddReservation(Reservation reservation)
    {
        try
        {
            if (reservation == null)
            {
                throw new ArgumentNullException(nameof(reservation), "Reservation cannot be null");
            }

            return _reservations.Add(reservation);
        }
        catch (ArgumentNullException ex)
        {
            // TODO: Log the exception for null reservation
            return false;
        }
        catch (Exception ex)
        {
            // TODO: Log any other unexpected exception
            return false;
        }
    }

    /// <summary>
    /// Removes a reservation from the collection
    /// </summary>
    /// <param name="reservation">The <see cref="Reservation"/> to remove from the collection</param>
    /// <returns>Returns <c>true</c> if the reservation was successfully removed; otherwise, <c>false</c></returns>
    public static bool RemoveReservation(Reservation reservation)
    {
        try
        {
            if (reservation == null)
            {
                throw new ArgumentNullException(nameof(reservation), "Reservation cannot be null");
            }

            return _reservations.Remove(reservation);
        }
        catch (ArgumentNullException ex)
        {
            // TODO: Log the exception for null reservation
            return false;
        }
        catch (Exception ex)
        {
            // TODO: Log any other unexpected exception
            return false;
        }
    }

    /// <summary>
    /// Imports reservations from a JSON string
    /// </summary>
    /// <param name="data">The serialized string containing reservation data</param>
    public static void ImportReservations(string data)
    {
        try
        {
            // Check if the data is null or empty
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentException("Data cannot be null or empty", nameof(data));
            }

            // Call the Import method on the _reservations object
            _reservations.Import(data);
        }
        catch (ArgumentException ex)
        {
            // Argument exception is explicitly thrown if data is invalid
            throw new ArgumentException("Invalid data provided for import", ex);
        }
        catch (Exception ex)
        {
            // General unexpected exception
            throw new InvalidOperationException("An error occurred while importing reservations", ex);
        }
    }

    /// <summary>
    /// Exports the current list of reservations to a JSON file
    /// </summary>
    /// <param name="filePath">The path where the JSON file should be saved</param>
    public static void ExportReservations(string filePath)
    {
        try
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));
            }

            string jsonData = _reservations.Export();

            // Check if the directory exists; throw an exception if it does not
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException($"Directory '{directory}' not found.");
            }

            // Write the JSON data to the specified file
            File.WriteAllText(filePath, jsonData);
        }
        catch (ArgumentException ex)
        {
            // TODO: Log the exception
            throw new ArgumentException($"Invalid file path: {ex.Message}", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            // TODO: Log the exception
            throw new UnauthorizedAccessException($"Permission error: {ex.Message}", ex);
        }
        catch (IOException ex)
        {
            // TODO: Log the exception
            throw new IOException($"File operation error: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Adds a new accommodation to the collection
    /// </summary>
    /// <param name="accommodation">The <see cref="Accommodation"/> to add to the collection</param>
    /// <returns>Returns <c>true</c> if the accommodation was successfully added; otherwise, <c>false</c></returns>
    public static bool AddAccommodation(Accommodation accommodation)
    {
        try
        {
            if (accommodation == null)
            {
                throw new ArgumentNullException(nameof(accommodation), "Accommodation cannot be null");
            }

            return _accommodations.Add(accommodation);
        }
        catch (ArgumentNullException ex)
        {
            // TODO: Log the exception for null accommodation
            return false;
        }
        catch (Exception ex)
        {
            // TODO: Log any other unexpected exception
            return false;
        }
    }

    /// <summary>
    /// Removes an accommodation from the collection
    /// </summary>
    /// <param name="accommodation">The <see cref="Accommodation"/> to remove from the collection</param>
    /// <returns>Returns <c>true</c> if the accommodation was successfully removed; otherwise, <c>false</c></returns>
    public static bool RemoveAccommodation(Accommodation accommodation)
    {
        try
        {
            if (accommodation == null)
            {
                throw new ArgumentNullException(nameof(accommodation), "Accommodation cannot be null");
            }

            return _accommodations.Remove(accommodation);
        }
        catch (ArgumentNullException ex)
        {
            // TODO: Log the exception for null accommodation
            return false;
        }
        catch (Exception ex)
        {
            // TODO: Log any other unexpected exception
            return false;
        }
    }

    /// <summary>
    /// Imports accommodations from a JSON string
    /// </summary>
    /// <param name="data">The serialized string containing accommodation data</param>
    public static void ImportAccommodations(string data)
    {
        try
        {
            // Check if the data is null or empty
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentException("Data cannot be null or empty", nameof(data));
            }

            // Call the Import method on the _accommodations object
            _accommodations.Import(data);
        }
        catch (ArgumentException ex)
        {
            // Argument exception is explicitly thrown if data is invalid
            throw new ArgumentException("Invalid data provided for import", ex);
        }
        catch (Exception ex)
        {
            // General unexpected exception
            throw new InvalidOperationException("An error occurred while importing accommodations", ex);
        }
    }

    /// <summary>
    /// Exports the current list of accommodations to a JSON file
    /// </summary>
    /// <param name="filePath">The path where the JSON file should be saved</param>
    public static void ExportAccommodations(string filePath)
    {
        try
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));
            }

            string jsonData = _accommodations.Export();

            // Check if the directory exists; throw an exception if it does not
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException($"Directory '{directory}' not found.");
            }

            // Write the JSON data to the specified file
            File.WriteAllText(filePath, jsonData);
        }
        catch (ArgumentException ex)
        {
            // TODO: Log the exception
            throw new ArgumentException($"Invalid file path: {ex.Message}", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            // TODO: Log the exception
            throw new UnauthorizedAccessException($"Permission error: {ex.Message}", ex);
        }
        catch (IOException ex)
        {
            // TODO: Log the exception
            throw new IOException($"File operation error: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// The GetAllClients
    /// </summary>
    /// <returns>The <see cref="IReadOnlyCollection{Client}"/></returns>
    public static IReadOnlyCollection<Client> GetAllClients()
    {
        return _clients.GetAllClients();
    }

    /// <summary>
    /// The GetAllReservations
    /// </summary>
    /// <returns>The <see cref="IReadOnlyCollection{Reservation}"/></returns>
    public static IReadOnlyCollection<Reservation> GetAllReservations()
    {
        return _reservations.GetAllReservations();
    }

    /// <summary>
    /// The GetAllAccommodations
    /// </summary>
    /// <returns>The <see cref="IReadOnlyCollection{Accommodation}"/></returns>
    public static IReadOnlyCollection<Accommodation> GetAllAccommodations()
    {
        return _accommodations.GetAllAccommodations();
    }
}

}

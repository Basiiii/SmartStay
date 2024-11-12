/// <copyright file="Clients.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="Clients"/> class, which manages a collection of <see
/// cref="Client"/> objects. The class allows for adding, removing, importing, exporting, and searching clients by their
/// unique ID.
/// </file>
/// <summary>
/// Defines the <see cref="Clients"/> class, which represents a collection of <see cref="Client"/> objects.
/// The clients are stored in an internal dictionary for fast lookup by client ID. This class implements the
/// <see cref="IManageableEntity{Client}"/> interface, providing a standardized approach for managing clients within
/// the SmartStay application, including functionalities for adding, removing, importing, and exporting clients.
/// </summary>
/// <author>Enrique Rodrigues</author>
/// <date>11/11/2024</date>
/// <remarks>
/// This class is designed to facilitate the management of client data, with fast access to clients by their ID.
/// It also provides import and export capabilities to handle serialized data, allowing for easy integration with other
/// systems. The class is intended to be flexible for future changes in data formats (e.g., switching to XML or other
/// serialization formats).
/// </remarks>
using SmartStay.Models;
using SmartStay.Models.Interfaces;
using SmartStay.Utilities;

namespace SmartStay.Repositories
{
/// <summary>
/// Represents a collection of <see cref="Client"/> objects, managed in a dictionary for fast lookup by client ID.
/// Implements the <see cref="IManageableEntity{Client}"/> interface for standardized management.
/// </summary>
public class Clients : IManageableEntity<Client>
{
    /// <summary>
    /// Internal dictionary to store clients by their unique ID.
    /// </summary>
    readonly Dictionary<int, Client> _clientDictionary = [];

    /// <summary>
    /// Attempts to add a new client to the collection.
    /// </summary>
    /// <param name="client">The <see cref="Client"/> to add to the collection.</param>
    /// <returns>
    /// <c>true</c> if the client was successfully added to the collection;
    /// <c>false</c> if a client with the same ID already exists in the collection.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="client"/> is <c>null</c>.
    /// </exception>
    public bool Add(Client client)
    {
        if (client == null)
        {
            throw new ArgumentNullException(nameof(client), "Client cannot be null");
        }

        if (_clientDictionary.ContainsKey(client.Id))
        {
            return false; // Client already exists
        }

        _clientDictionary[client.Id] = client;
        return true; // Client added successfully
    }

    /// <summary>
    /// Removes a client from the collection.
    /// </summary>
    /// <param name="client">The <see cref="Client"/> object to remove from the collection.</param>
    /// <returns>
    /// <c>true</c> if the client was successfully removed from the collection;
    /// <c>false</c> if the client was not found.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="client"/> is <c>null</c>.</exception>
    public bool Remove(Client client)
    {
        if (client == null)
        {
            throw new ArgumentNullException(nameof(client), "Client cannot be null");
        }

        return _clientDictionary.Remove(client.Id); // Remove by client ID
    }

    /// <summary>
    /// Imports a list of clients from a JSON string. Replaces any existing clients with the same ID in the collection.
    /// </summary>
    /// <param name="data">The JSON string containing the list of clients.</param>
    /// <exception cref="ArgumentException">Thrown if the data is null or empty.</exception>
    /// <exception cref="ArgumentException">Thrown if deserialization of the data fails.</exception>
    public void Import(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            throw new ArgumentException("Data cannot be null or empty", nameof(data));
        }

        var clients = JsonHelper.DeserializeFromJson<Client>(data) ??
                      throw new ArgumentException("Deserialized client data cannot be null", nameof(data));

        foreach (var client in clients)
        {
            _clientDictionary[client.Id] = client; // Direct insertion for efficiency
        }
    }

    /// <summary>
    /// Exports the current list of clients to a JSON string.
    /// </summary>
    /// <returns>A JSON string representation of the clients in the collection.</returns>
    public string Export()
    {
        return JsonHelper.SerializeToJson(_clientDictionary.Values ?? Enumerable.Empty<Client>());
    }

    /// <summary>
    /// Finds a client by their unique ID.
    /// </summary>
    /// <param name="id">The unique ID of the client to find.</param>
    /// <returns>
    /// Returns the <see cref="Client"/> object if found; otherwise, <c>null</c>.
    /// </returns>
    public Client? FindClientById(int id)
    {
        _clientDictionary.TryGetValue(id, out Client? client);
        return client;
    }

    /// <summary>
    /// Retrieves all the clients in the collection.
    /// </summary>
    /// <returns>
    /// A read-only collection of <see cref="Client"/> objects.
    /// </returns>
    /// <remarks>
    /// Returns a copy of the internal dictionary's values to prevent external modification.
    /// </remarks>
    public IReadOnlyCollection<Client> GetAllClients()
    {
        return _clientDictionary.Values.ToList(); // Returns a copy of the client collection.
    }

    /// <summary>
    /// Counts the number of clients in the collection.
    /// </summary>
    /// <returns>
    /// The number of clients in the collection.
    /// </returns>
    public int CountClients()
    {
        return _clientDictionary.Count;
    }
}
}

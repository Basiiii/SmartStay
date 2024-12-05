/// <copyright file="Clients.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="Clients"/> class, which manages a collection of <see
/// cref="Client"/> objects. The class allows for adding, removing, importing, exporting, and searching clients by their
/// unique ID.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>11/11/2024</date>
#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
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
/// Represents a collection of <see cref="Client"/> objects, managed in a dictionary for fast lookup by client ID.
/// Implements the <see cref="IManageableEntity{Client}"/> interface for standardized management.
/// </summary>
[ProtoContract]
public class Clients : IManageableEntity<Client>
{
    /// <summary>
    /// Internal dictionary to store clients by their unique ID.
    /// </summary>
    readonly Dictionary<int, Client> _clientDictionary = new();

    /// <summary>
    /// A temporary list used for serialization by Protobuf. This list holds the clients
    /// that are copied from the dictionary during serialization. Protobuf-Net does not serialize
    /// dictionaries directly, so the dictionary is temporarily copied to this list before serialization.
    /// This list is cleared and rebuilt after deserialization from the binary data.
    /// </summary>
    [ProtoMember(1)] // Serialize the list of clients
    List<Client> _clientList = new();

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

        // Deserialize the data into a List<Client> instead of a single Client
        var clients = JsonHelper.DeserializeFromJson<Client>(data) ??
                      throw new ArgumentException("Deserialized client data cannot be null", nameof(data));

        int replacedCount = 0;
        int importedCount = 0;

        foreach (var client in clients)
        {
            if (_clientDictionary.ContainsKey(client.Id))
            {
                replacedCount++;
            }
            else
            {
                importedCount++;
            }
            _clientDictionary[client.Id] = client; // Direct insertion for efficiency
        }

        return new ImportResult { ImportedCount = importedCount, ReplacedCount = replacedCount };
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
    /// Prepares the object for serialization by copying all clients
    /// from the dictionary to the temporary list. This is necessary because
    /// Protobuf-Net serializes the list and not the dictionary directly.
    /// </summary>
    [ProtoBeforeSerialization]
    private void PrepareForSerialization()
    {
        // Clear the temporary list to ensure no leftover data
        _clientList.Clear();

        // Add all clients from the dictionary to the temporary list
        foreach (var client in _clientDictionary.Values)
        {
            _clientList.Add(client);
        }
    }

    /// <summary>
    /// Rebuilds the dictionary from the list of clients after deserialization.
    /// This is necessary because Protobuf-Net deserializes the list and not the dictionary.
    /// </summary>
    [ProtoAfterDeserialization]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members",
                                                     Justification =
                                                         "IDE Error, this is called automatically by protobuf-net.")]
    private void AfterDeserialization()
    {
        // Clear the dictionary before rebuilding
        _clientDictionary.Clear();

        // Rebuild the dictionary using the data from the list
        foreach (var client in _clientList)
        {
            _clientDictionary[client.Id] = client;
        }

        // Clear the temporary list once the dictionary is rebuilt
        _clientList.Clear();
    }

    /// <summary>
    /// Saves the current state of the clients collection to a file by serializing
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
                // Serialize the clients object and write it to the file stream
                Serializer.Serialize(fileStream, this);
            }
        }
        catch (IOException ioEx)
        {
            throw new IOException("An error occurred while saving the clients data.", ioEx);
        }
        catch (SerializationException serEx)
        {
            throw new SerializationException("An error occurred during serialization while saving the clients data.",
                                             serEx);
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
                // Deserialize the clients object from the file
                var clients = Serializer.Deserialize<Clients>(fileStream);

                // Copy the data from the deserialized object to the current instance
                _clientDictionary.Clear();
                foreach (var client in clients._clientDictionary)
                {
                    _clientDictionary[client.Key] = client.Value;
                }
            }
        }
        catch (IOException ioEx)
        {
            throw new IOException("An error occurred while loading the clients data.", ioEx);
        }
        catch (SerializationException serEx)
        {
            throw new SerializationException("An error occurred during deserialization while loading the clients data.",
                                             serEx);
        }
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

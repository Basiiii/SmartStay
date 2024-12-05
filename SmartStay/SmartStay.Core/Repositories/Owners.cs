/// <copyright file="Owners.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="Owners"/> class, which manages a collection of <see
/// cref="Owner"/> objects. The class allows for adding, removing, importing, exporting, and searching owners by their
/// unique ID.
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
/// Represents a collection of <see cref="Owner"/> objects, managed in a dictionary for fast lookup by owner ID.
/// Implements the <see cref="IManageableEntity{Owner}"/> interface for standardized management.
/// </summary>
[ProtoContract]
public class Owners : IManageableEntity<Owner>
{
    /// <summary>
    /// Internal dictionary to store owners by their unique ID.
    /// </summary>
    readonly Dictionary<int, Owner> _ownerDictionary = new();

    /// <summary>
    /// A temporary list used for serialization by Protobuf. This list holds the owners
    /// that are copied from the dictionary during serialization. Protobuf-Net does not serialize
    /// dictionaries directly, so the dictionary is temporarily copied to this list before serialization.
    /// This list is cleared and rebuilt after deserialization from the binary data.
    /// </summary>
    [ProtoMember(1)] // Serialize the list of owners
    List<Owner> _ownerList = new();

    /// <summary>
    /// Attempts to add a new owner to the collection.
    /// </summary>
    /// <param name="owner">The <see cref="Owner"/> to add to the collection.</param>
    /// <returns>
    /// <c>true</c> if the owner was successfully added to the collection;
    /// <c>false</c> if an owner with the same ID already exists in the collection.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="owner"/> is <c>null</c>.
    /// </exception>
    public bool Add(Owner owner)
    {
        if (owner == null)
        {
            throw new ArgumentNullException(nameof(owner), "Owner cannot be null");
        }

        if (_ownerDictionary.ContainsKey(owner.Id))
        {
            return false; // Owner already exists
        }

        _ownerDictionary[owner.Id] = owner;
        return true; // Owner added successfully
    }

    /// <summary>
    /// Removes an owner from the collection.
    /// </summary>
    /// <param name="owner">The <see cref="Owner"/> object to remove from the collection.</param>
    /// <returns>
    /// <c>true</c> if the owner was successfully removed from the collection;
    /// <c>false</c> if the owner was not found.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="owner"/> is <c>null</c>.</exception>
    public bool Remove(Owner owner)
    {
        if (owner == null)
        {
            throw new ArgumentNullException(nameof(owner), "Owner cannot be null");
        }

        return _ownerDictionary.Remove(owner.Id); // Remove by owner ID
    }

    /// <summary>
    /// Imports a list of owners from a JSON string. Replaces any existing owners with the same ID in the collection.
    /// </summary>
    /// <param name="data">The JSON string containing the list of owners.</param>
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

        // Deserialize the data into a List<Owner> instead of a single Owner
        var owners = JsonHelper.DeserializeFromJson<Owner>(data) ??
                     throw new ArgumentException("Deserialized owner data cannot be null", nameof(data));

        int replacedCount = 0;
        int importedCount = 0;

        foreach (var owner in owners)
        {
            if (_ownerDictionary.ContainsKey(owner.Id))
            {
                replacedCount++;
            }
            else
            {
                importedCount++;
            }
            _ownerDictionary[owner.Id] = owner; // Direct insertion for efficiency
        }

        return new ImportResult { ImportedCount = importedCount, ReplacedCount = replacedCount };
    }

    /// <summary>
    /// Exports the current list of owners to a JSON string.
    /// </summary>
    /// <returns>A JSON string representation of the owners in the collection.</returns>
    public string Export()
    {
        return JsonHelper.SerializeToJson(_ownerDictionary.Values ?? Enumerable.Empty<Owner>());
    }

    /// <summary>
    /// Prepares the object for serialization by copying all owners from the dictionary to the temporary list.
    /// This is necessary because Protobuf-Net serializes the list and not the dictionary directly.
    /// </summary>
    [ProtoBeforeSerialization]
    private void PrepareForSerialization()
    {
        // Clear the temporary list to ensure no leftover data
        _ownerList.Clear();

        // Add all owners from the dictionary to the temporary list
        foreach (var owner in _ownerDictionary.Values)
        {
            _ownerList.Add(owner);
        }
    }

    /// <summary>
    /// Rebuilds the dictionary from the list of owners after deserialization.
    /// This is necessary because Protobuf-Net deserializes the list and not the dictionary.
    /// </summary>
    [ProtoAfterDeserialization]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members",
                                                     Justification =
                                                         "IDE Error, this is called automatically by protobuf-net.")]
    private void AfterDeserialization()
    {
        // Clear the dictionary before rebuilding
        _ownerDictionary.Clear();

        // Rebuild the dictionary using the data from the list
        foreach (var owner in _ownerList)
        {
            _ownerDictionary[owner.Id] = owner;
        }

        // Clear the temporary list once the dictionary is rebuilt
        _ownerList.Clear();

        // Set _lastOwnerId to the highest ID in the deserialized data
        if (_ownerDictionary.Count > 0)
        {
            // Find the highest ID from the loaded owners
            Owner.LastAssignedId = _ownerDictionary.Values.Max(o => o.Id);
        }
        else
        {
            // If no owners, reset to 0
            Owner.LastAssignedId = 0;
        }
    }

    /// <summary>
    /// Saves the current state of the owners collection to a file by serializing
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
                // Serialize the owners object and write it to the file stream
                Serializer.Serialize(fileStream, this);
            }
        }
        catch (IOException ioEx)
        {
            throw new IOException("An error occurred while saving the owners data.", ioEx);
        }
        catch (SerializationException serEx)
        {
            throw new SerializationException("An error occurred during serialization while saving the owners data.",
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
                // Deserialize the owners object from the file
                var owners = Serializer.Deserialize<Owners>(fileStream);

                // Copy the data from the deserialized object to the current instance
                _ownerDictionary.Clear();
                foreach (var owner in owners._ownerDictionary)
                {
                    _ownerDictionary[owner.Key] = owner.Value;
                }
            }
        }
        catch (IOException ioEx)
        {
            throw new IOException("An error occurred while loading the owners data.", ioEx);
        }
        catch (SerializationException serEx)
        {
            throw new SerializationException("An error occurred during deserialization while loading the owners data.",
                                             serEx);
        }
    }

    /// <summary>
    /// Finds an owner by their unique ID.
    /// </summary>
    /// <param name="id">The unique ID of the owner to find.</param>
    /// <returns>
    /// Returns the <see cref="Owner"/> object if found; otherwise, <c>null</c>.
    /// </returns>
    public Owner? FindOwnerById(int id)
    {
      _ownerDictionary.TryGetValue(id, out Owner? owner);
      return owner;
    }

    /// <summary>
    /// Counts the number of owners in the collection.
    /// </summary>
    /// <returns>
    /// The number of owners in the collection.
    /// </returns>
    public int CountOwners()
    {
        return _ownerDictionary.Count;
    }
}
}

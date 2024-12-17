/// <copyright file="Accommodations.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="Accommodations"/> class, which represents a collection of <see
/// cref="Accommodation"/> objects. The accommodations are stored in an internal dictionary for fast lookup by
/// accommodation ID. This class implements the <see cref="IManageableEntity{Accommodation}"/> interface, providing a
/// standardized approach for managing accommodations within the SmartStay application, including functionalities for
/// adding, removing, importing, and exporting accommodations.
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
/// Represents a collection of <see cref="Accommodation"/> objects, managed in a dictionary for fast lookup by
/// accommodation ID.
/// </summary>
[ProtoContract]
public class Accommodations : IManageableEntity<Accommodation>
{
    /// <summary>
    /// Internal dictionary to store accommodations by their unique ID.
    /// </summary>
    readonly Dictionary<int, Accommodation> _accommodationDictionary = new();

    /// <summary>
    /// A temporary list used for serialization by Protobuf. This list holds the accommodations
    /// that are copied from the dictionary during serialization. Protobuf-Net does not serialize
    /// dictionaries directly, so the dictionary is temporarily copied to this list before serialization.
    /// This list is cleared and rebuilt after deserialization from the binary data.
    /// </summary>
    [ProtoMember(1)] // Serialize the list of accommodations
    List<Accommodation> _accommodationList = new();

    /// <summary>
    /// Attempts to add a new accommodation to the collection.
    /// </summary>
    /// <param name="accommodation">The <see cref="Accommodation"/> to add to the collection.</param>
    /// <returns>
    /// <c>true</c> if the accommodation was successfully added to the collection;
    /// <c>false</c> if an accommodation with the same ID already exists in the collection.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="accommodation"/> is <c>null</c>.</exception>
    public bool Add(Accommodation accommodation)
    {
        if (accommodation == null)
        {
            throw new ArgumentNullException(nameof(accommodation), "Accommodation cannot be null");
        }

        if (_accommodationDictionary.ContainsKey(accommodation.Id))
        {
            return false; // Accommodation already exists
        }

        _accommodationDictionary[accommodation.Id] = accommodation;
        return true; // Accommodation added successfully
    }

    /// <summary>
    /// Removes an accommodation from the collection.
    /// </summary>
    /// <param name="accommodation">The <see cref="Accommodation"/> object to remove from the collection.</param>
    /// <returns>
    /// <c>true</c> if the accommodation was successfully removed from the collection;
    /// <c>false</c> if the accommodation was not found.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="accommodation"/> is <c>null</c>.</exception
    public bool Remove(Accommodation accommodation)
    {
        if (accommodation == null)
        {
            throw new ArgumentNullException(nameof(accommodation), "Accommodation cannot be null");
        }

        return _accommodationDictionary.Remove(accommodation.Id); // Remove using accommodation ID
    }

    /// <summary>
    /// Imports accommodations from a JSON string into the collection.
    /// Existing accommodations with the same ID are replaced.
    /// </summary>
    /// <param name="data">The JSON string containing the list of accommodations.</param>
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

        // Deserialize the data into a List<Accommodation> instead of a single Accommodation
        var accommodations =
            JsonHelper.DeserializeFromJson<Accommodation>(data) ??
            throw new ArgumentException("Deserialized accommodation data cannot be null", nameof(data));

        int replacedCount = 0;
        int importedCount = 0;

        foreach (var accommodation in accommodations)
        {
            if (_accommodationDictionary.ContainsKey(accommodation.Id))
            {
                replacedCount++;
            }
            else
            {
                importedCount++;
            }
            _accommodationDictionary[accommodation.Id] = accommodation; // Direct insertion for efficiency
        }

        return new ImportResult { ImportedCount = importedCount, ReplacedCount = replacedCount };
    }

    /// <summary>
    /// Exports the current list of accommodations to a JSON string.
    /// </summary>
    /// <returns>A JSON string representation of the accommodations in the collection.</returns
    public string Export()
    {
        return JsonHelper.SerializeToJson(_accommodationDictionary.Values);
    }

    /// <summary>
    /// Prepares the object for serialization by copying all accommodations
    /// from the dictionary to the temporary list. This is necessary because
    /// Protobuf-Net serializes the list and not the dictionary directly.
    /// </summary>
    [ProtoBeforeSerialization]
    private void PrepareForSerialization()
    {
        // Clear the temporary list to ensure no leftover data
        _accommodationList.Clear();

        // Add all accommodations from the dictionary to the temporary list
        foreach (var accommodation in _accommodationDictionary.Values)
        {
            _accommodationList.Add(accommodation);
        }
    }

    /// <summary>
    /// Rebuilds the dictionary from the list of accommodations after deserialization.
    /// This is necessary because Protobuf-Net deserializes the list and not the dictionary.
    /// </summary>
    [ProtoAfterDeserialization]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members",
                                                     Justification =
                                                         "IDE Error, this is called automatically by protobuf-net.")]
    private void AfterDeserialization()
    {
        // Clear the dictionary before rebuilding
        _accommodationDictionary.Clear();

        // Rebuild the dictionary using the data from the list
        foreach (var accommodation in _accommodationList)
        {
            _accommodationDictionary[accommodation.Id] = accommodation;
        }

        // Clear the temporary list once the dictionary is rebuilt
        _accommodationList.Clear();

        // Set _lastAccommodationId to the highest ID in the deserialized data
        if (_accommodationDictionary.Count > 0)
        {
            // Find the highest ID from the loaded accommodations
            Accommodation.LastAssignedId = _accommodationDictionary.Values.Max(a => a.Id);
        }
        else
        {
            // If no accommodations, reset to 0
            Accommodation.LastAssignedId = 0;
        }
    }

    /// <summary>
    /// Saves the current state of the accommodations collection to a file by serializing
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
                // Serialize the accommodations object and write it to the file stream
                Serializer.Serialize(fileStream, this);
            }
        }
        catch (IOException ioEx)
        {
            throw new IOException("An error occurred while saving the accommodations data.", ioEx);
        }
        catch (SerializationException serEx)
        {
            throw new SerializationException(
                "An error occurred during serialization while saving the accommodations data.", serEx);
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
                // Deserialize the accommodations object from the file
                var accommodations = Serializer.Deserialize<Accommodations>(fileStream);

                // Clear the current dictionary
                _accommodationDictionary.Clear();

                // Reset LastAssignedId to ensure consistency
                Accommodation.LastAssignedId = 0;

                // Iterate over the deserialized data and copy it
                foreach (var accommodation in accommodations._accommodationDictionary)
                {
                    _accommodationDictionary[accommodation.Key] = accommodation.Value;
                    Accommodation.LastAssignedId = Math.Max(Accommodation.LastAssignedId, accommodation.Value.Id);
                }
            }
        }
        catch (IOException ioEx)
        {
            throw new IOException("An error occurred while loading the accommodations data.", ioEx);
        }
        catch (SerializationException serEx)
        {
            throw new SerializationException(
                "An error occurred during deserialization while loading the accommodations data.", serEx);
        }
    }

    /// <summary>
    /// Finds an accommodation by its unique ID.
    /// </summary>
    /// <param name="accommodationId">The unique ID of the accommodation to find.</param>
    /// <returns>
    /// Returns the <see cref="Accommodation"/> object if found; otherwise, <c>null</c>.
    /// </returns>
    public Accommodation? FindAccommodationById(int accommodationId)
    {
      _accommodationDictionary.TryGetValue(accommodationId, out Accommodation? accommodation);
      return accommodation;
    }

    /// <summary>
    /// Counts the number of accommodations in the collection.
    /// </summary>
    /// <returns>
    /// The number of accommodations in the collection.
    /// </returns>
    public int CountAccommodations()
    {
        return _accommodationDictionary.Count;
    }
}
}

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
using System;
using System.Collections.Generic;
using System.Linq;
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
public class Accommodations : IManageableEntity<Accommodation>
{
    /// <summary>
    /// Internal dictionary to store accommodations by their unique ID.
    /// </summary>
    readonly Dictionary<int, Accommodation> _accommodationDictionary = new Dictionary<int, Accommodation>();

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
    /// <exception cref="ArgumentException">Thrown if the data is null or empty.</exception>
    /// <exception cref="ArgumentException">Thrown if deserialization of the data fails.</exception>
    public void Import(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            throw new ArgumentException("Data cannot be null or empty", nameof(data));
        }

        var accommodations =
            JsonHelper.DeserializeFromJson<Accommodation>(data) ??
            throw new ArgumentException("Deserialized accommodation data cannot be null", nameof(data));

        foreach (var accommodation in accommodations)
        {
            _accommodationDictionary[accommodation.Id] = accommodation;
        }
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
    /// Retrieves all the accommodations in the collection.
    /// </summary>
    /// <returns>
    /// A read-only collection of <see cref="Accommodation"/> objects.
    /// </returns>
    public IReadOnlyCollection<Accommodation> GetAllAccommodations()
    {
        return _accommodationDictionary.Values.ToList(); // Returns a copy of the accommodation collection.
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

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
/// Represents a collection of <see cref="Owner"/> objects, managed in a dictionary for fast lookup by owner ID.
/// Implements the <see cref="IManageableEntity{Owner}"/> interface for standardized management.
/// </summary>
public class Owners : IManageableEntity<Owner>
{
    /// <summary>
    /// Internal dictionary to store owners by their unique ID.
    /// </summary>
    readonly Dictionary<int, Owner> _ownerDictionary = new();

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
    /// <exception cref="ArgumentException">Thrown if the data is null or empty.</exception>
    /// <exception cref="ArgumentException">Thrown if deserialization of the data fails.</exception>
    public void Import(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            throw new ArgumentException("Data cannot be null or empty", nameof(data));
        }

        var owners = JsonHelper.DeserializeFromJson<Owner>(data) ??
                     throw new ArgumentException("Deserialized owner data cannot be null", nameof(data));

        foreach (var owner in owners)
        {
            _ownerDictionary[owner.Id] = owner; // Direct insertion for efficiency
        }
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

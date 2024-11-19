/// <copyright file="ManageableEntity.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the definition of the IManageableEntity interface, which provides a standard
/// structure for managing collections of entities within the SmartStay application.
///
/// This interface can be implemented by any collection class to provide a consistent API for managing
/// entities, facilitating code reuse and standardization across different types of entity collections
/// (e.g., Clients, Reservations, Accommodations).
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>11/11/2024</date>

/// <summary>
/// This namespace contains interfaces used within the SmartStay application.
/// </summary>
namespace Core.Models.Interfaces
{
/// <summary>
/// Defines the <see cref="IManageableEntity{T}" /> interface for managing a collection of entities
/// of type <typeparamref name="T"/>. This interface standardizes methods for adding, removing,
/// importing, and exporting entities.
/// </summary>
/// <typeparam name="T">The type of entities managed by the implementing collection class.</typeparam>
public interface IManageableEntity<in T>
{
    /// <summary>
    /// Adds a single entity of type <typeparamref name="T"/> to the collection.
    /// </summary>
    /// <param name="item">The entity to add to the collection.</param>
    /// <returns>Returns <c>true</c> if the entity was successfully added; otherwise, <c>false</c>.</returns>
    bool Add(T item);

    /// <summary>
    /// Removes a specified entity of type <typeparamref name="T"/> from the collection.
    /// </summary>
    /// <param name="item">The entity to remove from the collection.</param>
    /// <returns>Returns <c>true</c> if the entity was successfully removed; otherwise, <c>false</c>.</returns>
    bool Remove(T item);

    /// <summary>
    /// Imports a list of items from a serialized string.
    /// </summary>
    /// <param name="data">The serialized string representing a collection of items.</param>
    void Import(string data);

    /// <summary>
    /// Exports the current list of items as a serialized string.
    /// </summary>
    /// <returns>A serialized string representing the collection of items.</returns>
    string Export();
}
}

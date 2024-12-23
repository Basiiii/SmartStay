﻿/// <copyright file="JsonHelper.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains helper methods for working with JSON serialization and deserialization.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>11/11/2024</date>
using System.Collections.Generic;
using System.Text.Json;

/// <summary>
/// The <c>SmartStay.Utilities</c> namespace provides helper functions and utility classes used throughout the SmartStay
/// application. These utilities support common operations and enhance reusability across different components of the
/// application.
/// </summary>
namespace SmartStay.Core.Utilities
{
/// <summary>
/// Provides static methods to serialize and deserialize objects to and from JSON format.
/// </summary>
public static class JsonHelper
{
    /// <summary>
    /// The default JSON serialization options used for formatting the output.
    /// </summary>
    private static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    /// <summary>
    /// Serializes a collection of objects to a JSON string.
    /// </summary>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    /// <param name="items">The collection of items to be serialized to JSON.</param>
    /// <returns>A <see cref="string"/> representing the serialized JSON data.</returns>
    /// <remarks>
    /// This method uses <see cref="JsonSerializer"/> to convert a collection of objects
    /// into a JSON string with indented formatting to improve readability.
    /// </remarks>
    public static string SerializeToJson<T>(IEnumerable<T> items)
    {
        return JsonSerializer.Serialize(items, _jsonOptions);
    }

    /// <summary>
    /// Deserializes a JSON string into a list of objects of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects in the collection to be deserialized.</typeparam>
    /// <param name="json">A JSON string representing a collection of objects to be deserialized.</param>
    /// <returns>A <see cref="List{T}"/> of objects of type <typeparamref name="T"/>.</returns>
    /// <remarks>
    /// This method converts the provided JSON string back into a list of objects of type
    /// <typeparamref name="T"/>. If deserialization fails or the JSON is invalid, an empty list is returned.
    /// </remarks>
    public static List<T> DeserializeFromJson<T>(string json)
    {
        return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
    }
}
}

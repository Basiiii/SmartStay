/// <copyright file="AccommodationConverter.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="AccommodationConverter"/> class,
/// a custom JSON converter for <see cref="Accommodation"/> objects.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>10/11/2024</date>
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SmartStay.Core.Models;

/// <summary>
/// The <c>SmartStay.Utilities</c> namespace provides helper functions and utility classes used throughout the SmartStay
/// application. These utilities support common operations and enhance reusability across different components of the
/// application.
/// </summary>
namespace SmartStay.Core.Utilities
{
/// <summary>
/// Custom JSON converter for <see cref="Accommodation"/> objects, used to serialize and deserialize accommodations
/// to and from JSON format. It provides custom handling for the reserved dates and accommodation type.
/// </summary>
public class AccommodationConverter : JsonConverter<Accommodation>
{
    /// <summary>
    /// Reads and deserializes an <see cref="Accommodation"/> object from JSON.
    /// </summary>
    /// <param name="reader">The JSON reader containing the JSON data.</param>
    /// <param name="typeToConvert">The type of object to convert.</param>
    /// <param name="options">The options to use for deserialization.</param>
    /// <returns>The deserialized <see cref="Accommodation"/> object.</returns>
    /// <exception cref="JsonException">Thrown if the deserialization fails.</exception>
    public override Accommodation Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Attempt to deserialize the object
        var accommodation = JsonSerializer.Deserialize<Accommodation>(ref reader, options);

        // If deserialization fails, throw an exception
        return accommodation ?? throw new JsonException("Failed to deserialize the Accommodation object.");
    }

    /// <summary>
    /// Writes an <see cref="Accommodation"/> object as JSON.
    /// </summary>
    /// <param name="writer">The JSON writer to write the serialized object to.</param>
    /// <param name="value">The <see cref="Accommodation"/> object to serialize.</param>
    /// <param name="options">The options to use for serialization.</param>
    public override void Write(Utf8JsonWriter writer, Accommodation value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        // Serialize properties of the accommodation
        writer.WriteNumber("Id", value.Id);
        writer.WriteString("Type", value.Type.ToString());
        writer.WriteString("Name", value.Name);
        writer.WriteString("Address", value.Address);
        writer.WriteNumber("PricePerNight", value.PricePerNight);

        // Serialize reserved dates as an array
        writer.WriteStartArray("ReservationDates");
        foreach (var reservation in value.ReservationDates)
        {
            writer.WriteStartObject();
            writer.WriteString("Start", reservation.Start.ToString("yyyy-MM-dd"));
            writer.WriteString("End", reservation.End.ToString("yyyy-MM-dd"));
            writer.WriteEndObject();
        }
        writer.WriteEndArray();

        writer.WriteEndObject();
    }
}
}

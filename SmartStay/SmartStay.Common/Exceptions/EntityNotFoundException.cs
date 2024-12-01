/// <copyright file="EntityNotFoundException.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="EntityNotFoundException"/> class used in the SmartStay
/// application to handle errors related to missing entities.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>01/12/2024</date>

/// <summary>
/// This namespace contains custom exceptions used within the SmartStay application.
/// </summary>
namespace SmartStay.Common.Exceptions
{
/// <summary>
/// Represents an error that occurs when an entity is not found in the system.
/// This exception is thrown when an operation cannot proceed because the specified entity (e.g., Reservation, Room)
/// does not exist in the system.
/// </summary>
/// <remarks>
/// The <see cref="EntityNotFoundException"/> class extends the base <see cref="Exception"/> class,
/// providing more specific context about errors encountered when an entity is not found in the system.
/// This is typically used in scenarios where an operation cannot continue due to the absence of an expected entity.
/// </remarks>
public class EntityNotFoundException : Exception
{
    /// <summary>
    /// Gets the type of the entity that was not found.
    /// </summary>
    /// <value>
    /// A string representing the entity type, e.g., "Reservation", "Room".
    /// </value>
    /// <remarks>
    /// This property provides the name of the entity that caused the exception, allowing specific handling based
    /// on the entity type in exception filters or logging.
    /// </remarks>
    public string EntityType { get; }

    /// <summary>
    /// Gets the ID of the entity that was not found.
    /// </summary>
    /// <value>
    /// An integer representing the ID of the missing entity.
    /// </value>
    /// <remarks>
    /// This property provides the unique identifier of the entity that could not be found, allowing the caller to
    /// identify which specific entity was missing.
    /// </remarks>
    public int EntityId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with a specified entity type and
    /// ID.
    /// </summary>
    /// <param name="entityType">The type of the entity that was not found (e.g., "Reservation", "Room").</param>
    /// <param name="entityId">The ID of the entity that was not found.</param>
    public EntityNotFoundException(string entityType, int entityId)
        : base($"{entityType} with ID {entityId} was not found.")
    {
        EntityType = entityType;
        EntityId = entityId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with a specified entity type, ID,
    /// and message.
    /// </summary>
    /// <param name="entityType">The type of the entity that was not found (e.g., "Reservation", "Room").</param>
    /// <param name="entityId">The ID of the entity that was not found.</param>
    /// <param name="message">The message that describes the error.</param>
    public EntityNotFoundException(string entityType, int entityId, string message)
        : base($"{entityType} with ID {entityId} was not found. {message}")
    {
        EntityType = entityType;
        EntityId = entityId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with a specified entity type, ID,
    /// message, and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="entityType">The type of the entity that was not found (e.g., "Reservation", "Room").</param>
    /// <param name="entityId">The ID of the entity that was not found.</param>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public EntityNotFoundException(string entityType, int entityId, string message, Exception innerException)
        : base($"{entityType} with ID {entityId} was not found. {message}", innerException)
    {
        EntityType = entityType;
        EntityId = entityId;
    }

    /// <summary>
    /// Returns a string representation of the <see cref="EntityNotFoundException"/> instance, including the error
    /// message and any inner exceptions.
    /// </summary>
    /// <returns>
    /// A string that represents the current exception, typically including the error message and any inner exceptions.
    /// </returns>
    /// <example>
    /// For example, the string representation could look like:
    /// "Reservation with ID 123 was not found."
    /// </example>
    public override string ToString()
    {
        return $"{base.ToString()}";
    }
}
}

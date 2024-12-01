/// <copyright file="AccommodationCreationException.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="AccommodationCreationException"/> class used in the SmartStay
/// application to handle errors related to accommodation creation.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>01/12/2024</date>

/// <summary>
/// This namespace contains custom exceptions used within the SmartStay application.
/// </summary>
namespace SmartStay.Common.Exceptions
{
/// <summary>
/// Represents an error that occurs during the accommodation creation process in the SmartStay application.
/// This exception is thrown when there is an issue with validating or processing the accommodation's data.
/// </summary>
/// <remarks>
/// The <see cref="AccommodationCreationException"/> class extends the base <see cref="Exception"/> class,
/// providing more specific context about errors encountered during the creation of an accommodation object.
/// This is typically used when validation or other errors occur while trying to create a new accommodation.
/// </remarks>
public class AccommodationCreationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AccommodationCreationException"/> class with a specified error
    /// message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public AccommodationCreationException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AccommodationCreationException"/> class with a specified error
    /// message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public AccommodationCreationException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Gets the error message that explains the reason for the exception.
    /// </summary>
    /// <value>
    /// The error message that describes the reason for the exception.
    /// </value>
    /// <remarks>
    /// This property is inherited from the <see cref="Exception"/> class and can be used to retrieve the
    /// message passed when the exception was thrown.
    /// </remarks>
    public override string Message => base.Message;

    /// <summary>
    /// Returns a string representation of the <see cref="AccommodationCreationException"/> instance, including
    /// the error message and any inner exceptions.
    /// </summary>
    /// <returns>
    /// A string that represents the current exception, typically including the error message and any inner exceptions.
    /// </returns>
    /// <example>
    /// For example, the string representation could look like:
    /// "Accommodation creation failed due to invalid owner ID."
    /// </example>
    public override string ToString()
    {
        return $"{base.ToString()}";
    }
}
}

/// <copyright file="AddAccommodationSystemException.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="AddAccommodationSystemException"/> class used in the SmartStay
/// application to handle errors related to adding an accommodation to the system.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>01/12/2024</date>

/// <summary>
/// This namespace contains custom exceptions used within the SmartStay application.
/// </summary>
namespace SmartStay.Common.Exceptions
{
/// <summary>
/// Represents an error that occurs when an accommodation cannot be added to the system.
/// This exception is thrown when there is an issue during the addition process, such as conflicts,
/// system-level errors, or other reasons preventing the accommodation from being added.
/// </summary>
/// <remarks>
/// The <see cref="AddAccommodationSystemException"/> class extends the base <see cref="Exception"/> class,
/// providing more specific context about errors encountered while adding an accommodation to the system.
/// </remarks>
public class AddAccommodationSystemException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddAccommodationSystemException"/> class with a specified error
    /// message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public AddAccommodationSystemException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AddAccommodationSystemException"/> class with a specified error
    /// message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public AddAccommodationSystemException(string message, Exception innerException) : base(message, innerException)
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
    /// Returns a string representation of the <see cref="AddAccommodationSystemException"/> instance, including
    /// the error message and any inner exceptions.
    /// </summary>
    /// <returns>
    /// A string that represents the current exception, typically including the error message and any inner exceptions.
    /// </returns>
    /// <example>
    /// For example, the string representation could look like:
    /// "Failed to add accommodation to the system due to a conflict or system-level error."
    /// </example>
    public override string ToString()
    {
        return $"{base.ToString()}";
    }
}
}

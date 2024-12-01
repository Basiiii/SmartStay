/// <copyright file="ReservationCreationException.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="ReservationCreationException"/> class used in the SmartStay
/// application to handle errors related to reservation creation.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>01/12/2024</date>

/// <summary>
/// This namespace contains custom exceptions used within the SmartStay application.
/// </summary>
namespace SmartStay.Common.Exceptions
{
/// <summary>
/// Represents an error that occurs during the reservation creation process in the SmartStay application.
/// This exception is thrown when there is an issue with validating or processing the reservation's data,
/// such as invalid dates, cost calculations, or availability.
/// </summary>
/// <remarks>
/// The <see cref="ReservationCreationException"/> class extends the base <see cref="Exception"/> class,
/// providing more specific context about errors encountered during the creation of a reservation.
/// This is typically used when validation or other errors occur while trying to create a new reservation object.
/// </remarks>
public class ReservationCreationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReservationCreationException"/> class with a specified error
    /// message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ReservationCreationException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReservationCreationException"/> class with a specified error
    /// message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public ReservationCreationException(string message, Exception innerException) : base(message, innerException)
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
    /// Returns a string representation of the <see cref="ReservationCreationException"/> instance, including
    /// the error message and any inner exceptions.
    /// </summary>
    /// <returns>
    /// A string that represents the current exception, typically including the error message and any inner exceptions.
    /// </returns>
    /// <example>
    /// For example, the string representation could look like:
    /// "Reservation creation failed due to unavailable room for the specified dates."
    /// </example>
    public override string ToString()
    {
        return $"{base.ToString()}";
    }
}
}

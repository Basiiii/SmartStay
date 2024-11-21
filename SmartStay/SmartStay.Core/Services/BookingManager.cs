/// <copyright file="BookingManager.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="BookingManager"/> class,
/// which manages client, reservation, and accommodation operations for the booking system.
/// It provides static methods to add, remove, import, and export clients, reservations, and accommodations.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>11/11/2024</date>
#nullable enable
using SmartStay.Common.Enums;
using SmartStay.Core.Models;
using SmartStay.Core.Repositories;
using SmartStay.Core.Utilities;
using System.Linq;
using System.Xml.Linq;

/// <summary>
/// The <c>Core.Services</c> namespace contains service classes that implement business logic for the SmartStay
/// application. These services coordinate actions between repositories and models to fulfill application requirements.
/// </summary>
namespace SmartStay.Core.Services
{
/// <summary>
/// Provides a static facade for managing clients, reservations, and accommodations in the booking system.
/// This class centralizes all operations for adding, removing, importing, and exporting data for these entities.
/// It interacts with internal repositories to simplify the main API and ensure a standardized approach.
/// </summary>
/// <remarks>
/// This class offers a unified interface for handling key booking operations and data entities, facilitating
/// integrations with other system components or external applications.
/// </remarks>
public static class BookingManager
{
#region Collections

    /// <summary>
    /// Holds the collection of all clients in the system, stored in the <see cref="Clients"/> repository.
    /// </summary>
    internal static readonly Clients _clients = new Clients();

    /// <summary>
    /// Holds the collection of all reservations in the system, stored in the <see cref="Reservations"/> repository.
    /// </summary>
    internal static readonly Reservations _reservations = new Reservations();

    /// <summary>
    /// Holds the collection of all accommodations in the system, stored in the <see cref="Accommodations"/> repository.
    /// </summary>
    internal static readonly Accommodations _accommodations = new Accommodations();

#endregion

#region Helper Functions

    /// <summary>
    /// Validates a field only if it's not the default value (null or empty),
    /// and calls the validation method for that field.
    /// </summary>
    /// <typeparam name="T">The type of the field (e.g., string, PaymentMethod).</typeparam>
    /// <param name="fieldValue">The value of the field to validate.</param>
    /// <param name="defaultValue">The default value of the field (e.g., empty string or
    /// PaymentMethod.Unchanged).</param>
    /// <param name="validationFunc">The validation function to apply to the field.</param>
    public static void ValidateField<T>(T fieldValue, T defaultValue, Func<T, T> validationFunc)
    {
        // Only validate if the field value is not the default value
        if (!EqualityComparer<T>.Default.Equals(fieldValue, defaultValue))
        {
            validationFunc(fieldValue);
        }
    }

    /// <summary>
    /// Helper function to set a field's value if it is not the default value.
    /// </summary>
    /// <typeparam name="T">The type of the field (e.g., string, PaymentMethod).</typeparam>
    /// <param name="fieldValue">The value to set for the field.</param>
    /// <param name="defaultValue">The default value (e.g., empty string or PaymentMethod.Unchanged).</param>
    /// <param name="setterAction">The action to set the field if the value is not default.</param>
    public static void SetField<T>(T fieldValue, T defaultValue, Action<T> setterAction)
    {
        // Only set the field if the value is not the default value
        if (!EqualityComparer<T>.Default.Equals(fieldValue, defaultValue))
        {
            setterAction(fieldValue); // Call the setter action with the field value
        }
    }

#endregion

#region Accessors for Repositories

    /// <summary>
    /// Exposes the `Clients` repository as a read-only property.
    /// </summary>
    public static Clients Clients => _clients;

    /// <summary>
    /// Exposes the `Reservations` repository as a read-only property.
    /// </summary>
    public static Reservations Reservations => _reservations;

    /// <summary>
    /// Exposes the `Accommodations` repository as a read-only property.
    /// </summary>
    public static Accommodations Accommodations => _accommodations;

#endregion

#region Client Management

    /// <summary>
    /// Creates a new client with basic information and adds them to the system.
    /// </summary>
    /// <param name="firstName">The first name of the client.</param>
    /// <param name="lastName">The last name of the client.</param>
    /// <param name="email">The email address of the client.</param>
    /// <exception cref="ValidationException">Thrown when any of the input parameters are invalid.</exception>
    public static void CreateBasicClient(string firstName, string lastName, string email)
    {
        Client client = new Client(firstName, lastName, email);
        _clients.Add(client);
    }

    /// <summary>
    /// Creates a new client with all information and adds them to the system.
    /// </summary>
    /// <param name="firstName">The first name of the client.</param>
    /// <param name="lastName">The last name of the client.</param>
    /// <param name="email">The email address of the client.</param>
    /// <param name="phoneNumber">The phone number of the client.</param>
    /// <param name="address">The residential address of the client.</param>
    /// <exception cref="ValidationException">Thrown when any of the input parameters are invalid.</exception>
    public static void CreateCompleteClient(string firstName, string lastName, string email, string phoneNumber,
                                            string address)
    {
        Client client = new Client(firstName, lastName, email, phoneNumber, address);
        _clients.Add(client);
    }

    /// <summary>
    /// Finds a client in the system by their unique ID.
    /// </summary>
    /// <param name="clientId">The unique identifier for the client.</param>
    /// <returns>A <see cref="Client"/> object if found, otherwise null.</returns>
    /// <exception cref="ValidationException">Thrown when any of the input parameters are invalid.</exception>
    public static Client FindClientById(int clientId)
    {
        var client = _clients.FindClientById(clientId);

        return client ?? throw new ArgumentException($"Client with ID {clientId} not found.");
    }

    /// <summary>
    /// Updates the details of an existing client.
    /// </summary>
    /// <param name="clientId">The unique ID of the client to update.</param>
    /// <param name="firstName">The new first name of the client.</param>
    /// <param name="lastName">The new last name of the client.</param>
    /// <param name="email">The new email address of the client.</param>
    /// <param name="phoneNumber">The new phone number of the client.</param>
    /// <param name="address">The new address of the client.</param>
    /// <param name="paymentMethod">The new preferred payment method of the client.</param>
    /// <exception cref="ArgumentException">Thrown when the client ID is not found.</exception>
    public static void UpdateClient(int clientId, string firstName = "", string lastName = "", string email = "",
                                    string phoneNumber = "", string address = "",
                                    PaymentMethod paymentMethod = PaymentMethod.Unchanged)
    {
        // Find the client by ID
        var client =
            _clients.FindClientById(clientId) ?? throw new ArgumentException($"Client with ID {clientId} not found.");

        // Validate information before updating
        ValidateField(firstName, "", Validation.Validators.NameValidator.ValidateName);
        ValidateField(lastName, "", Validation.Validators.NameValidator.ValidateName);
        ValidateField(email, "", Validation.Validators.EmailValidator.ValidateEmail);
        ValidateField(phoneNumber, "", Validation.Validators.PhoneNumberValidator.ValidatePhoneNumber);
        ValidateField(address, "", Validation.Validators.AddressValidator.ValidateAddress);
        ValidateField(paymentMethod, PaymentMethod.Unchanged,
                      Validation.Validators.PaymentValidator.ValidatePaymentMethod);

        // Update client information with given fields
        SetField(firstName, "", value => client.FirstName = value);
        SetField(lastName, "", value => client.LastName = value);
        SetField(email, "", value => client.Email = value);
        SetField(phoneNumber, "", value => client.PhoneNumber = value);
        SetField(address, "", value => client.Address = value);
        SetField(paymentMethod, PaymentMethod.Unchanged, value => client.PreferredPaymentMethod = value);
    }

    /// <summary>
    /// Removes a client from the system.
    /// </summary>
    /// <param name="clientId">The unique ID of the client to remove.</param>
    /// <exception cref="ArgumentException">Thrown when the client ID is not found.</exception>
    public static void RemoveClient(int clientId)
    {
        // Find the client by ID
        var client = _clients.FindClientById(clientId);

        if (client == null)
        {
            throw new ArgumentException($"Client with ID {clientId} not found.");
        }

        // Remove the client from the list
        _clients.Remove(client);
    }

#endregion

#region Reservation Management

    /// <summary>
    /// Creates a new reservation for a client, checking accommodation availability, calculating the cost,
    /// and adding the reservation to the accommodation and the reservation system.
    /// </summary>
    /// <param name="clientId">The unique identifier of the client making the reservation.</param>
    /// <param name="accommodationId">The unique identifier of the accommodation being reserved.</param>
    /// <param name="checkIn">The check-in date for the reservation.</param>
    /// <param name="checkOut">The check-out date for the reservation.</param>
    /// <returns>The newly created <see cref="Reservation"/> object, containing reservation details.</returns>
    /// <exception cref="ArgumentException">Thrown when the accommodation is not found, is unavailable during the
    /// specified dates, or the accommodation cannot add the reservation.</exception>
    /// <remarks> This method performs the following steps:
    /// 1. Validates that the specified accommodation exists.
    /// 2. Checks if the accommodation is available for the selected check-in and check-out dates.
    /// 3. Calculates the total cost of the reservation based on accommodation type and duration.
    /// 4. Creates a new reservation and adds it to both the accommodation and the reservation list.
    /// </remarks>
    public static Reservation CreateReservation(int clientId, int accommodationId, DateTime checkIn, DateTime checkOut)
    {
        // Validate the reservation details (e.g., check availability of accommodation)
        var accommodation = _accommodations.FindAccommodationById(accommodationId) ??
                            throw new ArgumentException("Accommodation not found.");

        // Calculate total cost
        decimal totalCost = accommodation.CalculateTotalCost(checkIn, checkOut);

        // Create the reservation
        var reservation = new Reservation(clientId, accommodationId, accommodation.Type, checkIn, checkOut, totalCost);

        // Add reservation to accommodation
        bool success = accommodation.AddReservation(checkIn, checkOut);
        if (!success)
            throw new ArgumentException("Accommodation is not available for the selected dates.");

        // Add it to the reservation list
        _reservations.Add(reservation);

        // Return the reservation object
        return reservation;
    }

    public static Reservation CreateReservationBulk(int clientId, int accommodationId, DateTime checkIn,
                                                    DateTime checkOut)
    {
        // Validate the reservation details (e.g., check availability of accommodation)
        var accommodation = _accommodations.FindAccommodationById(accommodationId) ??
                            throw new ArgumentException("Accommodation not found.");

        // Calculate total cost
        decimal totalCost = accommodation.CalculateTotalCost(checkIn, checkOut);

        // Create the reservation
        var reservation = new Reservation(clientId, accommodationId, accommodation.Type, checkIn, checkOut, totalCost);

        // Add reservation to accommodation
        bool success = accommodation.AddReservation(checkIn, checkOut, skipAvailabilityCheck: true);
        if (!success)
            throw new ArgumentException("Accommodation is not available for the selected dates.");

        // Add it to the reservation list
        _reservations.Add(reservation);

        // Return the reservation object
        return reservation;
    }

    /// <summary>
    /// Updates the check-in and/or check-out dates of an existing reservation.
    /// </summary>
    /// <param name="reservationId">The unique ID of the reservation to update.</param>
    /// <param name="newCheckIn">The new check-in date for the reservation, or null if no change is required.</param>
    /// <param name="newCheckOut">The new check-out date for the reservation, or null if no change is required.</param>
    /// <exception cref="ArgumentException">Thrown when the reservation is not found or the new dates are
    /// invalid.</exception> <remarks> This method ensures that the accommodation associated with the reservation is
    /// available for the updated dates, excluding the current reservation's existing date range, before applying the
    /// changes. If no new dates are specified, the reservation remains unchanged.
    /// </remarks>
    public static void UpdateReservation(int reservationId, DateTime? newCheckIn = null, DateTime? newCheckOut = null)
    {
        // Find the reservation by ID
        var reservation = _reservations.FindReservationById(reservationId);
        if (reservation == null)
            throw new ArgumentException("Reservation not found.");

        // Find the associated accommodation
        var accommodation = _accommodations.FindAccommodationById(reservation.AccommodationId);
        if (accommodation == null)
            throw new InvalidOperationException(
                $"Accommodation with ID {reservation.AccommodationId} not found. Cannot update the reservation.");

        // Determine the new check-in and check-out dates
        DateTime effectiveCheckIn = newCheckIn ?? reservation.CheckInDate;
        DateTime effectiveCheckOut = newCheckOut ?? reservation.CheckOutDate;

        // Validate the new dates, excluding the current reservation's dates from the conflict check
        if (!IsAvailableForUpdate(accommodation, reservation.CheckInDate, reservation.CheckOutDate, effectiveCheckIn,
                                  effectiveCheckOut))
        {
            throw new ArgumentException("The accommodation is not available for the selected dates.");
        }

        // Update the reserved dates in the accommodation
        accommodation.RemoveReservation(reservation.CheckInDate,
                                        reservation.CheckOutDate);         // Safely remove current dates
        accommodation.AddReservation(effectiveCheckIn, effectiveCheckOut); // Add the new dates

        // Update the reservation dates
        reservation.CheckInDate = effectiveCheckIn;
        reservation.CheckOutDate = effectiveCheckOut;
    }

    /// <summary>
    /// Cancels a reservation by its unique ID, freeing up the associated accommodation for the specified dates.
    /// </summary>
    /// <param name="reservationId">The unique ID of the reservation to cancel.</param>
    /// <exception cref="ArgumentException">Thrown when the reservation with the given ID is not found.</exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the associated accommodation for the reservation cannot be found.
    /// </exception>
    /// <remarks>
    /// This method does not delete the reservation from the system. Instead, it marks the reservation's status as
    /// <see cref="ReservationStatus.Cancelled"/> and removes the reserved dates from the accommodation's availability.
    /// This ensures the system retains a record of the reservation for historical or reporting purposes.
    /// </remarks>
    public static void CancelReservation(int reservationId)
    {
        // Find the reservation from the given ID
        var reservation = _reservations.FindReservationById(reservationId);
        if (reservation == null)
            throw new ArgumentException("Reservation not found.");

        // Find the associated acommodation
        var accommodation = _accommodations.FindAccommodationById(reservation.AccommodationId);
        if (accommodation == null)
            throw new InvalidOperationException(
                $"Accommodation with ID {reservation.AccommodationId} not found. Cannot cancel the reservation.");

        // Remove the reservation from the accommodation's reserved dates
        accommodation.RemoveReservation(reservation.CheckInDate, reservation.CheckOutDate);

        // Set the reservation status to cancelled
        reservation.Status = ReservationStatus.Cancelled;
    }

    /// <summary>
    /// Checks whether an accommodation is available for a new date range, excluding the current reservation's dates.
    /// </summary>
    /// <param name="accommodation">The accommodation to check.</param>
    /// <param name="currentStart">The current check-in date of the reservation.</param>
    /// <param name="currentEnd">The current check-out date of the reservation.</param>
    /// <param name="newStart">The proposed new check-in date.</param>
    /// <param name="newEnd">The proposed new check-out date.</param>
    /// <returns>True if the accommodation is available for the new dates; otherwise, false.</returns>
    private static bool IsAvailableForUpdate(Accommodation accommodation, DateTime currentStart, DateTime currentEnd,
                                             DateTime newStart, DateTime newEnd)
    {
        DateRange existingReservation = new DateRange(currentStart, currentEnd);

        // Exclude the current reservation's dates from the availability check
        return accommodation.IsAvailable(newStart, newEnd, existingReservation);
    }

#endregion

#region Accommodation Management

    /// <summary>
    /// Creates and adds a new accommodation to the system.
    /// </summary>
    /// <param name="type">The type of the accommodation (e.g., hotel, apartment, etc.).</param>
    /// <param name="name">The name of the accommodation.</param>
    /// <param name="address">The address of the accommodation.</param>
    /// <param name="pricePerNight">The price per night for the accommodation.</param>
    /// <returns>The newly created accommodation.</returns>
    public static Accommodation CreateAccommodation(AccommodationType type, string name, string address,
                                                    decimal pricePerNight)
    {
        var accommodation = new Accommodation(type, name, address, pricePerNight);
        _accommodations.Add(accommodation);
        return accommodation;
    }

    /// <summary>
    /// Updates the details of an existing accommodation.
    /// </summary>
    /// <param name="accommodationId">The ID of the accommodation to update.</param>
    /// <param name="type">The new type of the accommodation (optional).</param>
    /// <param name="name">The new name of the accommodation (optional).</param>
    /// <param name="address">The new address of the accommodation (optional).</param>
    /// <param name="pricePerNight">The new price per night for the accommodation (optional).</param>
    /// <exception cref="ArgumentException">Thrown if the accommodation ID is not found.</exception>
    public static void UpdateAccommodation(int accommodationId, AccommodationType type = AccommodationType.None,
                                           string name = "", string address = "", decimal? pricePerNight = null)
    {
        var accommodation = _accommodations.FindAccommodationById(accommodationId) ??
                            throw new ArgumentException("Accommodation not found.");

        // Validate fields before updating
        ValidateField(type, AccommodationType.None,
                      Validation.Validators.AccommodationValidator.ValidateAccommodationType);
        ValidateField(name, "", Validation.Validators.NameValidator.ValidateAccommodationName);
        ValidateField(address, "", Validation.Validators.AddressValidator.ValidateAddress);
        if (pricePerNight != null)
        {
            Validation.Validators.PaymentValidator.ValidatePrice((decimal)pricePerNight);
        }

        // Use the SetField helper to update fields only if they are not the default
        SetField(type, AccommodationType.None, value => accommodation.Type = value);
        SetField(name, "", value => accommodation.Name = value);
        SetField(address, "", value => accommodation.Address = value);
        if (pricePerNight != null)
        {
            accommodation.PricePerNight = pricePerNight.Value;
        }
    }

    public static void RemoveAccommodation(int accommodationId)
    {
        var accommodation = _accommodations.FindAccommodationById(accommodationId);
        if (accommodation != null)
        {
            throw new InvalidOperationException();
        }
    }

#endregion
}
}

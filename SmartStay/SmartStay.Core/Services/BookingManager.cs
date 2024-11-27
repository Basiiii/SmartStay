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
    internal static readonly Clients _clients = new();

    /// <summary>
    /// Holds the collection of all reservations in the system, stored in the <see cref="Reservations"/> repository.
    /// </summary>
    internal static readonly Reservations _reservations = new();

    /// <summary>
    /// Holds the collection of all accommodations in the system, stored in the <see cref="Accommodations"/> repository.
    /// </summary>
    internal static readonly Accommodations _accommodations = new();

    /// <summary>
    /// Holds the collection of all owners in the system, stored in the <see cref="Owners"/> repository.
    /// </summary>
    internal static readonly Owners _owners = new();

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

#region Owner Management

    /// <summary>
    /// Creates a new owner with basic information and adds them to the system.
    /// </summary>
    /// <param name="firstName">The first name of the owner.</param>
    /// <param name="lastName">The last name of the owner.</param>
    /// <param name="email">The email address of the owner.</param>
    /// <exception cref="ValidationException">Thrown when any of the input parameters are invalid.</exception>
    public static void CreateBasicOwner(string firstName, string lastName, string email)
    {
        Owner owner = new Owner(firstName, lastName, email);
        _owners.Add(owner);
    }

    /// <summary>
    /// Creates a new owner with all information and adds them to the system.
    /// </summary>
    /// <param name="firstName">The first name of the owner.</param>
    /// <param name="lastName">The last name of the owner.</param>
    /// <param name="email">The email address of the owner.</param>
    /// <param name="phoneNumber">The phone number of the owner.</param>
    /// <param name="address">The residential address of the owner.</param>
    /// <exception cref="ValidationException">Thrown when any of the input parameters are invalid.</exception>
    public static void CreateCompleteOwner(string firstName, string lastName, string email, string phoneNumber,
                                           string address)
    {
        Owner owner = new Owner(firstName, lastName, email, phoneNumber, address);
        _owners.Add(owner);
    }

    /// <summary>
    /// Finds an owner in the system by their unique ID.
    /// </summary>
    /// <param name="ownerId">The unique identifier for the owner.</param>
    /// <returns>An <see cref="Owner"/> object if found, otherwise null.</returns>
    /// <exception cref="ValidationException">Thrown when any of the input parameters are invalid.</exception>
    public static Owner FindOwnerById(int ownerId)
    {
        var owner = _owners.FindOwnerById(ownerId);

        return owner ?? throw new ArgumentException($"Owner with ID {ownerId} not found.");
    }

    /// <summary>
    /// Updates the details of an existing owner.
    /// </summary>
    /// <param name="ownerId">The unique ID of the owner to update.</param>
    /// <param name="firstName">The new first name of the owner.</param>
    /// <param name="lastName">The new last name of the owner.</param>
    /// <param name="email">The new email address of the owner.</param>
    /// <param name="phoneNumber">The new phone number of the owner.</param>
    /// <param name="address">The new address of the owner.</param>
    /// <exception cref="ArgumentException">Thrown when the owner ID is not found.</exception>
    public static void UpdateOwner(int ownerId, string firstName = "", string lastName = "", string email = "",
                                   string phoneNumber = "", string address = "")
    {
        var owner = FindOwnerById(ownerId);

        // Validate information before updating
        if (!string.IsNullOrEmpty(firstName))
            owner.FirstName = firstName;
        if (!string.IsNullOrEmpty(lastName))
            owner.LastName = lastName;
        if (!string.IsNullOrEmpty(email))
            owner.Email = email;
        if (!string.IsNullOrEmpty(phoneNumber))
            owner.PhoneNumber = phoneNumber;
        if (!string.IsNullOrEmpty(address))
            owner.Address = address;
    }

    /// <summary>
    /// Removes an owner from the system.
    /// </summary>
    /// <param name="ownerId">The unique ID of the owner to remove.</param>
    /// <exception cref="ArgumentException">Thrown when the owner ID is not found.</exception>
    public static void RemoveOwner(int ownerId)
    {
        var owner = FindOwnerById(ownerId);

        _owners.Remove(owner);
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
    public static Reservation CreateReservation(int clientId, int accommodationId, int roomId, DateTime checkIn,
                                                DateTime checkOut)
    {
        var accommodation = _accommodations.FindAccommodationById(accommodationId) ??
                            throw new ArgumentException("Accommodation not found.");

        var room = accommodation.FindRoomById(roomId) ?? throw new ArgumentException("Room not found.");

        // Calculate total cost
        decimal totalCost = room.CalculateTotalCost(checkIn, checkOut);

        // Create the reservation
        var reservation =
            new Reservation(clientId, accommodationId, roomId, accommodation.Type, checkIn, checkOut, totalCost);

        // Add reservation to accommodation
        bool success = room.AddReservation(checkIn, checkOut);
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
    /// <param name="newCheckIn">The new check-in date for the reservation, or <c>null</c> if no change is
    /// required.</param> <param name="newCheckOut">The new check-out date for the reservation, or <c>null</c> if no
    /// change is required.</param>
    /// <returns><c>true</c> if the reservation was successfully updated; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentException">Thrown when the reservation with the specified ID is
    /// not found, the accommodation or room is not found, or if the new dates are invalid or unavailable.</exception>
    /// <remarks>
    /// This method updates the check-in and check-out dates of a reservation if necessary. It checks the availability
    /// of the associated accommodation and room for the new dates. The method excludes the current reservation's
    /// existing date range when verifying availability. If no new dates are specified, the reservation remains
    /// unchanged.
    /// </remarks>
    public static bool UpdateReservation(int reservationId, DateTime? newCheckIn = null, DateTime? newCheckOut = null)
    {
        // Find the reservation by ID
        var reservation = _reservations.FindReservationById(reservationId);
        if (reservation == null)
            throw new ArgumentException($"Reservation with ID {reservationId} not found.");

        // Find the associated accommodation
        var accommodation = _accommodations.FindAccommodationById(reservation.AccommodationId);
        if (accommodation == null)
            throw new ArgumentException($"Accommodation with ID {reservation.AccommodationId} not found.");

        // Find the associated room
        var room = accommodation.FindRoomById(reservation.RoomId);
        if (room == null)
            throw new ArgumentException($"Room with ID {reservation.RoomId} not found.");

        // Determine the new check-in and check-out dates
        DateTime effectiveCheckIn = newCheckIn ?? reservation.CheckInDate;
        DateTime effectiveCheckOut = newCheckOut ?? reservation.CheckOutDate;

        // Validate the new dates, excluding the current reservation's dates from the conflict check
        if (!IsAvailableForUpdate(room, reservation.CheckInDate, reservation.CheckOutDate, effectiveCheckIn,
                                  effectiveCheckOut))
        {
            return false; // Not available
        }

        // Update the reserved dates in the room
        room.RemoveReservation(reservation.CheckInDate,
                               reservation.CheckOutDate);         // Safely remove current dates
        room.AddReservation(effectiveCheckIn, effectiveCheckOut); // Add the new dates

        // Update the reservation dates
        reservation.CheckInDate = effectiveCheckIn;
        reservation.CheckOutDate = effectiveCheckOut;

        return true;
    }

    /// <summary>
    /// Cancels a reservation by its unique ID, freeing up the associated accommodation for the specified dates.
    /// </summary>
    /// <param name="reservationId">The unique ID of the reservation to cancel.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when the reservation with the specified ID cannot be found, or the associated accommodation or room
    /// cannot be found.
    /// </exception>
    /// <remarks>
    /// This method cancels a reservation by marking its status as <see cref="ReservationStatus.Cancelled"/> and
    /// removing the reserved dates from the room's availability. It does not delete the reservation from the system;
    /// the system retains the historical record of the reservation for reporting or auditing purposes.
    /// </remarks>
    public static void CancelReservation(int reservationId)
    {
        // Find the reservation from the given ID
        var reservation = _reservations.FindReservationById(reservationId);
        if (reservation == null)
            throw new ArgumentException("Reservation not found.");

        // Find the associated accommodation
        var accommodation = _accommodations.FindAccommodationById(reservation.AccommodationId);
        if (accommodation == null)
            throw new ArgumentException($"Accommodation with ID {reservation.AccommodationId} not found.");

        // Find the associated room
        var room = accommodation.FindRoomById(reservation.RoomId);
        if (room == null)
            throw new ArgumentException($"Room with ID {reservation.RoomId} not found.");

        // Remove the reservation from the accommodation's reserved dates
        room.RemoveReservation(reservation.CheckInDate, reservation.CheckOutDate);

        // Set the reservation status to cancelled
        reservation.Status = ReservationStatus.Cancelled;
    }

    /// <summary>
    /// Checks whether a room is available for a new date range, excluding the current reservation's dates.
    /// </summary>
    /// <param name="room">The room to check.</param>
    /// <param name="currentStart">The current check-in date of the reservation.</param>
    /// <param name="currentEnd">The current check-out date of the reservation.</param>
    /// <param name="newStart">The proposed new check-in date.</param>
    /// <param name="newEnd">The proposed new check-out date.</param>
    /// <returns>True if the accommodation is available for the new dates; otherwise, false.</returns>
    private static bool IsAvailableForUpdate(Room room, DateTime currentStart, DateTime currentEnd, DateTime newStart,
                                             DateTime newEnd)
    {
        DateRange existingReservation = new DateRange(currentStart, currentEnd);

        // Exclude the current reservation's dates from the availability check
        return room.IsAvailable(newStart, newEnd, existingReservation);
    }

#endregion

#region Accommodation Management

    /// <summary>
    /// Creates and adds a new accommodation to the system.
    /// </summary>
    /// <param name="ownerId">The ID of the accommodation owner.</param>
    /// <param name="type">The type of the accommodation (e.g., hotel, apartment, etc.).</param>
    /// <param name="name">The name of the accommodation.</param>
    /// <param name="address">The address of the accommodation.</param>
    /// <returns>The newly created accommodation.</returns>
    /// <exception cref="ArgumentException">Thrown if the owner is not found.</exception>
    public static Accommodation CreateAccommodation(int ownerId, AccommodationType type, string name, string address)
    {
        var owner = _owners.FindOwnerById(ownerId);
        if (owner == null)
            throw new ArgumentException("Owner not found in the system.");

        var accommodation = new Accommodation(ownerId, type, name, address);
        _accommodations.Add(accommodation);
        owner.AddAccommodation(accommodation);

        return accommodation;
    }

    /// <summary>
    /// Updates the details of an existing accommodation.
    /// </summary>
    /// <param name="accommodationId">The ID of the accommodation to update.</param>
    /// <param name="type">The new type of the accommodation (optional).</param>
    /// <param name="name">The new name of the accommodation (optional).</param>
    /// <param name="address">The new address of the accommodation (optional).</param>
    /// <exception cref="ArgumentException">Thrown if the accommodation is not found.</exception>
    public static void UpdateAccommodation(int accommodationId, AccommodationType type = AccommodationType.None,
                                           string name = "", string address = "")
    {
        var accommodation = _accommodations.FindAccommodationById(accommodationId) ??
                            throw new ArgumentException("Accommodation not found in the system.");

        // Validate fields before updating
        ValidateField(type, AccommodationType.None,
                      Validation.Validators.AccommodationValidator.ValidateAccommodationType);
        ValidateField(name, "", Validation.Validators.NameValidator.ValidateAccommodationName);
        ValidateField(address, "", Validation.Validators.AddressValidator.ValidateAddress);

        // Use the SetField helper to update fields only if they are not the default
        SetField(type, AccommodationType.None, value => accommodation.Type = value);
        SetField(name, "", value => accommodation.Name = value);
        SetField(address, "", value => accommodation.Address = value);
    }

    /// <summary>
    /// Removes an accommodation from the system and disassociates it from its owner.
    /// This method will first ensure the accommodation exists in the system, then check if the owner of the
    /// accommodation is valid. If both are found, the accommodation is removed from the system and from the owner's
    /// list of accommodations.
    /// </summary>
    /// <param name="accommodationId">The unique ID of the accommodation to remove from the system.</param>
    /// <exception cref="ArgumentException">
    /// Thrown if the accommodation with the given ID is not found in the system.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown if the owner associated with the accommodation is not found in the system.
    /// </exception>
    /// <remarks>
    /// This method ensures that both the accommodation and the associated owner are updated in the system to reflect
    /// the removal. The accommodation is removed from the system, and the relationship between the accommodation and
    /// the owner is also removed.
    /// </remarks>
    public static void RemoveAccommodation(int accommodationId)
    {
        var accommodation = _accommodations.FindAccommodationById(accommodationId);
        if (accommodation == null)
            throw new ArgumentException("Accommodation not found in the system.");

        var owner = _owners.FindOwnerById(accommodation.OwnerId);
        if (owner == null)
            throw new ArgumentException("Owner not found in the system.");

        _accommodations.Remove(accommodation);
        owner.RemoveAccommodation(accommodation);
    }

#endregion
}
}

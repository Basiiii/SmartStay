/// <copyright file="BookingManager.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="BookingManager"/> class,
/// which manages client, reservation, and accommodation operations for the booking system.
/// It provides methods to add, remove, import, and export clients, reservations, and accommodations.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>11/11/2024</date>
#nullable enable
using Microsoft.Extensions.Logging;
using SmartStay.Common.Enums;
using SmartStay.Common.Exceptions;
using SmartStay.Core.Models;
using SmartStay.Core.Repositories;
using SmartStay.Core.Utilities;
using SmartStay.Validation;

/// <summary>
/// The <c>Core.Services</c> namespace contains service classes that implement business logic for the SmartStay
/// application. These services coordinate actions between repositories and models to fulfill application requirements.
/// </summary>
namespace SmartStay.Core.Services
{
/// <summary>
/// Provides a facade for managing clients, reservations, and accommodations in the booking system.
/// This class centralizes all operations for adding, removing, importing, and exporting data for these entities.
/// It interacts with internal repositories to simplify the main API and ensure a standardized approach.
/// </summary>
/// <remarks>
/// This class offers a unified interface for handling key booking operations and data entities, facilitating
/// integrations with other system components or external applications.
/// </remarks>
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Minor Code Smell", "S2325:Methods and properties that don't access instance data should be static",
    Justification = "This class cannot be static becacuse the Razor App requires direct injection, thus needs to " +
                    "create an instance of this class.")]
public class BookingManager
{
#region Fields and Properties

    /// <summary>
    /// The logger object for logging information, warnings, errors, etc.
    /// </summary>
    readonly ILogger<BookingManager> _logger;

    /// <summary>
    /// Holds the collection of all owners in the system, stored in the <see cref="Owners"/> repository.
    /// </summary>
    internal readonly Owners _owners = new();

    /// <summary>
    /// Holds the collection of all clients in the system, stored in the <see cref="Clients"/> repository.
    /// </summary>
    internal readonly Clients _clients = new();

    /// <summary>
    /// Holds the collection of all reservations in the system, stored in the <see cref="Reservations"/> repository.
    /// </summary>
    internal readonly Reservations _reservations = new();

    /// <summary>
    /// Holds the collection of all accommodations in the system, stored in the <see cref="Accommodations"/> repository.
    /// </summary>
    internal readonly Accommodations _accommodations = new();

#endregion

#region Constructor

    /// <summary>
    /// Constructor to initialize the <see cref="BookingManager"/> with logger and repository dependencies.
    /// </summary>
    /// <param name="logger">The logger to be used for logging activities within the BookingManager.</param>
    public BookingManager(ILogger<BookingManager> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _logger.LogInformation("BookingManager initialized.");
    }

#endregion

#region Accessors for Repositories

    /// <summary>
    /// Exposes the `Owners` repository as a read-only property.
    /// </summary>
    public Owners Owners => _owners;

    /// <summary>
    /// Exposes the `Clients` repository as a read-only property.
    /// </summary>
    public Clients Clients => _clients;

    /// <summary>
    /// Exposes the `Reservations` repository as a read-only property.
    /// </summary>
    public Reservations Reservations => _reservations;

    /// <summary>
    /// Exposes the `Accommodations` repository as a read-only property.
    /// </summary>
    public Accommodations Accommodations => _accommodations;

#endregion

#region Helper Functions

    /// <summary>
    /// Helper function to set a field's value if it is not the default value.
    /// </summary>
    /// <typeparam name="T">The type of the field (e.g., string, PaymentMethod).</typeparam>
    /// <param name="fieldValue">The value to set for the field.</param>
    /// <param name="defaultValue">The default value (e.g., empty string or PaymentMethod.Unchanged).</param>
    /// <param name="setterAction">The action to set the field if the value is not default.</param>
    private void SetField<T>(T fieldValue, T defaultValue, Action<T> setterAction)
    {
        // Only set the field if the value is not the default value
        if (!EqualityComparer<T>.Default.Equals(fieldValue, defaultValue))
        {
            setterAction(fieldValue); // Call the setter action with the field value
            _logger.LogInformation("Field set to {FieldValue}.", fieldValue); // Log the field change
        }
        else
        {
            _logger.LogInformation($"Field value is unchanged, remaining as default.");
        }
    }

#endregion

#region System Management

    /// <summary>
    /// Saves all repositories (Clients, Accommodations, Reservations, Owners) to their respective files.
    /// </summary>
    public void SaveAll(string dataFolder)
    {
        try
        {
            // Define file paths for each repository
            string clientsFilePath = Path.Combine(dataFolder, "clients.dat");
            string accommodationsFilePath = Path.Combine(dataFolder, "accommodations.dat");
            string reservationsFilePath = Path.Combine(dataFolder, "reservations.dat");
            string ownersFilePath = Path.Combine(dataFolder, "owners.dat");

            // Save each repository to its corresponding file
            _clients.Save(clientsFilePath);
            _accommodations.Save(accommodationsFilePath);
            _reservations.Save(reservationsFilePath);
            _owners.Save(ownersFilePath);

            _logger.LogInformation("All repositories saved successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while saving all repositories:");
            throw new InvalidOperationException("Error occurred while saving all repositories.", ex);
        }
    }

    /// <summary>
    /// Loads all repositories (Clients, Accommodations, Reservations, Owners) from their respective files.
    /// </summary>
    public void LoadAll(string dataFolder)
    {
        try
        {
            // Define file paths for each repository
            string clientsFilePath = Path.Combine(dataFolder, "clients.dat");
            string accommodationsFilePath = Path.Combine(dataFolder, "accommodations.dat");
            string reservationsFilePath = Path.Combine(dataFolder, "reservations.dat");
            string ownersFilePath = Path.Combine(dataFolder, "owners.dat");

            // Load each repository from its corresponding file
            if (File.Exists(clientsFilePath))
                _clients.Load(clientsFilePath);
            if (File.Exists(accommodationsFilePath))
                _accommodations.Load(accommodationsFilePath);
            if (File.Exists(reservationsFilePath))
                _reservations.Load(reservationsFilePath);
            if (File.Exists(ownersFilePath))
                _owners.Load(ownersFilePath);

            _logger.LogInformation("All repositories loaded successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while loading all repositories.");
            throw new InvalidOperationException("Error occurred while loading all repositories.", ex);
        }
    }

#endregion

#region Client Management

    /// <summary>
    /// Creates a new client with basic information and adds them to the system.
    /// This method validates the input parameters and handles any validation errors.
    /// If an exception occurs during the client creation, a <see cref="ClientCreationException"/> is thrown.
    /// </summary>
    /// <param name="firstName">The first name of the client.</param>
    /// <param name="lastName">The last name of the client.</param>
    /// <param name="email">The email address of the client.</param>
    /// <exception cref="ClientCreationException">Thrown when an error occurs during the creation of the client,
    /// typically due to invalid input parameters or other issues that prevent the client from being
    /// created.</exception>
    /// <returns>The <see cref="Client"/> object created.</returns>
    public Client CreateBasicClient(string firstName, string lastName, string email)
    {
        try
        {
            // Log information before creating the client
            _logger.LogInformation(
                "Attempting to create a new client with Name: {FirstName} {LastName}, Email: {Email}.", firstName,
                lastName, email);

            // Create a new client
            Client client = new Client(firstName, lastName, email); // May throw exception

            // Add client to the system
            _clients.Add(client);

            // Log success information
            _logger.LogInformation("Successfully created client: {FirstName} {LastName}, Email: {Email}.", firstName,
                                   lastName, email);

            // Return the created client
            return client;
        }
        catch (ValidationException ex)
        {
            // Log the exception with details
            _logger.LogError(ex, "Error while creating client with Name: {FirstName} {LastName}, Email: {Email}.",
                             firstName, lastName, email);

            // Throw a new exception with more context
            throw new ClientCreationException("An error occurred while creating the client due to invalid input.", ex);
        }
    }

    /// <summary>
    /// Creates a new client with all information and adds them to the system.
    /// This method validates the input parameters and handles any validation errors.
    /// If an exception occurs during the client creation, a <see cref="ClientCreationException"/> is thrown.
    /// </summary>
    /// <param name="firstName">The first name of the client.</param>
    /// <param name="lastName">The last name of the client.</param>
    /// <param name="email">The email address of the client.</param>
    /// <param name="phoneNumber">The phone number of the client.</param>
    /// <param name="address">The residential address of the client.</param>
    /// <exception cref="ClientCreationException">Thrown when an error occurs during the creation of the client,
    /// typically due to invalid input parameters or other issues that prevent the client from being
    /// created.</exception>
    /// <returns>The <see cref="Client"/> object created.</returns>
    public Client CreateCompleteClient(string firstName, string lastName, string email, string phoneNumber,
                                       string address)
    {
        try
        {
            // Log information before creating the client
            _logger.LogInformation(
                "Attempting to create a new client with Name: {FirstName} {LastName}, Email: {Email}, " +
                    "Phone Number: {PhoneNumber}, Address: {Address}.",
                firstName, lastName, email, phoneNumber, address);

            // Create a new client with the provided information
            Client client = new Client(firstName, lastName, email, phoneNumber, address); // May throw exception

            // Add client to the system
            _clients.Add(client);

            // Log success information
            _logger.LogInformation("Successfully created client: {FirstName} {LastName}, Email: {Email}, Phone: " +
                                       "{PhoneNumber}, Address: {Address}.",
                                   firstName, lastName, email, phoneNumber, address);

            // Return the created client
            return client;
        }
        catch (ValidationException ex)
        {
            // Log the exception with details
            _logger.LogError(ex,
                             "Error while creating client with Name: {FirstName} {LastName}, Email: {Email}, " +
                                 "Phone Number: {PhoneNumber}, Address: {Address}.",
                             firstName, lastName, email, phoneNumber, address);

            // Throw a new exception with more context
            throw new ClientCreationException("An error occurred while creating the client due to invalid input.", ex);
        }
    }

    /// <summary>
    /// Finds a client in the system by their unique ID.
    /// </summary>
    /// <param name="clientId">The unique identifier for the client.</param>
    /// <exception cref="ValidationException">Thrown when any of the input parameters are invalid.</exception>
    /// <returns>A <see cref="Client"/> object if found, otherwise throws an exception.</returns>
    public Client FindClientById(int clientId)
    {
        _logger.LogInformation("Attempting to find client with ID: {ClientId}.", clientId);

        var client = _clients.FindClientById(clientId);

        if (client != null)
        {
            _logger.LogInformation("Successfully found client with ID: {ClientId}.", clientId);
            return client;
        }
        else
        {
            _logger.LogError("Error finding client with ID: {ClientId}.", clientId);
            throw new ArgumentException($"Client with ID {clientId} not found.");
        }
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
    /// <returns>Returns an <see cref="UpdateClientResult"/> indicating the result of the update operation.</returns>
    /// <remarks>
    /// This method attempts to update the details of an existing client by validating the provided data.
    /// If the client with the specified ID is not found, it returns <see cref="UpdateClientResult.ClientNotFound"/>.
    /// If any of the fields fail validation, the corresponding error code is returned. If all validations pass,
    /// the client details are updated, and <see cref="UpdateClientResult.Success"/> is returned.
    /// </remarks>
    public UpdateClientResult UpdateClient(int clientId, string firstName = null, string lastName = null,
                                           string email = null, string phoneNumber = null, string address = null,
                                           PaymentMethod paymentMethod = PaymentMethod.Unchanged)
    {
        _logger.LogInformation("Attempting to update client with ID: {ClientId}.", clientId);

        // Find the client by ID
        var client = _clients.FindClientById(clientId);
        if (client == null)
        {
            _logger.LogWarning("Client with ID: {ClientId} not found.", clientId);
            return UpdateClientResult.ClientNotFound;
        }

        // Validate information before updating (only if not null or default value)
        if (firstName != null && !Validation.Validators.NameValidator.IsValidName(firstName))
        {
            _logger.LogError("Invalid first name provided for client ID: {ClientId}.", clientId);
            return UpdateClientResult.InvalidFirstName;
        }

        if (lastName != null && !Validation.Validators.NameValidator.IsValidName(lastName))
        {
            _logger.LogError("Invalid last name provided for client ID: {ClientId}.", clientId);
            return UpdateClientResult.InvalidLastName;
        }

        if (email != null && !Validation.Validators.EmailValidator.IsValidEmail(email))
        {
            _logger.LogError("Invalid email provided for client ID: {ClientId}.", clientId);
            return UpdateClientResult.InvalidEmail;
        }

        if (phoneNumber != null && !Validation.Validators.PhoneNumberValidator.IsValidPhoneNumber(phoneNumber))
        {
            _logger.LogError("Invalid phone number provided for client ID: {ClientId}.", clientId);
            return UpdateClientResult.InvalidPhoneNumber;
        }

        if (address != null && !Validation.Validators.AddressValidator.IsValidAddress(address))
        {
            _logger.LogError("Invalid address provided for client ID: {ClientId}.", clientId);
            return UpdateClientResult.InvalidAddress;
        }

        if (paymentMethod != PaymentMethod.Unchanged &&
            !Validation.Validators.PaymentValidator.IsValidPaymentMethod(paymentMethod))
        {
            _logger.LogError("Invalid payment method provided for client ID: {ClientId}.", clientId);
            return UpdateClientResult.InvalidPaymentMethod;
        }

        // Log success before updating
        _logger.LogInformation("Validations passed. Updating details for client ID: {ClientId}.", clientId);

        // Update client information with given fields, if not null or default
        SetField(firstName, null,
                 value =>
                 {
                     if (firstName != null)
                         client.FirstName = value;
                 });
        SetField(lastName, null,
                 value =>
                 {
                     if (lastName != null)
                         client.LastName = value;
                 });
        SetField(email, null,
                 value =>
                 {
                     if (email != null)
                         client.Email = value;
                 });
        SetField(phoneNumber, null,
                 value =>
                 {
                     if (phoneNumber != null)
                         client.PhoneNumber = value;
                 });
        SetField(address, null,
                 value =>
                 {
                     if (address != null)
                         client.Address = value;
                 });
        SetField(paymentMethod, PaymentMethod.Unchanged,
                 value =>
                 {
                     if (paymentMethod != PaymentMethod.Unchanged)
                         client.PreferredPaymentMethod = value;
                 });

        _logger.LogInformation("Successfully updated client details for client ID: {ClientId}.", clientId);

        return UpdateClientResult.Success;
    }

    /// <summary>
    /// Removes a client from the system.
    /// </summary>
    /// <param name="clientId">The unique ID of the client to remove.</param>
    /// <returns>True if the client was found and removed, otherwise false.</returns>
    public bool RemoveClient(int clientId)
    {
        _logger.LogInformation("Attempting to remove client with ID: {ClientId}.", clientId);

        // Find the client by ID
        var client = _clients.FindClientById(clientId);

        if (client == null)
        {
            _logger.LogWarning("Client with ID: {ClientId} not found. No removal performed.", clientId);
            return false;
        }

        // Remove the client from the list
        _clients.Remove(client);
        _logger.LogInformation("Successfully removed client with ID: {ClientId}.", clientId);

        return true;
    }

#endregion

#region Owner Management

    /// <summary>
    /// Creates a new owner with basic information and adds them to the system.
    /// This method validates the input parameters and handles any validation errors.
    /// If an exception occurs during the owner creation, a <see cref="OwnerCreationException"/> is thrown.
    /// </summary>
    /// <param name="firstName">The first name of the owner.</param>
    /// <param name="lastName">The last name of the owner.</param>
    /// <param name="email">The email address of the owner.</param>
    /// <exception cref="OwnerCreationException">Thrown when an error occurs during the creation of the owner,
    /// typically due to invalid input parameters or other issues that prevent the owner from being
    /// created.</exception>
    /// <returns>The <see cref="Owner"/> object created.</returns>
    public Owner CreateBasicOwner(string firstName, string lastName, string email)
    {
        try
        {
            // Log information before creating the owner
            _logger.LogInformation(
                "Attempting to create a new owner with Name: {FirstName} {LastName}, Email: {Email}.", firstName,
                lastName, email);

            // Create a new owner
            Owner owner = new Owner(firstName, lastName, email); // May throw exception

            // Add owner to the system
            _owners.Add(owner);

            // Log success information
            _logger.LogInformation("Successfully created owner: {FirstName} {LastName}, Email: {Email}.", firstName,
                                   lastName, email);

            // Return the created owner
            return owner;
        }
        catch (ValidationException ex)
        {
            // Log the exception with details
            _logger.LogError(ex, "Error while creating owner with Name: {FirstName} {LastName}, Email: {Email}.",
                             firstName, lastName, email);

            // Throw a new exception with more context
            throw new OwnerCreationException("An error occurred while creating the owner due to invalid input.", ex);
        }
    }

    /// <summary>
    /// Creates a new owner with all information and adds them to the system.
    /// This method validates the input parameters and handles any validation errors.
    /// If an exception occurs during the owner creation, a <see cref="OwnerCreationException"/> is thrown.
    /// </summary>
    /// <param name="firstName">The first name of the owner.</param>
    /// <param name="lastName">The last name of the owner.</param>
    /// <param name="email">The email address of the owner.</param>
    /// <param name="phoneNumber">The phone number of the owner.</param>
    /// <param name="address">The residential address of the owner.</param>
    /// <exception cref="OwnerCreationException">Thrown when an error occurs during the creation of the owner,
    /// typically due to invalid input parameters or other issues that prevent the owner from being
    /// created.</exception>
    /// <returns>The <see cref="Owner"/> object created.</returns>
    public Owner CreateCompleteOwner(string firstName, string lastName, string email, string phoneNumber,
                                     string address)
    {
        try
        {
            // Log information before creating the owner
            _logger.LogInformation(
                "Attempting to create a new owner with Name: {FirstName} {LastName}, Email: {Email}, " +
                    "Phone Number: {PhoneNumber}, Address: {Address}.",
                firstName, lastName, email, phoneNumber, address);

            // Create a new owner with the provided information
            Owner owner = new Owner(firstName, lastName, email, phoneNumber, address); // May throw exception

            // Add owner to the system
            _owners.Add(owner);

            // Log success information
            _logger.LogInformation("Successfully created owner: {FirstName} {LastName}, Email: {Email}.", firstName,
                                   lastName, email);

            // Return the created owner
            return owner;
        }
        catch (ValidationException ex)
        {
            // Log the exception with details
            _logger.LogError(ex,
                             "Error while creating owner with Name: {FirstName} {LastName}, Email: {Email}, " +
                                 "Phone Number: {PhoneNumber}, Address: {Address}.",
                             firstName, lastName, email, phoneNumber, address);

            // Throw a new exception with more context
            throw new OwnerCreationException("An error occurred while creating the owner due to invalid input.", ex);
        }
    }

    /// <summary>
    /// Finds an owner in the system by their unique ID.
    /// </summary>
    /// <param name="ownerId">The unique identifier for the owner.</param>
    /// <returns>An <see cref="Owner"/> object if found, otherwise null.</returns>
    /// <exception cref="ValidationException">Thrown when any of the input parameters are invalid.</exception>
    public Owner FindOwnerById(int ownerId)
    {
        // Log information before attempting to find the owner
        _logger.LogInformation("Attempting to find owner with ID: {OwnerId}.", ownerId);

        var owner = _owners.FindOwnerById(ownerId);

        if (owner != null)
        {
            // Log success information if the owner is found
            _logger.LogInformation("Successfully found owner with ID: {OwnerId}.", ownerId);
            return owner;
        }
        else
        {
            // Log error if the owner is not found
            _logger.LogError("Error finding owner with ID: {OwnerId}.", ownerId);
            throw new ArgumentException($"Owner with ID {ownerId} not found.");
        }
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
    /// <returns>Returns an <see cref="UpdateOwnerResult"/> indicating the result of the update operation.</returns>
    /// <remarks>
    /// This method attempts to update the details of an existing owner by validating the provided data.
    /// If the owner with the specified ID is not found, it returns <see cref="UpdateOwnerResult.OwnerNotFound"/>.
    /// If any of the fields fail validation, the corresponding error code is returned. If all validations pass,
    /// the owner details are updated, and <see cref="UpdateOwnerResult.Success"/> is returned.
    /// </remarks>
    public UpdateOwnerResult UpdateOwner(int ownerId, string firstName = null, string lastName = null,
                                         string email = null, string phoneNumber = null, string address = null)
    {
        _logger.LogInformation("Attempting to update owner with ID: {OwnerId}.", ownerId);

        // Find the owner by ID
        var owner = _owners.FindOwnerById(ownerId);
        if (owner == null)
        {
            _logger.LogWarning("Owner with ID: {OwnerId} not found.", ownerId);
            return UpdateOwnerResult.OwnerNotFound;
        }

        // Validate information before updating (only if not null)
        if (firstName != null && !Validation.Validators.NameValidator.IsValidName(firstName))
        {
            _logger.LogError("Invalid first name provided for owner ID: {OwnerId}.", ownerId);
            return UpdateOwnerResult.InvalidFirstName;
        }

        if (lastName != null && !Validation.Validators.NameValidator.IsValidName(lastName))
        {
            _logger.LogError("Invalid last name provided for owner ID: {OwnerId}.", ownerId);
            return UpdateOwnerResult.InvalidLastName;
        }

        if (email != null && !Validation.Validators.EmailValidator.IsValidEmail(email))
        {
            _logger.LogError("Invalid email provided for owner ID: {OwnerId}.", ownerId);
            return UpdateOwnerResult.InvalidEmail;
        }

        if (phoneNumber != null && !Validation.Validators.PhoneNumberValidator.IsValidPhoneNumber(phoneNumber))
        {
            _logger.LogError("Invalid phone number provided for owner ID: {OwnerId}.", ownerId);
            return UpdateOwnerResult.InvalidPhoneNumber;
        }

        if (address != null && !Validation.Validators.AddressValidator.IsValidAddress(address))
        {
            _logger.LogError("Invalid address provided for owner ID: {OwnerId}.", ownerId);
            return UpdateOwnerResult.InvalidAddress;
        }

        // Log success before updating
        _logger.LogInformation("Validations passed. Updating details for owner ID: {OwnerId}.", ownerId);

        // Update owner information with given fields, only if not null
        SetField(firstName, null,
                 value =>
                 {
                     if (firstName != null)
                         owner.FirstName = value;
                 });
        SetField(lastName, null,
                 value =>
                 {
                     if (lastName != null)
                         owner.LastName = value;
                 });
        SetField(email, null,
                 value =>
                 {
                     if (email != null)
                         owner.Email = value;
                 });
        SetField(phoneNumber, null,
                 value =>
                 {
                     if (phoneNumber != null)
                         owner.PhoneNumber = value;
                 });
        SetField(address, null,
                 value =>
                 {
                     if (address != null)
                         owner.Address = value;
                 });

        _logger.LogInformation("Successfully updated owner details for owner ID: {OwnerId}.", ownerId);

        return UpdateOwnerResult.Success;
    }

    /// <summary>
    /// Removes an owner from the system.
    /// </summary>
    /// <param name="ownerId">The unique ID of the owner to remove.</param>
    /// <returns>True if the owner was found and removed, otherwise false.</returns>
    public bool RemoveOwner(int ownerId)
    {
        _logger.LogInformation("Attempting to remove owner with ID: {OwnerId}.", ownerId);

        var owner = _owners.FindOwnerById(ownerId);
        if (owner != null)
        {
            _owners.Remove(owner);
            _logger.LogInformation("Successfully removed owner with ID: {OwnerId}.", ownerId);
            return true;
        }
        else
        {
            _logger.LogWarning("Owner with ID: {OwnerId} not found, unable to remove.", ownerId);
            return false;
        }
    }

#endregion

#region Reservation Management

    /// <summary>
    /// Creates a new reservation for a specified client, accommodation, and room within a given date range.
    /// </summary>
    /// <param name="clientId">The unique identifier of the client creating the reservation.</param>
    /// <param name="accommodationId">The unique identifier of the accommodation where the reservation is being
    /// made.</param> <param name="roomId">The unique identifier of the room to be reserved.</param> <param
    /// name="checkIn">The check-in date for the reservation.</param> <param name="checkOut">The check-out date for the
    /// reservation.</param> <returns> A <see cref="Reservation"/> object representing the successfully created
    /// reservation.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if:
    /// <list type="bullet">
    /// <item><description>The specified accommodation is not found.</description></item>
    /// <item><description>The specified room is not found within the accommodation.</description></item>
    /// <item><description>The specified room is unavailable for the given date range.</description></item>
    /// <item><description>The reservation could not be added to the reservation list.</description></item>
    /// </list>
    /// </exception>
    /// <exception cref="TotalCostException">
    /// Thrown if there is an error calculating the total cost of the reservation.
    /// </exception>
    /// <exception cref="ReservationCreationException">
    /// Thrown if there is an error validating the reservation details during creation.
    /// </exception>
    /// <remarks>
    /// This method performs the following steps:
    /// <list type="number">
    /// <item><description>Logs the reservation attempt with the provided parameters.</description></item>
    /// <item><description>Finds the specified accommodation and room.</description></item>
    /// <item><description>Validates the room's availability using <see cref="Room.IsAvailable"/>.</description></item>
    /// <item><description>Calculates the total cost of the reservation using <see
    /// cref="Room.CalculateTotalCost"/>.</description></item> <item><description>Creates a new <see
    /// cref="Reservation"/> object after validation.</description></item> <item><description>Attempts to add the
    /// reservation to the room and the central reservation list.</description></item>
    /// </list>
    /// If any step fails, appropriate exceptions are thrown with detailed logging.
    /// </remarks>
    public Reservation CreateReservation(int clientId, int accommodationId, int roomId, DateTime checkIn,
                                         DateTime checkOut)
    {
        // Log the attempt to create a reservation
        _logger.LogInformation("Attempting to create reservation for client {ClientId} at accommodation " +
                                   "{AccommodationId}, room {RoomId}, from {CheckIn} to {CheckOut}.",
                               clientId, accommodationId, roomId, checkIn, checkOut);

        // Find the accommodation
        var accommodation = _accommodations.FindAccommodationById(accommodationId);
        if (accommodation == null)
        {
            _logger.LogError("Accommodation with ID {AccommodationId} not found.", accommodationId);
            throw new ArgumentException("Accommodation not found.");
        }

        // Find the room in the accommodation
        var room = accommodation.FindRoomById(roomId);
        if (room == null)
        {
            _logger.LogError("Room with ID {RoomId} not found in accommodation {AccommodationId}.", roomId,
                             accommodationId);
            throw new ArgumentException("Room not found.");
        }

        // Check if room is available upfront for a quick exit. AddReservation will validate again to ensure
        // consistency.
        var available = room.IsAvailable(checkIn, checkOut);
        if (!available)
        {
            _logger.LogError("Room {RoomId} in accommodation {AccommodationId} is not available for the dates " +
                                 "{CheckIn} to {CheckOut}.",
                             roomId, accommodationId, checkIn, checkOut);
            throw new ArgumentException("Accommodation is not available for the selected dates.");
        }

        // Calculate the total cost of the reservation
        decimal totalCost = 0;
        try
        {
            totalCost = room.CalculateTotalCost(checkIn, checkOut);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Error calculating total cost for reservation: {Message}", ex.Message);
            throw new TotalCostException("An error occurred while calculating the total cost for the reservation.", ex);
        }

        // Log the total cost calculation
        _logger.LogInformation(
            "Calculated total cost for reservation: {TotalCost} for {ClientId} from {CheckIn} to {CheckOut}.",
            totalCost, clientId, checkIn, checkOut);

        // Create the reservation
        Reservation reservation;
        try
        {
            reservation =
                new Reservation(clientId, accommodationId, roomId, accommodation.Type, checkIn, checkOut, totalCost);
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex,
                             "Validation failed when creating reservation for client {ClientId} at accommodation " +
                                 "{AccommodationId}, room {RoomId}. Error: {ErrorCode}",
                             clientId, accommodationId, roomId, ex.ErrorCode);
            throw new ReservationCreationException(
                $"An error occurred while creating the reservation due to invalid input.", ex);
        }

        // Attempt to add the reservation to the room (this also checks for availability)
        bool success = room.AddReservation(checkIn, checkOut);
        if (!success)
        {
            _logger.LogError("Room {RoomId} in accommodation {AccommodationId} is not available for the dates " +
                                 "{CheckIn} to {CheckOut}.",
                             roomId, accommodationId, checkIn, checkOut);
            throw new ArgumentException("Accommodation is not available for the selected dates.");
        }

        // Add the reservation to the reservation list
        bool reservationAdded = _reservations.Add(reservation);
        if (!reservationAdded)
        {
            _logger.LogError("Failed to add reservation for client {ClientId} at accommodation " +
                                 "{AccommodationId}, room {RoomId}. Reservation may already exist.",
                             clientId, accommodationId, roomId);
            throw new ArgumentException("Failed to add reservation.");
        }

        // Log successful reservation creation
        _logger.LogInformation("Successfully created reservation {ReservationId} for client {ClientId} at " +
                                   "accommodation {AccommodationId}, room {RoomId}. Total cost: {TotalCost}.",
                               reservation.Id, clientId, accommodationId, roomId, totalCost);

        // Return the newly created reservation
        return reservation;
    }

    /// <summary>
    /// Updates the check-in and/or check-out dates of an existing reservation.
    /// </summary>
    /// <param name="reservationId">The unique ID of the reservation to update.</param>
    /// <param name="newCheckIn">The new check-in date for the reservation, or <c>null</c> if no change is
    /// required.</param> <param name="newCheckOut">The new check-out date for the reservation, or <c>null</c> if no
    /// change is required.</param> <returns>Returns an <see cref="UpdateReservationResult"/> indicating the result of
    /// the update operation.</returns> <remarks> This method updates the check-in and check-out dates of a reservation
    /// if necessary. It checks the availability of the associated accommodation and room for the new dates. The method
    /// excludes the current reservation's existing date range when verifying availability. If no new dates are
    /// specified, the reservation remains unchanged.
    ///
    /// The following conditions are checked during the update:
    /// <list type="bullet">
    ///     <item>Whether the reservation exists</item>
    ///     <item>Whether the accommodation and room associated with the reservation exist</item>
    ///     <item>Whether the new dates are valid (check-out must be later than check-in)</item>
    ///     <item>Whether the room is available for the new dates (excluding the current reservation's dates)</item>
    /// </list>
    /// </remarks>
    public UpdateReservationResult UpdateReservation(int reservationId, DateTime? newCheckIn = null,
                                                     DateTime? newCheckOut = null)
    {
        _logger.LogInformation("Attempting to update reservation {ReservationId}.", reservationId);

        try
        {
            // Use helper to find the reservation
            var reservation = FindReservation(reservationId);

            // Use helper to find accommodation and room
            var (accommodation, room) = FindAssociatedEntities(reservation);

            // Determine effective check-in and check-out dates
            var (effectiveCheckIn, effectiveCheckOut) = GetEffectiveDates(reservation, newCheckIn, newCheckOut);

            _logger.LogInformation(
                "Effective dates for reservation {ReservationId}: Check-In: {CheckIn}, Check-Out: {CheckOut}.",
                reservationId, effectiveCheckIn, effectiveCheckOut);

            // Check availability using helper
            if (!IsAvailableForUpdate(room, reservation.CheckInDate, reservation.CheckOutDate, effectiveCheckIn,
                                      effectiveCheckOut))
            {
                _logger.LogWarning(
                    "Room {RoomId} in accommodation {AccommodationId} is unavailable for the new dates: " +
                        "Check-In: {CheckIn}, Check-Out: {CheckOut}.",
                    room.Id, accommodation.Id, effectiveCheckIn, effectiveCheckOut);
                return UpdateReservationResult.DatesUnavailable;
            }

            // Update room reservation (handles removing and adding)
            UpdateRoomReservation(room, reservation.CheckInDate, reservation.CheckOutDate, effectiveCheckIn,
                                  effectiveCheckOut);

            // Update reservation dates
            reservation.CheckInDate = effectiveCheckIn;
            reservation.CheckOutDate = effectiveCheckOut;

            _logger.LogInformation("Successfully updated reservation {ReservationId}.", reservationId);
            return UpdateReservationResult.Success;
        }
        catch (EntityNotFoundException ex) when (ex.EntityType == nameof(Reservation))
        {
            _logger.LogWarning(ex, "Reservation with ID {ReservationId} not found.", reservationId);
            return UpdateReservationResult.ReservationNotFound;
        }
        catch (EntityNotFoundException ex) when (ex.EntityType == nameof(Accommodation))
        {
            _logger.LogWarning(ex, "Accommodation for reservation {ReservationId} not found.", reservationId);
            return UpdateReservationResult.AccommodationNotFound;
        }
        catch (EntityNotFoundException ex) when (ex.EntityType == nameof(Room))
        {
            _logger.LogWarning(ex, "Room for reservation {ReservationId} not found.", reservationId);
            return UpdateReservationResult.RoomNotFound;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogWarning(ex, "Room was null for reservation {ReservationId}.", reservationId);
            return UpdateReservationResult.RoomNotFound;
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid check-in/check-out dates for reservation {ReservationId}.", reservationId);
            return UpdateReservationResult.InvalidDates;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating reservation {ReservationId}.", reservationId);
            return UpdateReservationResult.Error;
        }
    }

    /// <summary>
    /// Cancels a reservation by its unique ID, freeing up the associated accommodation for the specified dates.
    /// </summary>
    /// <param name="reservationId">The unique ID of the reservation to cancel.</param>
    /// <returns>
    /// A <see cref="CancellationResult"/> value indicating the outcome of the cancellation attempt.
    /// </returns>
    /// <remarks>
    /// This method cancels a reservation, which involves:
    /// <list type="bullet">
    ///     <item>Finding the reservation by its unique ID.</item>
    ///     <item>Finding the associated accommodation and room based on the reservation.</item>
    ///     <item>Removing the reservation from the room's reserved dates.</item>
    ///     <item>Marking the reservation as cancelled and freeing up the accommodation for future bookings.</item>
    /// </list>
    ///
    /// The following exceptions are handled during the cancellation process:
    /// <list type="bullet">
    ///     <item>If the reservation does not exist, a <see cref="EntityNotFoundException"/> is thrown.</item>
    ///     <item>If the accommodation or room associated with the reservation cannot be found, a <see
    ///     cref="EntityNotFoundException"/> is thrown.</item>
    /// </list>
    ///
    /// If an error occurs during the cancellation process (e.g., failure to remove the reservation from the room),
    /// an appropriate error message is logged and the cancellation attempt is considered a failure.
    ///
    /// <note type="warning">
    /// The cancellation will not succeed if the room cannot be found or if there is an error in removing the
    /// reservation.
    /// </note>
    /// </remarks>
    public CancellationResult CancelReservation(int reservationId)
    {
        _logger.LogInformation("Attempting to cancel reservation {ReservationId}.", reservationId);

        try
        {
            // Use helper to find the reservation
            var reservation = FindReservation(reservationId);

            // Use helper to find the associated accommodation and room
            var (accommodation, room) = FindAssociatedEntities(reservation);

            // Attempt to remove the reservation from the room's reserved dates
            bool removeResult = room.RemoveReservation(reservation.CheckInDate, reservation.CheckOutDate);
            if (!removeResult)
            {
                _logger.LogError("Failed to remove reservation {ReservationId} from room {RoomId} in accommodation " +
                                     "{AccommodationId}.",
                                 reservationId, room.Id, accommodation.Id);
                return CancellationResult.Error;
            }

            // Mark the reservation as cancelled
            reservation.Status = ReservationStatus.Cancelled;
            _logger.LogInformation("Successfully cancelled reservation {ReservationId}.", reservationId);

            return CancellationResult.Success;
        }
        catch (EntityNotFoundException ex) when (ex.EntityType == nameof(Reservation))
        {
            _logger.LogWarning(ex, "Reservation with ID {ReservationId} not found.", reservationId);
            return CancellationResult.ReservationNotFound;
        }
        catch (EntityNotFoundException ex) when (ex.EntityType == nameof(Accommodation))
        {
            _logger.LogWarning(ex, "Accommodation for reservation {ReservationId} not found.", reservationId);
            return CancellationResult.AccommodationNotFound;
        }
        catch (EntityNotFoundException ex) when (ex.EntityType == nameof(Room))
        {
            _logger.LogWarning(ex, "Room for reservation {ReservationId} not found.", reservationId);
            return CancellationResult.RoomNotFound;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while cancelling reservation {ReservationId}.", reservationId);
            return CancellationResult.Error;
        }
    }

#region Reservation Helper Functions

    /// <summary>
    /// Checks whether a room is available for a new date range, excluding the current reservation's dates.
    /// </summary>
    /// <param name="room">The room to check.</param>
    /// <param name="currentStart">The current check-in date of the reservation.</param>
    /// <param name="currentEnd">The current check-out date of the reservation.</param>
    /// <param name="newStart">The proposed new check-in date.</param>
    /// <param name="newEnd">The proposed new check-out date.</param>
    /// <returns>True if the accommodation is available for the new dates; otherwise, false.</returns>
    /// <exception cref="ArgumentException">Thrown if the new date range is invalid.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the room is null.</exception>
    private bool IsAvailableForUpdate(Room room, DateTime currentStart, DateTime currentEnd, DateTime newStart,
                                      DateTime newEnd)
    {
        if (room == null)
            throw new ArgumentNullException(nameof(room), "Room cannot be null.");

        if (newEnd <= newStart)
            throw new ArgumentException("New check-out date must be after the new check-in date.");

        DateRange existingReservation = new DateRange(currentStart, currentEnd);

        _logger?.LogDebug("Checking availability for update: Current [{CurrentStart} - {CurrentEnd}], Proposed " +
                              "[{NewStart} - {NewEnd}].",
                          currentStart, currentEnd, newStart, newEnd);

        // Exclude the current reservation's dates from the availability check
        return room.IsAvailable(newStart, newEnd, existingReservation);
    }

    /// <summary>
    /// Finds a reservation by its unique ID.
    /// </summary>
    /// <param name="reservationId">The unique ID of the reservation.</param>
    /// <returns>The <see cref="Reservation"/> object if found.</returns>
    /// <exception cref="EntityNotFoundException">Thrown if the reservation is not found.</exception>
    private Reservation FindReservation(int reservationId)
    {
        _logger.LogInformation("Looking for reservation with ID {ReservationId}.", reservationId);

        var reservation = _reservations.FindReservationById(reservationId);
        if (reservation == null)
        {
            _logger.LogWarning("Reservation with ID {ReservationId} not found.", reservationId);
            throw new EntityNotFoundException(nameof(Reservation), reservationId);
        }

        _logger.LogInformation("Successfully found reservation with ID {ReservationId}.", reservationId);
        return reservation;
    }

    /// <summary>
    /// Finds the accommodation and room associated with a reservation.
    /// </summary>
    /// <param name="reservation">The reservation for which to find associated entities.</param>
    /// <returns>
    /// A tuple containing the <see cref="Accommodation"/> and <see cref="Room"/> objects.
    /// </returns>
    /// <exception cref="EntityNotFoundException">Thrown if the accommodation or room is not found.</exception>
    private (Accommodation, Room) FindAssociatedEntities(Reservation reservation)
    {
        _logger.LogInformation("Finding associated accommodation and room for reservation ID {ReservationId}.",
                               reservation.Id);

        // Find accommodation
        var accommodation = _accommodations.FindAccommodationById(reservation.AccommodationId);
        if (accommodation == null)
        {
            _logger.LogWarning("Accommodation with ID {AccommodationId} not found for reservation {ReservationId}.",
                               reservation.AccommodationId, reservation.Id);
            throw new EntityNotFoundException(nameof(Accommodation), reservation.AccommodationId);
        }

        _logger.LogInformation(
            "Successfully found accommodation with ID {AccommodationId} for reservation {ReservationId}.",
            accommodation.Id, reservation.Id);

        // Find room
        var room = accommodation.FindRoomById(reservation.RoomId);
        if (room == null)
        {
            _logger.LogWarning(
                "Room with ID {RoomId} not found in accommodation {AccommodationId} for reservation {ReservationId}.",
                reservation.RoomId, accommodation.Id, reservation.Id);
            throw new EntityNotFoundException(nameof(Room), reservation.RoomId);
        }

        _logger.LogInformation("Successfully found room with ID {RoomId} in accommodation {AccommodationId} for " +
                                   "reservation {ReservationId}.",
                               room.Id, accommodation.Id, reservation.Id);

        return (accommodation, room);
    }

    /// <summary>
    /// Determines the effective check-in and check-out dates for a reservation update.
    /// </summary>
    /// <param name="reservation">The reservation being updated.</param>
    /// <param name="newCheckIn">The new check-in date, or <c>null</c> if no change is required.</param>
    /// <param name="newCheckOut">The new check-out date, or <c>null</c> if no change is required.</param>
    /// <returns>
    /// A tuple containing the effective check-in and check-out dates.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if the new dates are invalid.</exception>
    private (DateTime, DateTime) GetEffectiveDates(Reservation reservation, DateTime? newCheckIn, DateTime? newCheckOut)
    {
        _logger.LogInformation("Determining effective dates for reservation ID {ReservationId}.", reservation.Id);

        DateTime effectiveCheckIn = newCheckIn ?? reservation.CheckInDate;
        DateTime effectiveCheckOut = newCheckOut ?? reservation.CheckOutDate;

        if (effectiveCheckOut <= effectiveCheckIn)
        {
            _logger.LogError(
                "Invalid dates for reservation {ReservationId}: Check-In: {CheckIn}, Check-Out: {CheckOut}.",
                reservation.Id, effectiveCheckIn, effectiveCheckOut);
            throw new ArgumentException("Check-out date must be later than check-in date.");
        }

        _logger.LogInformation(
            "Effective dates for reservation {ReservationId}: Check-In: {CheckIn}, Check-Out: {CheckOut}.",
            reservation.Id, effectiveCheckIn, effectiveCheckOut);

        return (effectiveCheckIn, effectiveCheckOut);
    }

    /// <summary>
    /// Updates the room reservation by removing the current dates and adding the new dates.
    /// </summary>
    /// <param name="room">The room to update.</param>
    /// <param name="currentStart">The current check-in date of the reservation.</param>
    /// <param name="currentEnd">The current check-out date of the reservation.</param>
    /// <param name="newStart">The new check-in date for the reservation.</param>
    /// <param name="newEnd">The new check-out date for the reservation.</param>
    /// <exception cref="Exception">Thrown if the update fails.</exception>
    private void UpdateRoomReservation(Room room, DateTime currentStart, DateTime currentEnd, DateTime newStart,
                                       DateTime newEnd)
    {
        _logger.LogInformation(
            "Updating room reservation for Room {RoomId}: Removing dates {CurrentStart} to {CurrentEnd}, " +
                "and adding dates {NewStart} to {NewEnd}.",
            room.Id, currentStart, currentEnd, newStart, newEnd);

        // Safely remove current reservation dates
        room.RemoveReservation(currentStart, currentEnd);

        // Add the new reservation dates
        room.AddReservation(newStart, newEnd);

        _logger.LogInformation(
            "Successfully updated room reservation for Room {RoomId} with new dates {NewStart} to {NewEnd}.", room.Id,
            newStart, newEnd);
    }

#endregion

#endregion

#region Accommodation Management

    /// <summary>
    /// Creates and adds a new accommodation to the system. This method validates the input parameters, checks if the
    /// owner exists, and handles any validation or system errors. If any issue occurs during the creation of the
    /// accommodation or adding it to the system, specific exceptions are thrown to handle the error appropriately.
    /// </summary>
    /// <param name="ownerId">The unique ID of the accommodation owner.</param>
    /// <param name="type">The type of the accommodation (e.g., hotel, apartment, etc.).</param>
    /// <param name="name">The name of the accommodation (e.g., "Luxury Hotel").</param>
    /// <param name="address">The address of the accommodation (e.g., "123 Main St, City, Country").</param>
    /// <returns>
    /// The newly created <see cref="Accommodation"/> object, representing the accommodation that was successfully
    /// created and added.
    /// </returns>
    /// <exception cref="EntityNotFoundException">
    /// Thrown if the owner with the specified ID is not found in the system.
    /// This ensures that the accommodation is associated with a valid owner before creation.
    /// </exception>
    /// <exception cref="OwnerAddAccommodationException">
    /// Thrown if the accommodation could not be added to the owner's list of accommodations.
    /// This exception is raised if the owner has any issues adding the accommodation to their list, such as a conflict
    /// or internal error.
    /// </exception>
    /// <exception cref="AddAccommodationSystemException">
    /// Thrown if an error occurs while adding the accommodation to the system (e.g., system error, storage issue).
    /// This ensures that the accommodation is successfully added to the system after being added to the owner's list.
    /// </exception>
    /// <exception cref="AccommodationCreationException">
    /// Thrown if an error occurs during the accommodation creation process, including issues with owner association or
    /// system errors. This is a general exception to catch and rethrow more specific errors during the accommodation
    /// creation process.
    /// </exception>
    /// <remarks>
    /// This method attempts to create a new accommodation, ensuring that the accommodation type, name, and address are
    /// valid. The owner is validated by ID, and the accommodation is added both to the owner's list and the overall
    /// system. If any step fails, appropriate exceptions are thrown to allow for specific error handling.
    /// </remarks>
    public Accommodation CreateAccommodation(int ownerId, AccommodationType type, string name, string address)
    {
        try
        {
            // Log information before creating the accommodation
            _logger.LogInformation("Attempting to create a new accommodation for owner ID {OwnerId}: Type: {Type}, " +
                                       "Name: {Name}, Address: {Address}.",
                                   ownerId, type, name, address);

            // Find the owner by ID
            var owner = _owners.FindOwnerById(ownerId);
            if (owner == null)
            {
                _logger.LogError("Owner with ID {OwnerId} not found.", ownerId);
                throw new EntityNotFoundException(nameof(Owner), ownerId);
            }

            // Create a new accommodation
            var accommodation = new Accommodation(ownerId, type, name, address); // May throw exception

            // Attempt to add the accommodation to the owner's list of accommodations
            bool addSuccess = owner.AddAccommodation(accommodation);
            if (!addSuccess)
            {
                _logger.LogError("Failed to add accommodation {Name} to owner ID {OwnerId}.", name, ownerId);
                throw new OwnerAddAccommodationException("Failed to add accommodation to owner's list.");
            }

            // Attempt to add the accommodation to the system
            bool addToSystemSuccess = _accommodations.Add(accommodation);
            if (!addToSystemSuccess)
            {
                _logger.LogError("Failed to add accommodation {Name} to the system.", name);
                throw new AddAccommodationSystemException(
                    "An error occurred while adding the accommodation to the system.");
            }

            // Log success information
            _logger.LogInformation(
                "Successfully created accommodation: {Type}, {Name}, Address: {Address} for owner ID {OwnerId}.", type,
                name, address, ownerId);

            // Return the created accommodation
            return accommodation;
        }
        catch (EntityNotFoundException ex)
        {
            // Log the exception with details
            _logger.LogError(ex,
                             "Error while creating accommodation for owner ID {OwnerId}: Type: {Type}, Name: {Name}, " +
                                 "Address: {Address}.",
                             ownerId, type, name, address);

            // Rethrow the exception as a more specific one
            throw new AccommodationCreationException(
                "An error occurred while creating the accommodation due to missing owner.", ex);
        }
        catch (OwnerAddAccommodationException ex)
        {
            // Log the exception if adding accommodation fails
            _logger.LogError(ex, "Error while adding accommodation {Name} for owner ID {OwnerId}.", name, ownerId);
            throw new AccommodationCreationException(
                "An error occurred while trying to add accommodation to the owner accommodation list.");
        }
        catch (AddAccommodationSystemException ex)
        {
            // Log the exception if adding accommodation fails
            _logger.LogError(ex, "Error while adding accommodation {Name} to the system.", name);
            throw new AccommodationCreationException(
                "An error occurred while trying to add accommodation to the system.");
        }
    }

    /// <summary>
    /// Updates the details of an existing accommodation.
    /// </summary>
    /// <param name="accommodationId">The ID of the accommodation to update.</param>
    /// <param name="type">The new type of the accommodation (optional).</param>
    /// <param name="name">The new name of the accommodation (optional).</param>
    /// <param name="address">The new address of the accommodation (optional).</param>
    /// <returns>Returns an <see cref="UpdateAccommodationResult"/> indicating the result of the update
    /// operation.</returns> <remarks> This method updates the type, name, and address of an accommodation. It performs
    /// validation before updating any fields and ensures that only valid information is used to update the
    /// accommodation details.
    /// </remarks>
    public UpdateAccommodationResult UpdateAccommodation(int accommodationId,
                                                         AccommodationType type = AccommodationType.None,
                                                         string name = null, string address = null)
    {
        _logger.LogInformation("Attempting to update accommodation with ID: {AccommodationId}.", accommodationId);

        // Find the accommodation by ID
        var accommodation = _accommodations.FindAccommodationById(accommodationId);
        if (accommodation == null)
        {
            _logger.LogWarning("Accommodation with ID: {AccommodationId} not found.", accommodationId);
            return UpdateAccommodationResult.AccommodationNotFound;
        }

        // Validate information before updating (only if not default value)
        if (type != AccommodationType.None &&
            !Validation.Validators.AccommodationValidator.IsValidAccommodationType(type))
        {
            _logger.LogError("Invalid type provided for accommodation ID: {AccommodationId}.", accommodationId);
            return UpdateAccommodationResult.InvalidType;
        }

        if (name != null && !Validation.Validators.NameValidator.IsValidAccommodationName(name))
        {
            _logger.LogError("Invalid name provided for accommodation ID: {AccommodationId}.", accommodationId);
            return UpdateAccommodationResult.InvalidName;
        }

        if (address != null && !Validation.Validators.AddressValidator.IsValidAddress(address))
        {
            _logger.LogError("Invalid address provided for accommodation ID: {AccommodationId}.", accommodationId);
            return UpdateAccommodationResult.InvalidAddress;
        }

        // Log success before updating
        _logger.LogInformation("Validations passed. Updating details for accommodation ID: {AccommodationId}.",
                               accommodationId);

        // Update accommodation information with given fields, only if not default
        SetField(type, AccommodationType.None,
                 value =>
                 {
                     if (type != AccommodationType.None)
                         accommodation.Type = value;
                 });
        SetField(name, null,
                 value =>
                 {
                     if (name != null)
                         accommodation.Name = value;
                 });
        SetField(address, null,
                 value =>
                 {
                     if (address != null)
                         accommodation.Address = value;
                 });

        // Log success after updating
        _logger.LogInformation("Successfully updated accommodation details for accommodation ID: {AccommodationId}.",
                               accommodationId);

        return UpdateAccommodationResult.Success;
    }

    /// <summary>
    /// Removes an accommodation from the system and disassociates it from its owner.
    /// This method will first ensure the accommodation exists in the system, then check if the owner of the
    /// accommodation is valid. If both are found, the accommodation is removed from the system and from the owner's
    /// list of accommodations.
    /// </summary>
    /// <param name="accommodationId">The unique ID of the accommodation to remove from the system.</param>
    /// <returns>Returns a <see cref="RemoveAccommodationResult"/> indicating the result of the removal
    /// operation.</returns> <remarks> This method ensures that both the accommodation and the associated owner are
    /// updated in the system to reflect the removal. The accommodation is removed from the system, and the relationship
    /// between the accommodation and the owner is also removed.
    /// </remarks>
    public RemoveAccommodationResult RemoveAccommodation(int accommodationId)
    {
        _logger.LogInformation("Attempting to remove accommodation with ID: {AccommodationId}.", accommodationId);

        try
        {
            // Find the accommodation by ID
            var accommodation = _accommodations.FindAccommodationById(accommodationId);
            if (accommodation == null)
            {
                _logger.LogWarning("Accommodation with ID: {AccommodationId} not found, unable to remove.",
                                   accommodationId);
                return RemoveAccommodationResult.AccommodationNotFound;
            }

            // Find the owner by ID
            var owner = _owners.FindOwnerById(accommodation.OwnerId);
            if (owner == null)
            {
                _logger.LogWarning("Owner with ID: {OwnerId} not found for accommodation ID: {AccommodationId}.",
                                   accommodation.OwnerId, accommodationId);
                return RemoveAccommodationResult.OwnerNotFound;
            }

            // Log before removal
            _logger.LogInformation("Owner with ID: {OwnerId} found. Removing accommodation ID: {AccommodationId}.",
                                   accommodation.OwnerId, accommodationId);

            // Attempt to remove accommodation from the system
            bool removeSuccess = _accommodations.Remove(accommodation);
            if (!removeSuccess)
            {
                _logger.LogError("Failed to remove accommodation ID: {AccommodationId} from the system.",
                                 accommodationId);
                return RemoveAccommodationResult.AccommodationRemovalFailed;
            }

            // Attempt to remove accommodation from the owner's list
            bool ownerRemoveSuccess = owner.RemoveAccommodation(accommodation);
            if (!ownerRemoveSuccess)
            {
                _logger.LogError("Failed to disassociate accommodation ID: {AccommodationId} from owner ID: {OwnerId}.",
                                 accommodationId, accommodation.OwnerId);
                return RemoveAccommodationResult.AccommodationDisassociationFailed;
            }

            // Log success after removal
            _logger.LogInformation("Successfully removed accommodation ID: {AccommodationId} and disassociated it " +
                                       "from owner ID: {OwnerId}.",
                                   accommodationId, accommodation.OwnerId);

            return RemoveAccommodationResult.Success;
        }
        catch (Exception ex)
        {
            // Log any unexpected errors
            _logger.LogError(ex, "Unexpected error occurred while removing accommodation with ID: {AccommodationId}.",
                             accommodationId);
            return RemoveAccommodationResult.Error;
        }
    }

#endregion
}
}

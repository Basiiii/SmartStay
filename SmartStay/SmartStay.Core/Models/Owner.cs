/// <copyright file="Owner.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the <see cref="Owner"/> class, which stores information about individual accommodation owners,
/// including their personal details and associated accommodations. The class manages owner data effectively and
/// validates the provided input for consistency and correctness.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>07/10/2024</date>
using System.Text.Encodings.Web;
using System.Text.Json;
using SmartStay.Common.Enums;
using SmartStay.Validation.Validators;

/// <summary>
/// The <c>SmartStay.Core.Models</c> namespace contains the primary data models used within the SmartStay application.
/// These models represent core entities and structures essential for managing application data.
/// </summary>
namespace SmartStay.Core.Models
{
/// <summary>
/// Defines the <see cref="Owner"/> class, which encapsulates the details of an accommodation owner,
/// including personal information such as first name, last name, email address, phone number, and a list of owned
/// accommodations. This class validates the provided data upon creation or when modifying specific properties, ensuring
/// that all data is consistent and correct.
/// </summary>
public class Owner
{
    static int _lastOwnerId = 0; // Last assigned owner ID
    static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions() {
        WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    }; // JSON Serializer options

    readonly int _id;                                          // ID of the owner
    string _firstName;                                         // First name of the owner
    string _lastName;                                          // Last name of the owner
    string _email;                                             // Email address of the owner
    string _phoneNumber = string.Empty;                        // Phone number of the owner
    string _address = string.Empty;                            // Address of the owner
    readonly List<Accommodation> _accommodationsOwned = new(); // List of accommodations owned by the owner

    /// <summary>
    /// Constructor to initialize a new owner with basic details: first name, last name, and email.
    /// Validates the input parameters.
    /// </summary>
    /// <param name="firstName">The first name of the owner.</param>
    /// <param name="lastName">The last name of the owner.</param>
    /// <param name="email">The email address of the owner.</param>
    /// <exception cref="ValidationException">Thrown when any of the input parameters are invalid.</exception>
    public Owner(string firstName, string lastName, string email)
    {
        NameValidator.ValidateName(firstName);
        NameValidator.ValidateName(lastName);
        EmailValidator.ValidateEmail(email);

        _id = GenerateOwnerId();
        _firstName = firstName;
        _lastName = lastName;
        _email = email;
    }

    /// <summary>
    /// Constructor to initialize a new owner with basic details (first name, last name, email)
    /// and additional details (phone number and address).
    /// </summary>
    /// <param name="firstName">The first name of the owner.</param>
    /// <param name="lastName">The last name of the owner.</param>
    /// <param name="email">The email address of the owner.</param>
    /// <param name="phoneNumber">The phone number of the owner.</param>
    /// <param name="address">The residential address of the owner.</param>
    /// <exception cref="ValidationException">Thrown when any of the input parameters are invalid.</exception>
    public Owner(string firstName, string lastName, string email, string phoneNumber, string address)
        : this(firstName, lastName, email)
    {
        PhoneNumberValidator.ValidatePhoneNumber(phoneNumber);
        AddressValidator.ValidateAddress(address);

        _phoneNumber = phoneNumber;
        _address = address;
    }

    /// <summary>
    /// Public getter for the owner ID.
    /// </summary>
    public int Id => _id;

    /// <summary>
    /// Public getter and setter for the FirstName.
    /// Sets the value after validating it.
    /// </summary>
    public string FirstName
    {
        get => _firstName;
        set => _firstName = NameValidator.ValidateName(value);
    }

    /// <summary>
    /// Public getter and setter for the LastName.
    /// Sets the value after validating it.
    /// </summary>
    public string LastName
    {
        get => _lastName;
        set => _lastName = NameValidator.ValidateName(value);
    }

    /// <summary>
    /// Public getter and setter for the Email.
    /// Sets the value after validating it.
    /// </summary>
    public string Email
    {
        get => _email;
        set => _email = EmailValidator.ValidateEmail(value);
    }

    /// <summary>
    /// Public getter and setter for the PhoneNumber.
    /// Sets the value after validating it.
    /// </summary>
    public string PhoneNumber
    {
        get => _phoneNumber;
        set => _phoneNumber = PhoneNumberValidator.ValidatePhoneNumber(value);
    }

    /// <summary>
    /// Public getter and setter for the Address.
    /// Sets the value after validating it.
    /// </summary>
    public string Address
    {
        get => _address;
        set => _address = AddressValidator.ValidateAddress(value);
    }

    /// <summary>
    /// Public getter for the list of accommodations owned by the owner.
    /// </summary>
    public List<Accommodation> Accommodations => _accommodationsOwned;

    /// <summary>
    /// Adds an accommodation to the list of accommodations owned by the owner.
    /// </summary>
    /// <param name="accommodation">The accommodation to add.</param>
    public void AddAccommodation(Accommodation accommodation)
    {
        if (accommodation == null)
            throw new ArgumentNullException(nameof(accommodation), "Accommodation cannot be null.");

        _accommodationsOwned.Add(accommodation);
    }

    /// <summary>
    /// Removes an accommodation from the list of accommodations owned by the owner.
    /// </summary>
    /// <param name="accommodation">The accommodation to add.</param>
    public void RemoveAccommodation(Accommodation accommodation)
    {
        if (accommodation == null)
            throw new ArgumentNullException(nameof(accommodation), "Accommodation cannot be null.");

        _accommodationsOwned.Remove(accommodation);
    }

    /// <summary>
    /// Generates a unique owner ID in a thread-safe manner using Interlocked.Increment.
    /// </summary>
    /// <returns>A unique owner ID.</returns>
    /// <exception cref="InvalidOperationException">Thrown when max limit of int is reached.</exception>
    private static int GenerateOwnerId()
    {
        // Check if the current value exceeds the max limit of int (2,147,483,647)
        if (_lastOwnerId >= int.MaxValue)
        {
            throw new InvalidOperationException("Owner ID limit exceeded.");
        }

        return Interlocked.Increment(ref _lastOwnerId);
    }

    /// <summary>
    /// Overridden ToString method to provide owner information in a readable JSON format.
    /// </summary>
    /// <returns>A JSON string representation of the owner object.</returns>
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, _jsonOptions);
    }
}
}

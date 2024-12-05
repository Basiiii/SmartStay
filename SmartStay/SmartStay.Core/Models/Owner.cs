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
using System.Text.Json.Serialization;
using ProtoBuf;
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
[ProtoContract]
public class Owner
{
    /// <summary>
    /// The last assigned owner ID. This value is used to track the highest ID assigned to any owner.
    /// </summary>
    static int _lastOwnerId = 0;

    /// <summary>
    /// JSON serializer options used for formatting and serializing client data in JSON.
    /// <para>
    /// - <see cref="WriteIndented"/> is enabled to improve readability with indented formatting.
    /// - <see cref="Encoder"/> is set to <see cref="JavaScriptEncoder.UnsafeRelaxedJsonEscaping"/> to allow unsafe
    /// characters
    ///   (such as `<`, `>`, and other special characters) to be included without escaping.
    /// - <see cref="Converters"/> contains a <see cref="JsonStringEnumConverter"/> to serialize enum values as their
    /// string names
    ///   instead of integer values.
    /// </para>
    /// </summary>
    static readonly JsonSerializerOptions _jsonOptions =
        new() { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                Converters = { new JsonStringEnumConverter() } };

    /// <summary>
    /// The unique ID of the owner. This field uniquely identifies each owner.
    /// </summary>
    [ProtoMember(1)]
    readonly int _id; // ID of the owner

    /// <summary>
    /// The first name of the owner. This field stores the owner's first name.
    /// </summary>
    [ProtoMember(2)]
    string _firstName; // First name of the owner

    /// <summary>
    /// The last name of the owner. This field stores the owner's last name.
    /// </summary>
    [ProtoMember(3)]
    string _lastName; // Last name of the owner

    /// <summary>
    /// The email address of the owner. This field stores the owner's email.
    /// </summary>
    [ProtoMember(4)]
    string _email; // Email address of the owner

    /// <summary>
    /// The phone number of the owner. This field stores the owner's phone number.
    /// It is optional and can be left empty.
    /// </summary>
    [ProtoMember(5)]
    string _phoneNumber = string.Empty; // Phone number of the owner

    /// <summary>
    /// The address of the owner. This field stores the owner's address.
    /// It is optional and can be left empty.
    /// </summary>
    [ProtoMember(6)]
    string _address = string.Empty; // Address of the owner

    /// <summary>
    /// The list of accommodations owned by the owner. This field stores the accommodations that the owner is associated
    /// with.
    /// </summary>
    [ProtoMember(7)]
    readonly List<Accommodation> _accommodationsOwned = []; // List of accommodations owned by the owner

    /// <summary>
    /// Initializes a new instance of the <see cref="Owner"/> class.
    /// <para>This constructor is required for Protobuf-net serialization/deserialization.</para>
    /// <para>It should **not** be used directly in normal application code. Instead, use the constructor with
    /// parameters for creating instances of <see cref="Owner"/>.</para>
    /// </summary>
#pragma warning disable CS8618
    public Owner()
#pragma warning restore CS8618
    {
        // This constructor is intentionally empty and only needed for Protobuf-net deserialization.
    }

    /// <summary>
    /// Constructor to initialize a new owner with basic details: first name, last name, and email.
    /// Validates the input parameters.
    /// </summary>
    /// <param name="firstName">The first name of the owner.</param>
    /// <param name="lastName">The last name of the owner.</param>
    /// <param name="email">The email address of the owner.</param>
    /// <exception cref="ValidationException">Thrown when any of the input parameters are invalid.
    ///     Each validation has a specific error code:
    ///     <br/><b>InvalidName:</b> if the first or last name is invalid (from the basic constructor).
    ///     <br/><b>InvalidEmail:</b> if the email address is invalid (from the basic constructor).
    /// </exception>
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
    /// <exception cref="ValidationException">Thrown when any of the input parameters are invalid.
    ///     Each validation has a specific error code:
    ///     <br/><b>InvalidName:</b> if the first or last name is invalid (from the basic constructor).
    ///     <br/><b>InvalidEmail:</b> if the email address is invalid (from the basic constructor).
    ///     <br/><b>InvalidPhoneNumber:</b> if the phone number is invalid.
    ///     <br/><b>InvalidAddress:</b> if the address is invalid.
    /// </exception>
    public Owner(string firstName, string lastName, string email, string phoneNumber, string address)
        : this(firstName, lastName, email)
    {
        PhoneNumberValidator.ValidatePhoneNumber(phoneNumber);
        AddressValidator.ValidateAddress(address);

        _phoneNumber = phoneNumber;
        _address = address;
    }

    /// <summary>
    /// Constructor to initialize a new <see cref="Owner"/> with all details, including a manually specified ID,
    /// first name, last name, email, phone number, address, and a list of accommodations owned. <b>This constructor
    /// should be avoided in normal cases</b> as it allows manual assignment of the owner ID, which can lead to
    /// conflicts and issues with ID uniqueness. The system is designed to automatically handle unique ID assignment,
    /// and using other constructors is recommended for creating owner objects to ensure proper handling of IDs.
    /// <br/>This constructor is marked with <see cref="[JsonConstructor]"/> so it will be used for JSON deserialization
    /// purposes, but it should not be used when creating new owner objects manually.
    /// </summary>
    /// <param name="id">The manually specified ID of the owner. This should not be used under normal circumstances as
    /// the system handles ID assignment automatically.</param>
    /// <param name="firstName">The first name of the owner.</param>
    /// <param name="lastName">The last name of the owner.</param>
    /// <param name="email">The email address of the owner.</param>
    /// <param name="phoneNumber">The phone number of the owner.</param>
    /// <param name="address">The residential address of the owner.</param>
    /// <param name="accommodationsOwned">The list of accommodations owned by the owner.</param>
    [JsonConstructor]
    public Owner(int id, string firstName, string lastName, string email, string phoneNumber, string address,
                 List<Accommodation> accommodationsOwned)
    {
        _id = id;
        UpdateLastOwnerId(id);
        _firstName = firstName;
        _lastName = lastName;
        _email = email;
        _phoneNumber = phoneNumber;
        _address = address;
        _accommodationsOwned = accommodationsOwned;
    }

    /// <summary>
    /// Public getter and setter for the last assigned ID.
    /// </summary>
    public static int LastAssignedId
    {
        get => _lastOwnerId;
        set {
            if (_lastOwnerId < value)
                _lastOwnerId = value;
        }
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
    /// <remarks>
    /// This property creates and returns a deep copy of the underlying accommodations collection.
    /// Modifications to the returned list or its elements will not affect the original data.
    /// <para>
    /// **Performance Note**: Creating a deep copy can incur a performance cost, especially for
    /// large collections. Use this property sparingly if performance is critical.
    /// </para>
    /// </remarks>
    public List<Accommodation> AccommodationsOwned => GetAccommodationsCopy();

    /// <summary>
    /// Adds the specified accommodation to the list of accommodations owned by the owner.
    /// </summary>
    /// <param name="accommodation">The accommodation to add. Cannot be null.</param>
    /// <returns>
    /// True if the accommodation was successfully added; false if the provided accommodation is null.
    /// </returns>
    /// <remarks>
    /// This method does not perform any additional checks to ensure the accommodation is unique
    /// or validate its state. Ensure external validation is performed if required.
    /// </remarks>
    public bool AddAccommodation(Accommodation accommodation)
    {
        if (accommodation == null)
            return false;

        _accommodationsOwned.Add(accommodation);
        return true;
    }

    /// <summary>
    /// Removes the specified accommodation from the list of accommodations owned by the owner.
    /// </summary>
    /// <param name="accommodation">The accommodation to remove. Cannot be null.</param>
    /// <returns>
    /// True if the accommodation was successfully removed; false if the provided accommodation is null
    /// or not found in the list.
    /// </returns>
    /// <remarks>
    /// This method assumes that the list allows duplicate entries. If duplicates exist,
    /// only the first occurrence of the accommodation will be removed.
    /// Ensure external validation is performed if additional checks are required.
    /// </remarks>
    public bool RemoveAccommodation(Accommodation accommodation)
    {
        if (accommodation == null)
            return false;

        _accommodationsOwned.Remove(accommodation);
        return true;
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
    /// Method to ensure _lastOwnerId is up-to-date after deserialization or manual assignment.
    /// </summary>
    /// <param name="id">Manually given ID.</param>
    private static void UpdateLastOwnerId(int id)
    {
        if (id > _lastOwnerId)
        {
            _lastOwnerId = id; // Update the last assigned owner ID if the new ID is larger
        }
    }

    /// <summary>
    /// Creates a deep copy of the accommodations list.
    /// </summary>
    /// <returns>A deep copy of the list of accommodations.</returns>
    private List<Accommodation> GetAccommodationsCopy()
    {
        // Deep copy each room
        return _accommodationsOwned.Select(accommodation => accommodation.Clone()).ToList();
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

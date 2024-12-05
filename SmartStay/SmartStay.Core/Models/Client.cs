/// <copyright file="Client.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the <see cref="Client"/> class, which stores information about individual clients,
/// including their personal details and payment preferences. This class manages client data effectively
/// while ensuring data integrity through input validation.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>07/10/2024</date>
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using ProtoBuf;
using SmartStay.Common.Enums;
using SmartStay.Core.Repositories;
using SmartStay.Validation.Validators;

/// <summary>
/// The <c>SmartStay.Core.Models</c> namespace contains the primary data models used within the SmartStay application.
/// These models represent core entities and structures essential for managing application data.
/// </summary>
namespace SmartStay.Core.Models
{
/// <summary>
/// Defines the <see cref="Client"/> class, which encapsulates the details of a client including
/// personal information such as first name, last name, email address, phone number, residential address,
/// and preferred payment method.
/// This class validates the provided data upon creation or when modifying specific properties,
/// ensuring that all data is consistent and correct.
/// </summary>
[ProtoContract]
public class Client
{
    /// <summary>
    /// The last assigned client ID. Used for generating unique IDs for new clients.
    /// </summary>
    static int _lastClientId = 0; // Last assigned client ID

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
        new JsonSerializerOptions() { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                                      Converters = { new JsonStringEnumConverter() } };

    /// <summary>
    /// The unique identifier for this client. This ID is used to distinguish one client from another.
    /// </summary>
    [ProtoMember(1)]
    readonly int _id; // ID of the client

    /// <summary>
    /// The first name of the client. This field holds the client's given name.
    /// </summary>
    [ProtoMember(2)]
    string _firstName; // First name of the client

    /// <summary>
    /// The last name of the client. This field holds the client's family name.
    /// </summary>
    [ProtoMember(3)]
    string _lastName; // Last name of the client

    /// <summary>
    /// The email address of the client. This field holds the client's contact email.
    /// </summary>
    [ProtoMember(4)]
    string _email; // Email address of the client

    /// <summary>
    /// The phone number of the client. This field holds the client's contact phone number.
    /// Defaults to an empty string if not provided.
    /// </summary>
    [ProtoMember(5)]
    string _phoneNumber = string.Empty; // Phone number of the client

    /// <summary>
    /// The address of the client. This field holds the client's home or billing address.
    /// Defaults to an empty string if not provided.
    /// </summary>
    [ProtoMember(6)]
    string _address = string.Empty; // Address of the client

    /// <summary>
    /// The preferred payment method of the client. This field indicates how the client prefers to pay for services.
    /// The value is set to `None` by default.
    /// </summary>
    [ProtoMember(7)]
    PaymentMethod _preferredPaymentMethod = PaymentMethod.None; // Preferred payment method of the client

    /// <summary>
    /// Initializes a new instance of the <see cref="Client"/> class.
    /// <para>This constructor is required for Protobuf-net serialization/deserialization.</para>
    /// <para>It should **not** be used directly in normal application code. Instead, use the constructor with
    /// parameters for creating instances of <see cref="Client"/>.</para>
    /// </summary>
#pragma warning disable CS8618
    public Client()
#pragma warning restore CS8618
    {
        // This constructor is intentionally empty and only needed for Protobuf-net deserialization.
    }

    /// <summary>
    /// Constructor to initialize a new client with basic details: first name, last name, and email.
    /// Validates the input parameters.
    /// <br/>Throws specific validation exceptions if any input is invalid.
    /// </summary>
    /// <param name="firstName">The first name of the client.</param>
    /// <param name="lastName">The last name of the client.</param>
    /// <param name="email">The email address of the client.</param>
    /// <exception cref="ValidationException">Thrown if the first name, last name, or email is invalid.
    ///     Each validation has a specific error code:
    ///     <br/><b>InvalidName:</b> if the first or last name is invalid.
    ///     <br/><b>InvalidEmail:</b> if the email address is invalid.
    /// </exception>
    public Client(string firstName, string lastName, string email)
    {
        NameValidator.ValidateName(firstName);
        NameValidator.ValidateName(lastName);
        EmailValidator.ValidateEmail(email);

        _id = GenerateClientId();
        _firstName = firstName;
        _lastName = lastName;
        _email = email;
    }

    /// <summary>
    /// Constructor to initialize a new client with basic details (first name, last name, email)
    /// and additional details (phone number and address).
    /// Validates the input parameters and throws validation exceptions for any invalid inputs.
    /// </summary>
    /// <param name="firstName">The first name of the client.</param>
    /// <param name="lastName">The last name of the client.</param>
    /// <param name="email">The email address of the client.</param>
    /// <param name="phoneNumber">The phone number of the client.</param>
    /// <param name="address">The residential address of the client.</param>
    /// <exception cref="ValidationException">Thrown when any of the input parameters are invalid.
    ///     Each validation has a specific error code:
    ///     <br/><b>InvalidName:</b> if the first or last name is invalid (from the basic constructor).
    ///     <br/><b>InvalidEmail:</b> if the email address is invalid (from the basic constructor).
    ///     <br/><b>InvalidPhoneNumber:</b> if the phone number is invalid.
    ///     <br/><b>InvalidAddress:</b> if the address is invalid.
    /// </exception>
    public Client(string firstName, string lastName, string email, string phoneNumber, string address)
        : this(firstName, lastName, email)
    {
        PhoneNumberValidator.ValidatePhoneNumber(phoneNumber);
        AddressValidator.ValidateAddress(address);

        _phoneNumber = phoneNumber;
        _address = address;
    }

    /// <summary>
    /// Constructor to initialize a new client with all details including the preferred payment method.
    /// </summary>
    /// <param name="firstName">The first name of the client.</param>
    /// <param name="lastName">The last name of the client.</param>
    /// <param name="email">The email address of the client.</param>
    /// <param name="phoneNumber">The phone number of the client.</param>
    /// <param name="address">The residential address of the client.</param>
    /// <param name="preferredPaymentMethod">The preferred payment method of the client.</param>
    /// <exception cref="ValidationException">Thrown when any of the input parameters are invalid:
    ///     <br/><b>InvalidName:</b> if the first or last name is invalid.
    ///     <br/><b>InvalidEmail:</b> if the email address is invalid.
    ///     <br/><b>InvalidPhoneNumber:</b> if the phone number is invalid.
    ///     <br/><b>InvalidAddress:</b> if the address is invalid.
    ///     <br/><b>InvalidPaymentMethod:</b> if the preferred payment method is invalid.
    /// </exception>
    /// <returns>The <see cref="Client"/> object created.</returns>
    public Client(string firstName, string lastName, string email, string phoneNumber, string address,
                  PaymentMethod preferredPaymentMethod)
        : this(firstName, lastName, email, phoneNumber, address)
    {
        PaymentValidator.ValidatePaymentMethod(preferredPaymentMethod);

        _preferredPaymentMethod = preferredPaymentMethod;
    }

    /// <summary>
    /// Constructor to initialize a new <see cref="Client"/> with all details, including a manually specified ID,
    /// first name, last name, email, phone number, address, and preferred payment method. <b>This constructor should be
    /// avoided in normal cases</b> as it allows manual assignment of the client ID, which can lead to conflicts and
    /// issues with ID uniqueness. The system is designed to automatically handle unique ID assignment, and using other
    /// constructors is recommended for creating client objects to ensure proper handling of IDs. <br/>This constructor
    /// is marked with <see cref="[JsonConstructor]"/> so it will be used for JSON deserialization purposes, but it
    /// should not be used when creating new client objects manually.
    /// </summary>
    /// <param name="id">The manually specified ID of the client. This should not be used under normal circumstances as
    /// the system handles ID assignment automatically.</param>
    /// <param name="firstName">The first name of the client.</param>
    /// <param name="lastName">The last name of the client.</param>
    /// <param name="email">The email address of the client.</param>
    /// <param name="phoneNumber">The phone number of the client.</param>
    /// <param name="address">The residential address of the client.</param>
    /// <param name="preferredPaymentMethod">The preferred payment method of the client.</param>
    [JsonConstructor]
    public Client(int id, string firstName, string lastName, string email, string phoneNumber, string address,
                  PaymentMethod preferredPaymentMethod)
    {
        _id = id;
        UpdateLastClientId(id);
        _firstName = firstName;
        _lastName = lastName;
        _email = email;
        _phoneNumber = phoneNumber;
        _address = address;
        _preferredPaymentMethod = preferredPaymentMethod;
    }

    /// <summary>
    /// Public getter and setter for the last assigned ID.
    /// </summary>
    public static int LastAssignedId
    {
        get => _lastClientId;
        set {
            if (_lastClientId < value)
                _lastClientId = value;
        }
    }

    /// <summary>
    /// Public getter for the user Id.
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
    /// Public getter and setter for the PreferredPaymentMethod.
    /// Sets the value after validating it.
    /// </summary>
    public PaymentMethod PreferredPaymentMethod
    {
        get => _preferredPaymentMethod;
        set => _preferredPaymentMethod = PaymentValidator.ValidatePaymentMethod(value);
    }

    /// <summary>
    /// Generates a unique client ID in a thread-safe manner using Interlocked.Increment.
    /// </summary>
    /// <returns>A unique client ID.</returns>
    /// <exception cref="InvalidOperationException">Thrown when max limit of int is reached.</exception>
    private static int GenerateClientId()
    {
        // Check if the current value exceeds the max limit of int (2,147,483,647)
        if (_lastClientId >= int.MaxValue)
        {
            throw new InvalidOperationException("Client ID limit exceeded.");
        }

        return Interlocked.Increment(ref _lastClientId);
    }

    /// <summary>
    /// Method to ensure _lastClientId is up-to-date after deserialization or manual assignment
    /// </summary>
    /// <param name="id">Manually given ID</param>
    private static void UpdateLastClientId(int id)
    {
        if (id > _lastClientId)
        {
            _lastClientId = id; // Update the last assigned client ID if the new ID is larger
        }
    }

    /// <summary>
    /// Overridden ToString method to provide client information in a readable JSON format.
    /// </summary>
    /// <returns>A JSON string representation of the client object.</returns>
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, _jsonOptions);
    }
}
}

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
using SmartStay.Models.Enums;
using SmartStay.Validation;

/// <summary>
/// The <c>SmartStay.Models</c> namespace contains the primary data models used within the SmartStay application.
/// These models represent core entities and structures essential for managing application data.
/// </summary>
namespace SmartStay.Models
{
/// <summary>
/// Defines the <see cref="Client"/> class, which encapsulates the details of a client including
/// personal information such as first name, last name, email address, phone number, residential address,
/// and preferred payment method.
/// This class validates the provided data upon creation or when modifying specific properties,
/// ensuring that all data is consistent and correct.
/// </summary>
public class Client
{
    static int _lastClientId = 0; // Last assigned client ID
    static readonly JsonSerializerOptions _jsonOptions =
        new() { WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }; // JSON Serializer options

    readonly int _id;                                           // ID of the client
    string _firstName;                                          // First name of the client
    string _lastName;                                           // Last name of the client
    string _email;                                              // Email address of the client
    string _phoneNumber = string.Empty;                         // Phone number of the client
    string _address = string.Empty;                             // Address of the client
    PaymentMethod _preferredPaymentMethod = PaymentMethod.None; // Preferred payment method of the client

    /// <summary>
    /// Constructor to initialize a new client with basic details: first name, last name, and email.
    /// Validates the input parameters.
    /// </summary>
    /// <param name="firstName">The first name of the client.</param>
    /// <param name="lastName">The last name of the client.</param>
    /// <param name="email">The email address of the client.</param>
    /// <exception cref="ValidationException">Thrown when any of the input parameters are invalid.</exception>
    public Client(string firstName, string lastName, string email)
    {
        if (!Validator.IsValidName(firstName))
            throw new ValidationException(ValidationErrorCode.InvalidName);
        if (!Validator.IsValidName(lastName))
            throw new ValidationException(ValidationErrorCode.InvalidName);
        if (!Validator.IsValidEmail(email))
            throw new ValidationException(ValidationErrorCode.InvalidEmail);

        _id = GenerateClientId();
        _firstName = firstName;
        _lastName = lastName;
        _email = email;
    }

    /// <summary>
    /// Constructor to initialize a new client with basic details (first name, last name, email)
    /// and additional details (phone number and address).
    /// </summary>
    /// <param name="firstName">The first name of the client.</param>
    /// <param name="lastName">The last name of the client.</param>
    /// <param name="email">The email address of the client.</param>
    /// <param name="phoneNumber">The phone number of the client.</param>
    /// <param name="address">The residential address of the client.</param>
    /// <exception cref="ValidationException">Thrown when any of the input parameters are invalid.</exception>
    public Client(string firstName, string lastName, string email, string phoneNumber, string address)
        : this(firstName, lastName, email)
    {
        if (!Validator.IsValidPhoneNumber(phoneNumber))
            throw new ValidationException(ValidationErrorCode.InvalidPhoneNumber);
        if (!Validator.IsValidAddress(address))
            throw new ValidationException(ValidationErrorCode.InvalidAddress);

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
    /// <exception cref="ValidationException">Thrown when any of the input parameters are invalid.</exception>
    public Client(string firstName, string lastName, string email, string phoneNumber, string address,
                  PaymentMethod preferredPaymentMethod)
        : this(firstName, lastName, email, phoneNumber, address)
    {
        if (!Validator.IsValidPaymentMethod(preferredPaymentMethod))
            throw new ValidationException(ValidationErrorCode.InvalidPaymentMethod);

        _preferredPaymentMethod = preferredPaymentMethod;
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
        set => _firstName = Validator.ValidateName(value);
    }

    /// <summary>
    /// Public getter and setter for the LastName.
    /// Sets the value after validating it.
    /// </summary>
    public string LastName
    {
        get => _lastName;
        set => _lastName = Validator.ValidateName(value);
    }

    /// <summary>
    /// Public getter and setter for the Email.
    /// Sets the value after validating it.
    /// </summary>
    public string Email
    {
        get => _email;
        set => _email = Validator.ValidateEmail(value);
    }

    /// <summary>
    /// Public getter and setter for the PhoneNumber.
    /// Sets the value after validating it.
    /// </summary>
    public string PhoneNumber
    {
        get => _phoneNumber;
        set => _phoneNumber = Validator.ValidatePhoneNumber(value);
    }

    /// <summary>
    /// Public getter and setter for the Address.
    /// Sets the value after validating it.
    /// </summary>
    public string Address
    {
        get => _address;
        set => _address = Validator.ValidateAddress(value);
    }

    /// <summary>
    /// Public getter and setter for the PreferredPaymentMethod.
    /// Sets the value after validating it.
    /// </summary>
    public PaymentMethod PreferredPaymentMethod
    {
        get => _preferredPaymentMethod;
        set => _preferredPaymentMethod = Validator.ValidatePaymentMethod(value);
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
    /// Overridden ToString method to provide client information in a readable JSON format.
    /// </summary>
    /// <returns>A JSON string representation of the client object.</returns>
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, _jsonOptions);
    }
}
}

/// <copyright file="Client.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the definition of the Client class used in the SmartStay application.
/// </file>
/// <summary>
/// Represents the <see cref="Client"/> class, which stores information about individual clients,
/// including their personal details and payment preferences. This class provides methods to validate
/// and manage client data effectively while ensuring data integrity through input validation.
/// </summary>
/// <author>Enrique Rodrigues</author>
/// <date>07/10/2024</date>

namespace SmartStay
{
/// <summary>
/// Defines the <see cref="Client"/> class, which encapsulates the details of a client including
/// personal information such as name, email, phone number, address, and preferred payment method.
/// </summary>
internal class Client
{
    private readonly int _Id;                                           // ID of the client
    private string _FirstName;                                          // First name of the client
    private string _LastName;                                           // Last name of the client
    private string _Email;                                              // Email address of the client
    private string _PhoneNumber = string.Empty;                         // Phone number of the client
    private string _Address = string.Empty;                             // Address of the client
    private PaymentMethod _PreferredPaymentMethod = PaymentMethod.None; // Preferred payment method of the client

    /// <summary>
    /// Constructor with only basic information (ID, First Name, Last Name, Email).
    /// </summary>
    /// <param name="id">The unique identifier for the client.</param>
    /// <param name="firstName">The first name of the client.</param>
    /// <param name="lastName">The last name of the client.</param>
    /// <param name="email">The email address of the client.</param>
    public Client(int id, string firstName, string lastName, string email)
    {
        try
        {
            Validator.ValidateId(id, nameof(id));
            _Id = id;

            Validator.ValidateName(firstName, nameof(firstName));
            _FirstName = firstName;

            Validator.ValidateName(lastName, nameof(lastName));
            _LastName = lastName;

            Validator.ValidateEmail(email, nameof(email));
            _Email = email;
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Failed to create object due to invalid input", ex);
        }
    }

    /// <summary>
    /// Constructor with all information provided.
    /// </summary>
    /// <param name="id">The unique identifier for the client.</param>
    /// <param name="firstName">The first name of the client.</param>
    /// <param name="lastName">The last name of the client.</param>
    /// <param name="email">The email address of the client.</param>
    /// <param name="phoneNumber">The phone number of the client.</param>
    /// <param name="address">The residential address of the client.</param>
    /// <param name="preferredPaymentMethod">The preferred payment method of the client.</param>
    public Client(int id, string firstName, string lastName, string email, string phoneNumber, string address,
                  PaymentMethod preferredPaymentMethod)
        : this(id, firstName, lastName, email)
    {
        try
        {
            Validator.ValidatePhoneNumber(phoneNumber, nameof(phoneNumber));
            _PhoneNumber = phoneNumber;

            Validator.ValidateAddress(address, nameof(address));
            _Address = address;

            Validator.ValidatePaymentMethod(preferredPaymentMethod, nameof(preferredPaymentMethod));
            _PreferredPaymentMethod = preferredPaymentMethod;
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Failed to create object due to invalid input", ex);
        }
    }

    /// <summary>
    /// Public getter for the user Id.
    /// </summary>
    public int Id => _Id;

    /// <summary>
    /// Public getter and setter for the FirstName.
    /// </summary>
    public string FirstName
    {
        get => _FirstName;
        set {
            Validator.ValidateName(value, nameof(FirstName));
            _FirstName = value;
        }
    }

    /// <summary>
    /// Public getter and setter for the LastName.
    /// </summary>
    public string LastName
    {
        get => _LastName;
        set {
            Validator.ValidateName(value, nameof(LastName));
            _LastName = value;
        }
    }

    /// <summary>
    /// Public getter and setter for the Email.
    /// </summary>
    public string Email
    {
        get => _Email;
        set {
            Validator.ValidateEmail(value, nameof(Email));
            _Email = value;
        }
    }

    /// <summary>
    /// Public getter and setter for the PhoneNumber.
    /// </summary>
    public string PhoneNumber
    {
        get => _PhoneNumber;
        set {
            Validator.ValidatePhoneNumber(value, nameof(PhoneNumber));
            _PhoneNumber = value;
        }
    }

    /// <summary>
    /// Public getter and setter for the Address.
    /// </summary>
    public string Address
    {
        get => _Address;
        set {
            Validator.ValidateAddress(value, nameof(Address));
            _Address = value;
        }
    }

    /// <summary>
    /// Public getter and setter for the PreferredPaymentMethod.
    /// </summary>
    public PaymentMethod PreferredPaymentMethod
    {
        get => _PreferredPaymentMethod;
        set {
            Validator.ValidatePaymentMethod(value, nameof(PreferredPaymentMethod));
            _PreferredPaymentMethod = value;
        }
    }

    /// <summary>
    /// Overriding the ToString method to display client information in a JSON format.
    /// </summary>
    /// <returns>The <see cref="string"/></returns>
    public override string ToString()
    {
        return $"{{ \"ID\": {Id}, \"FirstName\": \"{FirstName}\", \"LastName\": \"{LastName}\", " +
               $"\"Email\": \"{Email}\", \"Phone\": \"{PhoneNumber}\", " +
               $"\"Address\": \"{Address}\", \"PreferredPaymentMethod\": \"{PreferredPaymentMethod}\" }}";
    }
}
}

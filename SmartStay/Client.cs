/// <copyright file="Clients.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains...
/// </file>
/// <summary>
/// Defines ...
/// </summary>
/// <author>Enrique Rodrigues</author>
/// <date>07/10/2024</date>
using System.Text.Json;

namespace SmartStay
{
/// <summary>
/// Defines the <see cref="Client" />
/// </summary>
internal class Client
{
    /// <summary>
    /// Gets or sets the Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the FirstName
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the LastName
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the Email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the PhoneNumber
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the Address
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Client"/> class.
    /// </summary>
    /// <param name="id">The id<see cref="int"/></param>
    /// <param name="firstName">The firstName<see cref="string"/></param>
    /// <param name="lastName">The lastName<see cref="string"/></param>
    /// <param name="email">The email<see cref="string"/></param>
    /// <param name="phoneNumber">The phoneNumber<see cref="string"/></param>
    /// <param name="address">The address<see cref="string"/></param>
    public Client(int id, string firstName, string lastName, string email, string phoneNumber, string address)
    {
        this.Id = id;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Email = email;
        this.PhoneNumber = phoneNumber;
        this.Address = address;
    }

    /// <summary>
    /// Overriding the ToString method to display client information
    /// </summary>
    /// <returns>The <see cref="string"/></returns>
    public override string ToString()
    {
        return $"Client [ID: {Id}, Name: {FirstName} {LastName}, Email: {Email}, Phone: {PhoneNumber}, Address: {Address}]";
    }

    /// <summary>
    /// The SaveClientsToFile method serializes a dictionary of clients to JSON and saves to a file
    /// </summary>
    /// <param name="clients">The clients<see cref="Dictionary{int, Client}"/></param>
    /// <param name="filePath">The filePath<see cref="string"/></param>
    public static void SaveClientsToFile(Dictionary<int, Client> clients, string filePath)
    {
        // Convert dictionary values (clients) to a list for JSON serialization
        List<Client> clientList = new List<Client>(clients.Values);
        string jsonString = JsonSerializer.Serialize(clientList, new JsonSerializerOptions { WriteIndented = true });

        // Write the JSON string to a file
        File.WriteAllText(filePath, jsonString);
    }

    /// <summary>
    /// The LoadClientsFromFile method loads a dictionary of clients from a JSON file
    /// </summary>
    /// <param name="filePath">The filePath<see cref="string"/></param>
    /// <returns>The <see cref="Dictionary{int, Client}"/></returns>
    public static Dictionary<int, Client> LoadClientsFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File at {filePath} not found.");
        }

        // Read the JSON string from the file
        string jsonString = File.ReadAllText(filePath);

        // Deserialize the JSON string to a list of clients
        // Ensure that we handle a potential null value returned by the deserialization
        List<Client>? clientList = JsonSerializer.Deserialize<List<Client>>(jsonString);

        // Create an empty dictionary to store the clients
        Dictionary<int, Client> clientDictionary = new Dictionary<int, Client>();

        // Check if the deserialized list is not null
        if (clientList != null)
        {
            foreach (var client in clientList)
            {
                clientDictionary[client.Id] = client;
            }
        }

        return clientDictionary;
    }
}
}

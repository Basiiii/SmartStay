/// <copyright file="Accommodation.cs">
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
/// <remarks>
/// </remarks>
namespace SmartStay
{
/// <summary>
/// Represents an accommodation unit in the system
/// </summary>
internal class Accommodation
{
    /// <summary>
    /// Gets or sets the unique ID of the accommodation
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the type of accommodation
    /// </summary>
    public AccommodationType Type { get; set; }

    /// <summary>
    /// Gets or sets the name of the accommodation
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the address of the accommodation
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Gets or sets the capacity of the accommodation (number of people it can host)
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// Gets or sets the price per night for staying at the accommodation
    /// </summary>
    public decimal PricePerNight { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether IsAvailable
    /// Gets or sets the availability status of the accommodation.
    /// </summary>
    public bool IsAvailable { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Accommodation"/> class.
    /// </summary>
    /// <param name="id">Unique ID of the accommodation</param>
    /// <param name="name">Name of the accommodation</param>
    /// <param name="address">Address of the accommodation</param>
    /// <param name="capacity">Capacity of the accommodation (number of people)</param>
    /// <param name="pricePerNight">Price per night for staying at the accommodation</param>
    /// <param name="isAvailable">Initial availability status of the accommodation</param>
    public Accommodation(int id, AccommodationType accommodationType, string name, string address, int capacity,
                         decimal pricePerNight, bool isAvailable = true)
    {
        this.Id = id;
        this.Type = accommodationType;
        this.Name = name;
        this.Address = address;
        this.Capacity = capacity;
        this.PricePerNight = pricePerNight;
        this.IsAvailable = isAvailable;
    }

    /// <summary>
    /// Updates the availability status of the accommodation
    /// </summary>
    /// <param name="availability">New availability status</param>
    public void UpdateAvailability(bool availability)
    {
        IsAvailable = availability;
    }

    /// <summary>
    /// Calculates the total cost for a given number of nights
    /// </summary>
    /// <param name="nights">The number of nights the accommodation will be booked for</param>
    /// <returns>Total cost based on the price per night</returns>
    public decimal CalculateTotalCost(int nights)
    {
        return PricePerNight * nights;
    }

    /// <summary>
    /// Overrides the ToString method to provide a string representation of the accommodation details
    /// </summary>
    /// <returns>String representing the accommodation details</returns>
    public override string ToString()
    {
        return $"Accommodation [ID: {Id}, Name: {Name}, Address: {Address}, Capacity: {Capacity}, Price Per Night: {PricePerNight:C}, Available: {IsAvailable}]";
    }
}
}

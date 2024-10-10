using System;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace SmartStay
{
/// <summary>
/// Manages the operations related to clients, reservations, and accommodations.
/// </summary>
internal class BookingManager
{
    private readonly Dictionary<int, Client> clients;
    private readonly Dictionary<int, Accommodation> accommodations;
    private readonly Dictionary<int, Reservation> reservations;

    public BookingManager()
    {
        clients = [];
        accommodations = [];
        reservations = [];
    }

    /// <summary>
    /// Adds a client to the booking manager.
    /// </summary>
    /// <param name="client">The client to add.</param>
    public void AddClient(Client client)
    {
        clients[client.Id] = client;
        Console.WriteLine("Client added successfully.");
    }

    /// <summary>
    /// Removes a client from the booking manager.
    /// </summary>
    /// <param name="clientId">The Id of the client to remove.</param>
    public void RemoveClient(int clientId)
    {
        if (clients.Remove(clientId))
        {
            Console.WriteLine("Client removed successfully.");
        }
        else
        {
            Console.WriteLine("Client not found.");
        }
    }

    /// <summary>
    /// Gets a client by its Id.
    /// </summary>
    /// <param name="clientId">The Id of the client.</param>
    /// <returns>The client or null if not found.</returns>
    public Client? GetClient(int clientId)
    {
        if (clients.TryGetValue(clientId, out var client))
        {
            return client; // Client found
        }
        else
        {
            Console.WriteLine("Client not found.");
            return null; // Client not found, returning null
        }
    }

    // Add new accommodation
    public void AddAccommodation(Accommodation accommodation)
    {
        accommodations[accommodation.Id] = accommodation;
        Console.WriteLine("Accommodation added successfully.");
    }

    // Remove an accommodation
    public void RemoveAccommodation(int accommodationId)
    {
        if (accommodations.Remove(accommodationId))
        {
            Console.WriteLine("Accommodation removed successfully.");
        }
        else
        {
            Console.WriteLine("Accommodation not found.");
        }
    }

    // Make a reservation
    public void MakeReservation(Reservation reservation)
    {
        if (clients.ContainsKey(reservation.ClientId) && accommodations.ContainsKey(reservation.AccommodationId))
        {
            reservations[reservation.ReservationId] = reservation;
            Console.WriteLine("Reservation created successfully.");
        }
        else
        {
            Console.WriteLine("Invalid client or accommodation ID.");
        }
    }

    // Check-in functionality
    public void CheckIn(int reservationId)
    {
        if (reservations.ContainsKey(reservationId))
        {
            reservations[reservationId].CheckIn();
        }
        else
        {
            Console.WriteLine("Reservation not found.");
        }
    }

    // Check-out functionality
    public void CheckOut(int reservationId)
    {
        if (reservations.ContainsKey(reservationId))
        {
            reservations[reservationId].CheckOut();
        }
        else
        {
            Console.WriteLine("Reservation not found.");
        }
    }

    // Payment processing
    public void ProcessPayment(Payment payment)
    {
        if (reservations.ContainsKey(payment.ReservationId))
        {
            reservations[payment.ReservationId].MakePayment(payment.Amount);
            Console.WriteLine("Payment processed.");
        }
        else
        {
            Console.WriteLine("Reservation not found for the payment.");
        }
    }
}
}

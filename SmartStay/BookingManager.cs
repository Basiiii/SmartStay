using System;
using System.Collections.Generic;

namespace SmartStay
{
/// <summary>
/// Manages the operations related to clients, reservations, and accommodations.
/// </summary>
internal class BookingManager
{
    private Dictionary<int, Client> clients;
    private Dictionary<int, Accommodation> accommodations;
    private Dictionary<int, Reservation> reservations;

    public BookingManager()
    {
        clients = new Dictionary<int, Client>();
        accommodations = new Dictionary<int, Accommodation>();
        reservations = new Dictionary<int, Reservation>();
    }

    // Add new client
    public void AddClient(Client client)
    {
        clients[client.Id] = client;
        Console.WriteLine("Client added successfully.");
    }

    // Add new accommodation
    public void AddAccommodation(Accommodation accommodation)
    {
        accommodations[accommodation.Id] = accommodation;
        Console.WriteLine("Accommodation added successfully.");
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

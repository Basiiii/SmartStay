/// <copyright file="Program.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the entry point for the SmartStay application.
/// </file>
/// <summary>
/// The <see cref="Program"/> class is the main entry point for the SmartStay application.
/// This application is designed for managing tourist accommodations, including functionalities
/// for client management, reservations, and check-ins.
/// </summary>
/// <author>Enrique Rodrigues</author>
/// <date>07/10/2024</date>
/// <remarks>
/// This application supports features such as adding new clients, retrieving client information,
/// making reservations, and performing check-in processes. All client data is stored in JSON format
/// for persistence, and the application allows efficient retrieval using dictionary data
/// structures.
/// </remarks>

namespace SmartStay
{
/// <summary>
/// Defines the <see cref="Program" />
/// </summary>
internal static class Program
{
    /// <summary>
    /// The Main
    /// </summary>
    /// <param name="args">The args<see cref="string[]"/></param>
    internal static void Main(string[] args)
    {
        // TODO: test everything thoroughly
        // later on add reports etc
        // add ability to edit and remove/delete clients, accommodations etc

        BookingManager bookingManager = new BookingManager();

        // 1. Add Clients
        Client client1 = new Client(1, "John", "Doe", "john.doe@example.com", "123-456-7890", "123 Elm Street");
        bookingManager.AddClient(client1);

        // 2. Add Accommodations
        Accommodation room1 =
            new Accommodation(101, AccommodationType.DoubleRoom, "Sea View Room", "Example Address", 5, 150m);
        bookingManager.AddAccommodation(room1);

        // 3. Make a Reservation
        Reservation reservation1 =
            new Reservation(1, client1.Id, room1.Id, DateTime.Now.AddDays(1), DateTime.Now.AddDays(5), 600m);
        bookingManager.MakeReservation(reservation1);

        // 4. Check-In
        bookingManager.CheckIn(reservation1.ReservationId);

        // 5. Process Payment
        Payment payment1 = new Payment(1, reservation1.ReservationId, 600m, DateTime.Now, "Credit Card");
        bookingManager.ProcessPayment(payment1);

        // 6. Check-Out
        bookingManager.CheckOut(reservation1.ReservationId);

        //// 7. Export Report
        // string filePath = @"D:\reservations_report.csv";
        // ReportGenerator.ExportReservationsToCSV(bookingManager.GetReservations(), filePath);
    }
}
}

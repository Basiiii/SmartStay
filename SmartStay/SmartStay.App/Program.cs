using SmartStay.Common.Enums;
using SmartStay.Core.Models;
using SmartStay.Core.Repositories;
using SmartStay.Core.Services;
using SmartStay.Validation;

namespace SmartStay.App
{
internal static class Program
{
    static void Main()
    {
        // Create and add clients with exception handling
        try
        {
            BookingManager.CreateBasicClient("John", "Doe", "john.doe@example.com");
            Console.WriteLine("Basic client (ID 1) created and added to the system.");
        }
        catch (ValidationException ex)
        {
            Console.WriteLine($"Failed to create client: {ex.Message} (Code: {ex.ErrorCode})");
        }

        try
        {
            BookingManager.CreateCompleteClient("Jane", "Smith", "jane.smith@example.com", "+3515556482097",
                                                "Foo Address");
            Console.WriteLine("Basic client (ID 2) created and added to the system.");
        }
        catch (ValidationException ex)
        {
            Console.WriteLine($"Failed to create client: {ex.Message} (Code: {ex.ErrorCode})");
        }

        // Retrieve and display client details
        var foundClient1 = BookingManager.Clients.FindClientById(1);
        var foundClient2 = BookingManager.Clients.FindClientById(2);
        Console.WriteLine($"Found Client 1: {foundClient1}");
        Console.WriteLine($"Found Client 2: {foundClient2}");

        // Create and add accommodations with exception handling
        try
        {
            BookingManager.CreateAccommodation(AccommodationType.Hotel, "Grand Hotel", "123 Main Street", 100.00m);

            Console.WriteLine("Accommodation 1 created and added");
        }
        catch (ValidationException ex)
        {
            Console.WriteLine($"Failed to create accommodation: {ex.Message} (Code: {ex.ErrorCode})");
        }

        try
        {
            BookingManager.CreateAccommodation(AccommodationType.House, "Cozy Cottage", "456 Elm Street", 80.00m);

            Console.WriteLine("Accommodation 2 created and added");
        }
        catch (ValidationException ex)
        {
            Console.WriteLine($"Failed to create accommodation: {ex.Message} (Code: {ex.ErrorCode})");
        }

        // Retrieve and display accommodation details
        var foundAccommodation1 = BookingManager.Accommodations.FindAccommodationById(1);
        var foundAccommodation2 = BookingManager.Accommodations.FindAccommodationById(2);
        Console.WriteLine($"Found Accommodation 1: {foundAccommodation1}");
        Console.WriteLine($"Found Accommodation 2: {foundAccommodation2}");

        // Create reservations and add to accommodations
        try
        {
            if (foundClient1 != null && foundAccommodation1 != null)
            {
                BookingManager.CreateReservation(foundClient1.Id, foundAccommodation1.Id, DateTime.Now.AddDays(1),
                                                 DateTime.Now.AddDays(3));
                Console.WriteLine("Created and added reservation for client 1 in accommodation 1.");
            }
            if (foundClient2 != null && foundAccommodation2 != null)
            {

                BookingManager.CreateReservation(foundClient2.Id, foundAccommodation2.Id, DateTime.Now.AddDays(2),
                                                 DateTime.Now.AddDays(5));
                Console.WriteLine("Created and added reservation for client 2 in accommodation 2.");
            }
        }
        catch (ValidationException ex)
        {
            Console.WriteLine($"Failed to create reservation: {ex.Message} (Code: {ex.ErrorCode})");
        }

        // Show Accommodations with new reservations
        Console.WriteLine("\nAccommodations with reservations:");
        Console.WriteLine($"Accommodation 1: {foundAccommodation1}");
        Console.WriteLine($"Accommodation 2: {foundAccommodation2}");

        // Check accommodation availability after adding reservations
        if (foundAccommodation1 != null)
        {
            Console.WriteLine("\nChecking availability for Accommodation 1:");

            // Check availability on dates outside reserved range
            var checkDate1Start = DateTime.Now.AddDays(4);
            var checkDate1End = DateTime.Now.AddDays(6);
            var isAvailable1 = foundAccommodation1.IsAvailable(checkDate1Start, checkDate1End);
            Console.WriteLine($"Availability from {checkDate1Start} to {checkDate1End}: {isAvailable1}");

            // Check availability on dates overlapping with a reservation
            var checkDate2Start = DateTime.Now.AddDays(2);
            var checkDate2End = DateTime.Now.AddDays(4);
            var isAvailable2 = foundAccommodation1.IsAvailable(checkDate2Start, checkDate2End);
            Console.WriteLine($"Availability from {checkDate2Start} to {checkDate2End}: {isAvailable2}");
        }

        if (foundAccommodation2 != null)
        {
            Console.WriteLine("\nChecking availability for Accommodation 2:");

            // Check availability on dates outside reserved range
            var checkDate3Start = DateTime.Now.AddDays(6);
            var checkDate3End = DateTime.Now.AddDays(8);
            var isAvailable3 = foundAccommodation2.IsAvailable(checkDate3Start, checkDate3End);
            Console.WriteLine($"Availability from {checkDate3Start} to {checkDate3End}: {isAvailable3}");

            // Check availability on dates overlapping with a reservation
            var checkDate4Start = DateTime.Now.AddDays(3);
            var checkDate4End = DateTime.Now.AddDays(5);
            var isAvailable4 = foundAccommodation2.IsAvailable(checkDate4Start, checkDate4End);
            Console.WriteLine($"Availability from {checkDate4Start} to {checkDate4End}: {isAvailable4}");
        }
    }
}
}

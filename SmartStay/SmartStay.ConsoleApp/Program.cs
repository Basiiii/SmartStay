using Microsoft.Extensions.Logging;
using SmartStay.Core.Models;
using SmartStay.Core.Services;

namespace SmartStay.ConsoleApp
{
internal class Program
{
    public static void Main(string[] args)
    {
        // Set up logging
        using var loggerFactory = LoggerFactory.Create(builder =>
                                                       {
                                                           builder.AddConsole(); // Log to console
                                                       });
        ILogger<BookingManager> logger = loggerFactory.CreateLogger<BookingManager>();

        // Define the folder where data files will be saved/loaded from
        string dataFolder = Directory.GetCurrentDirectory();

        // Create the BookingManager instance
        var bookingManager = new BookingManager(logger);

        // Generate random data and populate repositories
        PopulateWithRandomData(bookingManager);

        // Save repositories to files
        bookingManager.SaveAll(dataFolder);
        Console.WriteLine("Repositories have been saved.");

        // Create a new BookingManager instance for loading data
        var bookingManagerLoaded = new BookingManager(logger);

        // Load repositories from files
        bookingManagerLoaded.LoadAll(dataFolder);
        Console.WriteLine("Repositories have been loaded.");

        // Verify the loaded data by exporting and printing it to the console
        Console.WriteLine("Clients:");
        Console.WriteLine(bookingManagerLoaded.Clients.Export());
        Console.WriteLine("Owners:");
        Console.WriteLine(bookingManagerLoaded.Owners.Export());
        Console.WriteLine("Reservations:");
        Console.WriteLine(bookingManagerLoaded.Reservations.Export());
        Console.WriteLine("Accommodations:");
        Console.WriteLine(bookingManagerLoaded.Accommodations.Export());
    }

    private static void PopulateWithRandomData(BookingManager bookingManager)
    {
        // Create random Clients
        var clients = new List<Client> { new Client("Client 1", "AA", "client1@example.com"),
                                         new Client("Client 2", "AA", "client2@example.com"),
                                         new Client("Client 3", "AA", "client3@example.com"),
                                         new Client("Client 4", "AA", "client4@example.com"),
                                         new Client("Client 5", "AA", "client5@example.com") };

        foreach (var client in clients)
        {
            bookingManager.Clients.Add(client);
        }

        // Create random Owners
        var owners = new List<Owner> { new Owner("Owner 1", "AA", "owner1@example.com"),
                                       new Owner("Owner 2", "AA", "owner2@example.com"),
                                       new Owner("Owner 3", "AA", "owner3@example.com") };

        foreach (var owner in owners)
        {
            bookingManager.Owners.Add(owner);
        }

        // Create random Accommodations
        var accommodations = new List<Accommodation> {
            new Accommodation(1, Common.Enums.AccommodationType.Hotel, "Location 1", "Address 1"),
            new Accommodation(2, Common.Enums.AccommodationType.Cabin, "Location 2", "Address 2"),
            new Accommodation(3, Common.Enums.AccommodationType.Hotel, "Location 3", "Address 3"),
            new Accommodation(4, Common.Enums.AccommodationType.Guesthouse, "Location 4", "Address 4")
        };

        foreach (var accommodation in accommodations)
        {
            accommodation.Rooms.Add(new Room(Common.Enums.RoomType.Deluxe, 10.0m));
            bookingManager.Accommodations.Add(accommodation);
        }

        // Variables to track last reservation's check-out date
        DateTime lastCheckOutDate = DateTime.Now;

        // Create random Reservations
        var rand = new Random();
        var reservations = new List<Reservation> {
            // First reservation: starts today, ends in 2-5 days
            new Reservation(1, 1, 1, Common.Enums.AccommodationType.Hotel, lastCheckOutDate.AddDays(rand.Next(1, 3)),
                            lastCheckOutDate.AddDays(rand.Next(4, 7)), 100.00m),

            // Second reservation: starts after the first reservation ends, ends in 2-5 days
            new Reservation(2, 2, 1, Common.Enums.AccommodationType.Cabin, lastCheckOutDate.AddDays(rand.Next(8, 10)),
                            lastCheckOutDate.AddDays(rand.Next(12, 15)), 150.00m),

            // Third reservation: starts after the second reservation ends, ends in 2-5 days
            new Reservation(3, 3, 2, Common.Enums.AccommodationType.Hotel, lastCheckOutDate.AddDays(rand.Next(18, 20)),
                            lastCheckOutDate.AddDays(rand.Next(22, 25)), 120.00m),

            // Fourth reservation: starts after the third reservation ends, ends in 2-5 days
            new Reservation(4, 4, 3, Common.Enums.AccommodationType.Guesthouse,
                            lastCheckOutDate.AddDays(rand.Next(28, 30)), lastCheckOutDate.AddDays(rand.Next(32, 35)),
                            80.00m),

            // Fifth reservation: starts after the fourth reservation ends, ends in 2-5 days
            new Reservation(5, 5, 2, Common.Enums.AccommodationType.Cabin, lastCheckOutDate.AddDays(rand.Next(38, 40)),
                            lastCheckOutDate.AddDays(rand.Next(42, 45)), 110.00m)
        };

        foreach (var reservation in reservations)
        {
            bookingManager.Reservations.Add(reservation);

            // Update the last check-out date for the next reservation to use
            lastCheckOutDate = reservation.CheckOutDate;
        }
    }
}
}

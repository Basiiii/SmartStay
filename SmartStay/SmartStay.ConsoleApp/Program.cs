using System.Net;
using Microsoft.Extensions.Logging;
using SmartStay.Common.Enums;
using SmartStay.Core.Models;
using SmartStay.Core.Repositories;
using SmartStay.Core.Services;
using SmartStay.Core.Utilities;

namespace SmartStay.ConsoleApp
{
internal class Program
{
    public static void Main(string[] args)
    {
        string jsonData = @"
        [
            {
                ""Id"": 1,
                ""FirstName"": ""John"",
                ""LastName"": ""Doe"",
                ""Email"": ""john.doe@example.com"",
                ""PhoneNumber"": ""1234567890"",
                ""Address"": ""123 Main St"",
                ""AccommodationsOwned"": [ 
                  { ""id"": 1, ""name"": ""Hotel Paradise"" },
                  { ""id"": 2, ""name"": ""Seaside Villa"" }
                ]
            },
            {
                ""Id"": 2,
                ""FirstName"": ""Jane"",
                ""LastName"": ""Smith"",
                ""Email"": ""jane.smith@example.com"",
                ""PhoneNumber"": ""0987654321"",
                ""Address"": ""456 Elm St"",
                ""AccommodationsOwned"": [
                  {
                    ""Id"": 42,
                    ""OwnerId"": 1,
                    ""Type"": 4,
                    ""Name"": ""nametest"",
                    ""Address"": ""addresstest"",
                    ""Rooms"": []
                  }
                ]
            }
        ]";

        // Create an instance of your class that contains the Import method
        var myOwnerManager = new Owners();

        // Call the Import method
        var result = myOwnerManager.Import(jsonData);

        // Display the result
        Console.WriteLine($"Imported: {result.ImportedCount}, Replaced: {result.ReplacedCount}");
        Console.WriteLine(myOwnerManager.Export());

        var acc = new Accommodations();
        acc.Add(new Accommodation(1, AccommodationType.Villa, "nametest", "addresstest"));

        var accom1 = acc.FindAccommodationById(1);
        accom1.AddRoom(new Room(RoomType.Accessible, 10.0m));
        accom1.AddRoom(new Room(RoomType.Family, 10.0m));

        var startDate = new DateTime(2024, 12, 10, 14, 0, 0); // December 10th, 2024 at 2:00 PM
        var endDate = new DateTime(2024, 12, 15, 11, 0, 0);   // December 15th, 2024 at 11:00 AM
        accom1.FindRoomById(1).AddReservation(startDate, endDate);

        var jsonacc = acc.Export();
        Console.WriteLine(jsonacc);

        var acc2 = new Accommodations();
        acc2.Import(jsonacc);
        var accommm = acc2.FindAccommodationById(1);
        var rooms = accommm.Rooms;

        // var clientRepo = new Clients();
        // var jsonData =
        //     "[{\"Id\": 1, \"FirstName\": \"John\", \"LastName\": \"Doe\", \"Email\": " +
        //     "\"johndoe@example.com\"}, {\"Id\": 23, \"FirstName\": " + "\"John\", \"LastName\": \"Doe\", \"Email\": "
        //     +
        //     "\"johndoe@example.com\", \"PhoneNumber\": \"+351123123123\"}]";

        //// Act
        // var result = clientRepo.Import(jsonData);

        // clientRepo.Add(new Client("test", "test", "Test@gmail.com"));

        // var owner = new Owner("test", "ffs", "example@gmail.com");
        // var ownerRepo = new Owners();
        // ownerRepo.Add(owner);
        // var json = ownerRepo.Export();

        // var jsonData12 =
        //     "[{\"id\": 1, \"firstName\": \"John\", \"lastName\": \"Doe\", \"email\": " +
        //     "\"johndoe@example.com\", \"phoneNumber\": \"+351123123123\", \"address\": \"123 Main St, " +
        //     "Anytown, USA\", \"accommodationsOwned\": [{\"id\": 101, \"name\": \"Beachside Villa\", " +
        //     "\"location\": \"Miami\", \"pricePerNight\": 300}]}, {\"id\": 2, \"firstName\": \"Alice\", " +
        //     "\"lastName\": \"Johnson\", \"email\": \"alice.johnson@example.com\", \"phoneNumber\": " +
        //     "\"+351987654321\", \"address\": \"456 Elm St, Othertown, USA\", \"accommodationsOwned\": []}]";

        // var ownerRepo2 = new Owners();
        // ownerRepo2.Import(jsonData12);

        // Console.WriteLine();

        string jsonData2 = @"[
    {
        ""Id"": 125,
        ""ClientId"": 125,
        ""AccommodationId"": 125,
        ""RoomId"": 152,
        ""AccommodationType"": 1,
        ""CheckInDate"": ""2024-12-04T23:56:21.3271239+00:00"",
        ""CheckOutDate"": ""2024-12-05T23:56:21.3319496+00:00"",
        ""TotalCost"": 100,
        ""Status"": 0,
        ""AmountPaid"": 0,
        ""Payments"": []
    }
]";

        // Deserialize into a List<Reservation>
        var reservations = JsonHelper.DeserializeFromJson<Reservation>(jsonData2);

        // Debugging: Print out the properties of each reservation
        foreach (var reservation in reservations)
        {
            Console.Write(reservation.ToString());
            Console.WriteLine($"Id: {reservation.Id}, ClientId: {reservation.ClientId}");
        }

        //    var accommodationRepo23 = new Accommodations();
        //    var jsonData23 = "[{\"Id\": 1, \"OwnerId\": 1, \"Type\": 1, \"Name\": \"Grand Hotel\", \"Address\": \"456
        //    " +
        //                     "Luxury Ave\", \"Rooms\": [], \"TotalRooms\": 0}]";

        //    // Act
        //    var result23 = accommodationRepo23.Import(jsonData23);
        //    Console.Write(accommodationRepo23.Export());

        //    var reservationsRepo = new Reservations();
        //    var reservation =
        //        new Reservation(1, 1, 101, AccommodationType.Hotel, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2),
        //        100m);
        //    reservationsRepo.Add(reservation);
        //    // Act
        //    var jsonData = reservationsRepo.Export();
        //    Console.WriteLine(jsonData);

        //    var reservationsRepo2 = new Reservations();
        //    var jsonData2 =
        //        "[{\"Id\": 1, \"ClientId\": 1, \"AccommodationId\": 1, \"RoomId\": 101, \"AccommodationType\": 1, " +
        //        "\"CheckInDate\": \"2024-12-04T23:56:21.3271239+00:00\", \"CheckOutDate\": " +
        //        "\"2024-12-05T23:56:21.3319496+00:00\", " +
        //        "\"TotalCost\": 100, \"Status\": 0, \"AmountPaid\": 0, \"Payments\": []}]";

        //    // Act
        //    var result = reservationsRepo2.Import(jsonData);
        //    Console.WriteLine(reservationsRepo2.Export());

        //    // Set up logging
        //    using var loggerFactory = LoggerFactory.Create(builder =>
        //                                                   {
        //                                                       builder.AddConsole(); // Log to console
        //                                                   });
        //    ILogger<BookingManager> logger = loggerFactory.CreateLogger<BookingManager>();

        //    // Define the folder where data files will be saved/loaded from
        //    string dataFolder = Directory.GetCurrentDirectory();

        //    // Create the BookingManager instance
        //    var bookingManager = new BookingManager(logger);

        //    // Generate random data and populate repositories
        //    PopulateWithRandomData(bookingManager);

        //    // Save repositories to files
        //    bookingManager.SaveAll(dataFolder);
        //    Console.WriteLine("Repositories have been saved.");

        //    // Create a new BookingManager instance for loading data
        //    var bookingManagerLoaded = new BookingManager(logger);

        //    // Load repositories from files
        //    bookingManagerLoaded.LoadAll(dataFolder);
        //    Console.WriteLine("Repositories have been loaded.");

        //    // Verify the loaded data by exporting and printing it to the console
        //    Console.WriteLine("Clients:");
        //    Console.WriteLine(bookingManagerLoaded.Clients.Export());
        //    Console.WriteLine("Owners:");
        //    Console.WriteLine(bookingManagerLoaded.Owners.Export());
        //    Console.WriteLine("Reservations:");
        //    Console.WriteLine(bookingManagerLoaded.Reservations.Export());
        //    Console.WriteLine("Accommodations:");
        //    Console.WriteLine(bookingManagerLoaded.Accommodations.Export());
        //}

        // private static void PopulateWithRandomData(BookingManager bookingManager)
        //{
        //     // Create random Clients
        //     var clients = new List<Client> { new Client("Client 1", "AA", "client1@example.com"),
        //                                      new Client("Client 2", "AA", "client2@example.com"),
        //                                      new Client("Client 3", "AA", "client3@example.com"),
        //                                      new Client("Client 4", "AA", "client4@example.com"),
        //                                      new Client("Client 5", "AA", "client5@example.com") };

        //    foreach (var client in clients)
        //    {
        //        bookingManager.Clients.Add(client);
        //    }

        //    // Create random Owners
        //    var owners = new List<Owner> { new Owner("Owner 1", "AA", "owner1@example.com"),
        //                                   new Owner("Owner 2", "AA", "owner2@example.com"),
        //                                   new Owner("Owner 3", "AA", "owner3@example.com") };

        // foreach (var owner in owners)
        //{
        //     bookingManager.Owners.Add(owner);
        // }

        //// Create random Accommodations
        // var accommodations = new List<Accommodation> {
        //     new Accommodation(1, Common.Enums.AccommodationType.Hotel, "Location 1", "Address 1"),
        //     new Accommodation(2, Common.Enums.AccommodationType.Cabin, "Location 2", "Address 2"),
        //     new Accommodation(3, Common.Enums.AccommodationType.Hotel, "Location 3", "Address 3"),
        //     new Accommodation(4, Common.Enums.AccommodationType.Guesthouse, "Location 4", "Address 4")
        // };

        // foreach (var accommodation in accommodations)
        //{
        //     accommodation.Rooms.Add(new Room(Common.Enums.RoomType.Deluxe, 10.0m));
        //     bookingManager.Accommodations.Add(accommodation);
        // }

        // Variables to track last reservation's check-out date
        // DateTime lastCheckOutDate = DateTime.Now;

        //// Create random Reservations
        // var rand = new Random();
        // var reservations = new List<Reservation> {
        //     // First reservation: starts today, ends in 2-5 days
        //     new Reservation(1, 1, 1, Common.Enums.AccommodationType.Hotel, lastCheckOutDate.AddDays(rand.Next(1, 3)),
        //                     lastCheckOutDate.AddDays(rand.Next(4, 7)), 100.00m),

        //    // Second reservation: starts after the first reservation ends, ends in 2-5 days
        //    new Reservation(2, 2, 1, Common.Enums.AccommodationType.Cabin, lastCheckOutDate.AddDays(rand.Next(8, 10)),
        //                    lastCheckOutDate.AddDays(rand.Next(12, 15)), 150.00m),

        //    // Third reservation: starts after the second reservation ends, ends in 2-5 days
        //    new Reservation(3, 3, 2, Common.Enums.AccommodationType.Hotel, lastCheckOutDate.AddDays(rand.Next(18,
        //    20)),
        //                    lastCheckOutDate.AddDays(rand.Next(22, 25)), 120.00m),

        //    // Fourth reservation: starts after the third reservation ends, ends in 2-5 days
        //    new Reservation(4, 4, 3, Common.Enums.AccommodationType.Guesthouse,
        //                    lastCheckOutDate.AddDays(rand.Next(28, 30)), lastCheckOutDate.AddDays(rand.Next(32, 35)),
        //                    80.00m),

        //    // Fifth reservation: starts after the fourth reservation ends, ends in 2-5 days
        //    new Reservation(5, 5, 2, Common.Enums.AccommodationType.Cabin, lastCheckOutDate.AddDays(rand.Next(38,
        //    40)),
        //                    lastCheckOutDate.AddDays(rand.Next(42, 45)), 110.00m)
        //};

        // foreach (var reservation in reservations)
        //{
        //     bookingManager.Reservations.Add(reservation);

        //    // Update the last check-out date for the next reservation to use
        //    lastCheckOutDate = reservation.CheckOutDate;
        //}
    }
}
}

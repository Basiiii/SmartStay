using System;

namespace SmartStay
{
/// <summary>
/// Represents a reservation in the system, including check-in status and payment details.
/// </summary>
internal class Reservation
{
    public int ReservationId { get; set; }
    public int ClientId { get; set; }
    public int AccommodationId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public decimal TotalCost { get; set; }

    /// <summary>
    /// Gets or sets the current status of the reservation (Pending, CheckedIn, CheckedOut).
    /// </summary>
    public ReservationStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the amount paid for the reservation.
    /// </summary>
    public decimal AmountPaid { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Reservation"/> class.
    /// </summary>
    /// <param name="reservationId">Unique ID of the reservation.</param>
    /// <param name="clientId">ID of the client who made the reservation.</param>
    /// <param name="accommodationId">ID of the accommodation being reserved.</param>
    /// <param name="checkInDate">Check-in date for the reservation.</param>
    /// <param name="checkOutDate">Check-out date for the reservation.</param>
    public Reservation(int reservationId, int clientId, int accommodationId, DateTime checkInDate,
                       DateTime checkOutDate, decimal totalCost)
    {
        this.ReservationId = reservationId;
        this.ClientId = clientId;
        this.AccommodationId = accommodationId;
        this.CheckInDate = checkInDate;
        this.CheckOutDate = checkOutDate;
        this.TotalCost = totalCost;
        this.Status = ReservationStatus.Pending; // Default status is Pending
        this.AmountPaid = 0;                     // No payment made initially
    }

    /// <summary>
    /// Updates the status of the reservation to CheckedIn.
    /// </summary>
    public void CheckIn()
    {
        if (Status == ReservationStatus.Pending)
        {
            Status = ReservationStatus.CheckedIn;
            Console.WriteLine($"Reservation {ReservationId} checked in.");
        }
        else
        {
            Console.WriteLine("Check-in is not allowed. Current status: " + Status);
        }
    }

    /// <summary>
    /// Updates the status of the reservation to CheckedOut.
    /// </summary>
    public void CheckOut()
    {
        if (Status == ReservationStatus.CheckedIn)
        {
            Status = ReservationStatus.CheckedOut;
            Console.WriteLine($"Reservation {ReservationId} checked out.");
        }
        else
        {
            Console.WriteLine("Check-out is not allowed. Current status: " + Status);
        }
    }

    /// <summary>
    /// Adds a payment for the reservation and checks if the total amount is paid.
    /// </summary>
    /// <param name="amount">The amount to be paid.</param>
    public void MakePayment(decimal amount)
    {
        AmountPaid += amount;
        if (AmountPaid >= TotalCost)
        {
            Console.WriteLine($"Reservation {ReservationId} is fully paid.");
        }
        else
        {
            Console.WriteLine($"Payment of {amount:C} made. Remaining balance: {(TotalCost - AmountPaid):C}.");
        }
    }

    /// <summary>
    /// Determines whether the reservation is fully paid.
    /// </summary>
    /// <returns>True if fully paid, false otherwise.</returns>
    public bool IsFullyPaid()
    {
        return AmountPaid >= TotalCost;
    }

    public override string ToString()
    {
        return $"Reservation [ID: {ReservationId}, Client ID: {ClientId}, Accommodation ID: {AccommodationId}, " +
               $"Check-In: {CheckInDate.ToShortDateString()}, Check-Out: {CheckOutDate.ToShortDateString()}, Total Cost: {TotalCost:C}, " +
               $"Amount Paid: {AmountPaid:C}, Status: {Status}]";
    }
}
}

using System;

namespace SmartStay
{
/// <summary>
/// Represents a payment made for a reservation.
/// </summary>
internal class Payment
{
    public int PaymentId { get; set; }
    public int ReservationId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; } // e.g., Credit Card, PayPal, etc.

    /// <summary>
    /// Initializes a new instance of the <see cref="Payment"/> class.
    /// </summary>
    /// <param name="paymentId">The payment's unique ID</param>
    /// <param name="reservationId">ID of the reservation</param>
    /// <param name="amount">Amount paid</param>
    /// <param name="paymentDate">Date of payment</param>
    /// <param name="paymentMethod">Method used for the payment</param>
    public Payment(int paymentId, int reservationId, decimal amount, DateTime paymentDate, string paymentMethod)
    {
        this.PaymentId = paymentId;
        this.ReservationId = reservationId;
        this.Amount = amount;
        this.PaymentDate = paymentDate;
        this.PaymentMethod = paymentMethod;
    }

    public override string ToString()
    {
        return $"Payment [ID: {PaymentId}, Reservation ID: {ReservationId}, Amount: {Amount:C}, Date: {PaymentDate}, Method: {PaymentMethod}]";
    }
}
}

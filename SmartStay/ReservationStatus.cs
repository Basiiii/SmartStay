namespace SmartStay
{
/// <summary>
/// Enum representing the current status of a reservation.
/// </summary>
public enum ReservationStatus
{
    Pending,   // Reservation made but not yet checked in
    CheckedIn, // Client has checked in
    CheckedOut // Client has checked out
}
}

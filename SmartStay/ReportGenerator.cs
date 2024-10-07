using System.IO;

namespace SmartStay
{
internal static class ReportGenerator
{
    public static void ExportReservationsToCSV(Dictionary<int, Reservation> reservations, string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(
                "ReservationId, ClientId, AccommodationId, CheckInDate, CheckOutDate, Status, AmountPaid, TotalCost");

            foreach (var reservation in reservations.Values)
            {
                writer.WriteLine(
                    $"{reservation.ReservationId}, {reservation.ClientId}, {reservation.AccommodationId}, " +
                    $"{reservation.CheckInDate.ToShortDateString()}, {reservation.CheckOutDate.ToShortDateString()}, " +
                    $"{reservation.Status}, {reservation.AmountPaid}, {reservation.TotalCost}");
            }
        }

        Console.WriteLine($"Reservations exported to {filePath}");
    }
}
}

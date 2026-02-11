namespace MovieReservationAPI.Models;

public class ReservationResponse
{
    public int Id { get; set; }
    public int ShowtimeId { get; set; }
    public string MovieTitle { get; set; }
    public DateTime ShowtimeDate { get; set; }
    public string Seat { get; set; }
    public DateTime ReservedAt { get; set; }
}
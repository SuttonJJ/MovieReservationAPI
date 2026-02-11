namespace MovieReservationAPI.Models;

public class CreateReservationRequest
{
    public int ShowtimeId { get; set; }
    public int SeatId { get; set; }
}
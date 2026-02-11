namespace MovieReservationAPI.Models;

public class CreateShowtimeRequest
{
    public int MovieId { get; set; }
    public DateTime Time { get; set; }
}
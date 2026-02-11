namespace MovieReservationAPI.Models;

public class UpdateShowtimeRequest
{
    public int MovieId { get; set; }
    public DateTime Time { get; set; }
}
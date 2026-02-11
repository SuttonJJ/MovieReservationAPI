namespace MovieReservationAPI.Models;

public class ShowtimeResponse
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public DateTime Time { get; set; }
}
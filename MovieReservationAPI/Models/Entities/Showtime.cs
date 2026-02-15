namespace MovieReservationAPI.Models.Entities;

public class Showtime
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public DateTime Time { get; set; }
    
    public Movie? Movie { get; set; } = null;
    public List<Reservation> Reservations { get; set; }
}
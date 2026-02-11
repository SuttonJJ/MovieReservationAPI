using MovieReservationAPI.Models.Auth;

namespace MovieReservationAPI.Models;

public class Reservation
{
    public int Id { get; set; }
    public int ShowtimeId { get; set; }
    public int SeatId { get; set; }
    public string UserId { get; set; }
    public DateTime ReservedAt { get; set; }
    
    public Showtime Showtime { get; set; }
    public Seat Seat { get; set; }
    public AppUser User { get; set; }
}
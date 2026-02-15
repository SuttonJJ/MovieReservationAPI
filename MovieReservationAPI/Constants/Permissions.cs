namespace MovieReservationAPI.Constants;

public class Permissions
{
    // Movie permissions
    public const string ViewMovie = "Permissions.Movie.View";
    public const string CreateMovie = "Permissions.Movie.Create";
    public const string UpdateMovie = "Permissions.Movie.Update";
    public const string DeleteMovie = "Permissions.Movie.Delete";
    
    // Showtime permissions
    public const string ViewShowtime = "Permissions.Showtime.View";
    public const string CreateShowtime = "Permissions.Showtime.Create";
    public const string UpdateShowtime = "Permissions.Showtime.Update";
    public const string DeleteShowtime = "Permissions.Showtime.Delete";
    
    public const string ViewReservation = "Permissions.Reservation.";
    public const string CreateReservation = "Permissions.Reservation.Create";
    public const string DeleteReservation = "Permissions.Reservation.Delete";
}
namespace MovieReservationAPI.Models.Entities;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;


    // Navigation property
    public List<Showtime> Showtimes { get; set; } = new();
    public List<MovieGenre> MovieGenres { get; set; }
}
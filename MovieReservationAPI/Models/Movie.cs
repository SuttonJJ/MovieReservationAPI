namespace MovieReservationAPI.Models.Movie;

public class Movie
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    
    // Navigation property
    public List<> Showtimes { get; set; } = new();
}
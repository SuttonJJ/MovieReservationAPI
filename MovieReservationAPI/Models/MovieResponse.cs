namespace MovieReservationAPI.Models;

public class MovieResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<string> Genres { get; set; }
}
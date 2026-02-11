namespace MovieReservationAPI.Models;

public class CreateMovieRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<int> GenreIds { get; set; }
}
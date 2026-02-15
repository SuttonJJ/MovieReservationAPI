using System.ComponentModel.DataAnnotations;

namespace MovieReservationAPI.Models.DTOs;

public class CreateMovieRequest
{
    [Required(ErrorMessage = "Title is required")]
    [Length(5, 200, ErrorMessage = "Please make the title between 5 and 200 characters")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Description is required")]
    [Length(10, 2000, ErrorMessage = "Description needs to be between 10 and 2000 characters")]
    public string Description { get; set; }
    [Required(ErrorMessage = "Genres are required")]
    [MinLength(1, ErrorMessage = "Please provide at least one genre")]
    public List<int> GenreIds { get; set; }
}
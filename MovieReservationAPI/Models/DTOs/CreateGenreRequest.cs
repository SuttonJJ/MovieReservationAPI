using System.ComponentModel.DataAnnotations;

namespace MovieReservationAPI.Models.DTOs;

public class CreateGenreRequest
{
    [Required(ErrorMessage = "Name is required")]
    [Length(2, 50, ErrorMessage = "Please provide a valid length")]
    public string Name { get; set; }
}
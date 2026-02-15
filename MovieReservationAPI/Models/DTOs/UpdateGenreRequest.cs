using System.ComponentModel.DataAnnotations;

namespace MovieReservationAPI.Models.DTOs;

public class UpdateGenreRequest
{
    [Required(ErrorMessage = "Please provide a name")]
    [Length(5, 50, ErrorMessage = "Please make sure the name is between 5 and 50 characters")]
    public string Name { get; set; }
}
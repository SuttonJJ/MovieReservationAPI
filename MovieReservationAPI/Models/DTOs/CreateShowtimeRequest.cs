using System.ComponentModel.DataAnnotations;

namespace MovieReservationAPI.Models.DTOs;

public class CreateShowtimeRequest
{
    [Required(ErrorMessage = "Please provide a movie")]
    public int MovieId { get; set; }
    [Required(ErrorMessage = "Please provide a time")]
    public DateTime Time { get; set; }
}
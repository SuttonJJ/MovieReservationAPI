using System.ComponentModel.DataAnnotations;

namespace MovieReservationAPI.Models.DTOs;

public class UpdateShowtimeRequest
{
    [Required(ErrorMessage = "Please provide a time")]
    public DateTime Time { get; set; }
}
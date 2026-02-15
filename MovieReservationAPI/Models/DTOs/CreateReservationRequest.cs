using System.ComponentModel.DataAnnotations;

namespace MovieReservationAPI.Models.DTOs;

public class CreateReservationRequest
{
    [Required(ErrorMessage = "Please provide a showtime")]
    public int ShowtimeId { get; set; }
    [Required(ErrorMessage = "Please provide a seat")]
    public int SeatId { get; set; }
}
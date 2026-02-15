using Microsoft.AspNetCore.Identity;
using MovieReservationAPI.Models.Entities;

namespace MovieReservationAPI.Models.Auth;

public class AppUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }

    public List<Reservation> Reservations { get; set; }
}
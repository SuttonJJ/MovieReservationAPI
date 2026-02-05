using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MovieReservationAPI.Models.Auth;

public class MovieContext : IdentityDbContext<AppUser>
{
    public MovieContext(DbContextOptions<MovieContext> options) : base(options)
    {
    }
}
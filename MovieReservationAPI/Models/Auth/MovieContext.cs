using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MovieReservationAPI.Models.Auth;

public class MovieContext : IdentityDbContext<AppUser>
{
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Showtime> Showtimes { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Seat> Seats { get; set; }
    
    
    public MovieContext(DbContextOptions<MovieContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<MovieGenre>()
            .HasKey(mg => new { mg.MovieId, mg.GenreId });
    
        builder.Entity<Seat>()
            .HasIndex(s => new { s.Row, s.Number })
            .IsUnique();
    
        builder.Entity<Reservation>()
            .HasIndex(r => new { r.ShowtimeId, r.SeatId })
            .IsUnique();
        
        // Seed seats
        var seats = new List<Seat>();
        int seatId = 1;
    
        // Create rows A-J with 10 seats each
        foreach (var row in new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" })
        {
            for (int number = 1; number <= 10; number++)
            {
                seats.Add(new Seat
                {
                    Id = seatId++,
                    Row = row,
                    Number = number
                });
            }
        }
    
        builder.Entity<Seat>().HasData(seats);
    }
}
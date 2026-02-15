using FluentResults;
using Microsoft.EntityFrameworkCore;
using MovieReservationAPI.Data;
using MovieReservationAPI.Models.DTOs;
using MovieReservationAPI.Models.Entities;

namespace MovieReservationAPI.Services;

public interface IReservationService
{
    public Task<Result<List<ReservationResponse>>> GetReservations(string userId);
    public Task<Result<ReservationResponse>> CreateReservation(CreateReservationRequest request, string userId);
    public Task<Result> DeleteReservation(int id, string userId);
}

public class ReservationService(MovieContext context) : IReservationService
{
    public async Task<Result<List<ReservationResponse>>> GetReservations(string userId)
    {
        var reservations = await context.Reservations
            .Include(r => r.Showtime)
                .ThenInclude(s => s.Movie)
            .Include(r => r.Seat)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.Showtime.Time)
            .ToListAsync();

        if (!reservations.Any()) return Result.Fail("No reservations found");
        
        var response = reservations.Select(r => new ReservationResponse
        {
            Id = r.Id,
            ShowtimeId = r.ShowtimeId,
            MovieTitle = r.Showtime.Movie.Title,
            ShowtimeDate = r.Showtime.Time,
            Seat = $"{r.Seat.Row}{r.Seat.Number}",
            ReservedAt = r.ReservedAt   
        }).ToList();

        return Result.Ok(response);
    }

    public async Task<Result<ReservationResponse>> CreateReservation(CreateReservationRequest request, string userId)
    {
        var showtime = await context.Showtimes.Include(s => s.Movie)
            .FirstOrDefaultAsync(s => s.Id == request.ShowtimeId);
        var seat = await context.Seats.FindAsync(request.SeatId);

        if (showtime == null || seat == null) return Result.Fail("Showtime or seat does not exist");

        if (showtime.Time < DateTime.UtcNow) return Result.Fail("Cannot reserve seats for past showtimes");

        var isReserved =
            await context.Reservations.AnyAsync(r => r.ShowtimeId == request.ShowtimeId && r.SeatId == request.SeatId);

        if (isReserved) return Result.Fail("This seat is taken");

        var reservation = new Reservation
        {
            SeatId = seat.Id,
            ShowtimeId = showtime.Id,
            ReservedAt = DateTime.UtcNow,
            UserId = userId
        };

        await context.Reservations.AddAsync(reservation);
        await context.SaveChangesAsync();

        var response = new ReservationResponse
        {
            Id = reservation.Id,
            MovieTitle = showtime.Movie.Title,
            ReservedAt = reservation.ReservedAt,
            Seat = $"{seat.Row}{seat.Number}",
            ShowtimeDate = showtime.Time,
            ShowtimeId = showtime.Id
        };

        return Result.Ok(response);
    }

    public async Task<Result> DeleteReservation(int id, string userId)
    {
        var reservation = await context.Reservations.FindAsync(id);
        if (reservation == null) return Result.Fail("Reservation doesnt exist");

        if (reservation.UserId != userId) return Result.Fail("User unauthorized");

        context.Reservations.Remove(reservation);
        await context.SaveChangesAsync();
        return Result.Ok();
    }
}
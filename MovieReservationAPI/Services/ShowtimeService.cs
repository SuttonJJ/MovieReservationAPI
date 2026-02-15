using FluentResults;
using Microsoft.EntityFrameworkCore;
using MovieReservationAPI.Data;
using MovieReservationAPI.Models.DTOs;
using MovieReservationAPI.Models.Entities;

namespace MovieReservationAPI.Services;

public interface IShowtimeService
{
    public Task<Result<List<ShowtimeResponse>>> GetMovieShowtimes(int movieId);
    public Task<Result<List<ShowtimeResponse>>> GetMovieShowtimeDate(int movieId, DateTime dateTime);
    public Task<Result<ShowtimeResponse>> CreateShowtime(CreateShowtimeRequest request);
    public Task<Result<ShowtimeResponse>> UpdateShowtime(int id, UpdateShowtimeRequest request);
    public Task<Result> DeleteShowtime(int id);
}

public class ShowtimeService(MovieContext context) : IShowtimeService
{
    public async Task<Result<List<ShowtimeResponse>>> GetMovieShowtimes(int movieId)
    {
        var showtimes = await context.Showtimes.Where(s => s.MovieId == movieId)
            .OrderBy(s => s.Time)
            .ToListAsync();
        
        List<ShowtimeResponse> response = showtimes.Select(s => new ShowtimeResponse
        {
            Id = s.Id,
            MovieId = s.MovieId,
            Time = s.Time
        }).ToList();

        return Result.Ok(response);
    }

    public async Task<Result<List<ShowtimeResponse>>> GetMovieShowtimeDate(int movieId, DateTime date)
    {
        var startOfDay = date.Date;
        var endOfDay = startOfDay.AddDays(1);
        
        var showtimes = await context.Showtimes.Where(s => s.MovieId == movieId && s.Time >= startOfDay && s.Time < endOfDay)
            .OrderBy(s => s.Time)
            .ToListAsync();
        
        List<ShowtimeResponse> response = showtimes.Select(s => new ShowtimeResponse
        {
            Id = s.Id,
            MovieId = s.MovieId,
            Time = s.Time
        }).ToList();

        return Result.Ok(response);
    }

    public async Task<Result<ShowtimeResponse>> CreateShowtime(CreateShowtimeRequest request)
    {
        var movie = await context.Movies.FindAsync(request.MovieId);
        if (movie == null) return Result.Fail("This movie does not exist");

        var showtime = new Showtime
        {
            MovieId = request.MovieId,
            Time = request.Time
        };

        await context.Showtimes.AddAsync(showtime);
        await context.SaveChangesAsync();

        var response = new ShowtimeResponse
        {
            Id = showtime.Id,
            MovieId = showtime.MovieId,
            Time = showtime.Time
        };

        return Result.Ok(response);
    }

    public async Task<Result<ShowtimeResponse>> UpdateShowtime(int showtimeId, UpdateShowtimeRequest request)
    {
        Showtime? showtime = await context.Showtimes.FindAsync(showtimeId);

        if (showtime == null) return Result.Fail("This showtime doesnt exist");

        var movie = await context.Movies.FindAsync(showtimeId);
        if (movie == null) return Result.Fail("This movie doesnt exist");
        
        showtime.MovieId = showtimeId;
        showtime.Time = request.Time;

        await context.SaveChangesAsync();

        return Result.Ok(new ShowtimeResponse
        {
            Id = showtime.Id,
            MovieId = showtimeId,
            Time = request.Time
        });
    }

    public async Task<Result> DeleteShowtime(int id)
    {
        var showtime = await context.Showtimes.FindAsync(id);

        if (showtime == null) return Result.Fail("This showtime does not exist");

        context.Showtimes.Remove(showtime);

        await context.SaveChangesAsync();

        return Result.Ok();
    }
}
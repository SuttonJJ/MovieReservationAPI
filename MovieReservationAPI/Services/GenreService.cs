using FluentResults;
using Microsoft.EntityFrameworkCore;
using MovieReservationAPI.Models.Auth;
using MovieReservationAPI.Models.DTOs;
using MovieReservationAPI.Models.Entities;

namespace MovieReservationAPI.Services;

public interface IGenreService
{
    Task<Result<List<GenreResponse>>> GetAllGenres();
    Task<Result<GenreResponse>> GetGenreById(int id);
    Task<Result<GenreResponse>> CreateGenre(CreateGenreRequest request);
    Task<Result<GenreResponse>> UpdateGenre(int id, UpdateGenreRequest request);
    Task<Result> DeleteGenre(int id);
}

public class GenreService(MovieContext context) : IGenreService
{
    public async Task<Result<List<GenreResponse>>> GetAllGenres()
    {
        var genres = await context.Genres.ToListAsync();

        if (!genres.Any()) 
            return Result.Fail("No genres available");

        var response = genres.Select(g => new GenreResponse
        {
            Id = g.Id,
            Name = g.Name
        }).ToList();

        return Result.Ok(response);
    }

    public async Task<Result<GenreResponse>> GetGenreById(int id)
    {
        var genre = await context.Genres.FindAsync(id);

        if (genre == null) 
            return Result.Fail("Genre not found");

        var response = new GenreResponse
        {
            Id = genre.Id,
            Name = genre.Name
        };

        return Result.Ok(response);
    }

    public async Task<Result<GenreResponse>> CreateGenre(CreateGenreRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result.Fail("Genre name is required");

        var exists = await context.Genres.AnyAsync(g => g.Name == request.Name);
        if (exists)
            return Result.Fail("Genre already exists");

        var genre = new Genre
        {
            Name = request.Name
        };

        await context.Genres.AddAsync(genre);
        await context.SaveChangesAsync();

        var response = new GenreResponse
        {
            Id = genre.Id,
            Name = genre.Name
        };

        return Result.Ok(response);
    }

    public async Task<Result<GenreResponse>> UpdateGenre(int id, UpdateGenreRequest request)
    {
        var genre = await context.Genres.FindAsync(id);

        if (genre == null)
            return Result.Fail("Genre not found");

        if (string.IsNullOrWhiteSpace(request.Name))
            return Result.Fail("Genre name is required");

        var exists = await context.Genres
            .AnyAsync(g => g.Name == request.Name && g.Id != id);
        if (exists)
            return Result.Fail("Genre name already exists");

        genre.Name = request.Name;

        await context.SaveChangesAsync();

        var response = new GenreResponse
        {
            Id = genre.Id,
            Name = genre.Name
        };

        return Result.Ok(response);
    }

    public async Task<Result> DeleteGenre(int id)
    {
        var genre = await context.Genres
            .Include(g => g.MovieGenres)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (genre == null)
            return Result.Fail("Genre not found");

        if (genre.MovieGenres.Any())
            return Result.Fail("Cannot delete genre that is assigned to movies");

        context.Genres.Remove(genre);
        await context.SaveChangesAsync();

        return Result.Ok();
    }
}
using FluentResults;
using Microsoft.EntityFrameworkCore;
using MovieReservationAPI.Models.Auth;
using MovieReservationAPI.Models.DTOs;
using MovieReservationAPI.Models.Entities;

namespace MovieReservationAPI.Services;

public interface IMovieService
{
    public Task<Result<List<MovieResponse>>> GetAllMovies();
    public Task<Result<MovieResponse>> AddMovie(CreateMovieRequest request);
    public Task<Result<MovieResponse>> UpdateMovie(int id, UpdateMovieRequest request);
    public Task<Result> DeleteMovie(int id);
}

public class MovieService(MovieContext context) : IMovieService
{
    public async Task<Result<List<MovieResponse>>> GetAllMovies()
    {
        var movies = await context.Movies.Include(m => m.MovieGenres)
            .ThenInclude(mg => mg.Genre)
            .ToListAsync();

        if (!movies.Any()) return Result.Fail("No movies available");

        var movieList = movies.Select(m => new MovieResponse
        {
            Id = m.Id,
            Title = m.Title,
            Description = m.Description,
            Genres = m.MovieGenres
                .Select(mg => mg.Genre.Name)
                .ToList()
        }).ToList();

        return Result.Ok(movieList);
    }

    public async Task<Result<MovieResponse>> AddMovie(CreateMovieRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Description))
            return Result.Fail("Please provide a title and description");

        var validGenreIds = await context.Genres
            .Where(g => request.GenreIds.Contains(g.Id))
            .Select(g => g.Id)
            .ToListAsync();

        if (validGenreIds.Count != request.GenreIds.Count) return Result.Fail("Invalid genre ID found"); 
            
        var movie = new Movie
        {
            Title = request.Title,
            Description = request.Description,
            MovieGenres = new List<MovieGenre>()
        };

        foreach (var genreId in request.GenreIds)
        {
            movie.MovieGenres.Add(new MovieGenre
            {
                Movie = movie,
                GenreId = genreId
            });
        }

        await context.Movies.AddAsync(movie);
        await context.SaveChangesAsync();

        await context.Entry(movie)
            .Collection(m => m.MovieGenres)
            .Query()
            .Include(mg => mg.Genre)
            .LoadAsync();
        
        var response = new MovieResponse
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            Genres = movie.MovieGenres
                .Select(mg => mg.Genre.Name)
                .ToList()
        };

        return Result.Ok(response);
    }

    public async Task<Result<MovieResponse>> UpdateMovie(int id, UpdateMovieRequest request)
    {    
        var movie = await context.Movies
            .Include(m => m.MovieGenres)
            .FirstOrDefaultAsync(m => m.Id == id);
    
        if (movie == null)
            return Result.Fail<MovieResponse>("Movie not found");

        var validGenreIds = await context.Genres.Where(g => request.GenreIds.Contains(g.Id))
            .Select(g => g.Id)
            .ToListAsync();
        
        if (validGenreIds.Count != request.GenreIds.Count)
            return Result.Fail<MovieResponse>("Invalid genre ID found");
    
        movie.Title = request.Title;
        movie.Description = request.Description;
    
        movie.MovieGenres.Clear();
    
        foreach (var genreId in request.GenreIds)
        {
            movie.MovieGenres.Add(new MovieGenre
            {
                MovieId = id,
                GenreId = genreId
            });
        }
    
        await context.SaveChangesAsync();
        
        await context.Entry(movie)
            .Collection(m => m.MovieGenres)
            .Query()
            .Include(mg => mg.Genre)
            .LoadAsync();

        var response = new MovieResponse
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            Genres = movie.MovieGenres
                .Select(mg => mg.Genre.Name)
                .ToList()
        };
    
        return Result.Ok(response);
    }

    public async Task<Result> DeleteMovie(int movieId)
    {
        var movie = await context.Movies.FindAsync(movieId);

        if (movie == null) return Result.Fail("Could not find this movie");

        context.Movies.Remove(movie);
        await context.SaveChangesAsync();

        return Result.Ok();
    }
}
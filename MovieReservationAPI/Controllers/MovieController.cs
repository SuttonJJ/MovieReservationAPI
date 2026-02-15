using Microsoft.AspNetCore.Mvc;
using MovieReservationAPI.Models;
using MovieReservationAPI.Models.DTOs;
using MovieReservationAPI.Services;

namespace MovieReservationAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieController(IMovieService movieService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllMovies()
    {
        var result = await movieService.GetAllMovies();

        if (result.IsFailed) return BadRequest(result.Errors);
        
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> AddMovie(CreateMovieRequest request)
    {
        var result = await movieService.AddMovie(request);

        if (result.IsFailed) return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMovie(int id, UpdateMovieRequest request)
    {
        var result = await movieService.UpdateMovie(id, request);

        if (result.IsFailed) return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        var result = await movieService.DeleteMovie(id);

        if (result.IsFailed) return NotFound(result.Errors);

        return NoContent();
    }
}
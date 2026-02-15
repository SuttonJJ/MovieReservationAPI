using Microsoft.AspNetCore.Mvc;
using MovieReservationAPI.Models;
using MovieReservationAPI.Models.DTOs;
using MovieReservationAPI.Services;

namespace MovieReservationAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenreController(IGenreService genreService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllGenres()
    {
        var result = await genreService.GetAllGenres();
        
        if (result.IsFailed) return BadRequest(result.Errors);
        
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGenreById(int id)
    {
        var result = await genreService.GetGenreById(id);
        
        if (result.IsFailed) return BadRequest(result.Errors);
        
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateGenre([FromBody] CreateGenreRequest request)
    {
        var result = await genreService.CreateGenre(request);
        
        if (result.IsFailed) return BadRequest(result.Errors);
        
        return Ok(result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGenre(int id, [FromBody] UpdateGenreRequest request)
    {
        var result = await genreService.UpdateGenre(id, request);
        
        if (result.IsFailed) return BadRequest(result.Errors);
        
        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGenre(int id)
    {
        var result = await genreService.DeleteGenre(id);
        
        if (result.IsFailed) return BadRequest(result.Errors);

        return NoContent();
    }
}
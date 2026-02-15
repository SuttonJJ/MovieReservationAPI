using Microsoft.AspNetCore.Mvc;
using MovieReservationAPI.Models.DTOs;
using MovieReservationAPI.Services;

namespace MovieReservationAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShowtimeController(IShowtimeService showtimeService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetShowtimes(int id, [FromQuery] DateTime? date = null)
    {
        // Checks whether to get movie on a certain date or all dates
        var result = date.HasValue 
            ? await showtimeService.GetMovieShowtimeDate(id, date.Value)
            : await showtimeService.GetMovieShowtimes(id);
    
        if (result.IsFailed) return NotFound(result.Errors);
    
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateShowtime(CreateShowtimeRequest request)
    {
        var result = await showtimeService.CreateShowtime(request);

        if (result.IsFailed) return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateShowtime(int id, UpdateShowtimeRequest request)
    {
        var result = await showtimeService.UpdateShowtime(id, request);

        if (result.IsFailed) return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShowtime(int id)
    {
        var result = await showtimeService.DeleteShowtime(id);
        
        if (result.IsFailed) return NotFound(result.Errors);

        return NoContent();
    }
}
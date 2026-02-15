using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MovieReservationAPI.Models;
using MovieReservationAPI.Models.DTOs;
using MovieReservationAPI.Services;

namespace MovieReservationAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationController(IReservationService reservationService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUserReservations()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var result = await reservationService.GetReservations(userId);

        if (result.IsFailed) return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReservation([FromBody] CreateReservationRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await reservationService.CreateReservation(request, userId);

        if (result.IsFailed) return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReservation(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await reservationService.DeleteReservation(id, userId);

        if (result.IsFailed) return BadRequest(result.Errors);

        return NoContent();
    }
}
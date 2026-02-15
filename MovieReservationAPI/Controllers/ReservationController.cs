using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReservationAPI.Constants;
using MovieReservationAPI.Models;
using MovieReservationAPI.Models.DTOs;
using MovieReservationAPI.Services;

namespace MovieReservationAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationController(IReservationService reservationService) : ControllerBase
{
    [Authorize(Policy = Permissions.ViewReservation)]
    [HttpGet]
    public async Task<IActionResult> GetUserReservations()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var result = await reservationService.GetReservations(userId);

        if (result.IsFailed) return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [Authorize(Policy = Permissions.CreateReservation)]
    [HttpPost]
    public async Task<IActionResult> CreateReservation([FromBody] CreateReservationRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await reservationService.CreateReservation(request, userId);

        if (result.IsFailed) return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [Authorize(Policy = Permissions.DeleteReservation)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReservation(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await reservationService.DeleteReservation(id, userId);

        if (result.IsFailed) return BadRequest(result.Errors);

        return NoContent();
    }
}
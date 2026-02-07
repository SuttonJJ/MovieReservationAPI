using Microsoft.AspNetCore.Mvc;
using MovieReservationAPI.Models.Auth;
using MovieReservationAPI.Services;

namespace MovieReservationAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await authService.LoginAsync(request);

        if (result.IsFailed) return Unauthorized(result.Errors);

        return Ok(result.Value);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await authService.RegisterAsync(request);

        if (result.IsFailed) return Unauthorized(result.Errors);

        return Ok(result.Value);
    }
}
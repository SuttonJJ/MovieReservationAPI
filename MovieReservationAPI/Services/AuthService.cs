using FluentResults;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using MovieReservationAPI.Models.Auth;

namespace MovieReservationAPI.Services;

public interface IAuthService
{
    Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request);
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request);
    Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken);
}

public class AuthService(MovieContext context, UserManager<AppUser> userManager, ITokenService tokenService) : IAuthService
{
    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await userManager.FindByEmailAsync(request.Email);
        if (existingUser != null) return Result.Fail<AuthResponse>("A user with this email already exists!");

        AppUser user = new AppUser
        {
            UserName = request.Email,
            Email = request.Email
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return Result.Fail<AuthResponse>(errors);
        }

        await userManager.AddToRoleAsync(user, "User");
        
        // Gen tokens
        var accessToken = await tokenService.GenerateJwtToken(user);
        var refreshToken = tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await userManager.UpdateAsync(user);

        return Result.Ok(new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Email = user.Email
        });
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }
}
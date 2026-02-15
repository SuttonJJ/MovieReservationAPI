using System.Security.Claims;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using MovieReservationAPI.Data;
using MovieReservationAPI.Models.Auth;

namespace MovieReservationAPI.Services;

public interface IAuthService
{
    Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request);
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request);
    Task<Result<AuthResponse>> RefreshTokenAsync(TokenRequest refreshToken);
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
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null) return Result.Fail<AuthResponse>("Email not found");

        var success = await userManager.CheckPasswordAsync(user, request.Password);
        if (!success) return Result.Fail<AuthResponse>("Email or password incorrect");

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

    public async Task<Result<AuthResponse>> RefreshTokenAsync(TokenRequest request)
    {
        if (request == null) return Result.Fail<AuthResponse>("Invalid request");

        string accessToken = request.AccessToken;
        string refreshToken = request.RefreshToken;
        
        ClaimsPrincipal principal;
        try
        {
            principal = tokenService.GetPrincipalFromExpiredToken(accessToken);
        }
        catch
        {
            return Result.Fail<AuthResponse>("Invalid access token");
        }

        string? username = principal.Identity?.Name;
        if (string.IsNullOrEmpty(username))
        {
            return Result.Fail<AuthResponse>("Invalid token claims");
        }
        var user = await userManager.FindByNameAsync(username);
        
        if (user == null || 
            user.RefreshToken != refreshToken || 
            user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return Result.Fail<AuthResponse>("Invalid refresh token");
        }

        var newAccessToken = await tokenService.GenerateJwtToken(user);
        var newRefreshToken = tokenService.GenerateRefreshToken();
        
        user.RefreshToken = newRefreshToken;
        await userManager.UpdateAsync(user);

        return Result.Ok(new AuthResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            Email = user.Email
        });
    }
}
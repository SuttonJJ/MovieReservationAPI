using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieReservationAPI.Constants;
using MovieReservationAPI.Data;
using MovieReservationAPI.Models.Auth;
using MovieReservationAPI.Services;

namespace MovieReservationAPI;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddScoped<IGenreService, GenreService>();
        builder.Services.AddScoped<IMovieService, MovieService>();
        builder.Services.AddScoped<IReservationService, ReservationService>();
        builder.Services.AddScoped<IShowtimeService, ShowtimeService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IAuthService, AuthService>();

        builder.Services.AddDbContext<MovieContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<MovieContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
                };
            });

        // Permission management
        builder.Services.AddAuthorization(options =>
        {
            // Movie
            options.AddPolicy(Permissions.ViewMovie, policy => policy.RequireClaim("Permission", Permissions.ViewMovie));
            options.AddPolicy(Permissions.CreateMovie, policy => policy.RequireClaim("Permission", Permissions.CreateMovie));
            options.AddPolicy(Permissions.DeleteMovie, policy => policy.RequireClaim("Permission", Permissions.DeleteMovie));
            options.AddPolicy(Permissions.UpdateMovie, policy => policy.RequireClaim("Permission", Permissions.UpdateMovie));
            
            // Showtime
            options.AddPolicy(Permissions.ViewShowtime, policy => policy.RequireClaim("Permission", Permissions.ViewShowtime));
            options.AddPolicy(Permissions.CreateShowtime, policy => policy.RequireClaim("Permission", Permissions.CreateShowtime));
            options.AddPolicy(Permissions.UpdateShowtime, policy => policy.RequireClaim("Permission", Permissions.UpdateShowtime));
            options.AddPolicy(Permissions.DeleteShowtime, policy => policy.RequireClaim("Permission", Permissions.DeleteShowtime));
            
            // Reservation
            options.AddPolicy(Permissions.ViewReservation, policy => policy.RequireClaim("Permission", Permissions.ViewReservation));
            options.AddPolicy(Permissions.CreateReservation, policy => policy.RequireClaim("Permission", Permissions.CreateReservation));
            options.AddPolicy(Permissions.DeleteReservation, policy => policy.RequireClaim("Permission", Permissions.DeleteReservation));
        });

        builder.Services.AddOpenApi();
        builder.Services.AddControllers();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        
        // Seeding defaults
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            await RoleSeeder.SeedRolesAndPermissions(roleManager, userManager);
        }

        app.Run();
    }
}
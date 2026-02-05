using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MovieReservationAPI.Constants;
using MovieReservationAPI.Models.Auth;

namespace MovieReservationAPI.Services;

public class RoleSeeder
{
    public static async Task SeedRolesAndPermissions(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
    {
        await SeedAdminRole(roleManager);
        await SeedUserRole(roleManager);
        await SeedAdmins(userManager);
    }

    private static async Task SeedAdminRole(RoleManager<IdentityRole> roleManager)
    {
        if (await roleManager.RoleExistsAsync("Admin"))
            return;

        var adminRole = new IdentityRole("Admin");
        await roleManager.CreateAsync(adminRole);

        // Movie Permissions
        await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.ViewMovie));
        await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.CreateMovie));
        await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.UpdateMovie));
        await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.DeleteMovie));
        
        // Showtime Permissions
        await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.ViewShowtime));
        await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.CreateShowtime));
        await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.UpdateShowtime));
        await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.DeleteShowtime));

        // TODO: RESERVATIONS
    }

    private static async Task SeedUserRole(RoleManager<IdentityRole> roleManager)
    {
        if (await roleManager.RoleExistsAsync("User"))
            return; 

        var userRole = new IdentityRole("User");
        await roleManager.CreateAsync(userRole);

        await roleManager.AddClaimAsync(userRole, new Claim("Permission", Permissions.ViewMovie));
        await roleManager.AddClaimAsync(userRole, new Claim("Permission", Permissions.ViewShowtime));
        
        // TODO: RESERVATIONS
    }

    private static async Task SeedAdmins(UserManager<AppUser> userManager)
    {
        var user = await userManager.FindByEmailAsync("Johannes.sutton2003@gmail.com");
        if (user == null || await userManager.IsInRoleAsync(user, "Admin")) return;

        await userManager.AddToRolesAsync(user, new List<string>{"admin", "user"});
    }
}
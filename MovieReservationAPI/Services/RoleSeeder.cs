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

        // Movie 
        await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.ViewMovie));
        await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.CreateMovie));
        await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.UpdateMovie));
        await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.DeleteMovie));
        
        // Showtime 
        await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.ViewShowtime));
        await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.CreateShowtime));
        await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.UpdateShowtime));
        await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.DeleteShowtime));

        // Reservation
        await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.ViewReservation));
        await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.CreateReservation));
        await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.DeleteReservation));
    }

    private static async Task SeedUserRole(RoleManager<IdentityRole> roleManager)
    {
        if (await roleManager.RoleExistsAsync("User"))
            return; 

        var userRole = new IdentityRole("User");
        await roleManager.CreateAsync(userRole);

        await roleManager.AddClaimAsync(userRole, new Claim("Permission", Permissions.ViewMovie));
        await roleManager.AddClaimAsync(userRole, new Claim("Permission", Permissions.ViewShowtime));
        
        await roleManager.AddClaimAsync(userRole, new Claim("Permission", Permissions.ViewReservation));
        await roleManager.AddClaimAsync(userRole, new Claim("Permission", Permissions.CreateReservation));
    }

    private static async Task SeedAdmins(UserManager<AppUser> userManager)
    {
        var user = await userManager.FindByEmailAsync("johannes.sutton2003@gmail.com");
        if (user == null) return;

        // Add to Admin role if not already
        if (!await userManager.IsInRoleAsync(user, "Admin"))
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
    
        // Add to User role if not already
        if (!await userManager.IsInRoleAsync(user, "User"))
        {
            await userManager.AddToRoleAsync(user, "User");
        }
    }
}
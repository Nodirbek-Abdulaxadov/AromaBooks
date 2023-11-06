using Microsoft.AspNetCore.Identity;

namespace AromaBooks;

public static class Config
{
    public static async Task SeedRolesToDatabase(this IApplicationBuilder applicationBuilder)
    {
        using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
        var rolemanager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (!await rolemanager.RoleExistsAsync("ADMIN"))
        {
            await rolemanager.CreateAsync(new IdentityRole("ADMIN"));
        }

        if (!await rolemanager.RoleExistsAsync("USER"))
        {
            await rolemanager.CreateAsync(new IdentityRole("USER"));
        }
    }
}
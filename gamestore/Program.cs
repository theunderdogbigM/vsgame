using gamestore.Endpoints;
using FluentValidation;
using FluentValidation.AspNetCore;
using gamestore.DTOs;
using gamestore.Data;
using Microsoft.EntityFrameworkCore;
using gamestore.Entities;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Register services
        builder.Services.AddRazorPages();

        var connString = builder.Configuration.GetConnectionString("gamestore");
        builder.Services.AddSqlite<GamestoreDBContext>(connString);

        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateGameDTOValidator>();

        // Build the app
        var app = builder.Build();

        // Configure middleware pipeline
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthorization();

        // Map Razor Pages
        app.MapRazorPages();

        // Map your existing endpoints
        app.MapGamesEndPoints();
        app.MapGenreEndpoints();

        // Apply any pending migrations and create the database if it doesn't exist
        await app.MigrateDbAsync();

        // Run the app
        app.Run();
    }
}

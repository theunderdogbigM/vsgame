using FluentValidation.AspNetCore;
using gamestore.Data;
using gamestore.DTOs;
using gamestore.Endpoints;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Register services for database and validation
        var connString = builder.Configuration.GetConnectionString("gamestore");
        builder.Services.AddSqlite<GamestoreDBContext>(connString);

        // Register FluentValidation services
        builder.Services.AddFluentValidation(fv => 
        {
            fv.RegisterValidatorsFromAssemblyContaining<CreateGameDTOValidator>();
        });

        // Build the app
        var app = builder.Build();

        // Middleware: Serve static files from wwwroot
        app.UseStaticFiles();

        // Middleware: Enable routing for the application
        app.UseRouting();

        // Middleware: Configure endpoints
        app.MapGamesEndPoints();

        // Serve welcome.html at the root URL
        app.MapGet("/", async context =>
        {
            await context.Response.SendFileAsync("wwwroot/welcome.html");
        });

        // Apply any pending migrations and create the database if it doesn't exist
        await app.MigrateDbAsync();

        // Run the WebApplication
        app.Run();
    }
}

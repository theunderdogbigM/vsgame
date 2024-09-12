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


 builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    });

        // Build the app
        var app = builder.Build();
       


        // Middleware: Serve static files from wwwroot
        DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
        defaultFilesOptions.DefaultFileNames.Clear();
        defaultFilesOptions.DefaultFileNames.Add("/welcome.html");
        app.UseDefaultFiles(defaultFilesOptions);
        app.UseStaticFiles();
        
    
        // Middleware: Enable routing for the application
        app.UseRouting();

        // Middleware: Configure endpoints
        app.MapGamesEndPoints();
        
        // Apply any pending migrations and create the database if it doesn't exist
        await app.MigrateDbAsync();

        // Run the WebApplication
        app.Run();
    }
}

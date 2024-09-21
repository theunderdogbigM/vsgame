using FluentValidation.AspNetCore;
using gamestore.CreateUserValidator;
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
            fv.RegisterValidatorsFromAssemblyContaining<CreateUserDtoValidator>();
        });

        // Configure JSON options
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
            });

        // Build the app
        var app = builder.Build();

        // Exception handling for development/production
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        // Middleware: Redirect HTTP to HTTPS and serve static files
        app.UseHttpsRedirection();

        DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
        defaultFilesOptions.DefaultFileNames.Clear();
        defaultFilesOptions.DefaultFileNames.Add("/welcome.html");
        app.UseDefaultFiles(defaultFilesOptions);
        app.UseStaticFiles();

        // Middleware: Enable routing for the application
        app.UseRouting();

        // Map endpoints
        app.MapUserEndpoints();
        app.MapGamesEndPoints();

        // Apply any pending migrations and create the database if it doesn't exist
        await app.MigrateDbAsync();

        // Run the WebApplication
        app.Run();
    }
}


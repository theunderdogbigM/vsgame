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

        var connString = builder.Configuration.GetConnectionString("gamestore");
        builder.Services.AddSqlite<GamestoreDBContext>(connString);


        builder.Services.AddDbContext<GamestoreDBContext>(options =>
        options.UseSqlite("Data Source=gamestore.db")
           .EnableSensitiveDataLogging());


        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateGameDTOValidator>();
        var app = builder.Build();

       
        app.MapGamesEndPoints();
        app.MapGenreEndpoints();

        await app.MigrateDbAsync();
        app.Run();
    }
}

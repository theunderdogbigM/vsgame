using gamestore.Endpoints;
using FluentValidation;
using FluentValidation.AspNetCore;
using gamestore.DTOs;
using gamestore.Data;
using Microsoft.EntityFrameworkCore;


internal class Program
{
    private static void Main(string[] args)
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

        app.MigrateDb();
        app.Run();
    }
}

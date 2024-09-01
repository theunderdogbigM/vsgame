using gamestore.Endpoints;
using FluentValidation;
using FluentValidation.AspNetCore;
using gamestore.DTOs;
using gamestore.Data;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connString = builder.Configuration.GetConnectionString("gamestore");
        builder.Services.AddSqlite<GamestoreDBContext>(connString);


        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateGameDTOValidator>(); // Replace 'Validations' with the correct class if necessary

        var app = builder.Build();

       
        app.MapGamesEndPoints();

        
        app.Run();
    }
}

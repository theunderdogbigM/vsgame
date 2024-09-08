namespace gamestore.Endpoints;

using gamestore.DTOs;
using FluentValidation;
using gamestore.Data;
using gamestore.Entities;
using gamestore.Mapping;
using Microsoft.EntityFrameworkCore;


public static class GameEndpoints 
{
    const string GameEndpointName = "GetGame";


    public static RouteGroupBuilder MapGamesEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("games");

        group.MapGet("/", (GamestoreDBContext dBContext) =>
        dBContext.Games.Include(game=>game.Genre)
        .Select(game => game.ToGameSummaryDto()).AsNoTracking()
        );



        group.MapGet("/{id}", (int id, GamestoreDBContext dBContext) => 
        {
            Game? game = dBContext.Games.Find(id);
            return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
        })
        .WithName(GameEndpointName);







        group.MapPost("/", (CreateGameDTO newGame, GamestoreDBContext dbContext, IValidator<CreateGameDTO> validator) =>
        {
            var validationResult = validator.Validate(newGame);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

           
              Game game = newGame.ToEntity();
              

            dbContext.Games.Add(game);
            dbContext.SaveChanges();
            return Results.CreatedAtRoute(GameEndpointName, new { id = game.Id }, game.ToGameDetailsDto());
        });





        group.MapPut("/{id}", (int id, UpdateGameDTO updateGame, IValidator<UpdateGameDTO> validator, GamestoreDBContext dbContext) =>
    {
   
    var validationResult = validator.Validate(updateGame);
    
    if (!validationResult.IsValid)
    {
       
        return Results.BadRequest(validationResult.Errors);
    }
    
    
    var existingGame = dbContext.Games.Find(id);

    if (existingGame == null)
    {
        return Results.NotFound();
    }

  dbContext.Entry(existingGame).
  CurrentValues.SetValues(updateGame.ToEntity(id));

  dbContext.SaveChanges();

    return Results.NoContent();
});






group.MapDelete("/{id}", async (int id, GamestoreDBContext dbContext) =>
{
    var game = await dbContext.Games.FindAsync(id);
    if (game == null)
    {
        return Results.NotFound();
    }

    dbContext.Games.Remove(game);
    await dbContext.SaveChangesAsync();

    return Results.NoContent();
});


return group;
}
}

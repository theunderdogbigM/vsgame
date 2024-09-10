using gamestore.DTOs;
using FluentValidation;
using gamestore.Data;
using gamestore.Entities;
using gamestore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace gamestore.Endpoints
{
    public static class GameEndpoints
    {
        const string GameEndpointName = "GetGame";

        public static void MapGamesEndPoints(this WebApplication app)
        {
            // Create a route group for "games"
            var group = app.MapGroup("/games");

            // GET /games - Retrieve a list of games
            group.MapGet("/", async (GamestoreDBContext dBContext) =>
                await dBContext.Games.Include(game => game.Genre)
                    .Select(game => game.ToGameSummaryDto())
                    .AsNoTracking()
                    .ToListAsync()
            );

            // GET /games/{id} - Retrieve a game by ID
            group.MapGet("/{id}", async (int id, GamestoreDBContext dBContext) =>
            {
                var game = await dBContext.Games.FindAsync(id);
                return game == null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
            })
            .WithName(GameEndpointName);

            // POST /games - Create a new game
            group.MapPost("/", async (CreateGameDTO newGame, GamestoreDBContext dbContext, IValidator<CreateGameDTO> validator) =>
            {
                var validationResult = validator.Validate(newGame);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                var game = newGame.ToEntity();
                dbContext.Games.Add(game);
                await dbContext.SaveChangesAsync();
                return Results.CreatedAtRoute(GameEndpointName, new { id = game.Id }, game.ToGameDetailsDto());
            });

            // PUT /games/{id} - Update an existing game
            group.MapPut("/{id}", async (int id, UpdateGameDTO updateGame, IValidator<UpdateGameDTO> validator, GamestoreDBContext dbContext) =>
            {
                var validationResult = validator.Validate(updateGame);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                var existingGame = await dbContext.Games.FindAsync(id);
                if (existingGame == null)
                {
                    return Results.NotFound();
                }

                dbContext.Entry(existingGame).CurrentValues.SetValues(updateGame.ToEntity(id));
                await dbContext.SaveChangesAsync();
                return Results.NoContent();
            });

            // DELETE /games/{id} - Delete a game by ID
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
        }
    }
}

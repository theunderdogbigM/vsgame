namespace gamestore.Endpoints;

using gamestore.DTOs;
using FluentValidation;

public static class GameEndpoints 
{
    const string GameEndpointName = "GetGame";

    private static readonly List<Gamedtos> gameList = new()
    {
        new Gamedtos(1, "Mortal Kombat", "Fighting", 12.66M, new DateOnly(1991, 6, 8)),
        new Gamedtos(2, "Final Fantasy I", "Roleplaying", 59.66M, new DateOnly(2001, 10, 1)),
        new Gamedtos(3, "Mario Tennis", "Sports", 19.21M, new DateOnly(2003, 12, 25))
    };

    public static RouteGroupBuilder MapGamesEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("games");

        group.MapGet("/", () => gameList);

        group.MapGet("/{id}", (int id) => 
        {
            var game = gameList.Find(game => game.ID == id);
            return game is null ? Results.NotFound() : Results.Ok(game);
        })
        .WithName(GameEndpointName);

        group.MapPost("/", (CreateGameDTO newGame, IValidator<CreateGameDTO> validator) =>
        {
            var validationResult = validator.Validate(newGame);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            var game = new Gamedtos(
                gameList.Count + 1, 
                newGame.Name, 
                newGame.Genre, 
                newGame.price, 
                newGame.ReleaseDate);

            gameList.Add(game);

            return Results.CreatedAtRoute(GameEndpointName, new { id = game.ID }, game);
        });

        group.MapPut("/{id}", (int id, UpdateGameDTO updateGame, IValidator<UpdateGameDTO> validator) =>
{
    // Validate the incoming DTO
    var validationResult = validator.Validate(updateGame);
    
    if (!validationResult.IsValid)
    {
        // Return bad request with validation errors
        return Results.BadRequest(validationResult.Errors);
    }
    
    // Proceed with your logic if validation passes
    var index = gameList.FindIndex(game => game.ID == id);

    if (index == -1)
    {
        return Results.NotFound();
    }

    gameList[index] = new Gamedtos(
        id,
        updateGame.Name,
        updateGame.Genre,
        updateGame.price,
        updateGame.ReleaseDate
    );

    return Results.NoContent();
});


        group.MapDelete("/{id}", (int id) =>
        {
            var removed = gameList.RemoveAll(game => game.ID == id);
            return removed > 0 ? Results.NoContent() : Results.NotFound();
        });

        return group;
    }
}

namespace gamestore.Endpoints;
using gamestore.DTOs;

public static class GameEndpoints 
{
    const string GameEndpointName = "GetGame";


private readonly static List<Gamedtos> gameList = [
    new(1,
    "Mortal Kombat",
    "Fighting",
    12.66M,
    new DateOnly(1991,6,8)),

    new(2,
    "Final Fantasy I",
    "Roleplaying",
    59.66M,
    new DateOnly(2001,10,1)),

    new(3,
    "Mario Tennis",
    "Sports",
    19.21M,
    new DateOnly(2003,12,25)),
];


public static WebApplication MapGamesEndPoints (this WebApplication app)

{
    app.MapGet("games", ()=> gameList);

app.MapGet("games/{id}", (int id) => 
{
    
    Gamedtos? game = gameList.Find(game=>game.ID == id);

    return game is null? Results.NotFound() : Results.Ok(game);

})
.WithName(GameEndpointName);

app.MapPost("games",(CreateGameDTO newGame) =>{

   
    Gamedtos game = new(
        gameList.Count + 1, 
        newGame.Name, 
        newGame.Genre,
         newGame.price, 
         newGame.ReleaseDtae);

         gameList.Add(game);

         return Results.CreatedAtRoute(GameEndpointName, new {id = game.ID}, game);
});

app.MapPut("games/{id}", (int id, UpdateGameDTO updateGame) => {
    var index = gameList.FindIndex(game => game.ID == id);

    if(index == -1)
    {
       return Results.NotFound(); 
    }
gameList[index] = new Gamedtos(
    id,
    updateGame.Name,
    updateGame.Genre,
    updateGame.price,
    updateGame.ReleaseDtae);

    return Results.NoContent();
});


app.MapDelete("games/{id}",(int id) =>

{
    gameList.RemoveAll(game => game.ID ==id);

    return Results.NoContent();
}
);

return app;

 }
}

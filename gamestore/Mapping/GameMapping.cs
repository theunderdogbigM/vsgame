using gamestore.DTOs;
using gamestore.Entities;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace gamestore.Mapping;

public static class GameMapping
{
    public static Game ToEntity(this UpdateGameDTO newGame, int id)
    {
         return new Game()
            {
                Id = id,
                Name = newGame.Name,
                GenreId = newGame.GenreId,
                price = newGame.price,
                ReleaseDate = newGame.ReleaseDate
            };
    }


public static Game ToEntity(this CreateGameDTO newGame)
    {
         return new Game()
            {
                Name = newGame.Name,
                GenreId = newGame.GenreId,
                price = newGame.price,
                ReleaseDate = newGame.ReleaseDate
            };
    }
    public static GameSummaryDto ToGameSummaryDto(this Game game)
    {
        return new(
                game.Id,
                game.Name,
                game.Genre!.Name,
                game.price,
                game.ReleaseDate
            );
    }

    public static GameDetailsDto ToGameDetailsDto(this Game game)
    {
        return new(
                game.Id,
                game.Name,
                game.GenreId,
                game.price,
                game.ReleaseDate
            );
    }

 
}

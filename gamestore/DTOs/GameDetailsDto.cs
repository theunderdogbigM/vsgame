
namespace gamestore.DTOs;

public record class GameDetailsDto(int ID, string Name, int GenreId, decimal price,
DateOnly ReleaseDate);

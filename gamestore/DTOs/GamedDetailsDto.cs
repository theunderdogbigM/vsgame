
namespace gamestore.DTOs;

public record class Gamedtos(int ID, string Name, int GenreId, decimal price,
DateOnly ReleaseDate);

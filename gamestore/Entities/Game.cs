using gamestore.Genre;

namespace gamestore.Entities;

public class Game
{

    public int Id {get; set;}

    public required string Name{get; set;}

    public int GenreId {get; set;}
    public GenreType? Genre {get; set;}

    public decimal price {get; set;}

    public DateOnly ReleaseDate{get;set;}
}

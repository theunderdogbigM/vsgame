using gamestore.DTOs;
using gamestore.Genre;

namespace gamestore.Mapping;

public static class GenreMapping
{
    public static GenreDTO ToDto(this GenreType genre){
        return new GenreDTO(genre.Id, genre.Name);
    }
}

namespace gamestore.DTOs;

public record class GameSummaryDto(int ID, string Name, string Genre, decimal price,
DateOnly ReleaseDate);

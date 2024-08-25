namespace gamestore.DTOs;

public record class CreateGameDTO(string Name, string Genre, decimal price,
DateOnly ReleaseDtae);
namespace gamestore.DTOs;

public record class Gamedtos(int ID, string Name, string Genre, decimal price,
DateOnly ReleaseDtae);

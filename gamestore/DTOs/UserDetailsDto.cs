namespace gamestore.DTOs;

public record class UserDetailsDto
(int ID, string Name, string Email, string Password, string? Activity);

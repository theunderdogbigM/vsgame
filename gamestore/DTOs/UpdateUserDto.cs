namespace gamestore.DTOs;
using System.ComponentModel.DataAnnotations;
public record class UpdateUserDto
([Required][StringLength(50)]string Name,
[Required][StringLength(50)]string Email,
[Required][StringLength(50)]string Password
);

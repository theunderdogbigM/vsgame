using System.ComponentModel.DataAnnotations;
namespace gamestore.DTOs;

public record class CreateGameDTO(
[Required][StringLength(50)]string Name,
 int GenreId, 
 [Range(1,100)]decimal price,
[Required]DateOnly ReleaseDate);


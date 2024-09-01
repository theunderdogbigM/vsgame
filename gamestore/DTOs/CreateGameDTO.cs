using System.ComponentModel.DataAnnotations;
namespace gamestore.DTOs;

public record class CreateGameDTO(
[Required][StringLength(50)]string Name,
 [Required][StringLength(20)]string Genre, 
 [Range(1,100)]decimal price,
[Required]DateOnly ReleaseDate);


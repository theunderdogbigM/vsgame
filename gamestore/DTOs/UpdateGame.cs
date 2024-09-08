namespace gamestore.DTOs;
using System.ComponentModel.DataAnnotations;
public record class UpdateGameDTO(
[Required][StringLength(50)]string Name,
 int GenreId, 
 [Range(1,100)]decimal price,
[Required]DateOnly ReleaseDate);
namespace gamestore.DTOs;
using System.ComponentModel.DataAnnotations;
public record class UpdateGameDTO(
[Required][StringLength(50)]string Name,
 [Required][StringLength(20)]string Genre, 
 [Range(1,100)]decimal price,
[Required]DateOnly ReleaseDate);
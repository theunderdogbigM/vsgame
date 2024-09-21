using System.ComponentModel.DataAnnotations;
namespace gamestore.DTOs;

  public record class CreateUserDto
    (
        [Required][StringLength(50)] string Name,  // Changed from UserName to Name
        [Required][EmailAddress][StringLength(50)] string Email,  // Added Email validation
        [Required][StringLength(50)] string Password
    );

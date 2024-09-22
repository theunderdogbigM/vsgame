using System.ComponentModel.DataAnnotations;

namespace gamestore.DTOs
{
    public record class SignInDto
    (
        [Required][EmailAddress] string Email,
        [Required] string Password
    );
}

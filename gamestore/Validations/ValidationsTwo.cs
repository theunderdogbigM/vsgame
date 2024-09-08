
using FluentValidation;

namespace gamestore.DTOs
{
    public class UpdateGameDTOValidator : AbstractValidator<UpdateGameDTO>
    {
        public UpdateGameDTOValidator()
        {
            RuleFor(item => item.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(item => item.GenreId).NotEmpty().WithMessage("GenreId is required.");
            RuleFor(item => item.price).GreaterThan(0).WithMessage("Price must be greater than 0.");
            RuleFor(item => item.ReleaseDate).NotEmpty().WithMessage("Release date is required.");
        }
    }
}

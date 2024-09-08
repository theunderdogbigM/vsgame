using FluentValidation;

namespace gamestore.DTOs;

public class CreateGameDTOValidator : AbstractValidator<CreateGameDTO>
{
    public CreateGameDTOValidator()
    {
        RuleFor(item => item.Name)
            .NotEmpty()
            .WithMessage("Name is required.");

        RuleFor(item => item.GenreId)
            .NotEmpty()
            .WithMessage("Genre is required.");

        RuleFor(item => item.price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0.");

        RuleFor(item => item.ReleaseDate)
            .NotEmpty()
            .WithMessage("Release Date is required.");
    }
}

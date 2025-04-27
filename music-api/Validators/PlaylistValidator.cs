using FluentValidation;
using music_api.Entities;

namespace music_api.Validators
{
    public class PlaylistValidator : AbstractValidator<Playlist>
    {
        public PlaylistValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Назва не може бути порожньою!")
                .Length(3, 255).WithMessage("Назва повинна містити від 3 до 255 символів!");
            
            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Опис не повинен перевищувати 1000 символів!");
            
            RuleFor(x => x.UserId)
                .GreaterThan(0).When(x => x.UserId.HasValue).WithMessage("UserId повинен бути додатнім числом!");
        }
    }
}

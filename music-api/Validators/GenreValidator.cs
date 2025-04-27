using FluentValidation;
using music_api.Entities;

namespace music_api.Validators
{
    public class GenreValidator : AbstractValidator<Genre>
    {
        public GenreValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Назва жанру не може бути порожньою!")
                .Length(2, 100).WithMessage("Назва жанру повинна містити від 2 до 100 символів!");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Опис не повинен перевищувати 1000 символів!");
        }
    }
}

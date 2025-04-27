using FluentValidation;
using music_api.Entities;

namespace music_api.Validators
{
    public class SongValidator : AbstractValidator<Song>
    {
        public SongValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Назва не може бути порожньою!")
                .Length(1, 255).WithMessage("Назва повинна містити від 1 до 255 символів!");
            
            RuleFor(x => x.PerformerId)
                .GreaterThan(0).WithMessage("Потрібно обрати виконавця!");
            
            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage("Тривалість повинна бути більшою за 0!");
            
            RuleFor(x => x.Album)
                .MaximumLength(255).WithMessage("Назва альбому не повинна перевищувати 255 символів!");
            
            RuleFor(x => x.Year)
                .GreaterThan(0).When(x => x.Year.HasValue).WithMessage("Рік повинен бути додатнім числом!");
        }
    }
}

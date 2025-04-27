using FluentValidation;
using music_api.Entities;

namespace music_api.Validators
{
    public class PerformerValidator : AbstractValidator<Performer>
    {
        public PerformerValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ім'я виконавця не може бути порожнім!")
                .Length(2, 255).WithMessage("Ім'я виконавця повинно містити від 2 до 255 символів!");
            
            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Опис не повинен перевищувати 1000 символів!");
            
            RuleFor(x => x.Country)
                .MaximumLength(255).WithMessage("Країна не повинна перевищувати 255 символів!");
        }
    }
}

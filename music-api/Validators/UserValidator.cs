using FluentValidation;
using music_api.Entities;

namespace music_api.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email не может быть пустым!")
                .EmailAddress().WithMessage("Некоректний email!");
            
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Ім'я користувача не може бути порожнім!")
                .Length(3, 50).WithMessage("Ім'я користувача повинно містити від 3 до 50 символів!");
        }
    }
}

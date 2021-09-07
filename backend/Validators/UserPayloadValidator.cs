using FluentValidation;

namespace Todo
{
    public class UserPayloadValidator : AbstractValidator<UserPayloadModel>
    {
        public UserPayloadValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                    .WithMessage("Campo obrigatório")
                .EmailAddress()
                    .WithMessage("Email inválido");

            RuleFor(x => x.Password)
                .NotEmpty()
                    .WithMessage("Campo obrigatório")
                .MinimumLength(6)
                    .WithMessage("Senha precisa ter 6 caracteres no mínimo");
        }
    }
}

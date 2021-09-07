using FluentValidation;

namespace Todo
{
    public class TodoPayloadValidator : AbstractValidator<TodoPayloadModel>
    {
        public TodoPayloadValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("Campo obrigatório");
        }
    }
}

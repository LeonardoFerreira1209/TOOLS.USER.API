using APPLICATION.DOMAIN.DTOS.REQUEST.PEOPLE;
using APPLICATION.DOMAIN.UTILS.Extensions;
using APPLICATION.ENUMS;
using DocumentValidator;
using FluentValidation;

namespace APPLICATION.DOMAIN.VALIDATORS;

public class PersonValidator : AbstractValidator<PersonFastRequest>
{
    public PersonValidator()
    {
        RuleFor(person => person.FirstName).NotEmpty().NotNull().WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo FirstName.");

        RuleFor(person => person.LastName).NotEmpty().NotNull().WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo LastName.");

        RuleFor(person => person.CPF).NotEmpty().NotNull().When(person => CpfValidation.Validate(person.CPF)).WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo CPF ou coloqyue um CPF válido.");
    }
}

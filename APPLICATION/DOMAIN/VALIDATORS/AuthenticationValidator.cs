using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.UTILS.EXTENSIONS;
using APPLICATION.ENUMS;
using FluentValidation;

namespace APPLICATION.DOMAIN.VALIDATORS;

/// <summary>
/// Validator de login, verifica se os campos foram informados corretamente.
/// </summary>
public class AuthenticationValidator : AbstractValidator<LoginRequest>
{
    public AuthenticationValidator()
    {
        RuleFor(a => a.Username).NotEmpty().NotNull().WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo username.");

        RuleFor(a => a.Password).NotEmpty().NotNull().WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo password.");
    }
}

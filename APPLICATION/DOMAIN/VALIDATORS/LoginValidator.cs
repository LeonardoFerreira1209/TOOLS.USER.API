using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.UTILS;
using APPLICATION.ENUMS;
using FluentValidation;

namespace APPLICATION.DOMAIN.VALIDATORS;

/// <summary>
/// Validator de login, verifica se os campos foram informados corretamente.
/// </summary>
public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(c => c.Username).NotEmpty().NotNull().WithErrorCode(ErrorCode.CamposObrigatorios.ToCode());

        RuleFor(c => c.Password).NotEmpty().NotNull().WithErrorCode(ErrorCode.CamposObrigatorios.ToCode());
    }
}

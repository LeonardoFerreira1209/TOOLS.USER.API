using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.UTILS.EXTENSIONS;
using APPLICATION.ENUMS;
using FluentValidation;

namespace APPLICATION.DOMAIN.VALIDATORS
{
    public class CreateUserValidator : AbstractValidator<UserRequest>
    {
        public CreateUserValidator()
        {
            RuleFor(u => u.UserName).NotEmpty().NotNull().WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo username.");

            RuleFor(u => u.Password).NotEmpty().NotNull().WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo password.");

            RuleFor(u => u.Email).NotEmpty().NotNull().EmailAddress().WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo e-mail, ou válide se está no formato correto (example@example.com).");

        }
    }
}

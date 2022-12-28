using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.UTILS.EXTENSIONS;
using APPLICATION.ENUMS;
using DocumentValidator;
using FluentValidation;

namespace APPLICATION.DOMAIN.VALIDATORS
{
    public class CreateUserValidator : AbstractValidator<UserCreateRequest>
    {
        /// <summary>
        /// Classe responsavel por validar usuário.
        /// </summary>
        public CreateUserValidator()
        {
            RuleFor(user => user.FirstName).NotEmpty().NotNull().WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo FirstName.");

            RuleFor(user => user.LastName).NotEmpty().NotNull().WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo LastName.");

            RuleFor(user => user.PlanId).NotEmpty().NotNull().WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o plano de cadastro.");

            RuleFor(user => user.CPF).NotEmpty().NotNull().WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo CPF.").Must(CPFValidate).WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo CPF com um valor válido.");

            RuleFor(user => user.Gender).NotNull().WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo Gender.").Must(GenderValidate).WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo Gender com um valor válido (1 ou 2).");

            RuleFor(user => user.UserName).NotEmpty().NotNull().WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo Username.");

            RuleFor(user => user.Password).NotEmpty().NotNull().WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo Password.");

            RuleFor(user => user.Email).NotEmpty().NotNull().WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo E-mail.").EmailAddress().WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o e-mail com um valor válido (email@example.com).");
        }

        /// <summary>
        /// Método responsavel por validar um CPF.
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        private static bool CPFValidate(string CPF) => CpfValidation.Validate(CPF);

        /// <summary>
        /// Método responsavel por validar um sexo.
        /// </summary>
        /// <param name="gender"></param>
        /// <returns></returns>
        private static bool GenderValidate(Gender gender) => gender == Gender.Male || gender == Gender.Female;
    }
}

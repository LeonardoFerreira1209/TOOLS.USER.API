using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;
using APPLICATION.DOMAIN.UTILS.EXTENSIONS;
using APPLICATION.ENUMS;
using DocumentValidator;
using FluentValidation;

namespace APPLICATION.DOMAIN.VALIDATORS;

public class CreatePersonValidator : AbstractValidator<PersonFastRequest>
{
    /// <summary>
    /// Classe responsavel por validar pessoas.
    /// </summary>
    public CreatePersonValidator()
    {
        #region Person
        RuleFor(person => person.FirstName).NotEmpty().NotNull().WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo FirstName.");

        RuleFor(person => person.LastName).NotEmpty().NotNull().WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo LastName.");

        RuleFor(person => person.CPF).NotEmpty().NotNull().WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo CPF.").Must(CPFValidate).WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo CPF com um valor válido.");

        RuleFor(person => person.Gender).NotNull().WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo Gender.").Must(GenderValidate).WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo Gender com um valor válido (1 ou 2)."); ;
        #endregion

        #region User
        RuleFor(person => person.User.UserName).NotEmpty().NotNull().WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo Username.");

        RuleFor(person => person.User.Password).NotEmpty().NotNull().WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo Password.");

        RuleFor(person => person.User.Email).NotEmpty().NotNull().WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo E-mail.").EmailAddress().WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o e-mail com um valor válido (email@example.com).");
        #endregion
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

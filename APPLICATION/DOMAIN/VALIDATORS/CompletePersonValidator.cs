using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;
using APPLICATION.DOMAIN.UTILS.EXTENSIONS;
using APPLICATION.ENUMS;
using DocumentValidator;
using FluentValidation;

namespace APPLICATION.DOMAIN.VALIDATORS;

public class CompletePersonValidator : AbstractValidator<PersonFullRequest>
{
    /// <summary>
    /// Classe responsavel por validar pessoas.
    /// </summary>
    public CompletePersonValidator()
    {
        #region Person
        RuleFor(person => person.FirstName).NotEmpty().WithMessage("Nome não pode ser vazio").NotNull().WithMessage("Nome não pode ser nulo").WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo Nome.");

        RuleFor(person => person.LastName).NotEmpty().WithMessage("Sobrenome não pode ser vazio").NotNull().WithMessage("Sobrenome não pode ser nulo").WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo Sobrenome.");

        RuleFor(person => person.CPF).NotEmpty().WithMessage("CPF não pode ser vazio").NotNull().WithMessage("CPF não pode ser nulo").WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo CPF.").Must(CPFValidate).WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo CPF com um valor válido.");

        RuleFor(person => person.RG).NotEmpty().WithMessage("RG não pode ser vazio").NotNull().WithMessage("RG não pode ser nulo").WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo RG.").Must(RGValidate).WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo RG com um valor válido.");

        RuleFor(person => person.Gender).NotNull().WithMessage("Sexo não pode ser nulo").WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo Gender.").Must(GenderValidate).WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo Sexo com um valor válido (Masculino ou Feminino).");

        RuleFor(person => person.Age).Must(AgeValidate).WithMessage("A Idade minima é 18").NotNull().WithMessage("Idade não pode ser nula").WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo Idade.");

        RuleFor(person => person.BirthDay).NotEmpty().WithMessage("Aniversário não pode ser vazio").NotNull().WithMessage("Aniversário não pode ser nulo").WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo Aniversário.");

        RuleFor(person => person.Status).NotNull().WithMessage("Status não pode ser nulo").WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("Preencha o campo Status.");

        RuleFor(person => person.UserId).NotEmpty().WithMessage("Id do usuário não pode ser vazio").NotNull().WithMessage("Id do usuário não pode ser nulo").WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("O id do usuário não pode ser vazio ou nulo.");
        #endregion
    }

    /// <summary>
    /// Método responsavel por validar um CPF.
    /// </summary>
    /// <param name="CPF"></param>
    /// <returns></returns>
    private static bool CPFValidate(string CPF) => CpfValidation.Validate(CPF);

    /// <summary>
    /// Método responsavel por validar um RG.
    /// </summary>
    /// <param name="RG"></param>
    /// <returns></returns>
    private static bool RGValidate(string RG) => RGValidation.Validate(RG);

    /// <summary>
    /// Método responsavel por validar um sexo.
    /// </summary>
    /// <param name="gender"></param>
    /// <returns></returns>
    private static bool GenderValidate(Gender gender) => gender == Gender.Male || gender == Gender.Female;

    private static bool AgeValidate(int age) => age >= 18;
}

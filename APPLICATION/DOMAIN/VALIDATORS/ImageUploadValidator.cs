using APPLICATION.DOMAIN.UTILS.EXTENSIONS;
using APPLICATION.ENUMS;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace APPLICATION.DOMAIN.VALIDATORS;

/// <summary>
/// Validator de Upload de imagem.
/// </summary>
public class ImageUploadValidator : AbstractValidator<IFormFile>
{
    /// <summary>
    /// Validando os dados da imagem.
    /// </summary>
    public ImageUploadValidator()
    {
        RuleFor(a => a.ContentType.FileTypesAllowed()).NotEqual(false).WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("O tipo do arquivo é inválido.");

        RuleFor(a => a.Length).Must(ValidateLenght).WithErrorCode(ErrorCode.CamposObrigatorios.ToCode()).WithMessage("O tamanho do arquivo deve ser menor que 1MB");
    }

    private static bool ValidateLenght(long lenght) => (lenght / 100000) <= 1024;
}

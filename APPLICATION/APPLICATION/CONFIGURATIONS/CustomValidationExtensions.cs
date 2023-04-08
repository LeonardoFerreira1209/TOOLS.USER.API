using APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;
using APPLICATION.ENUMS;
using FluentValidation.Results;
using System.Diagnostics.CodeAnalysis;

namespace APPLICATION.APPLICATION.CONFIGURATIONS;

/// <summary>
/// Extensão para o Validation Customizados.
/// </summary>
[ExcludeFromCodeCoverage]
public static class CustomValidationExtensions
{
    /// <summary>
    /// Tratamentos de erros.
    /// </summary>
    /// <param name="validationResult"></param>
    /// <returns></returns>
    public static ApiResponse<object> CarregarErrosValidator(this ValidationResult validationResult, object dados = null)
    {
        var _notificacoes = new List<DadosNotificacao>();

        foreach (var erro in validationResult.Errors) _notificacoes.Add(new DadosNotificacao(erro.ErrorMessage));

        return new ApiResponse<object>(false, StatusCodes.ErrorBadRequest, dados, _notificacoes);
    }
}

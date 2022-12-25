using APPLICATION.DOMAIN.ENUM;

namespace APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;

/// <summary>
/// Dados a ser retornado em uma notificação do sistema.
/// </summary>
public class DadosNotificacao
{
    public DadosNotificacao(string mensagem) { Mensagem = mensagem; }

    /// <summary>
    /// Mensagem da notificação.
    /// </summary>
    public string Mensagem { get; set; }
}

/// <summary>
/// Retorno das APIS.
/// </summary>
/// <typeparam name="T"></typeparam>
public class ApiResponse<T> where T : class
{
    public ApiResponse(bool sucesso, StatusCodes statusCode, List<DadosNotificacao> notificacaos = null)
    {
        Sucesso = sucesso; StatusCode = statusCode; Notificacoes = notificacaos;
    }

    public ApiResponse(bool sucesso, StatusCodes statusCode, T dados = null, List<DadosNotificacao> notificacoes = null)
    {
        Sucesso = sucesso; StatusCode = statusCode; Dados = dados; Notificacoes = notificacoes;
    }

    /// <summary>
    /// Status cde.
    /// </summary>
    public StatusCodes StatusCode { get; set; }

    /// <summary>
    /// Retorna true se a requisição para API foi bem sucedida.
    /// </summary>
    public bool Sucesso { get; set; }

    /// <summary>
    /// Dados a serem retornados na requisição.
    /// </summary>
    public T Dados { get; set; }

    /// <summary>
    /// Notificações que retornam da requisição, sejam elas Sucesso, Erro, Informação.
    /// </summary>
    public List<DadosNotificacao> Notificacoes { get; set; }
}

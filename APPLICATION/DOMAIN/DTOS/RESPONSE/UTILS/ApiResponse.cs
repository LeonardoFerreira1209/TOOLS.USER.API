using APPLICATION.DOMAIN.ENUM;

namespace APPLICATION.DOMAIN.DTOS.RESPONSE.UTILS;

/// <summary>
/// Dados a ser retornado em uma notificação do sistema.
/// </summary>
public class DadosNotificacao
{
    public DadosNotificacao(StatusCodes statusCodes, string mensagem)
    {
        StatusCode = statusCodes; Mensagem = mensagem;
    }

    /// <summary>
    /// Código de status da requisição.
    /// </summary>
    public StatusCodes StatusCode { get; set; }

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
    public ApiResponse() { }

    public ApiResponse(bool sucesso, List<DadosNotificacao> notificacaos = null)
    {
        Sucesso = sucesso; Notificacoes = notificacaos;
    }

    public ApiResponse(bool sucesso, T dados = null, List<DadosNotificacao> notificacoes = null)
    {
        Sucesso = sucesso; Dados = dados; Notificacoes = notificacoes;
    }

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

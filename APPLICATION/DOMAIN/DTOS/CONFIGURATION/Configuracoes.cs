namespace APPLICATION.DOMAIN.DTOS.CONFIGURATION;

/// <summary>
/// Classe responsavel por receber os dados do Appsettings.
/// </summary>
public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; }
    public SwaggerInfo SwaggerInfo { get; set; }
    public Configuracoes Configuracoes { get; set; }
    public RetryPolicy RetryPolicy { get; set; }
    public Auth Auth { get; set; }
    public AzureStorage AzureStorage { get; set; }
    public UrlBase UrlBase { get; set; }
}

/// <summary>
/// Classe responsável por receber dados de retry policy.
/// </summary>
public class RetryPolicy
{
    public string RetryOn { get; set; }
    public int RetryCount { get; set; }
    public int RetryEachSecond { get; set; }
}

/// <summary>
/// Classe de conexões.
/// </summary>
public class ConnectionStrings
{
    public string BaseDados { get; set; }
    public string ServiceBus { get; set; }
}

/// <summary>
/// Classe de config do swagger.
/// </summary>
public class SwaggerInfo
{
    public string ApiDescription { get; set; }
    public string ApiVersion { get; set; }
    public string UriMyGit { get; set; }
}

/// <summary>
/// Classe de config diversas.
/// </summary>
public class Configuracoes
{
    public int TimeOutDefault { get; set; }
    public int NumeroThreadsConsumer { get; set; }
    public string TopicoExemploName { get; set; }
    public string SubscriptionExemploName { get; set; }
    public int TempoReagendamentoMinutos { get; set; }
    public int QuantidadeMaximaDeRetentativas { get; set; }
}

/// <summary>
/// Classe de config de autenticação.
/// </summary>
public class Auth
{
    public string ValidIssuer { get; set; }
    public string ValidAudience { get; set; }
    public string SecurityKey { get; set; }
    public int ExpiresIn { get; set; }
    public Password Password { get; set; }
}

/// <summary>
/// Classe de config do blob storage.
/// </summary>
public class AzureStorage
{
    public string ConnectionStringAzureStorageKey { get; set; }
    public string Container { get; set; }
}

/// <summary>
/// Classe de config de senha.
/// </summary>
public class Password
{
    public int RequiredLength { get; set; }
}

/// <summary>
/// Classe de url externas.
/// </summary>
public class UrlBase
{
    public string TOOLS_MAIL_API { get; set; }
    public string BASE_URL { get; set; }
    public string TOOLS_WEB_APP { get; set; }
}

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
    public UrlBase UrlBase { get; set; }
}

public class RetryPolicy
{
    public string RetryOn { get; set; }
    public int RetryCount { get; set; }
    public int RetryEachSecond { get; set; }
}

public class ConnectionStrings
{
    public string BaseDados { get; set; }
}

public class SwaggerInfo
{
    public string ApiDescription { get; set; }
    public string ApiVersion { get; set; }
    public string UriMyGit { get; set; }
}

public class Configuracoes
{
    public int TimeOutDefault { get; set; }
    public int NumeroThreadsConsumer { get; set; }
    public string TopicoExemploName { get; set; }
    public string SubscriptionExemploName { get; set; }
    public int TempoReagendamentoMinutos { get; set; }
    public int QuantidadeMaximaDeRetentativas { get; set; }
}

public class Auth
{
    public string ValidIssuer { get; set; }
    public string ValidAudience { get; set; }
    public string SecurityKey { get; set; }
    public int ExpiresIn { get; set; }
    public Password Password { get; set; }
}

public class Password
{
    public int RequiredLength { get; set; }
}

public class UrlBase
{
    public string TOOLS_MAIL_API { get; set; }
    public string BASE_URL { get; set; }
}

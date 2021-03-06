using APPLICATION.APPLICATION.CONFIGURATIONS;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using Microsoft.AspNetCore.Mvc;
using Serilog;

try
{
    // Preparando builder.
    var builder = WebApplication.CreateBuilder(args);

    // Pegando configurações do appsettings.json.
    var configurations = builder.Configuration;

    /// <summary>
    /// Chamada das configurações do projeto.
    /// </summary>
    builder.Services
        .AddHttpContextAccessor()
        .Configure<AppSettings>(configurations).AddSingleton<AppSettings>()
        .AddEndpointsApiExplorer()
        .AddOptions()
        .ConfigureLanguage()
        .ConfigureContexto(configurations)
        .ConfigureIdentityServer(configurations)
        .ConfigureAuthorization(configurations)
        .ConfigureAuthentication(configurations)
        .ConfigureApllicationCookie()
        .ConfigureSwagger(configurations)
        .ConfigureDependencies(configurations)
        .ConfigureRefit(configurations)
        .ConfigureTelemetry(configurations)
        .ConfigureApplicationInsights()
        .ConfigureSerilog()
        .ConfigureHealthChecks(configurations)
        .ConfigureCors()
        .AddControllers(options =>
        {
            options.EnableEndpointRouting = false;

            options.Filters.Add(new ProducesAttribute("application/json"));

        });

    // Preparando WebApplication Build.
    var applicationbuilder = builder.Build();

    // Chamada das connfigurações do WebApplication Build.
    applicationbuilder
        // -- Seeds
        //.Seeds().Result
        // -- Seeds
        .UseHttpsRedirection()
        .UseDefaultFiles()
        .UseStaticFiles()
        .UseRouting()
        .UseAuthorization()
        .UseAuthentication()
        .UseCors("CorsPolicy")
        .UseHealthChecks()
        .UseSwaggerConfigurations(configurations);

    // Chamando as configurações de Minimal APIS.
    applicationbuilder.UseMinimalAPI(configurations);

    Log.Information($"[LOG INFORMATION] - Inicializando aplicação [TOOLS.USER.API]\n");

    // Iniciando a aplicação com todas as configurações já carregadas.
    applicationbuilder.Run();
}
catch (Exception exception)
{
    Log.Error($"[LOG ERROR] - Ocorreu um erro ao inicializar a aplicacao [TOOLS.USER.API] - {exception.Message}\n");
}
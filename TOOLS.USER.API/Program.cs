using APPLICATION.APPLICATION.CONFIGURATIONS;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

try
{
    // Preparando builder.
    var builder = WebApplication.CreateBuilder(args);

    // Pegando configurações do appsettings.json.
    var configurations = builder.Configuration;

    // Pega o appsettings baseado no ambiente em execução.
    configurations
         .SetBasePath(builder.Environment.ContentRootPath).AddJsonFile("appsettings.json", true, true).AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true).AddEnvironmentVariables();

    builder.Services.AddSignalR();

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
        .ConfigureDependencies(configurations, builder.Environment)
        .ConfigureRefit(configurations);

    // Se for em produção executa.
    if (builder.Environment.IsProduction())
    {
        builder.Services
            .ConfigureTelemetry(configurations)
            .ConfigureApplicationInsights(configurations);
    }

    // Continuação do pipeline...
    builder.Services
        //.ConfigureSerilog()
        .ConfigureSubscribers()
        .ConfigureHealthChecks(configurations)
        .ConfigureCors()
        .ConfigureFluentSchedulerJobs()
        .ConfigureHangFire(configurations)
        .AddControllers(options =>
        {
            options.EnableEndpointRouting = false;

            options.Filters.Add(new ProducesAttribute("application/json"));

        }).AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

    // Preparando WebApplication Build.
    var applicationbuilder = builder.Build();

    // Chamada das connfigurações do WebApplication Build.
    applicationbuilder
        // -- Seeds
        .Seeds().Result
        // -- Seeds
        .UseHttpsRedirection()
        .UseDefaultFiles()
        .UseStaticFiles()
        .UseCookiePolicy()
        .UseRouting()
        .UseCors("CorsPolicy")
        .UseResponseCaching()
        .UseAuthorization()
        .UseAuthentication()
        .UseHealthChecks()
        .UseSwaggerConfigurations(configurations)
        .UseEndpoints()
        .UseHangfireDashboard();

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
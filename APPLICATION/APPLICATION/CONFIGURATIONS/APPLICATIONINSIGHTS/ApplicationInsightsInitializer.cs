using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace APPLICATION.APPLICATION.CONFIGURATIONS.APPLICATIONINSIGHTS;

[ExcludeFromCodeCoverage]
public sealed class ApplicationInsightsInitializer : ITelemetryInitializer
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly string _instrumentationKey;

    private readonly string _roleName;

    public ApplicationInsightsInitializer(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

        _instrumentationKey = configuration.GetSection("ApplicationInsights:InstrumentationKey").Value;

        _roleName = configuration.GetSection("ApplicationInsights:CloudRoleName").Value;
    }

    /// <summary>
    /// Inicializa as telemetry do appinsights.
    /// </summary>
    /// <param name="telemetry"></param>
    public void Initialize(ITelemetry telemetry)
    {
        telemetry.Context.InstrumentationKey = _instrumentationKey;

        telemetry.Context.Cloud.RoleName = _roleName;

        if (_httpContextAccessor.HttpContext != null)
        {
            foreach (var prop in _httpContextAccessor.HttpContext.Items)
                if (prop.Key is string)
                    telemetry.Context.GlobalProperties[prop.Key.ToString()] = prop.Value.ToString();
        }
    }
}


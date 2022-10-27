using APPLICATION.DOMAIN.CONTRACTS.CONFIGURATIONS.APPLICATIONINSIGHTS;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System.Diagnostics.CodeAnalysis;

namespace APPLICATION.APPLICATION.CONFIGURATIONS.APPLICATIONINSIGHTS;

[ExcludeFromCodeCoverage]
public sealed class ApplicationInsightsMetrics : IApplicationInsightsMetrics
{
    private readonly TelemetryClient _telemetry;

    public ApplicationInsightsMetrics(TelemetryClient telemetry, string key)
    {
        this._telemetry = telemetry;

        this._telemetry.Context.InstrumentationKey = key;
    }

    /// <summary>
    /// Adiciona as métricas.
    /// </summary>
    /// <param name="metrica"></param>
    public void AddMetric(CustomMetricDto metrica)
    {

        if (metrica != null)
        {
            _telemetry.TrackMetric(new MetricTelemetry
            {
                Name = metrica.NomeMetrica,
                Sum = metrica.ValorMetrica
            });

        }

    }

}



using APPLICATION.DOMAIN.CONTRACTS.CONFIGURATIONS.APPLICATIONINSIGHTS;
using Microsoft.ApplicationInsights;
using System.Diagnostics.CodeAnalysis;

namespace APPLICATION.APPLICATION.CONFIGURATIONS.APPLICATIONINSIGHTS;

[ExcludeFromCodeCoverage]
public sealed class TelemetryProxy : ITelemetryProxy
{
    private readonly TelemetryClient _telemetryClient;

    public TelemetryProxy(TelemetryClient telemetryClient) => _telemetryClient = telemetryClient;

    public void TrackEvent(string eventName) => _telemetryClient.TrackEvent(eventName);
}

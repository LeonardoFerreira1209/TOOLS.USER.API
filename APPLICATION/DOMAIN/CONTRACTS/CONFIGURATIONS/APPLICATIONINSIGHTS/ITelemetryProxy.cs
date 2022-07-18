
namespace APPLICATION.DOMAIN.CONTRACTS.CONFIGURATIONS.APPLICATIONINSIGHTS;

public interface ITelemetryProxy
{
    void TrackEvent(string eventName);
}


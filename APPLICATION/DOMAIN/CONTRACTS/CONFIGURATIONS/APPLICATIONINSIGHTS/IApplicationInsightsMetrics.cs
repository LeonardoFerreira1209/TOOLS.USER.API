using APPLICATION.DOMAIN.DTOS.CONFIGURATION;

namespace APPLICATION.DOMAIN.CONTRACTS.CONFIGURATIONS.APPLICATIONINSIGHTS;

public interface IApplicationInsightsMetrics
{
    void AddMetric(CustomMetricDto metrica);
}

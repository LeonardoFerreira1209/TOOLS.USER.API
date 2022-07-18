using APPLICATION.ENUMS;

namespace APPLICATION.DOMAIN.CONTRACTS.CONFIGURATIONS;

public interface ILogWithMetric
{
    void SetTitle(string title);

    void Start();

    void Finish();

    void Error(Exception ex);

    void LogStart();

    void LogFinish(string additionalInfo = "");

    void LogError(Exception e, string additionalInfo = "");

    void LogEmRetentativa(string additionalInfo = "");

    void LogInfo(string message);

    void LogWarning(string message);

    void LogWithEvent(string text, Metric metricEnum, Exception exception = null);
}

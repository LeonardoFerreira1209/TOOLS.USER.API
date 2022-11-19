using APPLICATION.DOMAIN.CONTRACTS.CONFIGURATIONS;
using APPLICATION.DOMAIN.CONTRACTS.CONFIGURATIONS.APPLICATIONINSIGHTS;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.ENUMS;
using Serilog;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace APPLICATION.APPLICATION.CONFIGURATIONS;


[ExcludeFromCodeCoverage]
public sealed class LogWithMetric : ILogWithMetric
{

    private Stopwatch _stopwatch;

    private string _eventName;

    private string _eventFullName;

    private List<CustomMetricDto> _metrics;

    private readonly ITelemetryProxy _telemetryProxy;

    private readonly IApplicationInsightsMetrics _appInsightsMetrics;

    public LogWithMetric(IApplicationInsightsMetrics appInsightsMetrics, ITelemetryProxy telemetryProxy)
    {

        _appInsightsMetrics = appInsightsMetrics;

        _telemetryProxy = telemetryProxy;

    }

    public LogWithMetric(string eventName, IApplicationInsightsMetrics appInsightsMetrics)
    {

        _appInsightsMetrics = appInsightsMetrics;

        SetTitle(eventName);
        ConfigMetric(eventName);

    }

    public void LogWithEvent(string text, Metric metricEnum, Exception exception = null)
    {

        if (_telemetryProxy is null)
            return;

        switch (metricEnum)
        {
            case Metric.Inicio:
            case Metric.Sucesso:
                Log.Information($"{metricEnum} - {_eventFullName} - {text}");
                _telemetryProxy.TrackEvent($"{metricEnum} - {_eventFullName}");
                break;
            case Metric.Erro:
                if (exception != null)
                    Log.Error(exception, $"{metricEnum} - {_eventFullName} - {text}");
                else
                    Log.Error($"{metricEnum} - {_eventFullName} - {text}");
                _telemetryProxy.TrackEvent($"{metricEnum} - {_eventFullName}");
                break;
            default:
                Log.Information(text);
                _telemetryProxy.TrackEvent(_eventFullName);
                break;
        }

        _appInsightsMetrics.AddMetric(_metrics.FirstOrDefault(x => x.TipoMetrica == metricEnum));
    }

    public void LogWithCustomMetric(string text, Metric metricEnum, string metricName = null, Exception exception = null)
    {
        metricName = !string.IsNullOrEmpty(metricName) ? RemoveAccents(metricName) : _eventName;

        switch (metricEnum)
        {
            case Metric.Inicio:
            case Metric.Sucesso:
            case Metric.Geral:
            case Metric.EmRetentativa:
                Log.Information($"{metricEnum} - {_eventFullName} - {text}");
                break;
            case Metric.Erro:
                if (exception != null)
                    Log.Error(exception, $"{metricEnum} - {_eventFullName} - {text}");
                else
                    Log.Error($"{metricEnum} - {_eventFullName} - {text}");
                break;
            default:
                Log.Information(text);
                break;
        }

        _appInsightsMetrics.AddMetric(AddCustomMetric(metricName, metricEnum));
    }

    private static CustomMetricDto AddCustomMetric(string metricName, Metric metricEnum)
    {

        CustomMetricDto customMetric = metricEnum switch
        {
            Metric.Inicio => new CustomMetricDto(Metric.Inicio, $"QtdInicio{metricName}"),

            Metric.Sucesso => new CustomMetricDto(Metric.Sucesso, $"QtdSucesso{metricName}"),

            Metric.Erro => new CustomMetricDto(Metric.Erro, $"QtdErro{metricName}"),

            Metric.Geral => new CustomMetricDto(Metric.Geral, $"QtdErro{metricName}"),

            Metric.EmRetentativa => new CustomMetricDto(Metric.EmRetentativa, $"QtdErro{metricName}"),

            _ => new CustomMetricDto(Metric.Geral, $"{metricName}"),
        };

        return customMetric;
    }

    public void SetTitle(string title)
    {
        _eventFullName = title;

        _eventName = RemoveAccents(title);

        ConfigMetric(title);
    }

    public void Start()
    {
        _stopwatch = new Stopwatch();

        _stopwatch.Start();

        LogWithEvent($"DataHora: {DateTime.Now}", Metric.Inicio);
    }

    public void Finish()
    {
        _stopwatch.Stop();

        LogWithEvent($"Tempo processamento: {_stopwatch.Elapsed}", Metric.Sucesso);
    }

    public void Error(Exception ex) => LogWithEvent($"Erro: {ex.Message}", Metric.Erro, exception: ex);

    public void LogStart() => LogWithCustomMetric("Iniciando processo", Metric.Inicio);

    public void LogFinish(string additionalInfo = "") => LogWithCustomMetric($"Finalizando processo {additionalInfo}", Metric.Sucesso);

    public void LogEmRetentativa(string additionalInfo = "") => LogWithCustomMetric($"Finalizando processo com erro, haverá retentativa. {additionalInfo}", Metric.EmRetentativa);

    public void LogError(Exception e, string additionalInfo = "") => LogWithCustomMetric($"{additionalInfo} - Erro: {e.Message}", Metric.Erro, exception: e);

    public void LogInfo(string message)
    {
        Log.Information(message);
    }

    private void ConfigMetric(string eventName) => _metrics = CustomMetricDto.MetricFactory($"QtdInicio{_eventFullName}", $"QtdSucesso{_eventName}", $"QtdErro{_eventName}, {eventName}");

    private static string RemoveAccents(string text)
    {
        text = text.Replace(" ", "").Trim();

        StringBuilder sbReturn = new StringBuilder();

        var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();

        foreach (char letter in arrayText) if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark) sbReturn.Append(letter);

        return sbReturn.ToString();
    }

    public void LogWarning(string message)
    {
        Log.Information(message);
    }
}


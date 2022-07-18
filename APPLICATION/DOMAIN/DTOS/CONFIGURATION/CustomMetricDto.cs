using APPLICATION.ENUMS;
using System.Diagnostics.CodeAnalysis;

namespace APPLICATION.DOMAIN.DTOS.CONFIGURATION;

[ExcludeFromCodeCoverage]
public sealed class CustomMetricDto
{
    public Metric TipoMetrica { get; set; }
    public string NomeMetrica { get; set; }
    public double ValorMetrica { get; set; }

    public CustomMetricDto(Metric tipoMetrica, string nomeMetrica, double valorMetrica = 1)
    {
        TipoMetrica = tipoMetrica;

        NomeMetrica = nomeMetrica;

        ValorMetrica = valorMetrica;
    }

    public static List<CustomMetricDto> MetricFactory(string nomeMetricaInicio, string nomeMetricaSucesso, string nomeMetricaErro)
    {
        var defaultValorMetrica = 1;

        if (string.IsNullOrEmpty(nomeMetricaInicio) || string.IsNullOrEmpty(nomeMetricaSucesso) || string.IsNullOrEmpty(nomeMetricaErro)) return new List<CustomMetricDto>();

        return new List<CustomMetricDto>()
        {
            new CustomMetricDto(Metric.Inicio, nomeMetricaInicio, defaultValorMetrica),
            new CustomMetricDto(Metric.Sucesso, nomeMetricaSucesso, defaultValorMetrica),
            new CustomMetricDto(Metric.Erro, nomeMetricaErro, defaultValorMetrica)
        };
    }
}

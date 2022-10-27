using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace APPLICATION.APPLICATION.CONFIGURATIONS.SWAGGER;

public class HealthCheckSwagger : IDocumentFilter
{
    /// <summary>
    /// Configuração do HelthCheck.
    /// </summary>
    /// <param name="swaggerDoc"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var pathItem = new OpenApiPathItem();

        pathItem.Operations.Add(OperationType.Get, new OpenApiOperation
        {
            OperationId = "HeathCheck",
            Tags = new OpenApiTag[] { new OpenApiTag { Name = "HealthCheck" } },
            Responses = new OpenApiResponses
            {
                ["200"] = new OpenApiResponse { Description = "Healthy" },
                ["503"] = new OpenApiResponse { Description = "Unhealthy" }
            }
        });

        swaggerDoc.Paths.Add(ExtensionsConfigurations.HealthCheckEndpoint, pathItem);
    }
}

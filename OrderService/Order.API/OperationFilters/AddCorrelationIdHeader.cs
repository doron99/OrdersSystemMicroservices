using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Order.API.OperationFilters
{
    public class AddCorrelationIdHeader : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "x-correlation-id",
                In = ParameterLocation.Header,
                Required = false,
                Description = "Correlation ID for tracking requests",
                Schema = new OpenApiSchema { Type = "string" }
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "traceparent",
                In = ParameterLocation.Header,
                Required = false,
                Description = "Traceparent for distributed tracing",
                Schema = new OpenApiSchema { Type = "string" }
            });
        }
    }
}

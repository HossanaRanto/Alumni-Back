using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Alumni_Back
{
    public class AuthorizationOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check if the operation has the custom authorization attribute
            if (context.ApiDescription.CustomAttributes().OfType<Authorization>().Any())
            {
                // Set the security property of the operation to your custom authorization scheme
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement()
                };
            }
        }
    }
}
